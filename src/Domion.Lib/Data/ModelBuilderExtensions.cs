using Microsoft.EntityFrameworkCore;

namespace Domion.Lib.Data
{
    // As suggested in: https://github.com/aspnet/EntityFramework/issues/2805

    public static class ModelBuilderExtensions
    {
        public static void AddConfiguration<TEntity>(this ModelBuilder modelBuilder, EntityTypeConfiguration<TEntity> configuration)
            where TEntity : class
        {
            configuration.Map(modelBuilder.Entity<TEntity>());
        }
    }
}
