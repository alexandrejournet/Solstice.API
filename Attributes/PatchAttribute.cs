using Microsoft.AspNetCore.Mvc;

namespace Solstice.API.Attributes;

/// <summary>
/// The PatchAttribute class represents a custom attribute to indicate that an action method should handle HTTP PATCH requests.
/// It has been derived from the HttpPatchAttribute class provided by the ASP.NET Core MVC framework.
/// </summary>
public class PatchAttribute : HttpPatchAttribute
{
    /// <summary>
    /// Default constructor for the PatchAttribute class.
    /// </summary>
    public PatchAttribute()
    {

    }
    /// <summary>
    /// Constructor with a parameter for the PatchAttribute class.
    /// The template parameter is the route template associated with this attribute.
    /// </summary>
    /// <param name="template">The string that contains the template to associate with this attribute.</param>
    public PatchAttribute(string template) : base(template)
    {
    }
}