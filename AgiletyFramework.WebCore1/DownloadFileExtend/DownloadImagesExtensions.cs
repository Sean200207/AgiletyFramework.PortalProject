using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.WebCore1.DownloadFileExtend
{
    public static class DownloadImagesExtensions
    {
        public static void UseDownloadImages(this IApplicationBuilder app,string directoryPath)
        {
            app.UseMiddleware<DownloadImagesMiddleware>(Directory.GetCurrentDirectory());
        }
    }
}
