using System;
using System.Collections.Generic;

namespace AspNetIdentity.WebApi.Models
{
    /// <summary>
    /// It was used before migration, for first initalization. Unused now, replaced with migrations.
    /// </summary>
    public class XDatabaseInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<XAppDbContext>
    {
        protected override void Seed(XAppDbContext context)
        {
            var users = new List<XUser>
            {
                new XUser { FirstName= "Krz", LastName="Ko", UserName = "krzyhook", Address = "addr 0 wro", CreationDate = DateTime.Now },
                new XUser { FirstName= "Carson", LastName="Alexander", UserName = "ACarson", Address = "addr 1" , CreationDate = DateTime.Now },
                new XUser { FirstName = "Meredith", LastName="Alonso", UserName = "AMeredith", Address = "addr 2", CreationDate = DateTime.Now },
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var orders = new List<UserOrder>
            {
                new UserOrder { OrderName = "first order", UserId = 1, CreationDate = DateTime.Now },                     
                new UserOrder { OrderName = "first order 2", UserId = 1, CreationDate = DateTime.Now },                     
                new UserOrder { OrderName = "second order", UserId = 2, CreationDate = DateTime.Now },                     
                new UserOrder { OrderName = "third order", UserId = 3, CreationDate = DateTime.Now }
            };
            orders.ForEach(o => context.UserOrders.Add(o));
            context.SaveChanges();
        }
    }
}
