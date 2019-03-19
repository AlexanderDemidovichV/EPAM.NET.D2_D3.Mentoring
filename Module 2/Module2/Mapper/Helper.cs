using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Mapper
{
    public static class LambdaExtensions
    {
        public static void SetPropertyValue<T, TValue>(this T target,
            Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }
    }

    static class Helper
    {
        public static MemberInfo GetFieldOrProperty(this Type type, string name)
            => type.GetInheritedMember(name) ?? throw new ArgumentOutOfRangeException(nameof(name), $"Cannot find member {name} of type {type}.");

        public static MemberInfo GetInheritedMember(this Type type, string name) => type.GetAllMembers().FirstOrDefault(mi => mi.Name == name);

        private static IEnumerable<MemberInfo> GetAllMembers(this Type type) =>
            type.GetTypeInheritance().Concat(type.GetTypeInfo().ImplementedInterfaces).SelectMany(i => i.GetTypeInfo().DeclaredMembers);

        public static IEnumerable<Type> GetTypeInheritance(this Type type)
        {
            yield return type;

            var baseType = type.BaseType();
            while (baseType != null)
            {
                yield return baseType;
                baseType = baseType.BaseType();
            }
        }

        public static Type BaseType(this Type type) => type.GetTypeInfo().BaseType;
    }


}
