using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FishingReportsMVC.Startup))]
namespace FishingReportsMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
