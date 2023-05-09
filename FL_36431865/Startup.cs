using Owin;
using Microsoft.Owin;
//using Microsoft.Owin.Security.Authorization.Infrastructure;
//using IdentityModel;

[assembly: OwinStartup(typeof(Startup))]
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        //ConfigureAuth(app)
        //app.UseAuthorization(options =>
        //{
        //    options.AddPolicy("AbleToCreateUser", policy => policy.RequireClaim(JwtClaimTypes.Role, "Manager"));
        //});
    }
} 