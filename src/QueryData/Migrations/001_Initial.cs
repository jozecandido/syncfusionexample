using FluentMigrator;
using QueryModel;

namespace QueryData.Migrations
{
    [Migration(1, "Initial")]
    public class InitialMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table(nameof(QueryDbDataContext.Entities))
                .WithColumn(nameof(Entity.Id)).AsGuid().PrimaryKey()
                .WithColumn(nameof(Entity.Name)).AsFixedLengthString(50).NotNullable()
                .WithColumn(nameof(Entity.Status)).AsFixedLengthString(16).NotNullable()
                .WithColumn(nameof(Entity.Description)).AsFixedLengthString(500).Nullable()
                .WithColumn(nameof(Entity.Field1)).AsFixedLengthString(50).Nullable()
                .WithColumn(nameof(Entity.Field2)).AsFixedLengthString(50).Nullable()
                .WithColumn(nameof(Entity.Field3)).AsFixedLengthString(50).Nullable()
                .WithColumn(nameof(Entity.RelationId)).AsGuid().NotNullable();

            for (int i = 0; i < 5000; i++)
                Insert.IntoTable(nameof(QueryDbDataContext.Entities)).Row(
                    new { 
                        Id = Guid.NewGuid(),
                        Name = "Entity " + i,
                        Status = (Status) (i % 3),
                        Description = "Description " + i % 100,
                        Field1 = i % 3 == 0 ? "Field1" : string.Empty,
                        Field2 = i % 5 == 0 ? "Field2" : string.Empty,
                        Field3 = i % 7 == 0 ? "Field3" : string.Empty,
                        RelationId = Guid.NewGuid()
                     });
        }
    }
}
