namespace AspNetIdentity.WebApi.Models
{
    using System;

    public class UserOrder
    {
        public long Id { get; set; }

        public string OrderName { get; set; }

        public DateTime CreationDate { get; set; }

        public long UserId { get; set; }
        public virtual XUser User { get; set; }
    }
}
