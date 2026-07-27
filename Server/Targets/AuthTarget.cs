using System.Security.Cryptography;
using EchoLib.Core;
using EchoLib.Crypto.Signing;
using EchoLib.Models.Data.User;
using EchoLib.Models.Params.Auth;
using EchoLib.Models.States;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing;
using EchoLib.Routing.Identification;
using Microsoft.Extensions.Logging;
using Server.Database;
using Server.Database.Models.Public;

namespace Server.Targets;

public class AuthTarget : TargetBase<AuthTarget>
{
    private readonly Config _config;
    private readonly ClientManager _clientManager;
    private readonly DbHub _db;

    public override string Name => "auth";

    public AuthTarget(
        ILogger<AuthTarget> logger,
        ClientManager clientManager,
        Config config,
        DbHub db
    ) : base(logger)
    {
        _config = config;
        _db = db;
        _clientManager = clientManager;
    }

    public class SigninState
    {
        public SigninStage Stage = SigninStage.NotStarted;

        public byte[]? SignChallenge;
        public byte[]? EncryptChallenge;
    }

    [Route("hello")]
    public async Task HandleHello(RoutingContext ctx, ClientHelloParameters parameters)
    {
        Logger.LogInformation("New Client, Hello! Key: {PublicSigningKey}", parameters.KeyPair.SigningKey);

        // Associate this client with their claimed ID (THIS DOES NOT MEAN THEY ARE AUTHENTICATED!!!!!!!!!)
        ServerClient? client = _clientManager.Get(ctx.Socket);

        // Check if client is null, if so somethings fucked 
        if (client is null) throw new SocketRegistryException();

        // Set client keys and id 
        client.Id = parameters.KeyPair.SigningKey;
        client.KeyPair = parameters.KeyPair;

        // Respond with the server-hello
        await ctx.ReplyAsync(new ServerHelloParameters { ServerName = _config.Appearance.BroadcastName });
    }

    [Route("signup")]
    public async Task HandleSignup(RoutingContext ctx, SignupParameters parameters)
    {
        // Check if this user already exists
        if (await _db.Users.GetAsync(parameters.Keys.SigningKey) != null) throw new KeyConflictException();

        // Create this user in the db
        await ctx.ReplyAsync(
            new SignupCompleteParameters
            {
                User = DtoMapper.Map<UserDbm, JUser>(
                    await _db.Users.InsertAsync(new UserDbm.New
                    {
                        Id = parameters.Keys.SigningKey,
                        Ek = parameters.Keys.EncryptionKey,
                        Username = parameters.Profile.Username,
                        Tag = (short) parameters.Profile.Tag,
                        Profile = parameters.Profile,

                    }))
            }
        );
    }

    #region Signin

    [Route("signin-start")]
    public async Task HandleSigninStart(RoutingContext ctx, SigninStartParameters parameters)
    {
        // Get this client from the manager
        ServerClient? client = _clientManager.Get(ctx.Socket);

        if (client is null)
        {
            // Client is somehow not in manager, this means they skipped the client-hello, disconnect them 
            ctx.Socket.CloseAsync();
            return;
        }

        Logger.LogDebug("Client {Id} signin-start", client.Id);

        // Check to ensure that this socket does not have an existing signin session 
        if (client.SigninState.Stage != SigninStage.NotStarted) throw new SigninAlreadyStartedException();

        // Check if this client exists in the db
        if (await _db.Users.GetAsync(client.Id!) == null)
        {
            Logger.LogError("Client {Id} not found!", client.Id);
            throw new NotFoundException();
        }

        // Generate a set of challenges for the client
        byte[] signChallenge = RandomNumberGenerator.GetBytes(64);
        byte[] encryptChallenge = RandomNumberGenerator.GetBytes(64);

        // Store the challenges in the client's signin state
        client.SigninState.SignChallenge = signChallenge;
        client.SigninState.EncryptChallenge = encryptChallenge;
        client.SigninState.Stage = SigninStage.Challenged;

        // Send the challenges to the client
        await ctx.ReplyAsync(new SigninChallengeParameters
        {
            SignChallenge = Convert.ToBase64String(signChallenge),
            EncryptChallenge =
                Convert.ToBase64String(
                    client.KeyPair!.EncryptionKey!
                        .Encrypt(
                            encryptChallenge)) // Encrypt the encrypt challenge with the client's encryption key so only they can read it
        });
    }

    [Route("signin-response")]
    public async Task HandleSigninResponse(RoutingContext ctx, SigninResponseParameters parameters)
    {
        // Get this client from the manager
        ServerClient? client = _clientManager.Get(ctx.Socket);

        if (client is null)
        {
            // Client is somehow not in manager, this means they skipped the client-hello, disconnect them 
            ctx.Socket.CloseAsync();
            return;
        }

        Logger.LogDebug("Client {Id} signin-response", client.Id);

        // Check to ensure that this socket has an active signin session
        if (client.SigninState.Stage != SigninStage.Challenged) throw new SigninNotStartedException();

        // Update stage
        client.SigninState.Stage = SigninStage.ChallengeResponded;

        // Convert the response challenges back from base64
        Signature sig = new(parameters.Signature);
        byte[] encryptResponse = Convert.FromBase64String(parameters.Decrypted);

        // Verify the signature and encryption response
        bool sigValid = sig.Verify(client.Id!.KeyParams, client.SigninState.SignChallenge!);
        bool encryptValid = client.SigninState.EncryptChallenge!.SequenceEqual(encryptResponse);

        if (sigValid && encryptValid)
        {
            // Authentication successful, update client state and respond with success
            client.SigninState.Stage = SigninStage.Completed;

            // Get the user 
            UserDbm userDbm = (await _db.Users.GetAsync(client.Id!))!;

            await ctx.ReplyAsync(new SigninCompleteParameters
            {
                User = new JUser
                {
                    Id = userDbm.Id,
                    Ek = userDbm.Ek,
                    CreatedAt = userDbm.CreatedAt,
                    Profile = new JProfile
                    {
                        Username =  userDbm.Username,
                        Tag = (ushort)userDbm.Tag,
                    }
                }
            });
        }
        else
        {
            // Authentication failed, disconnect the client
            ctx.Socket.CloseAsync();
        }
    }

    #endregion
}