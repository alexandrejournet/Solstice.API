using Microsoft.AspNetCore.Mvc;

namespace Solstice.API.Attributes;

/// <summary>
/// DeleteAttribute is a class that extends the HttpDeleteAttribute class in the Microsoft.AspNetCore.Mvc package.
/// It allows customization of the DELETE HTTP verb attribute in a Controller action method signature.
/// </summary>
public class DeleteAttribute : HttpDeleteAttribute
{
    /// <summary>
    /// Default constructor that creates a new instance of DeleteAttribute.
    /// </summary>
    public DeleteAttribute()
    {
    }

    /// <summary>
    /// Constructor that creates a new instance of DeleteAttribute with a route template.
    /// </summary>
    /// <param name="template">A string that specifies the URI route template for this attribute.</param>
    public DeleteAttribute(string template) : base(template)
    {
    }
}