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
        private StringBuilder _buf = new StringBuilder();

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
            var propertyName = memberExpression.Member.Name;
            return $"model.{propertyName}";
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
            string operatorString;
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
                case ExpressionType.Add:
                    operatorString = "+";
                    break;
                case ExpressionType.Multiply:
                    operatorString = "*";
                    break;
                case ExpressionType.Divide:
                    operatorString = "/";
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
            _buf.Append(JsonConvert.SerializeObject(value));

            return base.VisitConstant(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            throw new NotSupportedException();
        }
    }
}