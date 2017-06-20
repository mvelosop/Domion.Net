using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Data;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using Domion.FluentAssertions.Extensions;
using Domion.Lib.Extensions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace DFlow.Budget.Lib.Tests.Tests
{
    [Trait("Type", "Integration")]
    public class BudgetClassManager_IntegrationTests
    {
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=DFlow.Budget.Lib.Tests;Integrated Security=SSPI;MultipleActiveResultSets=true";

        private static readonly BudgetDbSetupHelper DbSetupHelper;
        private static readonly Lazy<BudgetClassDataMapper> LazyBudgetClassEntityHelper;

        static BudgetClassManager_IntegrationTests()
        {
            DbSetupHelper = SetupDatabase(ConnectionString);

            LazyBudgetClassEntityHelper = new Lazy<BudgetClassDataMapper>(() => new BudgetClassDataMapper());
        }

        public BudgetClassDataMapper Mapper => LazyBudgetClassEntityHelper.Value;

        [Fact]
        public void TryDelete_DeletesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Delete-Success-Valid - Inserted", TransactionType.Income);

            UsingManagerHelper(helper =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(manager =>
            {
                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                errors = manager.TryDelete(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper(helper =>
            {
                helper.AssertEntitiesDoNotExist(data);
            });
        }

        [Fact]
        public void TryInsert_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Insert-Error-Duplicate - Inserted", TransactionType.Income);

            UsingManagerHelper(helper =>
            {
                helper.EnsureEntitiesExist(data);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(manager =>
            {
                BudgetClass entity = Mapper.CreateEntity(data);

                errors = manager.TryInsert(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            IEnumerable<ValidationResult> errors = null;

            // Arrange ---------------------------

            var data = new BudgetClassData("Insert-Success-Valid - Inserted", TransactionType.Income);

            UsingManagerHelper(helper =>
            {
                helper.EnsureEntitiesDoNotExist(data);
            });

            // Act -------------------------------

            UsingManager(manager =>
            {
                BudgetClass entity = Mapper.CreateEntity(data);

                errors = manager.TryInsert(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper(helper =>
            {
                helper.AssertEntitiesExist(data);
            });
        }

        [Fact]
        public void TryUpdate_Fails_WhenDuplicateKeyData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update-Error-Duplicate - Inserted first", TransactionType.Income);
            var update = new BudgetClassData("Update-Error-Duplicate - Inserted second", TransactionType.Income);

            UsingManagerHelper(helper =>
            {
                helper.EnsureEntitiesExist(data, update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(manager =>
            {
                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                entity = Mapper.UpdateEntity(entity, update);

                errors = manager.TryUpdate(entity).ToList();
            });

            // Assert ----------------------------

            errors.Should().ContainErrorMessage(BudgetClassManager.duplicateByNameError);
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            // Arrange ---------------------------

            var data = new BudgetClassData("Update-Success-Valid - Inserted", TransactionType.Income);
            var update = new BudgetClassData("Update-Success-Valid - Updated", TransactionType.Income);

            UsingManagerHelper(helper =>
            {
                helper.EnsureEntitiesExist(data);
                helper.EnsureEntitiesDoNotExist(update);
            });

            // Act -------------------------------

            IEnumerable<ValidationResult> errors = null;

            UsingManager(manager =>
            {
                BudgetClass entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                entity = Mapper.UpdateEntity(entity, update);

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            });

            // Assert ----------------------------

            errors.Should().BeEmpty();

            UsingManagerHelper(helper =>
            {
                helper.AssertEntitiesExist(update);
            });
        }

        private static BudgetDbSetupHelper SetupDatabase(string connectionString)
        {
            var dbHelper = new BudgetDbSetupHelper(connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }

        private void UsingManager(Action<BudgetClassManager> action)
        {
            using (BudgetDbContext dbContext = DbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                action.Invoke(manager);
            }
        }

        private void UsingManagerHelper(Action<BudgetClassManagerHelper> action)
        {
            using (BudgetDbContext dbContext = DbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);
                var helper = new BudgetClassManagerHelper(manager, DbSetupHelper);

                action.Invoke(helper);
            }
        }
    }
}
