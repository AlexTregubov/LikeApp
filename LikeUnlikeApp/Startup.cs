using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LikeUnlikeApp.Startup))]
namespace LikeUnlikeApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
