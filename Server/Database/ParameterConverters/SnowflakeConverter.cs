using System.Data;
using Dapper;
using EchoLib.Core.Snowflake;

namespace Server.Database.ParameterConverters;

public class SnowflakeConverter : SqlMapper.TypeHandler<Snowflake>
{
	public override void SetValue(IDbDataParameter parameter, Snowflake value)
	{
		parameter.DbType = DbType.UInt64;
		parameter.Value = value;
	}

	public override Snowflake Parse(object value)
	{
		return new Snowflake((ulong)value);
	}
}