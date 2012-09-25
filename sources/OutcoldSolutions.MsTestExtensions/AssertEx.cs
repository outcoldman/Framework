// --------------------------------------------------------------------------------------------------------------------
// Outcold Solutions (http://outcoldman.ru)
// --------------------------------------------------------------------------------------------------------------------

namespace OutcoldSolutions
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class AssertEx
    {
        public static void Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
                Assert.Fail("Action should throw exception with type '{0}'.", typeof(TException));
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception e)
            {
                if (!(e is TException))
                {
                    Assert.Fail(
                        "Action should throw exception with type '{0}', but it throws '{1}' instead.",
                        typeof(TException),
                        e.GetType());
                }
            }
        }
    }
}
