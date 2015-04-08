namespace BddIdeas.CsxRunner
{
    using ScriptCs;
    using ScriptCs.Contracts;

    public class SpecScriptHostFactory : IScriptHostFactory
    {
        public SpecScriptHost Host { get; private set;}

        public IScriptHost CreateScriptHost(IScriptPackManager scriptPackManager, string[] scriptArgs)
        {
            Host = new SpecScriptHost(scriptPackManager, new ScriptEnvironment(scriptArgs));
            return Host;
        }
    }
}