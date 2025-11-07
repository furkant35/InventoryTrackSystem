using Autofac;
using Autofac.Extras.DynamicProxy;
using InventoryTrackSystem.Business.Services;
using InventoryTrackSystem.Business.Utilities.Security;
using Castle.DynamicProxy;
using InventoryTrackSystem.Core.Utilities.Interceptors;
using InventoryTrackSystem.Core.Utilities.IoC;
using InventoryTrackSystem.Core.Utilities.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using InventoryTrackSystem.Model.Concrete;
using InventoryTrackSystem.Business.Interfaces;
using InventoryTrackSystem.Model.Dtos.User;
using InventoryTrackSystem.Model.Dtos.Product;
using InventoryTrackSystem.Model.Dtos.Brand;

namespace InventoryTrackSystem.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module, ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IBrandService), typeof(BrandService));

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped(typeof(IPasswordHasherService), typeof(IdentityPasswordHasherService));



            // ---- CRUD generic kayıtlar ---

            services.AddScoped<
                ICrudService<UserCreateDto, 
                               UserUpdateDto, 
                               UserGetDto, 
                               long>, UserService>();

            services.AddScoped<
                ICrudService<ProductCreateDto, 
                               ProductUpdateDto, 
                               ProductGetDto, 
                               long>, ProductService>();

            services.AddScoped<
                ICrudService<BrandCreateDto, 
                               BrandUpdateDto, 
                               BrandGetDto, 
                               long>, BrandService>();



        }
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                { Selector = new AspectInterceptorSelector() })
                .SingleInstance();
        }
    }
}
