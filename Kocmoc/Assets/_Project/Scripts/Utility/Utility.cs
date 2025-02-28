namespace Kocmoc
{
    public static class Utility
    {
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
