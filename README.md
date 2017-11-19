Example of custom membership system using ASP.NET Identity 2, ASP.NET Web API 2 and OWIN Bearer token authentication
==========================

The **AspNetIdentity.WebApi** is an HTTP ASP.NET WebApi 2 service which acts as back-end for SPA frontend  **AspNetIdentity.Angular** built using Angular 5.

Default users with passwords (e.g _SuperPowerUser_) are created in `AspNetIdentity.WebApi.Migrations.Configuration.CreateUsers`

# Backend

## Custom UserManager and tables
Implementation for the custom `Microsoft.AspNet.Identity.UserManager` with own custom database entities:
- `XUser` with custom columns
- `XRole`
- `XUserRole`
- `XLogin`
- `XClaim`
- managers: `XUserManager` and `XRoleManager`

EntityFramework entities are mapped to custom database tables.

## OAuth 2.0 Bearer Access Token

OAuth 2.0 Bearer Access Token generation and consumption is implemented in `Startup.Auth.cs` mainly by 
- `Identity\Providers\CustomOAuthProvider.cs`
- `Identity\Providers\JwtTokenFormat.cs`
- `Identity\Providers\RefreshTokenProvider.cs`

---

# Frontend 
## AspNetIdentity.Angular
The app was generated with [angular-cli](https://github.com/angular/angular-cli) and can be run by `npm install` and then `npm start` and is hosted on [http://localhost:4200/](http://localhost:4200/)

---

## References & Sources
[tjoudeh/AspNetIdentity.WebApi](https://github.com/tjoudeh/AspNetIdentity.WebApi)

[Angular 2/4 JWT Authentication Example & Tutorial](http://jasonwatmore.com/post/2016/08/16/angular-2-jwt-authentication-example-tutorial)

[auth0/angular2-jwt](https://github.com/auth0/angular2-jwt)