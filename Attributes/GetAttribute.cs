using Microsoft.AspNetCore.Mvc;

namespace Radiant.NET.API.Attributes;

/// <summary>
/// GetAttribute class inherits from the HttpGetAttribute class in Microsoft.AspNetCore.Mvc. It works as a HTTP GET method attribute
/// with optional route template. This class has two constructors. The default constructor creates a new GetAttribute with no parameters.
/// The second one accepts a string parameter that represents a route template.
/// </summary>
///
/// <remarks>
/// This attribute is applied to the actions and is used to influence the action selection process during the Routing in ASP.Net Core.
/// It helps the routing mechanism to select the correct action method to execute when a given request arrives. The route template is
/// an optional parameter which can be used to define the route for this attribute.
/// </remarks>
public class GetAttribute : HttpGetAttribute
{
    public GetAttribute()
    {
    }

    public GetAttribute(string template) : base(template)
    {
    }
}