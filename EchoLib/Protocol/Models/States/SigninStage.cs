namespace EchoLib.Protocol.Models.States;

public enum SigninStage
{
	NotStarted,
	Started,
	Challenged,
	ChallengeResponded,
	Completed, 
	Failed
}