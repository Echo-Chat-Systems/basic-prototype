using EchoLib.Models.States;

namespace TuiClient.Events;

public class SigninStageChangedEventArgs(SigninStage before, SigninStage now)
{
	public SigninStage Before { get; } = before;
	public SigninStage Now { get; } = now;
}