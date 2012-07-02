using System;
using System.Collections.Generic;
using WebServices.AsyncHcpcsService;

namespace WebServices
{
	internal class SerialDescriber : Describer
	{
		public SerialDescriber(Action<string, string> descriptionFound) : base(descriptionFound)
		{
		}

		public override void GetDescriptions(IEnumerable<string> codes)
		{
			foreach (string code in codes)
			{
				HCPCS hcpcsCode = WebServiceConnections.AsyncClient.GetDetailsByCode(code);
				string description = hcpcsCode == null ? "<null>" : hcpcsCode.ShortDescription;
				DescriptionFound(code, description);
			}
		}
	}
}