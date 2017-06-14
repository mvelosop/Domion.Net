using DFlow.Budget.Core.Model;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetClassDataMapper
    {
        public BudgetClassData CreateData(BudgetClass entity)
        {
            var data = new BudgetClassData
            {
                Name = entity.Name,

                TransactionType = entity.TransactionType,
            };

            return data;
        }

        public BudgetClass CreateEntity(BudgetClassData data)
        {
            var entity = new BudgetClass();

            return UpdateEntity(entity, data);
        }

        public BudgetClass UpdateEntity(BudgetClass entity, BudgetClassData data)
        {
            entity.Name = data.Name;
            entity.TransactionType = data.TransactionType;

            return entity;
        }
    }
}
