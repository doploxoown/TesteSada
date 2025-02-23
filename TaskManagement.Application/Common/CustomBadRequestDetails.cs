using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Application.Common
{
    public class CustomBadRequestDetails : ValidationProblemDetails
    {
        public CustomBadRequestDetails(ActionContext context)
        {
            Status = StatusCodes.Status400BadRequest;
            Title = "Um ou mais erros de validação ocorreram.";
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value.Errors.Select(er => er.ErrorMessage).ToArray()
                );
            foreach (var error in errors)
            {
                Errors.Add(error.Key, error.Value);
            }
        }
    }

}
