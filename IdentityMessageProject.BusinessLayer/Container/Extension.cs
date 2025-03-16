using IdentityMessageProject.BusinessLayer.Abstract;
using IdentityMessageProject.BusinessLayer.Concrete;
using IdentityMessageProject.DataAccessLayer.Abstract;
using IdentityMessageProject.DataAccessLayer.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityMessageProject.BusinessLayer.Container
{
    public static class Extension
    {
        public static void ContainerDependencies(this IServiceCollection services)
        {
            services.AddScoped<IMessageDal, EfMessageDal>();
            services.AddScoped<IMessageService, MessageManager>();
        }
    }
}
