using System.Data.Entity.ModelConfiguration;
using TestWebApp.Models.Entities;

namespace TestWebApp.Models.Mappings
{
    public class IdentifierEntityMap : EntityTypeConfiguration<IdentifierEntity>
    {
        public IdentifierEntityMap()
        {
            HasMany(x => x.SharedEntities)
               .WithOptional(x => x.IdentifierEntity)
               .HasForeignKey(x => x.IdentifierEntityId);
        }
    }

    public class SharedEntityMap : EntityTypeConfiguration<SharedEntity>
    {
        public SharedEntityMap()
        {
            Map(x => x.MapInheritedProperties());

            HasOptional(x => x.IdentifierEntity)
                .WithMany(x => x.SharedEntities)
                .HasForeignKey(x => x.IdentifierEntityId);
        }
    }

    public class InheritedSharedEntityMap : EntityTypeConfiguration<InheritedSharedEntity>
    {
        public InheritedSharedEntityMap()
        {
            ToTable("InheritedSharedEntities");

            const string discriminator = "ObjectType";
            Map<InheritedStringEntity>(x => x.Requires(discriminator).HasValue("StringObject"));
            Map<InheritedIntEntity>(x => x.Requires(discriminator).HasValue("IntObject"));
            Map<InheritedBoolEntity>(x => x.Requires(discriminator).HasValue("BoolObject"));
        }
    }
}