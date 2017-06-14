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
        public BudgetClassManagerHelper(
            BudgetClassManager classManager,
            BudgetDbSetupHelper dbSetupHelper)
        {
            Manager = classManager;
            DbSetupHelper = dbSetupHelper;

            Mapper = new BudgetClassDataMapper();
        }

        private BudgetClassDataMapper Mapper { get; }

        private BudgetClassManager Manager { get; }

        private BudgetDbSetupHelper DbSetupHelper { get; }

        public void AssertEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            using (BudgetDbContext dbContext = DbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = manager.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().BeNull(@"because BudgetClass ""{0}"" MUST NOT EXIST!", data.Name);
                }
            }
        }

        public void AssertEntitiesExist(params BudgetClassData[] dataSet)
        {
            using (BudgetDbContext dbContext = DbSetupHelper.GetDbContext())
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

        public void EnsureEntitiesDoNotExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = Manager.SingleOrDefault(e => e.Name == data.Name);

                if (entity == null) continue;

                var errors = Manager.TryDelete(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be removed!", data.Name);
            }

            Manager.SaveChanges();

            AssertEntitiesDoNotExist(dataSet);
        }

        public void EnsureEntitiesExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = Manager.SingleOrDefault(e => e.Name == data.Name);

                entity = entity == null ? Mapper.CreateEntity(data) : Mapper.UpdateEntity(entity, data);

                var errors = Manager.TryUpsert(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be added!", data.Name);
            }

            Manager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }
    }
}
