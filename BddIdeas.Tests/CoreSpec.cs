﻿using System;
using System.Configuration;
using System.Diagnostics;
using BddIdeas.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddIdeas.Tests
{
	public class CoreSpec
	{
		public CoreSpec(Idea idea)
		{
			var describe = idea.Describe;
			var it = idea.It;
			var before = idea.Before;

			describe(typeof (Idea), () =>
			{
				var a = 0;
				before(() => a = 10);

				it("executes it blocks", () =>
				{
					Assert.AreEqual(0, 0);
				});

				it("gracefully handles exceptions", () =>
				{
					throw new Exception("This was completely intentional.");
				});

				describe("nested contexts", () =>
				{
					before(() => a = 1);

					it("handles them", () =>
					{
						Assert.IsTrue(true);
					});

					it("handles before blocks correctly", () =>
					{
						Assert.AreEqual(a, 1);
					});
				});

				describe("intentional failures", () =>
				{
					it("does what testing frameworks do", () =>
					{
						Assert.AreEqual(4, 5);
					});
				});

				describe("other neat features", () =>
				{
					idea.Pending("specs are allowed.", () =>
					{
						Assert.Fail("This won't be executed");
					});
				});
			});
		}
	}
}