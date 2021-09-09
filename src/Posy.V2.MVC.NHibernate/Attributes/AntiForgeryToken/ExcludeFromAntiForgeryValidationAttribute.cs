using System;

namespace Posy.V2.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExcludeFromAntiForgeryValidationAttribute : Attribute
    {
    }
}