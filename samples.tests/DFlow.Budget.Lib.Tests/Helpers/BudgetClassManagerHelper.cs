using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Data;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Setup;
using Domion.Lib.Extensions;
using FluentAssertions;
using System;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetClassManagerHelper
    {
        private readonly BudgetClassEntityDataMapper _dataMapper;
        private readonly BudgetClassManager _classManager;
        private readonly BudgetDbSetupHelper _dbSetupHelper;

        public BudgetClassManagerHelper(
            BudgetClassManager classManager,
            BudgetDbSetupHelper dbSetupHelper)
        {
            _classManager = classManager;
            _dbSetupHelper = dbSetupHelper;

            _dataMapper = new BudgetClassEntityDataMapper();
        }

        private BudgetClassEntityDataMapper BudgetClassEntityDataMapper => _dataMapper;

        private BudgetClassManager BudgetClassManager => _classManager;

        private BudgetDbSetupHelper BudgetDbSetupHelper => _dbSetupHelper;

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

        public void AssertEntitiesExist(params BudgetClassData[] dataSet)
        {
            using (BudgetDbContext dbContext = BudgetDbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                foreach (BudgetClassData data in dataSet)
                {
                    BudgetClass entity = manager.SingleOrDefault(e => e.Name == data.Name);

                    entity.Should().NotBeNull(@"because BudgetClass ""{0}"" MUST EXIST!", data.Name);

                    var entityData = new BudgetClassData(entity);

                    entityData.ShouldBeEquivalentTo(data);
                }
            }
        }

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

        public void EnsureEntitiesExist(params BudgetClassData[] dataSet)
        {
            foreach (BudgetClassData data in dataSet)
            {
                BudgetClass entity = BudgetClassManager.SingleOrDefault(e => e.Name == data.Name);

                if (entity == null)
                {
                    entity = BudgetClassEntityDataMapper.CreateEntityFrom(data);
                }
                else
                {
                    entity = BudgetClassEntityDataMapper.UpdateEntityWith(entity, data);
                }

                var errors = BudgetClassManager.TryUpsert(entity);

                errors.Should().BeEmpty(@"because BudgetClass ""{0}"" has to be added!", data.Name);
            }

            BudgetClassManager.SaveChanges();

            AssertEntitiesExist(dataSet);
        }
    }
}
