using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AvaloniaClient.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaClient;

public static class ViewModelCollector
{
	public static IServiceCollection AddViewModels(this IServiceCollection services)
	{
		// Enumerate over all implementations of ViewModelBase
		IEnumerable<Type> types = Assembly.GetExecutingAssembly()
			.GetTypes()
			.Where(t => t is { IsAbstract: false } && typeof(ViewModelBase).IsAssignableFrom(t));

		foreach (Type type in types)
		{
			// Get assigned attributes on type
			List<Attribute> attributes = type
				.GetCustomAttributes()
				.Where(a => a.GetType() == typeof(TransientModelAttribute) || a.GetType() == typeof(SingletonModelAttribute))
				.ToList();

			// If there is more than one of these applied, throw an error
			if (attributes.Count != 1) throw new DiAttributeSpecificationException();

			// Switch on type
			switch (attributes[0])
			{
				case TransientModelAttribute:
					services.AddTransient(type);
					break;
				case SingletonModelAttribute:
					services.AddSingleton(type);
					break;
				default:
					throw new DiAttributeSpecificationException();
			}
		}

		return services;
	}
}

[AttributeUsage(AttributeTargets.Class)]
public class TransientModelAttribute : Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class SingletonModelAttribute : Attribute;

public class DiAttributeSpecificationException : Exception;