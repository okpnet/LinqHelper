namespace LinqHelper
{
    public static class LinqExtension
    {
        /// <summary>
        /// Convert nullable to non-nullable 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source)
        {
            if (source == null) return Enumerable.Empty<T>();
            return source.Where(x => x != null)!;
        }
    }
}
