namespace BddIdeas.CsxRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Common.Logging.Simple;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ScriptCs.Contracts;
    using ScriptCs.Engine.Roslyn;
    using ScriptCs.Hosting;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var console = new ScriptConsole();
            var logger = new NoOpLogger();

            var scriptcs = new ScriptServicesBuilder(console, logger)
                .Debug()
                .ScriptEngine<RoslynScriptInMemoryEngine>()
                .Build();

            var assemblies = new[]
            {
                typeof(Idea).Assembly,
                typeof(Assert).Assembly,
            };
            scriptcs.Executor.AddReferences(assemblies);
            scriptcs.Executor.Initialize(Enumerable.Empty<string>(), Enumerable.Empty<IScriptPack>());

            // HACK!
            var pathToSpecs = Path.GetFullPath(
                Path.Combine(
                    Environment.CurrentDirectory,
                    "..",
                    "..",
                    "..",
                    "Scripts")
                    );

            var results = Directory
                .GetFiles(pathToSpecs, "sample_spec.csx")
                .Select(spec => scriptcs.Executor.Execute(spec))
                .ToList();

            Console.Read();
        }
    }
}