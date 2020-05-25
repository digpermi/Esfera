using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bussines
{
    public partial class EsferaContext : DbContext
    {
        public EsferaContext()
        {
        }

        public EsferaContext(DbContextOptions<EsferaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<ExternalSystem> ExternalSystems { get; set; }
        public virtual DbSet<IdentificationType> IdentificationTypes { get; set; }
        public virtual DbSet<Interest> Interests { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Audit> Audit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasComment("contiene la informacion de los clientes de SAC y SGC");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(200);

                entity.Property(e => e.MobileNumber)
                    .HasColumnName("mobileNumber")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Identification)
                    .HasColumnName("identification")
                    .HasMaxLength(50);

                entity.Property(e => e.IdentificationTypeId).HasColumnName("identificationTypeId");

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(200);

                entity.Property(e => e.FirstName)
                    .HasColumnName("firstname")
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.SystemUpdateDate)
                    .IsRequired()
                    .HasColumnName("systemUpdateDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExtenalUpdateDate)
                    .HasColumnName("extenalUpdateDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.PolicyData).HasColumnName("policyData");

                entity.Property(e => e.ExternalSystemId)
                    .IsRequired()
                    .HasColumnName("externalsystemid");

                entity.HasOne(d => d.IdentificationType)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.IdentificationTypeId)
                    .HasConstraintName("FK_Customers_IdentificationTypes");

                entity.HasOne(d => d.ExternalSystem)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.ExternalSystemId)
                    .HasConstraintName("FK_Customers_ExternalSystems");
            });

            modelBuilder.Entity<ExternalSystem>(entity =>
            {
                entity.ToTable("ExternalSystems");
                entity.HasComment("sistemas externos donde se cuentra la informacion de los clientes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<IdentificationType>(entity =>
            {
                entity.ToTable("IdentificationTypes");
                entity.HasComment("tipos de identificación");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Interest>(entity =>
            {
                entity.ToTable("Interests");
                entity.HasComment("Contiene la informacion de los intereses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Persons");
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreationDate)
                    .IsRequired()
                    .HasColumnName("creationDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateDate)
                    .IsRequired()
                    .HasColumnName("updateDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Birthdate)
                    .IsRequired()
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasColumnName("mobileNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Identification)
                    .IsRequired()
                    .HasColumnName("identification")
                    .HasMaxLength(50);

                entity.Property(e => e.IdentificationTypeId)
                    .IsRequired()
                    .HasColumnName("identificationTypeId");

                entity.Property(e => e.InterestId)
                    .IsRequired()
                    .HasColumnName("interestId");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("lastName")
                    .HasMaxLength(200);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("firstname")
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.PolicyData).HasColumnName("policyData");

                entity.Property(e => e.RelationshipId).HasColumnName("relationshipid");

                entity.Property(e => e.ExternalSystemId)
                    .IsRequired()
                    .HasColumnName("externalsystemid");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Persons_Customers");

                entity.HasOne(d => d.IdentificationType)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.IdentificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Persons_IdentificationTypes");

                entity.HasOne(d => d.Interest)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.InterestId)
                    .HasConstraintName("FK_Persons_Interests");

                entity.HasOne(d => d.Relationship)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.RelationshipId)
                    .HasConstraintName("FK_Persons_Relationsships");

                entity.HasOne(d => d.ExternalSystem)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.ExternalSystemId)
                    .HasConstraintName("FK_Persons_ExternalSystems");
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.ToTable("Relationships");
                entity.HasComment("contiene la informacion de las relaciones de las personas con un cliente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Audit>(entity =>
            {
                entity.ToTable("Audits");
                entity.HasComment("Auditoria de la aplicacion esfera");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasColumnName("Date")
                    .HasColumnType("date");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasMaxLength(200);

                entity.Property(e => e.OperationAudit).HasColumnName("operationAuditId")
                .HasConversion(new EnumToNumberConverter<OperationAudit, byte>())
                .IsRequired();
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
