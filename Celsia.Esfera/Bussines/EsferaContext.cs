using Entities.Models;
using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<Relationship> Relationsships { get; set; }

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

                entity.Property(e => e.CellPhone)
                    .HasColumnName("cellPhone")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.Identification)
                    .IsRequired()
                    .HasColumnName("identification")
                    .HasMaxLength(50);

                entity.Property(e => e.IdentificationType).HasColumnName("identificationType");

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(200);

                entity.Property(e => e.FistName)
                    .HasColumnName("firstname")
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.PolicyData).HasColumnName("policyData");

                entity.Property(e => e.System).HasColumnName("system");

                entity.HasOne(d => d.IdentificationTypeNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.IdentificationType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customers_IdentificationTypes");

                entity.HasOne(d => d.SystemNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.System)
                    .HasConstraintName("FK_Customers_ExternalSystems");
            });

            modelBuilder.Entity<ExternalSystem>(entity =>
            {
                entity.ToTable("ExternalSystem");
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

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthday")
                    .HasColumnType("date");

                entity.Property(e => e.CellNumber)
                    .HasColumnName("cellNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.Code).HasColumnName("code");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Identification)
                    .IsRequired()
                    .HasColumnName("identification")
                    .HasMaxLength(50);

                entity.Property(e => e.IdentificationType).HasColumnName("identificationType");

                entity.Property(e => e.Interested).HasColumnName("interested");

                entity.Property(e => e.LastName)
                    .HasColumnName("lastName")
                    .HasMaxLength(200);

                entity.Property(e => e.FistName)
                    .HasColumnName("name")
                    .HasMaxLength(200);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(50);

                entity.Property(e => e.PolicyData).HasColumnName("policyData");

                entity.Property(e => e.Relation).HasColumnName("relation");

                entity.Property(e => e.System).HasColumnName("system");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Persons_Customers");

                entity.HasOne(d => d.IdentificationTypeNavigation)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.IdentificationType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Persons_IdentificationTypes");

                entity.HasOne(d => d.InterestedNavigation)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.Interested)
                    .HasConstraintName("FK_Persons_Interests");

                entity.HasOne(d => d.RelationNavigation)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.Relation)
                    .HasConstraintName("FK_Persons_Relationsships");

                entity.HasOne(d => d.SystemNavigation)
                    .WithMany(p => p.Persons)
                    .HasForeignKey(d => d.System)
                    .HasConstraintName("FK_Persons_ExternalSystems");
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.ToTable("Relationsships");
                entity.HasComment("contiene la informacion de las relaciones de las personas con un cliente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
