namespace AspNetIdentity.WebApi.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using AspNetIdentity.WebApi.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<AspNetIdentity.WebApi.Models.XAppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AspNetIdentity.WebApi.Models.XAppDbContext";
        }

        protected override void Seed(AspNetIdentity.WebApi.Models.XAppDbContext context)
        {
            var users = new List<XUser>
            {
                new XUser { FirstName= "Krz", LastName="Ko", UserName = "krzyhook", Address = "addr 0 wro", CreationDate = DateTime.Parse("2017-11-22") },
                new XUser { FirstName= "Carson", LastName="Alexander", UserName = "ACarson", Address = "addr 1x" , CreationDate = DateTime.Parse("2017-11-22") },
                new XUser { FirstName = "Meredith", LastName="Alonso", UserName = "AMeredith", Address = "addr 2x", CreationDate = DateTime.Parse("2017-11-22") },
            };

            users.ForEach(u => context.Users.AddOrUpdate(uu => uu.UserName, u));
            context.SaveChanges();

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
    }
}
