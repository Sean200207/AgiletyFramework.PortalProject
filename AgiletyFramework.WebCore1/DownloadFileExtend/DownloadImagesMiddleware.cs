﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.WebCore1.DownloadFileExtend
{
    public class DownloadImagesMiddleware
    {
        private readonly RequestDelegate _next;
        private string? _directoryPath = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        /// <param name="directoryPath"></param>
        public DownloadImagesMiddleware(RequestDelegate next, string directoryPath)
        {
            _next = next;
            _directoryPath = directoryPath;
        }

        /// <summary>
        /// 读取图片信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {

            bool bResult = context.Request.Path.Value!.EndsWith(".jpg")
                || context.Request.Path.Value!.EndsWith(".png")
                || context.Request.Path.Value!.EndsWith(".gif");

            if (context.Request.Path.Value!.EndsWith(".jpg"))//规则支持自定义
            {
                string fileUrl = $"{_directoryPath}{context.Request.Path.Value}";

                if (!File.Exists(fileUrl)) //如果文件不存在就继续往后的流程
                {
                    await _next(context);//啥也不干
                }
                else
                {
                    context.Request.EnableBuffering();
                    context.Request.Body.Position = 0;
                    var responseStream = context.Response.Body;

                    using (FileStream newStream = new FileStream(fileUrl, FileMode.Open))
                    {
                        context.Response.Body = newStream;
                        newStream.Position = 0;
                        var responseReader = new StreamReader(newStream);
                        var responseContent = await responseReader.ReadToEndAsync();
                        newStream.Position = 0;
                        await newStream.CopyToAsync(responseStream);
                        context.Response.Body = responseStream;
                    }

                }
            }
            else
            {
                //继续往后去执行
                await _next(context);//啥也不干
            }

        }
    }
}
