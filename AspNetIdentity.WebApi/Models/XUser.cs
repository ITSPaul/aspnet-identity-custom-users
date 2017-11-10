namespace AspNetIdentity.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;

    public class XUser
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<UserOrder> UserOrders { get; set; }
    }
}
