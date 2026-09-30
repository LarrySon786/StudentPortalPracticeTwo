using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace StudentPortalPracticeTwo.Components.Services.Extensions;

public static class ValidationHelper
{
    
    public static List<ValidationResult> ValidateModel<T>(T model, List<ValidationResult>? errors = null)
    {
        if (errors == null) errors = new();
        if (model == null) throw new ArgumentNullException("Model is null. Cannot validate object. ");

        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, errors, validateAllProperties:true);

        return errors;
    }

}