// Copyright DEFRA (c). All rights reserved.
// Licensed under the Open Government License v3.0.

using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Defra.Trade.API.Daera.Certificates.Extensions;

public static class ValidationResultExtension
{
    public static IActionResult FluentValidationProblem(this ControllerBase controller, ValidationResult result)
    {
        result.AddToModelState(controller.ModelState);

        return controller.ValidationProblem();
    }

    private static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        if (result.IsValid)
            return;
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }
}