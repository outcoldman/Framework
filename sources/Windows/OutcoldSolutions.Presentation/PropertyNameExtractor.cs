// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------
namespace OutcoldSolutions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// The property name extractor
    /// </summary>
    public class PropertyNameExtractor
    {
        /// <summary>
        /// Get the property name from expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>Property name.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="expression"/> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="expression"/> does not contain property.</exception>
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