namespace Remy.Extra
{
    public static class Array
    {
        public static int FindIndex(string[] array, int startIndex, Predicate<string> match)
        {
            for (int i = startIndex; i < array.Length; i++)
            {
                string x = array[i];

                if (match(x)) return i;
            }

            return -1;
        }
    }
}