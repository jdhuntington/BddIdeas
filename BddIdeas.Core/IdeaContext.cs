using System;
using System.Collections.Generic;

namespace BddIdeas.Core
{
	public class IdeaContext
	{
		public IdeaContext(string description = null, IdeaContext parent = null)
		{
			Parent = parent;
			Children = new List<IdeaContext>();
			if (parent != null)
			{
				parent.Children.Add(this);
			}
			Specs = new List<Spec>();
			BeforeBlocks = new List<Action>();
			Description = description;
		}

		public string Description { get; set; }

		public string FullDescription
		{
			get
			{
				return (Parent != null && Parent.Description != null ? Parent.FullDescription + " " : "")
				       + Description;
			}
		}

		public List<IdeaContext> Children { get; protected set; }
		public List<Spec> Specs { get; set; }
		public IdeaContext Parent { get; set; }

		public IEnumerable<Spec> AllSpecs
		{
			get
			{
				var specs = new List<Spec>(Specs);
				foreach (var child in Children)
				{
					specs.AddRange(child.AllSpecs);
				}
				return specs;
			}
		}

		public List<Action> BeforeBlocks { get; private set; }

		public void AddSpec(Spec spec)
		{
			Specs.Add(spec);
		}

		public void RunBeforeBlocks()
		{
			if (Parent != null)
			{
				Parent.RunBeforeBlocks();
			}
			foreach (var beforeBlock in BeforeBlocks)
			{
				beforeBlock();
			}
		}
	}
}