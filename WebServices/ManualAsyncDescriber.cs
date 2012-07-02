using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.AsyncHcpcsService;

namespace WebServices
{
	internal class ManualAsyncDescriber : AsyncDescriber
	{
		public ManualAsyncDescriber(Action<string, string> descriptionFound) : base(descriptionFound)
		{
		}

		public override void GetDescriptions(IEnumerable<string> codes)
		{
			Task[] tasks =
				(from code in _Throttled(codes)
				select _GetCodeAsync(code)).ToArray();
			Task.WaitAll(tasks);
		}

		private Task _GetCodeAsync(string code)
		{
			return WebServiceConnections.AsyncClient.GetDetailsByCodeAsync(code).ContinueWith(prev =>
			{
				string description;
				try
				{
					HCPCS hcpcs = prev.Result;
					description = hcpcs == null ? "<null>" : hcpcs.ShortDescription;
				}
				catch (AggregateException exception)
				{
					description = exception.InnerException.Message;
				}
				DescriptionFound(code, description);
			}, TaskContinuationOptions.AttachedToParent);
		}
	}
}