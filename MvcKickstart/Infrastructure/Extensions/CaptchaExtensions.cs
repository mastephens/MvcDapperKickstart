using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcKickstart.ViewModels.Shared;

namespace MvcKickstart.Infrastructure.Extensions
{
    public static class CaptchaExtensions
    {
        public static void ValidateCaptcha(this MobileCaptchaBase model, bool isRecaptchaValid, HttpRequestBase request, ModelStateDictionary modelState)
        {
             if (model.ShouldDisplayMobileCaptcha(request))
            {
                model.Captcha = new MobileCaptcha();
                if (!model.Captcha.ValidateCaptcha(model.CaptchaAnswer, model.CaptchaEncoded))
                {
                    modelState.AddModelError("CaptchaAnswer", "You did not complete the math problem correctly. Please try again.");
                }
            }
             else if (!isRecaptchaValid)
            {
                modelState.AddModelError("PassedCaptcha", "You did not type the verification word correctly. Please try again.");
            }
        }

        public static void ResetCaptcha(this MobileCaptchaBase model, ModelStateDictionary modelState)
        {
            modelState.SetModelValue("CaptchaEncoded", new ValueProviderResult(null, string.Empty, CultureInfo.InvariantCulture));
        }
    }
}