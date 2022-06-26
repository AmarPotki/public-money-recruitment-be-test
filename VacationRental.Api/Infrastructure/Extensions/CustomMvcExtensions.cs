using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Application.Validations;

namespace VacationRental.Api.Infrastructure.Extensions
{
    public static class CustomMvcExtensions
    {
        public static void  AddCustomMvc(this IServiceCollection services)
        {

            services.AddMvc(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssemblyContaining<CreateRentalValidator>();
                });

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Info { Title = "Vacation rental information", Version = "v1" }));
            services.AddRouting(options =>
                options.ConstraintMap.Add("noZeroes", typeof(NoZeroesRouteConstraint)));
        }
    }
}