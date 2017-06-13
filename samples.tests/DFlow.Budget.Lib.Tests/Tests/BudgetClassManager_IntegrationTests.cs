using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Data;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Lib.Tests.Helpers;
using DFlow.Budget.Setup;
using Domion.Lib.Extensions;
using FluentAssertions;
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

        private static BudgetDbSetupHelper dbSetupHelper;

        static BudgetClassManager_IntegrationTests()
        {
            dbSetupHelper = SetupDatabase(ConnectionString);
        }

        [Fact]
        public void TryInsert_InsertsRecord_WhenValidData()
        {
            IEnumerable<ValidationResult> errors;

            // Arrange ---------------------------

            var data = new BudgetClassData("Insert-Success-Valid - Inserted", TransactionType.Income);

            // Ensure entitiy does not exist
            using (var dbContext = dbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                if (entity != null)
                {
                    errors = manager.TryDelete(entity);

                    errors.Should().BeEmpty();

                    manager.SaveChanges();
                }
            }

            // Act -------------------------------

            // Insert entity
            using (var dbContext = dbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                BudgetClass entity = new BudgetClass { Name = data.Name, TransactionType = data.TransactionType };

                errors = manager.TryInsert(entity);

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            // Verify entity exists
            using (var dbContext = dbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                entity.Should().NotBeNull();
            }
        }

        [Fact]
        public void TryUpdate_UpdatesRecord_WhenValidData()
        {
            IEnumerable<ValidationResult> errors;

            // Arrange ---------------------------

            var data = new BudgetClassData("Update-Success-Valid - Inserted", TransactionType.Income);
            var update = new BudgetClassData("Update-Success-Valid - Updated", TransactionType.Income);

            // Ensure entitiy "data" exists and "update" does not exist
            using (var dbContext = dbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var existing = manager.SingleOrDefault(bc => bc.Name == data.Name);

                if (existing == null)
                {
                    existing = new BudgetClass { Name = data.Name, TransactionType = data.TransactionType };

                    errors = manager.TryInsert(existing);

                    errors.Should().BeEmpty();
                }

                var updated = manager.SingleOrDefault(bc => bc.Name == data.Name);

                if (updated != null)
                {
                    errors = manager.TryDelete(updated);

                    errors.Should().BeEmpty();
                }

                manager.SaveChanges();
            }

            // Act -------------------------------

            using (var dbContext = dbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var entity = manager.SingleOrDefault(bc => bc.Name == data.Name);

                entity.Name = update.Name;

                errors = manager.TryUpdate(entity).ToList();

                manager.SaveChanges();
            }

            // Assert ----------------------------

            errors.Should().BeEmpty();

            using (var dbContext = dbSetupHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                var existing = manager.SingleOrDefault(bc => bc.Name == update.Name);

                existing.Should().NotBeNull();
            }
        }

        private static BudgetDbSetupHelper SetupDatabase(string connectionString)
        {
            BudgetDbSetupHelper dbHelper = new BudgetDbSetupHelper(connectionString);

            dbHelper.SetupDatabase();

            return dbHelper;
        }
    }
}
