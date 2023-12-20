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

        public static IEnumerable<T> CreateTree<T>(this IEnumerable<T> source,Func<T?,bool> firstCondition, Func<T,T, bool> isChild, Action<T, IEnumerable<T>> setChildren)
        {
            foreach (var item in source.Where(firstCondition))
            {
                yield return item;
                var children=ChildrenCreate(source,item, isChild, setChildren);
                setChildren(item, children);
             }
        }

        public static IEnumerable<T> ChildrenCreate<T>(IEnumerable<T> source,T? item ,Func<T, bool> isChild, Action<T, IEnumerable<T>> setChildren)
        {
            if(item is null)yield break;
            yield return item;
            
            var children = source.Where(isChild);
            setChildren(item, children);
            
            foreach(var l in children)
            {
                yield return l;
                var lChildren=ChildrenCreate(source,l,isChild,setChildren);
                setChildren(l, lChildren);
            }
        }

        public static IEnumerable<T> IsChildren<T>(IEnumerable<T> source,T item,Func<T,T,bool> isChild)
        {
            source.Where(t=>t.Eq)
            foreach(var item in source.Where(t=>isChild(t,)
            {
                
            }
        }
    }
}
