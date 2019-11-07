using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ButikBlog.ViewModels
{
    public class UploadAvatarViewModel
    {
        public string Photo { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}