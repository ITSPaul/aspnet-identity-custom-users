namespace AspNetIdentity.WebApi.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Annotations;

    using AspNetIdentity.WebApi.Models.Auth.Identity;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class XAppDbContext : IdentityDbContext<XUser, XRole, long, XLogin, XUserRole, XClaim>
    {
        public XAppDbContext() : base("DefaultConnection")
        {
//            Database.SetInitializer<XAppDbContext>(new XDatabaseInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<XAppDbContext, WebApi.Migrations.Configuration>());
#if DEBUG
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<XAppDbContext>());
#endif
        }

        public static XAppDbContext Create()
        {
            return new XAppDbContext();
        }

        // from base IdentityDbContext
        //public DbSet<XUser> Users { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // this must be first
            base.OnModelCreating(modelBuilder);

            ConfigureUser(modelBuilder);
            ConfigureOrders(modelBuilder);
        }

        private static void ConfigureOrders(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserOrder>().ToTable("UserOrders");

            modelBuilder.Entity<UserOrder>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<UserOrder>().Property(t => t.OrderName).IsRequired();
            modelBuilder.Entity<UserOrder>().Property(t => t.OrderName).HasMaxLength(500);
            //modelBuilder.Entity<UserOrder>().Property(t => t.CreationDate).IsRequired();
            //modelBuilder.Entity<UserOrder>().Property(t => t.UserId).IsRequired();
        }

        private static void ConfigureUser(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<XUser>().ToTable("Users");
            modelBuilder.Entity<XUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<XUser>()
                .Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute { IsUnique = true }));

            modelBuilder.Entity<XUser>().Property(e => e.FirstName).HasMaxLength(30);
            modelBuilder.Entity<XUser>().Property(e => e.LastName).HasMaxLength(30);
            //modelBuilder.Entity<XUser>().Property(t => t.CreationDate).IsRequired();

            // Disable some fields we don't need 
            modelBuilder.Entity<XUser>().Ignore(u => u.TwoFactorEnabled);
            modelBuilder.Entity<XUser>().Ignore(u => u.PhoneNumberConfirmed);
            modelBuilder.Entity<XUser>().Ignore(u => u.PhoneNumber);
            modelBuilder.Entity<XUser>().Ignore(u => u.EmailConfirmed);
            //            modelBuilder.Entity<XUser>().Property(u => u.Id).HasColumnName("ID_User");
            //            modelBuilder.Entity<XUser>().Ignore(u => u.SecurityStamp);
            //            modelBuilder.Entity<XUser>().Ignore(u => u.PasswordHash);
            //            modelBuilder.Entity<XUser>().Ignore(u => u.LockoutEndDateUtc);
            //            modelBuilder.Entity<XUser>().Ignore(u => u.LockoutEnabled);
            //            modelBuilder.Entity<XUser>().Ignore(u => u.AccessFailedCount);

            // Override some column mappings that do not match our default
            //            modelBuilder.Entity<XUser>().Property(u => u.Password).HasColumnName("Password");
            //            modelBuilder.Entity<XUser>().Property(u => u.IsActive).HasColumnName("IsActive");

            modelBuilder.Entity<XRole>().ToTable("Roles");
            modelBuilder.Entity<XClaim>().ToTable("UserClaims");
            modelBuilder.Entity<XLogin>().ToTable("UserLogins");
            modelBuilder.Entity<XUserRole>().ToTable("UserRoles");
        }
    }
}
