using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestUsers.Startup))]
namespace TestUsers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
