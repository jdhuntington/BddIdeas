namespace BddIdeas.CsxRunner
{
    using System;
    using Core;
    using ScriptCs;
    using ScriptCs.Contracts;

    public class SpecScriptHost : ScriptHost
    {
        private readonly Idea _root = new Idea();

        public Action<object, Action> Describe { get; private set; }
        public Action<string, Action> It { get; private set; }
        public Action<string, Action> Pending { get; private set; }
        public Action<Action> Before { get; private set; }

        public IdeaContext GetRootContext()
        {
            return _root.RootContext; 
        }

        public SpecScriptHost(IScriptPackManager scriptPackManager, ScriptEnvironment environment)
            : base(scriptPackManager, environment)
        {
            Describe = _root.Describe;
            It = _root.It;
            Pending = _root.Pending;
            Before = _root.Before;
        }
    }
}