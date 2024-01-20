using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace LinqHelper
{
    public static class LambdaExtension
    {
        /// <summary>
        /// ラムダ式からパス取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public static string GetPropertyPath<T>(Expression<Func<T, object>> member) where T : class
        {
            Expression? expression = null;
            if (member.Body is MemberExpression)
            {
                expression = (MemberExpression)member.Body;
            }
            else if (member.Body is UnaryExpression)
            {
                var mb = (UnaryExpression)member.Body;
                expression = mb.Operand;
            }
            if (expression == null) return "";

            var list = ExpressionRecursive(expression);

            var strBuffers = list.Reverse().Select(a => a.Member.Name).ToArray();
            return string.Join(".", strBuffers);
        }
        /// <summary>
        /// プロパティパスの文字列からPropertyInfoを取得する
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        public static PropertyInfo? GetPropertyFromPath(this Type type,string propertyPath)
        {
            foreach (var propName in propertyPath.Split('.'))
            {
                var info = type.GetProperty(propName);
                if(info is not null)
                {
                    return info;
                }
            }
            return null;
        }
        /// <summary>
        /// プロパティパスの文字列からtargetへvalueをセットする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="propertyPath"></param>
        /// <param name="value"></param>
        /// <exception cref="Exception">値の型が違うとき</exception>
        public static void SetPropertyValueFromPath<T, TValue>(this T target,string propertyPath,TValue value) where T:class
        {
            var property=GetPropertyFromPath(target.GetType(),propertyPath);
            if(property is null || property.SetMethod is null)return;
            try
            {
                property.SetValue(target, value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Different value '{typeof(TValue).Name}' type. ", ex);
            }
        }
        /// <summary>
        /// プロパティパスのラムダ式からtargetへvalueをセットする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="member"></param>
        /// <param name="value"></param>
        /// <exception cref="Exception">値の型が違うとき</exception>
        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> member, TValue value) 
        {
            var memberSelectorExpression = member.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                if (memberSelectorExpression.Member is not PropertyInfo property || property.SetMethod is null) return;
                try
                {
                    property.SetValue(target, value);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Different value '{typeof(TValue).Name}' type. ", ex);
                }
            }
        }
        /// <summary>
        /// タイプとパスからフィールド式を取得
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        public static Expression GetExpressionFieldFromType(this Type type, string propertyPath)
        {
            //var param = Expression.Parameter(type,"_arg_"+type.Name);
            Expression body = Expression.Parameter(type);
            foreach (var member in propertyPath.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            };
            return body;
        }

        /// <summary>
        /// インスタンスとパスから、子プロパティを含めて検索してオブジェクトを取り出す
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyPath"></param>
        /// <returns></returns>
        public static object? GetPropertyValueFromPath<T>(this T target, string propertyPath) where T : class
        {
            object? value = target;
            foreach (var propName in propertyPath.Split('.'))
            {
                if (value is null) return null;
#if DEBUG
                var properties = value.GetType().GetProperties();
#endif
                var info = value.GetType().GetProperty(propName);
                value = info?.GetValue(value, null);
            }
            return value;
        }
        /// <summary>
        /// メンバー式取得の再帰
        /// </summary>
        /// <param name="expresion"></param>
        /// <returns></returns>
        public static IEnumerable<MemberExpression> ExpressionRecursive(Expression expresion)
        {
            if (expresion is MemberExpression member && member != null)
            {
                if (member.Expression == null) yield break;
                yield return member;
                var list = ExpressionRecursive(member.Expression);
                foreach (var item in list)
                    yield return item;
            }
            yield break;
        }
    }
}
