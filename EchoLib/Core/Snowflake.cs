namespace EchoLib.Core;

public class Snowflake
{
	/// <summary>
	/// Lock object to ensure thread safety when generating snowflakes.
	/// This is necessary because multiple threads may attempt to generate snowflakes simultaneously,
	/// which could lead to duplicate values or other inconsistencies.
	/// </summary>
	private static readonly object Lock = new();

	/// <summary>
	/// Increment value used to ensure unique snowflakes within the same millisecond.
	/// </summary>
	private static ushort _increment = 0;

	/// <summary>
	/// Last timestamp in milliseconds used to generate a snowflake. Used to ensure the clock does not move backwards.
	/// </summary>
	private static long _lastMs = 0;

	/// <summary>
	/// Epoch time in milliseconds from which snowflakes are generated.
	/// This is set to 01/01/2025 00:00:00 UTC.
	/// </summary>
	// ReSharper disable once MemberCanBePrivate.Global
	public static readonly long Epoch = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

	public ulong Value { get; }
	
	public DateTime Timestamp { get; }
	public byte ApiVersion { get; }
	public ulong Increment { get; }
	
	public Snowflake(ulong snowflake)
	{
		Value = snowflake;
		long timestamp = (long)(snowflake >> 20) + Epoch;
		Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
		ApiVersion = (byte)((snowflake >> 16) & 0xF);
		Increment = snowflake & 0xFFFF;
	}

	public Snowflake(long snowflake) : this((ulong)snowflake)
	{
	}

	public static Snowflake New(byte apiVersion = 1)
	{
		lock (Lock)
		{
			long ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Epoch;
			if (ms < _lastMs)
			{
				throw new InvalidOperationException("Clock moved backwards. Unable to generate snowflake.");
			}

			if (ms == _lastMs)
			{
				_increment++;
				if (_increment > 4095) // 12 bits for increment
				{
					throw new InvalidOperationException("Increment overflow. Unable to generate snowflake.");
				}
			}
			else
			{
				_increment = 0;
			}

			_lastMs = ms;

			return new Snowflake((ulong)(ms << 44) | ((ulong)apiVersion << 16) | _increment);
		}
	}
}