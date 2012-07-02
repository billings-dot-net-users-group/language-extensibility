using System;
using System.Diagnostics;
using System.Threading;

namespace WebServices
{
	class Program
	{
		private readonly Describer mDescriber;

		public Program(Describer describer)
		{
			mDescriber = describer;
		}
		public void Run()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			
			mDescriber.GetDescriptions();

			stopwatch.Stop();
			Console.WriteLine("Completed in {0}", stopwatch.Elapsed);
		}

		public static void WriteCodeAndDescription(string code, string description)
		{
			Console.WriteLine("{0}: {1}: {2}", Thread.CurrentThread.GetHashCode(), code, description);
		}

		public static void Main( string[] args )
		{
			var describer = new SerialDescriber( WriteCodeAndDescription );
			//var describer = new OldSchoolDescriber( WriteCodeAndDescription );
			//var describer = new SerialDescriber( WriteCodeAndDescription );
			//var describer = new ManualAsyncDescriber( WriteCodeAndDescription );
			//var describer = new AsyncAwaitDescriber(WriteCodeAndDescription);
			
			var program = new Program(describer);
			
			program.Run();
			Console.ReadKey();
		}
	}
}
