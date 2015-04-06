#load "bdd.csx"

Describe(typeof (Idea), () =>
{
      var a = 0;
      Before(() => a = 10);

      It("executes it blocks", () =>
      {
          Assert.AreEqual(0, 0);
      });

      It("gracefully handles exceptions", () =>
      {
          throw new Exception("This was completely intentional.");
      });

      Describe("nested contexts", () =>
      {
          Before(() => a = 1);

          It("handles them", () =>
          {
              Assert.IsTrue(true);
          });

          It("handles before blocks correctly", () =>
          {
              Assert.AreEqual(a, 1);
          });
      });

      Describe("intentional failures", () =>
      {
          It("does what testing frameworks do", () =>
          {
              Assert.AreEqual(4, 5);
          });
      });

      Describe("other neat features", () =>
      {
          idea.Pending("specs are allowed.", () =>
          {
              Assert.Fail("This won't be executed");
          });
      });
});

return idea;
