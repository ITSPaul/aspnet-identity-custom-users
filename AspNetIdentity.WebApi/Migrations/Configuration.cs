namespace AspNetIdentity.WebApi.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using AspNetIdentity.WebApi.Models;
    using AspNetIdentity.WebApi.Models.Auth.Identity;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<AspNetIdentity.WebApi.Models.XAppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AspNetIdentity.WebApi.Models.XAppDbContext";
        }

        protected override void Seed(AspNetIdentity.WebApi.Models.XAppDbContext context)
        {
            CreateUsers(context);

            var orders = new List<UserOrder>
            {
                new UserOrder { OrderName = "first order", UserId = 1, CreationDate = DateTime.Parse("2017-11-22") },
                new UserOrder { OrderName = "first order 2", UserId = 1, CreationDate = DateTime.Parse("2017-11-22") },
                new UserOrder { OrderName = "second order", UserId = 2, CreationDate = DateTime.Parse("2017-11-22") },
                new UserOrder { OrderName = "third order", UserId = 3, CreationDate = DateTime.Parse("2017-11-22") },
                new UserOrder { OrderName = "4th order", UserId = 3, CreationDate = DateTime.Parse("2017-11-23") }
            };

            foreach (UserOrder o in orders)
            {
                var orderInDataBase = context.UserOrders.SingleOrDefault(d => d.User.Id == o.UserId && d.OrderName == o.OrderName);
                if (orderInDataBase == null)
                {
                    context.UserOrders.Add(o);
                }
            }
            context.SaveChanges();
        }

        private static void CreateUsers(XAppDbContext context)
        {
            var users = new List<XUser>
            {
                new XUser { FirstName = "K", LastName = "KK", UserName = "krzyhook", Email = "krzyhook@example.com", Address = "addr 0 wro", CreationDate = DateTime.Parse("2017-11-22") },
                new XUser { FirstName = "Carson", LastName = "Alexander", UserName = "ACarson", Address = "addr 1x", CreationDate = DateTime.Parse("2017-11-22") },
                new XUser { FirstName = "Meredith", LastName = "Alonso", UserName = "AMeredith", Address = "addr 2x", CreationDate = DateTime.Parse("2017-11-22") }
            };

            users.ForEach(u => context.Users.AddOrUpdate(uu => uu.UserName, u));
            context.SaveChanges();
            
            var userManager = new XUserManager(new UserStore<XUser, XRole, long, XLogin, XUserRole, XClaim>(context));
            var roleManager = new XRoleManager(new RoleStore<XRole, long, XUserRole>(context));
            
            var superPowerUser = new XUser
            {
                UserName = "SuperPowerUser",
                Email = "SuperAdmin-test@example.com",
                FirstName = "Admin",
                LastName = "Power",
                CreationDate = DateTime.Parse("2017-11-24")
            };
            userManager.Create(superPowerUser, "P@ssw0rd");

            var krzyhook = userManager.FindByName("krzyhook");
            if (krzyhook.PasswordHash == null)
            {
                userManager.AddPassword(krzyhook.Id, "P@ssw0rd");
            }

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new XRole { Name = "SuperAdmin" });
                roleManager.Create(new XRole { Name = "Admin" });
                roleManager.Create(new XRole { Name = "User" });
            }

            var adminUser = userManager.FindByName(superPowerUser.UserName);
            userManager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
        }
    }
}
