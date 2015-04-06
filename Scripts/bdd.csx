using Microsoft.VisualStudio.TestTools.UnitTesting;
using BddIdeas.Core;

public static Idea idea = new Idea();

public static Action<object, Action> Describe = idea.Describe;
public static Action<string, Action> It = idea.It;
public static Action<Action> Before = idea.Before;
