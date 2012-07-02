using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace WebServices
{
	/// <summary>
	/// Base class for Describer implementations that get data asynchronously. This class provides two important facilities.
	/// 
	/// 1) Request throttling. If there are too many concurrent requests pending for the service at one time,
	/// some requests will start to time out. It is important to make sure that there is a limit to the number of pending
	/// requests. In practice I have seen that 100 is a workable limit.
	/// 
	/// 2) Message queuing. Rather than have the DescriptionFound(string, string) method run on any old thread, this
	/// class overrides DescriptionFound(string, string) to queue codes into a ConcurrentQueue. This queue is processed
	/// on a single thread by calling the ProcessResults() method. This makes it safer to write UI code for displaying
	/// results. ProcessResults() is called automatically on the last message.
	/// </summary>
	public abstract class AsyncDescriber : Describer
	{
		private const int CONCURRENT_REQUESTS = 100;
		private readonly int mMaxConcurrentRequests;
		private readonly ConcurrentQueue<Tuple<string, string>> mResults = new ConcurrentQueue<Tuple<string, string>>();
		private int mPendingRequests;

		protected AsyncDescriber(Action<string, string> descriptionFound, int maxConcurrentRequests = CONCURRENT_REQUESTS)
			: base(descriptionFound)
		{
			mMaxConcurrentRequests = maxConcurrentRequests;
		}

		private void _ProcessResults()
		{
			Tuple<string, string> result;
			while (mResults.TryDequeue(out result))
			{
				base.DescriptionFound(result.Item1, result.Item2);
			}
		}

		protected override void DescriptionFound(string code, string description)
		{
			Interlocked.Decrement(ref mPendingRequests);
			base.DescriptionFound(code, description);

			//process results if we have recieved the last message
			if (mPendingRequests == 0 && mResults.Count > 0)
			{
				_ProcessResults();
			}
		}

		protected IEnumerable<T> _Throttled<T>(IEnumerable<T> values)
		{
			foreach (T value in values)
			{
				Interlocked.Increment(ref mPendingRequests);
				if (mPendingRequests >= mMaxConcurrentRequests)
				{
					_ProcessResults();
					SpinWait.SpinUntil(() => mPendingRequests < mMaxConcurrentRequests);
				}
				yield return value;
			}
		}
	}
}