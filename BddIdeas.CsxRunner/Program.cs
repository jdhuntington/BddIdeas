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
            var factory = new SpecScriptHostFactory();

            var console = new ScriptConsole();
            var logger = new NoOpLogger();

            var builder = new ScriptServicesBuilder(console, logger)
                .Debug()
                .ScriptEngine<RoslynScriptInMemoryEngine>();

            //HACK! this will be better with the next scriptcs release
            var overrides = ((ScriptServicesBuilder)builder).Overrides;
            overrides[typeof(IScriptHostFactory)] = factory;

            var scriptcs = builder.Build();

            var assemblies = new[]
            {
                typeof(Idea).Assembly,
                typeof(Assert).Assembly,
            };
            scriptcs.Executor.AddReferences(assemblies);
            // for the ambient `Assert`
            scriptcs.Executor.ImportNamespaces("Microsoft.VisualStudio.TestTools.UnitTesting");
            scriptcs.Executor.Initialize(Enumerable.Empty<string>(), Enumerable.Empty<IScriptPack>());

            // HACK! hardcoded to look for specs
            var pathToSpecs = Path.GetFullPath(
                Path.Combine(
                    Environment.CurrentDirectory,
                    "..",
                    "..",
                    "..",
                    "Specs")
                    );

            var results = Directory
                .GetFiles(pathToSpecs, "*.csx")
                .Select(spec => new
                {
                    Result = scriptcs.Executor.Execute(spec),
                    File = spec
                })
                .ToList();

            results
                .Where(x => x.Result.CompileExceptionInfo != null)
                .ToList()
                .ForEach(x =>
                {
                    Console.WriteLine("Unable to compile spec:");
                    Console.WriteLine(x.File);
                    Console.WriteLine(x.Result.CompileExceptionInfo.SourceException.Message);
                });

            var context = factory.Host.GetRootContext();

            var failures = new List<Spec>();
            NestedRunner(failures, context);
            if (failures.Count > 0)
            {
                Console.WriteLine("\nWe've got some things we need to talk about.\n");
                foreach (var failure in failures)
                {
                    Console.WriteLine("{0}\n  {1}\n", failure.FullDescription, failure.Error.Message);
                }
            }

            Console.Read();
        }

        private static void NestedRunner(List<Spec> failures, IdeaContext context, int nest = 0)
        {
            Console.WriteLine("{0}{1}", new String(' ', nest * 2), context.Description);
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
                Console.WriteLine("{0}{1}", new String(' ', (nest + 1) * 2), spec.Description);
                Console.ResetColor();
            }
            foreach (var ideaContext in context.Children)
            {
                NestedRunner(failures, ideaContext, nest + 1);
            }
        }
    }
}