namespace Scythe.CyclomaticComplexityTest
{
    public class TestClass
    {
        public int ValidMethod(int foo)
        {
            if (foo == 1) return foo;
            if (foo == 2) return foo + 2;
            if (foo == 3) return foo + 3;
            return foo;
        }

        public int ValidMethod2(int foo)
        {
            if (foo == 1) foo += 1;
            if (foo == 2) foo += 2;
            if (foo == 3) foo += 3;
            return foo;
        }

        public int ValidMethod3(int foo)
        {
            if (foo == 1)
            {
                foo += 1;
            }
            else
            {
                foo += foo;
            }

            return foo;
        }

        public int ValidMethod4(int foo)
        {
            if (foo == 1)
            {
                foo += 1;
            }
            else if (foo == 2)
            {
                foo *= foo;
            }
            else
            {
                foo += foo;
            }

            return foo;
        }

        public int InvalidMethod(int foo)
        {
            if (foo == 1) return foo;
            if (foo == 2) return foo + 2;
            if (foo == 3) return foo + 3;
            if (foo == 4) return foo + 4;
            if (foo == 5) return foo + 5;
            return foo;
        }
    }
}