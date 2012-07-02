using System;
using System.Collections.Generic;
using System.Linq;
using WebServices.BeginEndHcpcsService;

namespace WebServices
{
	internal class OldSchoolDescriber : AsyncDescriber
	{
		public OldSchoolDescriber(Action<string, string> descriptionFound) : base(descriptionFound)
		{
		}

		public IAsyncResult BeginGetDetailsByCode(string code)
		{
			return WebServiceConnections.BeginEndClient.BeginGetDetailsByCode(code, _OnGetDetailsByCodeComplete, code);
		}

		public override void GetDescriptions(IEnumerable<string> codes)
		{
			IEnumerable<IAsyncResult> asyncRequests =
				from code in codes
				select BeginGetDetailsByCode(code);

			List<IAsyncResult> pendingRequests = _Throttled(asyncRequests).ToList();

			//wait for all the requests to finish
			foreach (IAsyncResult request in pendingRequests)
			{
				if (!request.IsCompleted)
				{
					request.AsyncWaitHandle.WaitOne();
				}
			}
		}

		

		private void _OnGetDetailsByCodeComplete(IAsyncResult asyncResult)
		{
			string description;
			try
			{
				HCPCS result = WebServiceConnections.BeginEndClient.EndGetDetailsByCode(asyncResult);
				description = result == null ? "<null>" : result.ShortDescription;
			}
			catch (Exception exception)
			{
				description = exception.Message;
			}
			var code = (string) asyncResult.AsyncState;
			DescriptionFound(code, description);
		}
	}
}