using HospitalManagementSystem.Core;
using HospitalManagementSystem.Errors;
using HospitalManagementSystem.Helpers;
using HospitalManagementSystem.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.Extesions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(MappingProfiles));

            #region Error Handling

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {

                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(P => P.ErrorMessage)
                                                         .ToList();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            #endregion

            return services;
        }
    }
}
