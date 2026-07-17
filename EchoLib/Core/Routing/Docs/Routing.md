# EchoLib routing

This is a planning document used to lay out how I want the routing rebuild to function. This document is intended to be 
used in conjunction with an LLM to help me plan in accordance with my goals, importantly the LLM must not be in charge
but rather assist in the planning process. Importantly, this document is intended as a live planning environment where the 
concrete details of implementation are intended to be hashed out down to the level of class names and function signatures,
with the intention being to have a document that I can then go and implement in code.

There are essentially two types of messages. Fire-and-forget messages, where there is no response expected, and request-response 
messages, where a response is expected. The routing system will need to handle both types of messages efficiently.

## Protocol format

The protocol format is strongly defined with the aim of being relatively easy to type and route in strongly typed languages. 

```json Protocol Message Example
{
    "mid": "random-guid",
    "target": "users",
    "data": {
        "action": "get",
        "params": { 
            "id": "useridhere"
        }
    }
}
```

## Goals and Constraints

There are a few explicit goals and constraints of the actual implementation.

### #1 Syntax

I am going to be putting a lot of work into this project, and so I want to make sure that the syntax is as clean and easy to use as possible. This means that I want to avoid
verbosity and boilerplate as much as possible. As such I've decided to use a conjoined routing system.

Routing will be done through Target classes, which will be responsible for handling messages of a specific *Target*.

Each Target class must have a `Name` property, which will be read at startup and used to register the target with the routing system.

Each Target class will have methods marked with a `[Route]` attribute, which will be used to register the method as a route handler for a specific *action*.

```csharp Target Example

public interface ITarget 
{
    /// <summary>
	/// Target name. Used for routing messages to this target.
	/// </summary>
	string Name { get; }
}
// Note: Repo filled by DI
public class AuthHandler(IUsersRepo usersRepo) : ITarget
{
    public string Name => "auth";
    
    [Route("login")]
    public Task<Response<LoginResponse>> HandleLogin(RouteContext ctx, LoginParams loginParams)
    {
        // Handle login logic here
    }
}
```

### #2 No Runtime Reflection

The system must minimise to the maximum possible extent, the level of runtime reflection, ideally reducing it to 
0, except in circumstances where reflection is more performative than other solutions (I doubt this will be the case).

## Fire-And-Forget Messages

Fire-and-forget messages are intended for use cases where the sender does not require a response from the receiver. 
Examples of this include notifications, error responses, and most server→client messages (but importantly not all).

The intended routing path for these messages is as follows.

Message Received → Message Envelope Deserialized → Message sent to appropriate target → Message processed by target → Message discarded

## Request-Response Messages

Request-response messages are intended for use cases where the sender expects a response from the receiver.
The most common example of this the auth flow, where the client and server exchange messages repeatedly to establish a 
session. Importantly, error responses are also considered request-response messages, as the sender must know if the 
server throws an error or not.

The intended routing path for these messages is as follows.

Client sends request → Server receives request → Server processes request → Server sends response → Client receives response (from here the client can either send another request or discard the response)

```csharp Syntax Example
// In server
public class UsersHandler(IUsersRepo users) : ITarget 
{
    public string Name => "users";
    
    [Route("get")] 
    public async Task<Response<GetUserResponse>> GetUser(RoutingContext ctx, GetUserParameters parameters) {
        return await users.Get(parameters.Id);
    }
} 

// In client (gui pseudocode cause I hate writing GUI code)
public class UsersHandler : ITarget 
{
    public string Name => "users";
    
    public async Task<UserData?> GetUser(UserId id) 
    {
        return this.Connection.AwaitResponse<GetUserParameters>(id);
    }
}

public class Window 
{
    public required Router router { get; }
    
    public Task Run()
    {
        var user = await router.Get<UsersHandler>.GetUser(id);
    }
}

```