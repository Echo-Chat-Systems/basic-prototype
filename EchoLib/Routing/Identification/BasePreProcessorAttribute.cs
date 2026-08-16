namespace EchoLib.Routing.Identification;

[AttributeUsage(AttributeTargets.Method)]
public abstract class BasePreProcessorAttribute : Attribute
{
	/// <summary>
	///		Run the preprocessor.
	/// </summary>
	/// <param name="services">
	///		Services.
	///	</param>
	/// <param name="ctx">
	///		Routing context.
	/// </param>
	/// <returns>Determines if execution should continue to the next preprocessor/route.</returns>
	public abstract Task<bool> Run(RoutingContext ctx);
}