using System.Reflection;
using EchoLib.Routing.Storage;

namespace EchoLib.Routing.Identification;

public interface ITargetHub
{
	internal void Populate(TargetInstanceRegistry targets)
	{
		// Enumerate over targets provided and match them with property names
		foreach (
				PropertyInfo p in GetType()
					.GetProperties()
					.Where(p => typeof(ITarget).IsAssignableFrom(p.PropertyType))
			)
			// Initialise prop with matching target if exists
			if (targets.TryGet(p.PropertyType, out ITarget? target))
				AssignProp(p, target!);
	}

	private void AssignProp(PropertyInfo prop, ITarget target)
	{
		prop.SetValue(this, target);
	}
}