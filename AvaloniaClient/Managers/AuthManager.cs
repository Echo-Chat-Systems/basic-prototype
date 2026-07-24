using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Logging;
using AvaloniaClient.Targets;
using CommunityToolkit.Mvvm.ComponentModel;
using EchoLib.Models.Params.Auth;
using EchoLib.Protocol.Exceptions;
using EchoLib.Routing.Identification;
using EchoLib.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebSocketSharper;

namespace AvaloniaClient.Managers;

public partial class AuthManager : ObservableObject
{
	private readonly ILogger<AuthManager> _logger;
	private readonly IServiceProvider _services;
	private readonly AppState _state;
	private readonly TargetHub _targets;

	#region Exposed UI Logs For Frontend

	[ObservableProperty] public partial ObservableCollection<string> Logs { get; private set; } = [];

	private void Log(string message, params object[] args) => Logs.Add(string.Format(message, args));

	#endregion

	public AuthManager
	(
		ILogger<AuthManager> logger,
		IServiceProvider services,
		AppState state,
		TargetHub targets
	)
	{
		_logger = logger;
		_services = services;
		_state = state;
		_targets = targets;

		_state.Local.PropertyChanged += LocalStateChanged;
	}

	private void LocalStateChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(LocalState.AuthState) && _state.Local.AuthState == LocalState.AuthStates.StartConnect)
		{
			StartAsync().Forget(_logger);
		}
	}

	private async Task StartAsync()
	{
		if (_state.Local.UserFile == null) throw new InvalidOperationException("Cannot start auth flow with missing userfile");

		// Pull a dodgy and use ActivatorUtilities to create a new instance of EchoClient for state
		_state.Net.Client = ActivatorUtilities.CreateInstance<EchoClient>(_services);

		// Wait on connection
		await _state.Net.Client.Connect();

		try
		{
			Log("Sending hello...");
			ServerHelloParameters response = await _targets.Auth.SendHello();
			Log("Hello response received. Server name {0}", response.ServerName);
			_state.Remote.ServerName = response.ServerName;
		}
		catch (ProtocolException e)
		{
			Log("Sever hello failed with error {}", e.Message);
			_state.Local.AuthState = LocalState.AuthStates.Failed;
		}

		SigninCompleteParameters complete;
		try
		{
			complete = await _targets.Auth.Signin();
		}
		catch (NotFoundException)
		{
			_state.Local.AuthState = LocalState.AuthStates.CreatingNewAccount;
			return;
		}
		catch (ProtocolException e)
		{
			Log("Signin failed with error {}", e.Message);
			_state.Local.AuthState = LocalState.AuthStates.Failed;
		}
	}
}