using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using Domion.Lib.Extensions;
using FluentAssertions;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public static class BudgetClassManagerExtensions
    {
        /// <summary>
        /// Gets the entity by key data value or fails the assertion.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="name">Key data value</param>
        /// <returns>The entity</returns>
        public static BudgetClass AssertGetByKeyData(this BudgetClassManager manager, string name)
        {
            BudgetClass entity = manager.SingleOrDefault(c => c.Name == name);

            entity.Should().NotBeNull(@"because BudgetClass ""{0}"" MUST EXIST!", name);

            return entity;
        }
    }
}
