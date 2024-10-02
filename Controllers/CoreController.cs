using Microsoft.AspNetCore.Mvc;
using Radiant.NET.Domain.Exceptions;
using Radiant.NET.Domain.Extensions;
using System.Collections;

namespace Radiant.API.Controllers;

/// <summary>
/// The CoreController is responsible for controlling the behavior of the system when it has to Radiant.NET.
/// This includes authorizing the user who is asking for the system restart, and providing the necessary Radiant.NET API endpoint.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoreController : ControllerBase
{
    public static ActionResult OkOrNotFound<T>(T nullable)
        where T : class
    {
        if (nullable == null)
        {
            return new NotFoundObjectResult(CoreExceptionEnum.HTTP_404.Get());
        }

        // Check if T is a list or implements IEnumerable<T>
        if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
        {
            var enumerable = (IEnumerable)nullable;
            if (!enumerable.Cast<object>().IsNotNullOrEmpty())
            {
                return new NotFoundObjectResult(CoreExceptionEnum.HTTP_404.Get());
            }
        }

        return new OkObjectResult(nullable);
    }

    public static ActionResult OkOrNoContent<T>(T nullable)
        where T : class
    {
        if (nullable == null)
        {
            return new NoContentResult();
        }

        // Check if T is a list or implements IEnumerable<T>
        if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
        {
            var enumerable = (IEnumerable)nullable;
            if (!enumerable.Cast<object>().IsNotNullOrEmpty())
            {
                return new NoContentResult();
            }
        }

        return new OkObjectResult(nullable);
    }
}