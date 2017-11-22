namespace AspNetIdentity.WebApi.Models.Auth.Identity
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    public class XUserManager : UserManager<XUser, long>
    {
        public XUserManager(IUserStore<XUser, long> store, IdentityFactoryOptions<XUserManager> options) : base(store)
        {
            SetupValidation(this);
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                this.UserTokenProvider = new DataProtectorTokenProvider<XUser, long>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            //manager.RegisterTwoFactorProvider(
            //    "PhoneCode",
            //    new PhoneNumberTokenProvider<MyUser, int>
            //    {
            //        MessageFormat = "Your security code is: {0}"
            //    });

            //manager.RegisterTwoFactorProvider(
            //    "EmailCode",
            //    new EmailTokenProvider<XUser, long>
            //    {
            //        Subject = "Security Code",
            //        BodyFormat = "Your security code is: {0}"
            //    });
        }

        public static XUserManager Create(IdentityFactoryOptions<XUserManager> options, IOwinContext context)
        {
            var store = new UserStore<XUser, XRole, long, XLogin, XUserRole, XClaim>(context.Get<XAppDbContext>());
            var manager = new XUserManager(store, options ?? new IdentityFactoryOptions<XUserManager>());
            
            SetupValidation(manager);

            return manager;
        }

        private static void SetupValidation(XUserManager manager)
        {
            manager.UserValidator = new UserValidator<XUser, long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
        }
    }
}
