// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The inject attribute. Use it if you class has more than one constructor to 
    /// mark which constructor should be used for injection.
    /// </summary>
    /// <example>
    /// <code>
    /// class ClassA
    /// {
    ///     [Inject]
    ///     public ClassA(ClassB a)
    ///     {
    ///     }
    /// 
    ///     public ClassA(ClassB a, string str)
    ///     {
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class InjectAttribute : Attribute
    {
    }
}