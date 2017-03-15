using System;
using TestWebApp.Common.Interfaces;

namespace TestWebApp.Common.Dtos
{
    public class EntityDto : IEntity, IVersionable, IAuditable
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime ModifiedOnUtc { get; set; }
    }

    public class IdentifierEntityDto : EntityDto, IIdentifierEntity
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }

    public class SharedEntityDto : EntityDto, ISharedEntity
    {
        public string EventType { get; set; }
        public int EventCount { get; set; }

        public Guid? IdentifierEntityId { get; set; }
        public IdentifierEntityDto IdentifierEntity { get; set; }
    }

    public class InheritedSharedEntityDto : SharedEntityDto, IInheritedSharedEntity
    {
        public string GroupType { get; set; }
    }

    public class InheritedStringEntityDto : InheritedSharedEntityDto, IInheritedStringEntity
    {
        public string StringValue { get; set; }
    }

    public class InheritedIntEntityDto : InheritedSharedEntityDto, IInheritedIntEntity
    {
        public int IntValue { get; set; }
    }

    public class InheritedBoolEntityDto : InheritedSharedEntityDto, IInheritedBoolEntity
    {
        public bool BoolValue { get; set; }
    }
}