namespace EchoLib.Models.States;

public enum SigninStage
{
	NotStarted,
	Started,
	Challenged,
	ChallengeResponded,
	Completed,
	Failed
}