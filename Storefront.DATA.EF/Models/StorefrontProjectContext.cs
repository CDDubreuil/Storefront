using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Storefront.DATA.EF.Models
{
    public partial class StorefrontProjectContext : DbContext
    {
        public StorefrontProjectContext()
        {
        }

        public StorefrontProjectContext(DbContextOptions<StorefrontProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<CustomerData> CustomerData { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<ProductStatus> ProductStatuses { get; set; } = null!;
        public virtual DbSet<Record> Records { get; set; } = null!;
        public virtual DbSet<RecordOrder> RecordOrders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=StorefrontProject;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Encrypt=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("Artist");

                entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

                entity.Property(e => e.ArtistImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ArtistName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Members)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.YearsActive)
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Artists)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Artist_Genre");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<CustomerData>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK_CustomerData_1");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(128)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.CustomerCity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerCountry)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerState)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.CustomerZip)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("CustomerZIP")
                    .IsFixedLength();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.GenreDescription)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.GenreName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(128)
                    .HasColumnName("CustomerID");

                entity.Property(e => e.FulfillmentStatus)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_CustomerData");
            });

            modelBuilder.Entity<ProductStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("ProductStatus");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.RecordName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.Property(e => e.RecordId).HasColumnName("RecordID");

                entity.Property(e => e.ArtistId).HasColumnName("ArtistID");

                entity.Property(e => e.CoverArt)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ExecutiveProducer)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.RecordName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Year)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Records_Artist");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Records_ProductStatus");
            });

            modelBuilder.Entity<RecordOrder>(entity =>
            {
                entity.Property(e => e.RecordOrderId).HasColumnName("RecordOrderID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.RecordId).HasColumnName("RecordID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.RecordOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecordOrders_Order");

                entity.HasOne(d => d.Record)
                    .WithMany(p => p.RecordOrders)
                    .HasForeignKey(d => d.RecordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RecordOrders_Records");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
