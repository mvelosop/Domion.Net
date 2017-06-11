using DFlow.Budget.Lib.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DFlow.Budget.Setup
{
    public class BudgetDbSetupHelper
    {
        private string _connectionString;
        private DbContextOptions<BudgetDbContext> _options;

        public BudgetDbSetupHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Returns the DbContext if the database has been set up.
        /// </summary>
        /// <returns></returns>
        public BudgetDbContext GetDbContext()
        {
            if (_options == null) throw new InvalidOperationException($"Must run {nameof(BudgetDbSetupHelper)}.{nameof(SetupDatabase)} first!");

            return new BudgetDbContext(_options);
        }

        /// <summary>
        /// Creates the database and applies pending migrations.
        /// </summary>
        public void SetupDatabase()
        {
            var optionBuilder = new DbContextOptionsBuilder<BudgetDbContext>();

            optionBuilder.UseSqlServer(_connectionString);

            _options = optionBuilder.Options;

            using (var dbContext = GetDbContext())
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
