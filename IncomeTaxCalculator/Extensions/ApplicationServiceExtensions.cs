using Application.Core;
using Application.CsvEntityReader;
using Application.CsvEntityReader.Employee;
using Application.Employees;
using Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace IncomeTaxCalculator.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<HrContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton(typeof(AnnualncomeCalculatorService));

            services.AddScoped(typeof(ICsvEntityReader<>), typeof(CsvEntityReader<>));
            services.AddScoped(typeof(IEmployeeCsvReader), typeof(EmployeeCsvReader));

            services.AddMediatR(typeof(CreateFromCsv.Handler));

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            return services;
        }
    }
}