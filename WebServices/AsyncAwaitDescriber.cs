using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.AsyncHcpcsService;

namespace WebServices
{
	class AsyncAwaitDescriber : AsyncDescriber
	{
		public AsyncAwaitDescriber(Action<string, string> descriptionFound) : base(descriptionFound)
		{
		}

		private async Task _GetDescription(string code)
		{
			HCPCS hcpcsCode = await WebServiceConnections.AsyncClient.GetDetailsByCodeAsync(code);
			string description = hcpcsCode == null ? "<null>" : hcpcsCode.ShortDescription;
			DescriptionFound(code, description);
		}

		public override void GetDescriptions(IEnumerable<string> codes)
		{
			IEnumerable<Task> tasks = 
				from code in _Throttled(codes)
				select _GetDescription(code);

			Task.WaitAll(tasks.ToArray());
		}
	}
}
