// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.com)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    /// <summary>
    /// The inject attribute. Use it if you class has more than one constructor to 
    /// mark which constructor should be used for injection. 
    /// Also this attribute can be used to mark methods which should be invokes with injected values
    /// after object will be created.
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
    /// Injection for methods:
    /// <code>
    /// class ClassA
    /// {
    ///     public ClassA(ClassB a)
    ///     {
    ///     }
    /// 
    ///     [Inject]
    ///     public void MethodA(ClassB a, string str)
    ///     {
    ///     }
    /// }
    /// </code>
    /// In second example first IoC will inject constructor and after this it will inject method MethodA.
    /// </example>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class InjectAttribute : Attribute
    {
    }
}