using System;

namespace Scythe.TestBoxing
{
    public class TestClass
    {
        public void InvalidMethod(int number)
        {
            var bar = (object)number;
            Console.WriteLine(bar);

            for (var i = 0; i <= 100; i++)
            {
                var foo = (object) number;
                Console.WriteLine(foo);
            }
        }

        public void ValidMethod(int number)
        {
            var foo = (object)number;
            Console.WriteLine(foo);
        }

        public void ValidMethod2(int number)
        {
            for (var i = 0; i <= 100; i++)
            {
                Console.WriteLine(number);
            }
        }
    }
}
