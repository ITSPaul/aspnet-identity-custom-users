namespace AspNetIdentity.WebApi.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Annotations;

    public class XAppDbContext : DbContext
    {
        public XAppDbContext() : base("DefaultConnection")
        {
//            Database.SetInitializer<XAppDbContext>(new XDatabaseInitializer());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<XAppDbContext, WebApi.Migrations.Configuration>());
#if DEBUG
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<XAppDbContext>());
#endif
        }

        public DbSet<XUser> Users { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ConfigureUser(modelBuilder);
            ConfigureOrders(modelBuilder);

            base.OnModelCreating(modelBuilder);
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
    }
}
