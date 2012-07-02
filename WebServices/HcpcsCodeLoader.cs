using System.Collections.Generic;
using System.IO;

namespace WebServices
{
	public class HcpcsCodeLoader
	{
		public IEnumerable<string> GetHcpcsCodes()
		{
			using(var stream = GetType().Assembly.GetManifestResourceStream(GetType(), "HCPCSCodes.txt"))
			{
				using(var reader = new StreamReader(stream))
				{
					string line;
					while((line = reader.ReadLine()) != null)
					{
						if(!line.StartsWith("#")) yield return line;
					}
				}
			}
		}
	}
}
