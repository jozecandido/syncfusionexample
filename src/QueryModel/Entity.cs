using System;
using LinqToDB.Mapping;

namespace QueryModel
{
    [Table("Entities")]
    public class Entity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [Column(Length = 50, CanBeNull = false)]
        public string Name { get; set; }

        [Column(CanBeNull = false)]
        public Status Status { get; set; }

        [Column(Length = 500, CanBeNull = true)]
        public string? Description { get; set; }

        [Column(Length = 50, CanBeNull = true)]
        public string? Field1 { get; set; }

        [Column(Length = 50, CanBeNull = true)]
        public string? Field2 { get; set; }

        [Column(Length = 50, CanBeNull = true)]
        public string? Field3 { get; set; }

        [Column(CanBeNull = true)]
        public Guid RelationId { get; set; }
    }
}
