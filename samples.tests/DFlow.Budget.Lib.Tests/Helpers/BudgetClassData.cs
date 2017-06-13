using DFlow.Budget.Core.Model;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetClassData
    {
        public BudgetClassData()
        {
        }

        public BudgetClassData(BudgetClass entity)
        {
            Name = entity.Name;

            TransactionType = entity.TransactionType;
        }

        public BudgetClassData(string name, TransactionType transactionType)
        {
            Name = name;

            TransactionType = transactionType;
        }

        public string Name { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}
