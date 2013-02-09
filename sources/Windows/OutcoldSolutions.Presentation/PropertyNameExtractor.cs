// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions.Presentation
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class PropertyNameExtractor
    {
        public static string GetPropertyName(Expression<Func<object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            MemberExpression memberExpression;

            var body = expression.Body as UnaryExpression;
            if (body != null)
            {
                memberExpression = body.Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }

            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression", "expression");
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException("The member access expression does not access a property", "expression");
            }

            var getMethod = property.GetMethod;
            if (getMethod.IsStatic)
            {
                throw new ArgumentException("The referenced property is a static property", "expression");
            }

            return memberExpression.Member.Name;
        }
    }
}