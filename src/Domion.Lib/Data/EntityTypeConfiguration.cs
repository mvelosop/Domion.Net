using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domion.Lib.Data
{
    // As suggested in: https://github.com/aspnet/EntityFramework/issues/2805

    public abstract class EntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        public abstract void Map(EntityTypeBuilder<TEntity> builder);
    }
}
