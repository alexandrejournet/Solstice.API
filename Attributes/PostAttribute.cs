using Microsoft.AspNetCore.Mvc;

namespace Radiant.NET.API.Attributes;

/// <summary>
/// The 'PostAttribute' class is a decorator extending from the 'HttpPostAttribute' class in the Microsoft ASP.NET Core MVC framework.
/// It is used to specify the HTTP POST request methods for the action methods in the controller class.
/// </summary>
///
/// <Remarks>
/// This class has two constructors.
/// The first constructor, 'PostAttribute()', is a parameterless constructor.
/// The second constructor, 'PostAttribute(string template)', takes a string template as a parameter. It initializes the base
/// 'HttpPostAttribute' with the passed template.
/// </Remarks>
public class PostAttribute : HttpPostAttribute
{
    public PostAttribute()
    {
    }

    public PostAttribute(string template) : base(template)
    {
    }
}