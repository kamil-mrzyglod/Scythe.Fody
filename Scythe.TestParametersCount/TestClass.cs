namespace Scythe.TestParametersCount
{
    public class TestClass
    {
        public void ValidClass(int param)
        {
            var foo = param;
        }

        public void InvalidClass(int param1, int param2, int param3, int param4)
        {
            var foo = param1 + param2 + param3 + param4;
        }
    }
}