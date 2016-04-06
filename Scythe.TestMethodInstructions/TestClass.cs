namespace Scythe.TestMethodInstructions
{
    using System.Collections.Generic;
    using System.Linq;

    public class TestClass
    {
        public void ValidMethod()
        {
            var foo = "bar";
        }

        public void InvalidMethod()
        {
            var foo = new List<int>();
            for (var i = 0; i < 11; i++)
            {
                foo.Add(i);
            }
        }
    }
}