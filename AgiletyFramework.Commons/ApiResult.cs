using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.Commons
{
    /// <summary>
    /// Api  JSON通用返回格式
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 返回是否正常
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 处理消息
        /// </summary>
        public string? Message { get; set; }

    }

    public class ApiDataResult<T> : ApiResult
    {
        /// <summary>
        /// 结果集
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// 冗余结果
        /// </summary>
        public object? OValue { get; set; }

    }
}
