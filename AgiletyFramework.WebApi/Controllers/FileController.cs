using AgiletyFramework.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgiletyFramework.WebApi.Controllers
{
    
    /// <summary>
    /// 文件管理
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        /// <summary>
        /// [构造函数]
        /// </summary>
        /// <param name="logger"></param>
        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 文件上传Api
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFiles([FromForm] IFormFile file)
        {
            string suffix = string.Empty;
            #region 获取文件后缀
            string filename = file.FileName.Trim();
            int index = filename.LastIndexOf(".");
            if(index > 0 && index < filename.Length - 1)
            {
                suffix = filename.Substring(index + 1);
            }
            #endregion

            #region 重新命名保存文件的名字
            string saveDirectory = $"FileUpload\\{DateTime.Now.ToString("yyyy-MM-dd")}";
            string allSavePath = $"{Directory.GetCurrentDirectory()}\\{saveDirectory}";
            if(Directory.Exists(allSavePath) == false)
            {
                Directory.CreateDirectory(allSavePath);
            }
            //保存的新文件名
            string newFileName = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}_{Guid.NewGuid().ToString()}.{suffix.ToLower()}";

            //保存的文件名
            string allSaveFilePath = $"{allSavePath}\\{newFileName}";
            #endregion
            try
            {
                using (var stream = System.IO.File.Create(allSaveFilePath))
                {
                    file.CopyToAsync(stream);
                }
                return new JsonResult(new ApiDataResult<string>()
                {
                    Success = true,
                    Message = "文件上传成功",
                    Data = $"{saveDirectory}\\{newFileName}"
                });
            }
            catch (Exception)
            {
                return new JsonResult(new ApiDataResult<string>()
                {
                    Success = false,
                    Message = "文件上传失败了"
                });
            }
        }
    }
}
