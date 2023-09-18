﻿using Application.Features.Behavior;
using Application.Services;
using Application.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly())
                    .AddMediatR(Assembly.GetExecutingAssembly())
                    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                    .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
                    .AddTransient<ICustomerService,CustomerService>();
                    //.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachePipeline<,>))
                    //.AddTransient(typeof(IPipelineBehavior<,>), typeof(RamovalCachePipeline<,>));


        }
    }
}
