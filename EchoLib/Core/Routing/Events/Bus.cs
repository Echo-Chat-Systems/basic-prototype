using System.Collections.Concurrent;

namespace EchoLib.Core.Routing.Events;

public class EventBus
{
	private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();

	/// <summary>
	/// Subscribe to an event of type T.
	/// Returns an IDisposable that can be used to unsubscribe.
	/// </summary>
	public IDisposable Subscribe<T>(Func<T, Task> handler) where T : IEvent
	{
		List<Delegate> list = _handlers.GetOrAdd(typeof(T), _ => []);

		lock (list)
		{
			list.Add(handler);
		}

		return new Subscription(() =>
		{
			lock (list)
			{
				list.Remove(handler);
			}
		});
	}

	/// <summary>
	/// Publish an event and await all handlers.
	/// </summary>
	public async Task PublishAsync<T>(T evt) where T : IEvent
	{
		if (!_handlers.TryGetValue(typeof(T), out List<Delegate>? list))
			return;

		List<Delegate> snapshot;

		lock (list)
		{
			snapshot = list.ToList();
		}

		foreach (Delegate handler in snapshot)
			try
			{
				await ((Func<T, Task>)handler)(evt);
			}
			catch (Exception ex)
			{
				// TODO: Replace with your logging system
				Console.WriteLine($"[EventBus] Handler error: {ex}");
			}
	}

	/// <summary>
	/// Fire-and-forget publish.
	/// </summary>
	public void Publish<T>(T evt) where T : IEvent
	{
		_ = PublishAsync(evt);
	}

	private class Subscription : IDisposable
	{
		private readonly Action _unsubscribe;
		private bool _disposed;

		public Subscription(Action unsubscribe)
		{
			_unsubscribe = unsubscribe;
		}

		public void Dispose()
		{
			if (_disposed) return;
			_unsubscribe();
			_disposed = true;
		}
	}
}