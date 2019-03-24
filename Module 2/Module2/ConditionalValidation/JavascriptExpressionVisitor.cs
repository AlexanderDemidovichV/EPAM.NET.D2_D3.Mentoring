using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace ConditionalValidation
{
    public class JavascriptExpressionVisitor : ExpressionVisitor
    {
        private StringBuilder _buf;

        public string Translate(Expression expression)
        {
            _buf = new StringBuilder();
            Visit(expression);
            return _buf.ToString().Trim();
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var expression = ConvertMemberExpression(node);
            _buf.Append(expression);
            return node;
        }

        private string ConvertMemberExpression(MemberExpression memberExpression)
        {
            if (memberExpression.Expression == null)
            {
                // only support DateTime
                if (memberExpression.Member.DeclaringType != typeof(DateTime)
                    && memberExpression.Member.MemberType != MemberTypes.Property
                    )
                {
                    throw new ArgumentException();
                }
                var value = GetDatePropertyValue(memberExpression);
                var serialisedValue = SerializeDate(value);
                return serialisedValue;
            }
            var propertyName = memberExpression.Member.Name;
            return $"gv('*.{propertyName}')";
        }

        private DateTime GetDatePropertyValue(MemberExpression memberExpression)
        {
            var propertyInfo = (PropertyInfo)memberExpression.Member;
            var value = (DateTime)propertyInfo.GetValue(null, null);
            return value;
        }


        private string SerializeDate(DateTime value)
        {
            return $"new Date({value.Year},{value.Month - 1},{value.Day},{value.Hour},{value.Minute},{value.Second})";
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType != ExpressionType.Not) 
                throw new NotSupportedException();

            _buf.Append("!");
            return base.VisitUnary(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            string operatorString = null;
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    operatorString = "&&";
                    break;
                case ExpressionType.OrElse:
                    operatorString = "||";
                    break;
                case ExpressionType.And:
                    operatorString = "&";
                    break;
                case ExpressionType.Or:
                    operatorString = "|";
                    break;
                case ExpressionType.LessThan:
                    operatorString = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    operatorString = "<=";
                    break;
                case ExpressionType.GreaterThan:
                    operatorString = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    operatorString = ">=";
                    break;
                case ExpressionType.Equal:
                    operatorString = "==";
                    break;
                case ExpressionType.NotEqual:
                    operatorString = "!=";
                    break;
                default:
                    throw new NotSupportedException();
            }

            Visit(node.Left);
            _buf.Append(" ");
            _buf.Append(operatorString);
            _buf.Append(" ");
            Visit(node.Right);

            return node;
        }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            var value = node.Value;
            if (value is DateTime time)
                _buf.Append(SerializeDate(time));
            else
                _buf.Append(JsonConvert.SerializeObject(value));

            return base.VisitConstant(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            if (node.Constructor.DeclaringType != typeof(DateTime)) 
                throw new NotSupportedException();

            var args = GetConstArgumentValues(node.Arguments);
            var dateTime = (DateTime)node.Constructor.Invoke(args);
            _buf.Append(SerializeDate(dateTime));
            return node;
        }

        private object[] GetConstArgumentValues(
            IReadOnlyList<Expression> argumentExpressions)
        {
            var args = new object[argumentExpressions.Count];
            for (var i = 0; i < argumentExpressions.Count; i++)
            {
                var expression = argumentExpressions[i];
                args[i] = GetConstArgumentValue(expression);
            }
            return args;
        }

        private object GetConstArgumentValue(Expression expression)
        {
            if (expression.NodeType != ExpressionType.Constant)
            {
                throw new ArgumentException("expression must be a constant expression");
            }
            var constantExpression = (ConstantExpression)expression;
            return constantExpression.Value;
        }
    }
}