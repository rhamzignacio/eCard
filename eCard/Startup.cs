using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eCard.Startup))]
namespace eCard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
