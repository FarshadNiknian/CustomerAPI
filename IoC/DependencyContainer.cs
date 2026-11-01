using Application.Common;
using Application.Features.InvoiceLists.Queries;
using Domain.Interfaces; // <-- اضافه شد
using Infrastructure.Persistance;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection RegisterAppServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ==========================================
            // 1. ثبت لایه Application (MediatR & Validators)
            // ==========================================
            var applicationAssembly = typeof(GetInvoicesListQuery).Assembly;

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(applicationAssembly));

            services.AddValidatorsFromAssembly(applicationAssembly);

            // ==========================================
            // 2. ثبت Interceptor و دیتابیس
            // ==========================================
            var connectionString = configuration.GetConnectionString("CustomerDb")
                ?? throw new InvalidOperationException("Connection string 'CustomerDb' not found in appsettings.json.");

            services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();

                options.UseSqlServer(connectionString)
                       .AddInterceptors(interceptor);
            });

            // ==========================================
            // 3. ثبت Repositoryها و Unit of Work
            // ==========================================
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IInvoiceReadRepository, InvoiceReadRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>(); 

            return services;
        }
    }
}
