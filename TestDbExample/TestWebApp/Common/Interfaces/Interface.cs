using System;

namespace TestWebApp.Common.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsActive { get; set; }
    }

    public interface IVersionable
    {
        int Version { get; set; }
    }

    public interface IAuditable
    {
        DateTime CreatedOnUtc { get; set; }
        DateTime ModifiedOnUtc { get; set; }
    }

    public interface IIdentifierEntity
    {
        string Name { get; set; }
        DateTime Date { get; set; }
    }

    public interface ISharedEntity
    {
        string EventType { get; set; }
        int EventCount { get; set; }

        Guid? IdentifierEntityId { get; set; }
    }

    public interface IInheritedSharedEntity
    {
        string GroupType { get; set; }
    }

    public interface IInheritedStringEntity
    {
        string StringValue { get; set; }
    }

    public interface IInheritedIntEntity
    {
        int IntValue { get; set; }
    }

    public interface IInheritedBoolEntity
    {
        bool BoolValue { get; set; }
    }
}