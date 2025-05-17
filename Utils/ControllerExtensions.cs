using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Solstice.Domain.Exceptions;
using Solstice.Domain.Extensions;

namespace Solstice.API.Utils;

public static class ControllerExtensions
{
    public static ActionResult OkOrNotFound<T>(this ControllerBase _, T? result)
        where T : class
    {
        if (result == null)
        {
            return new NotFoundObjectResult(CoreExceptionEnum.HTTP_404.Get());
        }

        // Check if T is a list or implements IEnumerable<T>
        if (!typeof(IEnumerable).IsAssignableFrom(typeof(T))) return new OkObjectResult(result);
        
        var enumerable = (IEnumerable)result;
        if (!enumerable.Cast<object>().IsNotNullOrEmpty())
        {
            return new NotFoundObjectResult(CoreExceptionEnum.HTTP_404.Get());
        }

        return new OkObjectResult(result);
    }

    public static ActionResult OkOrNoContent<T>(this ControllerBase _, T? result)
        where T : class
    {
        if (result == null)
        {
            return new NoContentResult();
        }

        // Check if T is a list or implements IEnumerable<T>
        if (!typeof(IEnumerable).IsAssignableFrom(typeof(T))) return new OkObjectResult(result);
        
        var enumerable = (IEnumerable)result;
        if (!enumerable.Cast<object>().IsNotNullOrEmpty())
        {
            return new NoContentResult();
        }

        return new OkObjectResult(result);
    }
}