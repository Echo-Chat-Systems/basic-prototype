using System.Linq.Expressions;
using System.Reflection;

namespace Server.JmDbConverter;

public sealed class DbConversionRegistry
{
	private static Dictionary<(Type, Type), Delegate> _converters = BuildConverters();

	// Ultra-fast cache system that I still need to get explained because idk how this shit works 
	private static class Cache<TSource, TDest>
	{
		public static Func<TSource, TDest>? Converter;
	}

	public TDest Convert<TSource, TDest>(TSource src)
	{
		// Grab the cached converter
		Func<TSource, TDest>? fn = Cache<TSource, TDest>.Converter;

		// Check if there is a converter
		return fn is null
			? throw new InvalidOperationException($"No converter registered: {typeof(TSource)} -> {typeof(TDest)}")
			:
			// Return converter delegate
			fn(src);
	}

	//TODO: ALL OF THIS IS AI GEN, READ, AND COMMENT ALL OF IT, IT PROBABLY WON'T WORK OUT OF THE BOX
	#region Reflection Stuff

	private static Dictionary<(Type, Type), Delegate> BuildConverters()
	{
		IEnumerable<Type> SafeGetTypes(Assembly a)
		{
			try
			{
				return a.GetTypes();
			}
			catch
			{
				return [];
			}
		}

		// Working internal method map and model pairs
		Dictionary<(Type, Type), Delegate> map = new();
		List<(Type src, Type dest)> pairs = [];

		// Scan application assemblies 
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

		Type[] sourceTypes = assemblies
			.SelectMany(SafeGetTypes)
			.ToArray();

		// Find all instances of a registered conversion
		foreach (Type source in sourceTypes)
		{
			// Get all attributes this type implements with BindToModelAttribute
			IEnumerable<BindsToModelAttribute> modelAttributes = source.GetCustomAttributes<BindsToModelAttribute>();

			// Add each pairing to the list
			pairs.AddRange(modelAttributes.Select(attribute => (source, attribute.TargetType)));
		}

		// Build converters
		foreach ((Type src, Type dest) in pairs)
		{
			Delegate converter = BuildConverter(src, dest, map);

			map[(src, dest)] = converter;

			// populate fast cache
			Type cacheType = typeof(Cache<,>).MakeGenericType(src, dest);
			cacheType.GetField("Converter")!
				.SetValue(null, converter);
		}

		return map;
	}

	private static Delegate BuildConverter(
		Type sourceType,
		Type destType,
		Dictionary<(Type, Type), Delegate> registry)
	{
		
		ParameterExpression srcParam = Expression.Parameter(sourceType, "src");

		List<MemberBinding> bindings = [];

		PropertyInfo[] destProps = destType.GetProperties(
			BindingFlags.Public | BindingFlags.Instance);

		foreach (PropertyInfo destProp in destProps)
		{
			PropertyInfo? sourceProp = FindSourceProperty(sourceType, destType, destProp);
			if (sourceProp == null) continue;

			Expression valueExpr = BuildPropertyExpression(
				sourceProp.PropertyType,
				destProp.PropertyType,
				Expression.Property(srcParam, sourceProp),
				registry);

			bindings.Add(Expression.Bind(destProp, valueExpr));
		}

		MemberInitExpression body = Expression.MemberInit(
			Expression.New(destType),
			bindings);

		Type lambdaType = typeof(Func<,>).MakeGenericType(sourceType, destType);

		return Expression.Lambda(lambdaType, body, srcParam)
			.Compile();
	}

	private static Expression BuildPropertyExpression(
		Type sourceType,
		Type destType,
		Expression sourceExpr,
		Dictionary<(Type, Type), Delegate> registry)
	{
		// 1. direct assign
		if (destType.IsAssignableFrom(sourceType))
			return sourceExpr;

		// 2. nested converter exists
		if (registry.TryGetValue((sourceType, destType), out Delegate? del)) return Expression.Invoke(Expression.Constant(del), sourceExpr);

		// 3. string -> json model (optional hook point)
		if (sourceType == typeof(string))
		{
			MethodInfo method = typeof(System.Text.Json.JsonSerializer)
				.GetMethods()
				.First(m =>
					m.Name == "Deserialize" &&
					m.IsGenericMethodDefinition &&
					m.GetParameters().Length == 1)
				.MakeGenericMethod(destType);

			return Expression.Call(method, sourceExpr);
		}

		throw new InvalidOperationException(
			$"No mapping for {sourceType} -> {destType}");
	}

	private static PropertyInfo? FindSourceProperty(
		Type sourceType,
		Type destType,
		PropertyInfo destProp)
	{
		// 1. exact name match
		PropertyInfo? direct = sourceType.GetProperty(destProp.Name);
		if (direct != null) return direct;

		// 2. attribute-based mapping
		foreach (PropertyInfo prop in sourceType.GetProperties())
		{
			IEnumerable<MapsToAttribute> attrs = prop.GetCustomAttributes()
				.OfType<MapsToAttribute>();

			foreach (MapsToAttribute attr in attrs)
				if (attr.TargetType == destType &&
				    attr.TargetProperty == destProp.Name)
					return prop;
		}

		return null;
	}

	#endregion
}