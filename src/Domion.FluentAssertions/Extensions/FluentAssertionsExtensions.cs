using FluentAssertions.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domion.FluentAssertions.Extensions
{
    public static class FluentAssertionsExtensions
    {
        /// <summary>
        ///     Asserts that the ValidationError collection contains a message that starts with the constant part of errorMessage,
        ///     i.e. up to the first substitution placeholder ("{.*}"), if any.
        /// </summary>
        /// <param name="assertion"></param>
        /// <param name="errorMessage">Error message text, will be trimmed up to the first substitution placeholder ("{.*}").</param>
        /// <param name="because">The reason why the predicate should be satisfied.</param>
        /// <param name="becauseArgs">The parameters used when formatting the reason message.</param>
        public static void ContainErrorMessage(this GenericCollectionAssertions<ValidationResult> assertion, string errorMessage, string because = "", params object[] becauseArgs)
        {
            var errorMessageStart = errorMessage.Split('{')[0];

            assertion.Match(c => c.Any(vr => vr.ErrorMessage.StartsWith(errorMessageStart)), because, becauseArgs);
        }
    }
}
