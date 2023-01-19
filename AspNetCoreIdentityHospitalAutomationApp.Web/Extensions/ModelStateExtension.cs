using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AspNetCoreIdentityHospitalAutomationApp.Web.Extensions
{
    public static class ModelStateExtension
    {
        public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> errors)
        {
            errors.ForEach(error =>
            {
                modelState.AddModelError(string.Empty, error);
            });
        }
    }
}



