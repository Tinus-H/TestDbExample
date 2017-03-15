using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWebApp.Common.Interfaces;

namespace TestWebApp.Models.Entities
{
    public class Entity : IEntity, IVersionable, IAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }

    public class IdentifierEntity : Entity, IIdentifierEntity
    {
        public IdentifierEntity()
        {
            SharedEntities = new HashSet<SharedEntity>();
        }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        // Navigation Properties
        public ICollection<SharedEntity> SharedEntities { get; set; }
    }

    public class SharedEntity: Entity, ISharedEntity
    {
        public string EventType { get; set; }
        public int EventCount { get; set; }

        // Navigation Properties
        public Guid? IdentifierEntityId { get; set; }
        public IdentifierEntity IdentifierEntity { get; set; }
    }

    public class InheritedSharedEntity : SharedEntity, IInheritedSharedEntity
    {
        public string GroupType { get; set; }
    }

    public class InheritedStringEntity : InheritedSharedEntity, IInheritedStringEntity
    {
        public string StringValue { get; set; }
    }

    public class InheritedIntEntity : InheritedSharedEntity, IInheritedIntEntity
    {
        public int IntValue { get; set; }
    }

    public class InheritedBoolEntity : InheritedSharedEntity, IInheritedBoolEntity
    {
        public bool BoolValue { get; set; }
    }
}