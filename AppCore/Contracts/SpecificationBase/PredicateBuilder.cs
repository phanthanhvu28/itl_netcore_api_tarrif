using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Contracts.SpecificationBase
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Build<T>(string propertyName, string comparison, string value)
        {
            const string parameterName = "x";
            ParameterExpression parameter = Expression.Parameter(typeof(T), parameterName);
            Expression left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            Expression body = MakeComparison(left, comparison, value);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            ParameterExpression p = a.Parameters[0];

            SubstExpressionVisitor visitor = new();
            visitor.subst[b.Parameters[0]] = p;

            Expression body = Expression.And(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {
            ParameterExpression p = a.Parameters[0];

            SubstExpressionVisitor visitor = new();
            visitor.subst[b.Parameters[0]] = p;

            Expression body = Expression.Or(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        private static Expression MakeComparison(Expression left, string comparison, string value)
        {
            return comparison switch
            {
                "==" => MakeBinary(ExpressionType.Equal, left, value),
                "!=" => MakeBinary(ExpressionType.NotEqual, left, value),
                ">" => MakeBinary(ExpressionType.GreaterThan, left, value),
                ">=" => MakeBinary(ExpressionType.GreaterThanOrEqual, left, value),
                "<" => MakeBinary(ExpressionType.LessThan, left, value),
                "<=" => MakeBinary(ExpressionType.LessThanOrEqual, left, value),
                "Contains" or "StartsWith" or "EndsWith" => Expression.Call(MakeString(left), comparison,
                    Type.EmptyTypes, Expression.Constant(value, typeof(string))),
                "In" => MakeList(left, value.Split(',')),
                _ => throw new NotSupportedException($"Invalid comparison operator '{comparison}'.")
            };
        }

        private static Expression MakeList(Expression left, IEnumerable<string> codes)
        {
            List<object> objValues = codes.Cast<object>().ToList();
            Type type = typeof(List<object>);
            MethodInfo? methodInfo = type.GetMethod("Contains", new[] { typeof(object) });
            ConstantExpression list = Expression.Constant(objValues);
            MethodCallExpression body = Expression.Call(list, methodInfo, left);
            return body;
        }

        private static Expression MakeString(Expression source)
        {
            return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
        }

        private static Expression MakeBinary(ExpressionType type, Expression left, string value)
        {
            object typedValue = value;
            if (left.Type != typeof(string))
            {
                if (string.IsNullOrEmpty(value))
                {
                    typedValue = null;
                    if (Nullable.GetUnderlyingType(left.Type) == null)
                    {
                        left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
                    }
                }
                else
                {
                    Type valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                    typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                        valueType == typeof(Guid) ? Guid.Parse(value) :
                        Convert.ChangeType(value, valueType);
                }
            }

            ConstantExpression right = Expression.Constant(typedValue, left.Type);
            return Expression.MakeBinary(type, left, right);
        }
        private class SubstExpressionVisitor : ExpressionVisitor
        {
            public readonly Dictionary<Expression, Expression> subst = new();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return subst.TryGetValue(node, out Expression? newValue) ? newValue : node;
            }
        }
    }
}
