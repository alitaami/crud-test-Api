﻿using Application.Repository;
using Insfrastructure.Context;
using Insfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insfrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped(typeof(IRepository<>), typeof(Repository<>))
                           .AddDbContext<ApplicationDbContext>(options => options
                           .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
