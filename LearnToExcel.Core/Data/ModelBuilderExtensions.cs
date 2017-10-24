using Microsoft.EntityFrameworkCore;

namespace LearnToExcel.Core.Data
{
    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConventions(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                //skip shadow types
                if (entity.ClrType == null) continue;
                entity.Relational().TableName = entity.ClrType.Name;
            }
        }
    }
}
