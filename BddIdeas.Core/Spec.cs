using System;

namespace BddIdeas.Core
{
	public class Spec
	{
		public Spec(IdeaContext context, string description, Action implementation, bool enabled = true)
		{
			Description = description;
			Implementation = implementation;
			Context = context;
			Enabled = enabled;
		}

		public bool Enabled { get; set; }
		public IdeaContext Context { get; set; }
		public string Description { get; set; }

		public string FullDescription
		{
			get { return Context.FullDescription + " " + Description; }
		}

		public Action Implementation { get; set; }
		public bool Passed { get; set; }
		public Exception Error { get; set; }

		public void Run()
		{
			try
			{
				Context.RunBeforeBlocks();
				Implementation();
				Passed = true;
			}
			catch (Exception e)
			{
				Error = e;
			}
		}
	}
}