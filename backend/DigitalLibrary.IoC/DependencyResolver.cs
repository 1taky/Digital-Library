using Autofac;
using DigitalLibrary.BLL.Interfaces;
using DigitalLibrary.BLL.Services;
using DigitalLibrary.DAL.Data;
using DigitalLibrary.DAL.Interfaces;
using DigitalLibrary.DAL.Repositories;
using DigitalLibrary.DAL.UnitOfWork;
using DigitalLibrary.IoC.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DigitalLibrary.IoC;

public class DependencyResolver : Module
{
    private readonly IConfiguration _configuration;

    public DependencyResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(context =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<DigitalLibraryDbContext>();

            optionsBuilder.UseNpgsql(
                _configuration.GetConnectionString("DefaultConnection"));

            return new DigitalLibraryDbContext(optionsBuilder.Options);
        })
        .AsSelf()
        .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(GenericRepository<>))
            .As(typeof(IGenericRepository<>))
            .InstancePerLifetimeScope();

        builder.RegisterType<UserRepository>()
            .As<IUserRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterType<AuthService>()
            .As<IAuthService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<JwtService>()
    .As<IJwtService>()
    .InstancePerLifetimeScope();

        builder.RegisterType<DatabaseInitializer>()
            .AsSelf()
            .InstancePerLifetimeScope();
    }
}