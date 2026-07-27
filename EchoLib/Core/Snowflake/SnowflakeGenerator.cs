namespace EchoLib.Core.Snowflake;

internal class SnowflakeGenerator
{
	/// <summary>
	/// Epoch time in milliseconds from which snowflakes are generated.
	/// This is set to 01/01/2025 00:00:00 UTC.
	/// </summary>
	// ReSharper disable once MemberCanBePrivate.Global
	public static readonly long Epoch = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

	/// <summary>
	/// Lock object to ensure thread safety when generating snowflakes.
	/// This is necessary because multiple threads may attempt to generate snowflakes simultaneously,
	/// which could lead to duplicate values or other inconsistencies.
	/// </summary>
	private static readonly object Lock = new();

	/// <summary>
	/// Sequence value used to ensure unique snowflakes within the same millisecond.
	/// </summary>
	private static ushort _sequence = 0;

	/// <summary>
	/// Last timestamp in milliseconds used to generate a snowflake. Used to ensure the clock does not move backwards.
	/// </summary>
	private static long _lastMs = 0;

	/// <summary>
	/// Instance number for this specific server instance.
	/// </summary>
	public static int InstanceNumber { get; set; }

	/// <summary>
	/// Generate a new Snowflake.
	/// </summary>
	/// <param name="snowflakeVersion"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static Snowflake New(int snowflakeVersion = 1)
	{
		// Check to ensure snowflake version is valid
		if (snowflakeVersion > 16) throw new ArgumentException($"Snowflake Version {snowflakeVersion} cannot exist. Maximum is 16");

		lock (Lock)
		{
			// Grab ms
			long ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - Epoch;

			if (ms < _lastMs) throw new InvalidOperationException("Clock moved backwards. Unable to generate snowflake.");

			switch (snowflakeVersion)
			{
				case 1:
					if (ms == _lastMs)
					{
						_sequence++;

						if (_sequence > 4095) // 12 bits for increment
							throw new InvalidOperationException("Sequence overflow. Unable to generate snowflake.");
					}
					else
					{
						_sequence = 0;
					}

					_lastMs = ms;

					return new Snowflake((ulong)(ms << 42) | (ulong)(InstanceNumber << 22) | (ulong)(_sequence << 16) | ((ulong)snowflakeVersion - 1));

				default:
					throw new NotImplementedException($"Snowflake version {snowflakeVersion} not implemented");
			}
		}
	}
}