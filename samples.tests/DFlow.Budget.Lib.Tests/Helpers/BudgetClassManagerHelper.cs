using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Data;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Setup;
using Domion.Lib.Extensions;
using FluentAssertions;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetClassManagerHelper
    {
        /// <summary>
        ///     Creates a Helper for BudgetClassManager to help in the test's Arrange and Assert sections
        /// </summary>
        public BudgetClassManagerHelper(
            BudgetClassManager classBudgetClassManager,
            BudgetDbSetupHelper budgetDbSetupHelper)
        {
            BudgetClassManager = classBudgetClassManager;
            BudgetDbSetupHelper = budgetDbSetupHelper;

            BudgetClassMapper = new BudgetClassDataMapper();
        }

        private BudgetClassManager BudgetClassManager { get; }

        private BudgetClassDataMapper BudgetClassMapper { get; }

        private BudgetDbSetupHelper BudgetDbSetupHelper { get; }

        /// <summary>
        ///     Asserts that entities with the supplied key data values do not exist
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for</param>
        public void AssertEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            using (BudgetDbContext dbContext = BudgetDbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = manager.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().BeNull(@"because BudgetClass ""{0}"" MUST NOT EXIST!", data.Name);
                }
            }
        }

        /// <summary>
        ///     Asserts that entities equivalent to the supplied input data classes exist
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for</param>
        public void AssertEntitiesExist(params BudgetClassData[] dataSet)
        {
            using (BudgetDbContext dbContext = BudgetDbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);
                var mapper = new BudgetClassDataMapper();

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = manager.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().NotBeNull(@"because BudgetClass ""{0}"" MUST EXIST!", data.Name);

                    BudgetClassData entityData = mapper.CreateData(entity);

                    entityData.ShouldBeEquivalentTo(data);
                }
            }
        }

        /// <summary>
        ///     Ensures that the entities do not exist in the database or are succesfully removed
        /// </summary>
        /// <param name="dataSet">Data for the entities to be searched for and removed if necessary</param>
        public void EnsureEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = BudgetClassManager.SingleOrDefault(e => e.Name == data.Name);

                if (entity == null) continue;

                var errors = BudgetClassManager.TryDelete(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be removed!", data.Name);
            }

            BudgetClassManager.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        /// <summary>
        ///     Ensures that the entities exist in the database or are succesfully added
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="dataSet">Data for the entities to be searched for and added or updated if necessary</param>
        public void EnsureEntitiesExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = BudgetClassManager.SingleOrDefault(e => e.Name == data.Name);

                entity = entity == null ? BudgetClassMapper.CreateEntity(data) : BudgetClassMapper.UpdateEntity(entity, data);

                var errors = BudgetClassManager.TryUpsert(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be added!", data.Name);
            }

            BudgetClassManager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }
    }
}
