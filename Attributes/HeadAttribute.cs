using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Solstice.API.Attributes;

/// <summary>
/// Represents an attribute that specifies that an action supports the HTTP HEAD method.
/// </summary>
public class HeadAttribute : HttpHeadAttribute
{
    public HeadAttribute()
    {
    }

    public HeadAttribute([StringSyntax("Route")] string template) : base(template)
    {
    }
}
