using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TestWebApp.Common.Interfaces;
using TestWebApp.Models.Entities;

namespace TestWebApp.Models.Context
{
    ///Package Manager Console
    /// Enable-Migrations
    /// Add-Migration xxx -verbose
    /// Update-Database -verbose

    public class TestDbContext : DbContext
    {
        public TestDbContext()
            : this("TestDb")
        {
        }
        public TestDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            if (nameOrConnectionString == null) throw new ArgumentNullException(nameof(nameOrConnectionString));

            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer<TestDbContext>(new CreateDatabaseIfNotExists<TestDbContext>());
        }

        public DbSet<IdentifierEntity> IdentifierEntities { get; set; }
        public DbSet<SharedEntity> SharedEntities { get; set; }
        public DbSet<InheritedSharedEntity> InheritedSharedEntities { get; set; }

        public override int SaveChanges()
        {
            CheckAuditable();
            CheckIntegrity();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            return this.SaveChangesAsync(CancellationToken.None);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            CheckAuditable();
            CheckIntegrity();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.Load("TestWebApp"));
            base.OnModelCreating(modelBuilder);
        }

        protected virtual void CheckAuditable()
        {
            //Do audit trails
            DateTimeOffset currentDateTime = DateTime.UtcNow;
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
            {
                if (auditableEntity.State == EntityState.Added || auditableEntity.State == EntityState.Modified)
                {
                    auditableEntity.Entity.ModifiedOnUtc = currentDateTime.UtcDateTime;

                    switch (auditableEntity.State)
                    {
                        case EntityState.Added:
                            auditableEntity.Entity.CreatedOnUtc = currentDateTime.UtcDateTime;
                            break;
                        case EntityState.Modified:
                            auditableEntity.Property(p => p.CreatedOnUtc).IsModified = false;
                            break;
                    }
                }
            }

            //Do concurrency
            foreach (var versionableEntity in ChangeTracker.Entries<IVersionable>())
            {
                switch (versionableEntity.State)
                {
                    case EntityState.Added:
                        versionableEntity.Entity.Version = 1;
                        break;
                    case EntityState.Modified:
                        versionableEntity.Entity.Version++;
                        break;
                }
            }
        }

        protected virtual void CheckIntegrity()
        {
            //Check that the Id is not being changed
            foreach (var idEntity in ChangeTracker.Entries<IEntity>())
            {
                if (idEntity.State != EntityState.Added && idEntity.Property(p => p.Id).IsModified)
                {
                    if (idEntity.Property(p => p.Id).OriginalValue != idEntity.Property(p => p.Id).CurrentValue)
                        throw new DbEntityValidationException(
                            $"Attempt to change id on a modified {idEntity.Entity.GetType().FullName}:{idEntity.Entity.Id}");
                }
            }
        }
    }
}