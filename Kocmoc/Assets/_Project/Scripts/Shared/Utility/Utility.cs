
namespace Kocmoc
{
    public static class Utility
    {
        public static string ArrayToString<T>(T[] array)
        {
            string arrString = "(";
            foreach (T item in array)
                arrString += item.ToString() + ", ";
            arrString = arrString.Substring(0, arrString.Length - 2);
            arrString += ')';
            return arrString;
        }

        public static int TrailingZeroCount(int n)
        {
            int count = 0;
            while ((n & 1) == 0)
            {
                count++;
                n >>= 1;
            }
            return count;
        }

    }
}
