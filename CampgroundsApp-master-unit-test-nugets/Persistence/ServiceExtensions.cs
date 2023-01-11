using System;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            return services
                .AddRepositories()
                .AddSqlClient(configuration);
        }
        
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IUsersRepository, UsersRepository>();
            services.AddSingleton<ICampgroundsRepository, CampgroundsRepository>();
            services.AddSingleton<ICommentsRepository, CommentsRepository>();
            services.AddSingleton<IImagesRepository, ImagesRepository>();

            return services;
        }
        
        private static IServiceCollection AddSqlClient(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnectionString");
            
            return services.AddTransient<ISqlClient>(_ => new SqlClient(connectionString));
        }
    }
}