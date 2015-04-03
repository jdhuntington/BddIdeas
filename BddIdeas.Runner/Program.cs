using System;
using System.Collections.Generic;
using BddIdeas.Core;
using BddIdeas.Tests;

namespace BddIdeas.Runner
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.Error.WriteLine("Hardcoded to run the BddIdeas.Tests suite...");
			var core = new Idea();
			new CoreSpec(core);

			// Console.WriteLine("One potential runner...");
			// SimpleRunner(core);

			Console.WriteLine("And another runner...");
			var failures = new List<Spec>();
			NestedRunner(failures, core.RootContext);
			if (failures.Count > 0)
			{
				Console.WriteLine("\nWe've got some things we need to talk about.\n");
				foreach (var failure in failures)
				{
					Console.WriteLine("{0}\n  {1}\n", failure.FullDescription, failure.Error.Message);
				}
			}


			Console.Write("Done. Press the any key to exit.");
			Console.ReadKey();
		}

		private static void NestedRunner(List<Spec> failures, IdeaContext context, int nest = 0)
		{
			Console.WriteLine("{0}{1}", new String(' ', nest*2), context.Description);
			foreach (var spec in context.Specs)
			{
				if (spec.Enabled)
				{
					spec.Run();
					if (spec.Passed)
					{
						Console.ForegroundColor = ConsoleColor.Green;
					}
					else
					{
						failures.Add(spec);
						Console.ForegroundColor = ConsoleColor.Red;
					}
				}
				Console.WriteLine("{0}{1}", new String(' ', (nest + 1)*2), spec.Description);
				Console.ResetColor();
			}
			foreach (var ideaContext in context.Children)
			{
				NestedRunner(failures, ideaContext, nest + 1);
			}
		}

		private static void SimpleRunner(Idea ideaCore)
		{
			foreach (var spec in ideaCore.Specs)
			{
				try
				{
					if (spec.Enabled)
					{
						spec.Implementation();
						Console.ForegroundColor = ConsoleColor.Green;
					}
					Console.WriteLine(spec.FullDescription);
				}
				catch (Exception e)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(spec.FullDescription);
					Console.WriteLine(e.Message);
				}
				finally
				{
					Console.ResetColor();
				}
			}
		}
	}
}