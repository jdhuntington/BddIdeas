#r "Microsoft.VisualStudio.QualityTools.UnitTestFramework"
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BddIdeas.Core;

public static Idea idea = new Idea();

public static Action<object, Action> describe = idea.Describe;
public static Action<string, Action> it = idea.It;
public static Action<Action> before = idea.Before;
