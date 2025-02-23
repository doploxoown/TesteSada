using FluentValidation;
using FluentValidation.AspNetCore;
using TaskManagement.Application.DTOs;

namespace TaskManagementAPI.Extensions
{
    public static class RegisterServicesExtension
    {
        public static void RegisterValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<CreateTaskDto>, CreateTaskDtoValidator>();
            services.AddTransient<IValidator<UpdateTaskDto>, UpdateTaskDtoValidator>();
            services.AddFluentValidationAutoValidation();
        }
    }
}
