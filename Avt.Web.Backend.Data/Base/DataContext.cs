using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avt.Web.Backend.Data.Configuration;
using Avt.Web.Backend.Data.Entities;
using Avt.Web.Backend.Data.Spec;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using IDataContext = Avt.Web.Backend.Data.Spec.IDataContext;

namespace Avt.Web.Backend.Data.Base
{
    public class DataContext : DbContext, IDataContext
    {
        bool _disposed;
        public Guid UniqueId { get; internal set; }

        private const string CreationDate = "CreationDate";

        public DbSet<Vehicle> VehicleDbSet { get; set; }
        public DbSet<VehicleStatusDetail> VehicleStatusDetailDbSet { get; set; }
        public DbSet<VehicleAggregateOverview> VehicleAggregateOverviewDbSet { get; set; }


        public DataContext()
            : this("Deafult")
        {
        }

        public DataContext(string connectionString)
        {
            UniqueId = Guid.NewGuid();
        }

        public DataContext(DbContextOptions options)
            : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ContextChangeTracker.AutoDetectChangesEnabled = false;
            ContextChangeTracker.AutoDetectChangesEnabled = false;
            

            UniqueId = Guid.NewGuid();
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        public static DataContext Create(string connectionString)
        {
            return new DataContext(connectionString);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(builder => builder.Default(WarningBehavior.Throw));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EntityConfigurationManager.Configure(modelBuilder);
        }

        public ChangeTracker ContextChangeTracker => this.ChangeTracker;

        public override int SaveChanges()
        {
            UpdateFields();
            Validate();

            var changes = base.SaveChanges();
            return changes;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }

        public DbSet<T> SetEntity<T>() where T : class
        {
            return Set<T>();
        }

        public EntityEntry<IEntity<T>> SetEntityEntry<T>(IEntity<T> entity)
        {
            return this.Entry(entity);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateFields();
            Validate();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void Validate()
        {
            var validationResults = new List<ValidationResult>();

            var entities = (from e in ChangeTracker.Entries()
                               where e.State == EntityState.Added
                                     || e.State == EntityState.Modified
                               select e.Entity)
                             .Where(t => (t as IValidatableObject) != null)
                             .Cast<IValidatableObject>();

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);

                var entityValidationResults = entity.Validate(validationContext).ToArray();
                if (entityValidationResults.Any())
                {
                    validationResults.AddRange(entityValidationResults);
                }
            }

            if (validationResults.Any())
            {
                var msg = string.Join(Environment.NewLine, validationResults.Select(t => t.ErrorMessage + $"({string.Join("|", t.MemberNames)})"));
                throw new InvalidOperationException(msg);
            }
        }

        private void UpdateFields()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    UpdateFields(entry, CreationDate, DateTime.UtcNow);
                }
            }
        }

        private static void UpdateFields<T>(EntityEntry entry, string propName, T value)
        {
            if (entry.Entity.GetType().GetProperty(propName) != null && value != null)
            {
                entry.Property(propName).CurrentValue = value;
            }
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    base.Dispose();
                }

                _disposed = true;
            }
        }
    }
}