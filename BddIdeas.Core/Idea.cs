using System;
using System.Collections.Generic;

namespace BddIdeas.Core
{
	public class Idea
	{
		private IdeaContext _currentContext;

		public Idea()
		{
			Describe = DescribeImpl;
			It = ItImpl;
			Before = BeforeImpl;
			Pending = PendingImpl;
			RootContext = _currentContext = new IdeaContext();
		}

		public Action<object, Action> Describe { get; private set; }
		public Action<string, Action> It { get; private set; }
		public Action<string, Action> Pending { get; private set; }
		public Action<Action> Before { get; private set; }

		public IEnumerable<Spec> Specs
		{
			get { return RootContext.AllSpecs; }
		}

		public IdeaContext RootContext { get; private set; }

		private void PendingImpl(string description, Action implementation)
		{
			_currentContext.AddSpec(new Spec(_currentContext, description, implementation, false));
		}

		private void ItImpl(string description, Action implementation)
		{
			_currentContext.AddSpec(new Spec(_currentContext, description, implementation));
		}

		public void BeforeImpl(Action implementation)
		{
			_currentContext.BeforeBlocks.Add(implementation);
		}

		private void DescribeImpl(object description, Action action)
		{
			var context = new IdeaContext(description.ToString(), _currentContext);
			_currentContext = context;
			action();
			_currentContext = context.Parent;
		}
	}
}