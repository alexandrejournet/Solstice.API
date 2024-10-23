using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Solstice.API.Attributes;

/// <summary>
/// The PutAttribute class inherits from the HttpPutAttribute class and represents an attribute that is used to indicate the HTTP PUT
/// method for an action within a controller.
/// </summary>
public class PutAttribute : HttpPutAttribute
{
    /// <summary>
    /// Default constructor for the PutAttribute class. Initializes a new instance of the class without any parameters.
    /// </summary>
    public PutAttribute()
    {
    }

    /// <summary>
    /// Overloaded constructor for the PutAttribute class. Initializes a new instance of the class with string parameter to represent
    /// the template for route data.
    /// </summary>
    /// <param name="template"> The route template. This is a pattern against which the URL path is matched. </param>
    public PutAttribute([StringSyntax("Route")] string template) : base(template)
    {
    }
}