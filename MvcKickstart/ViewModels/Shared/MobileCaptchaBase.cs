using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MvcKickstart.ViewModels.Shared;

namespace MvcKickstart.ViewModels.Shared
{
    public abstract class MobileCaptchaBase
    {
        public MobileCaptcha Captcha { get; set; }
        [Display(Name = "Captcha answer")]
        public int? CaptchaAnswer { get; set; }
        public string CaptchaEncoded { get; set; }
        public string PassedCaptcha { get; set; }

        public void GenerateCaptcha()
        {
            Captcha = new MobileCaptcha();
            Captcha.GenerateCaptcha(MobileCaptcha.MathType.Plus);
            CaptchaEncoded = Captcha.EncodedValue;
        }


        public bool ShouldDisplayMobileCaptcha(HttpRequestBase request)
        {
            string isMobileString = request.Browser["IsMobile"];
            bool isMobile = false;
            if(bool.TryParse(isMobileString, out isMobile))
            {
                return isMobile;
            }
            return false;
        }
    }
}