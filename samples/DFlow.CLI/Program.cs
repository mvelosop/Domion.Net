using DFlow.Budget.Core.Model;
using DFlow.Budget.Lib.Services;
using DFlow.Budget.Setup;
using Domion.Lib.Extensions;
using System;
using System.Linq;
using System.Text;

namespace DFlow.CLI
{
    internal class Program
    {
        private static BudgetClass[] _dataSet = new BudgetClass[]
        {
            new BudgetClass { Name = "Income", Order = 1, TransactionType = TransactionType.Income },
            new BudgetClass { Name = "Expenses", Order = 2, TransactionType = TransactionType.Expense },
            new BudgetClass { Name = "Investments", Order = 3, TransactionType = TransactionType.Investment },
        };

        private static BudgetDbSetupHelper _dbHelper;

        private static void LoadSeedData()
        {
            Console.WriteLine("Seeding data...\n");

            using (var dbContext = _dbHelper.GetDbContext())
            {
                var manager = new BudgetClassManager(dbContext);

                foreach (var item in _dataSet)
                {
                    var entity = manager.SingleOrDefault(bc => bc.Name.StartsWith(item.Name));

                    if (entity == null)
                    {
                        manager.TryInsert(item);
                    }
                    else
                    {
                        var tokens = entity.Name.Split('-');

                        if (tokens.Length == 1)
                        {
                            entity.Name += " - 1";
                        }
                        else
                        {
                            entity.Name = tokens[0].Trim() + $" - {int.Parse(tokens[1]) + 1}";
                        }
                    }
                }

                manager.SaveChanges();
            }
        }

        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("EF Core App\n");

            SetupDb();

            LoadSeedData();

            PrintDb();

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }

        private static void PrintDb()
        {
            using (var dbContext = _dbHelper.GetDbContext())
            {
                Console.WriteLine("Printing data...\n");

                Console.WriteLine("Budget Classes");
                Console.WriteLine("--------------");

                int nameLength = _dataSet.Select(c => c.Name.Length).Max() + 5;
                int typeLength = _dataSet.Select(c => c.TransactionType.ToString().Length).Max();

                foreach (var item in dbContext.BudgetClasses)
                {
                    Console.WriteLine($"| {item.Name.PadRight(nameLength)} | {item.Order} | {item.TransactionType.ToString().PadRight(typeLength)} |");
                }

                Console.WriteLine();
            }
        }

        private static void SetupDb()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=DFlow.CLI;Integrated Security=SSPI;MultipleActiveResultSets=true";

            Console.WriteLine($"Setting up database\n ({connectionString})...\n");

            _dbHelper = new BudgetDbSetupHelper(connectionString);

            _dbHelper.SetupDatabase();
        }
    }
}
