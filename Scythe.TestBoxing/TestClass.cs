using System;

namespace Scythe.TestBoxing
{
    public class TestClass
    {
        public void InvalidMethod(int number)
        {
            for (var i = 0; i <= 100; i++)
            {
                var foo = (object) number;
                Console.WriteLine(foo);
            }
        }
    }
}
