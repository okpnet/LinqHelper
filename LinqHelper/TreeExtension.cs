namespace LinqHelper
{
    /// <summary>
    /// An extension that converts enumerable interfaces into tree structures.
    /// </summary>
    public static class TreeExtension
    {
        /// <summary>
        /// Create to tree structure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="firstCondition"></param>
        /// <param name="isChild">Comparisons to consider as child items</param>
        /// <param name="setChildren"></param>
        /// <returns></returns>
        public static IEnumerable<T> CreateTree<T>(this IEnumerable<T> source, Func<T, bool> firstCondition, Func<T,Func<T, bool>> isChild, Action<T, IEnumerable<T>> setChildren)
        {
            foreach (var item in source.Where(firstCondition))
            {
                 var result=SetChildren(source,item,isChild,setChildren);
                if (result is null) continue;
                yield return result;
            }
        }
        /// <summary>
        /// to parent item add child items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="isChild">Comparisons to consider as child items</param>
        /// <param name="setChildren"></param>
        /// <returns></returns>
        public static T? SetChildren<T>(IEnumerable<T> source, T? item, Func<T, Func<T, bool>> isChild, Action<T, IEnumerable<T>> setChildren)
        {
            if(item is null)  return default;
            var children=FindChild(source,item, isChild, setChildren);
            if (children.Any())
            {
                setChildren(item, children);
            }
            return item;
        }
        /// <summary>
        /// Find child items based on parent items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="isChild">Comparisons to consider as child items</param>
        /// <param name="setChildren"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindChild<T>(IEnumerable<T> source, T item, Func<T, Func<T, bool>> isChild, Action<T, IEnumerable<T>> setChildren)
        {
            var func = isChild(item);
            var children= source.Where(func);
            if(!children.Any()) yield break;

            foreach (var l in source.Where(func))
            {
                var result= SetChildren(source,l,isChild,setChildren);
                if(result is null)yield break;
                yield return result;
            }
        }
    }
}
