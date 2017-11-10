namespace AspNetIdentity.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AspNetIdentity.WebApi.Models.Auth.Identity;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class XUser : IdentityUser<long, XLogin, XUserRole, XClaim>
    {
        //from base IdentityUser
        //public long Id { get; set; }
        //public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<UserOrder> UserOrders { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(XUserManager userManager, string authenticationType)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, authenticationType);
            //var userIdentity = await userManager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        /*
               +public virtual string Email { get; set; }
               !public virtual bool EmailConfirmed { get; set; }
               +public virtual string PasswordHash { get; set; }
               +public virtual string SecurityStamp { get; set; }
               !public virtual string PhoneNumber { get; set; }
               !public virtual bool PhoneNumberConfirmed { get; set; }
               !public virtual bool TwoFactorEnabled { get; set; }
               +public virtual DateTime? LockoutEndDateUtc { get; set; }
               +public virtual bool LockoutEnabled { get; set; }
               +public virtual int AccessFailedCount { get; set; }

               +public virtual ICollection<TRole> Roles { get; }
               +public virtual ICollection<TClaim> Claims { get; }
               +public virtual ICollection<TLogin> Logins { get; }

               @public virtual TKey Id { get; set; }
               @public virtual string UserName { get; set; } 
        */
    }
}
