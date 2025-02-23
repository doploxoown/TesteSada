using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.Application.Common
{
    public class CustomInvalidModelStateResponseFactory
    {
        public static IActionResult ProduceErrorResponse(ActionContext context)
        {
            var problemDetails = new CustomBadRequestDetails(context);
            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }
    }

}
