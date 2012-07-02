using System;
using System.Collections.Generic;

namespace WebServices
{
	/// <summary>
	/// A Describer gets descriptions. Call the GetDescriptions() method to start the process.
	/// Each time a description is found for a HCPCS code, the implementation of the GetDescriptions() method
	/// should call the DescriptionFound(string, string) method.
	/// </summary>
	public abstract class Describer
	{
		private readonly HcpcsCodeLoader mCodeLoader = new HcpcsCodeLoader();
		private readonly Action<string, string> mDescriptionFoundCallback;

		protected Describer(Action<string, string> descriptionFound)
		{
			mDescriptionFoundCallback = descriptionFound;
		}

		protected virtual void DescriptionFound(string code, string description)
		{
			mDescriptionFoundCallback(code, description);
		}

		public void GetDescriptions()
		{
			GetDescriptions(mCodeLoader.GetHcpcsCodes());
		}

		public abstract void GetDescriptions(IEnumerable<string> codes);
	}
}