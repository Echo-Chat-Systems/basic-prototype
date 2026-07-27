using System.Data;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;

namespace Server.Database.ParameterConverters;

public class JsonbConverter<T> : SqlMapper.TypeHandler<T> where T : class
{
	public override void SetValue(IDbDataParameter parameter, T? value)
	{
		parameter.Value = value is null ? DBNull.Value : JsonConvert.ToString(value);

		if (parameter is NpgsqlParameter para) para.NpgsqlDbType = NpgsqlDbType.Jsonb;
	}

	public override T? Parse(object? value)
	{
		if (value is null || value == DBNull.Value) return null;
		return JsonConvert.DeserializeObject<T>(value.ToString()!);
	}
}