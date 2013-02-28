using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcKickstart.ViewModels.Shared
{
    public class MetaTags
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Type { get; set; }
        public string SiteName { get; set; }
    }
}