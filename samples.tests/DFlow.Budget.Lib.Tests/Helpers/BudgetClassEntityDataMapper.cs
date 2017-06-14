using DFlow.Budget.Core.Model;

namespace DFlow.Budget.Lib.Tests.Helpers
{
    public class BudgetClassEntityDataMapper
    {
        public BudgetClass CreateEntityFrom(BudgetClassData data)
        {
	        var entity = new BudgetClass();

	        return UpdateEntityWith(entity, data);
        }

	    public BudgetClass UpdateEntityWith(BudgetClass entity, BudgetClassData data)
	    {
		    entity.Name = data.Name;
		    entity.TransactionType = data.TransactionType;

		    return entity;
	    }
    }
}
