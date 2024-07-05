using AgiletyFramework.ModelDto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace AgiletyFramework.AuthenticationApi.Utility.JwtService
{
    public abstract class AbstractJwtService
    {
        protected readonly JWTTokenOptions _JWTTokenOptions;
        protected readonly IMemoryCache _IMemoryCache;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="jWTTokenOptions"></param>
        /// <param name="iMemoryCache"></param>
        public AbstractJwtService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions, IMemoryCache iMemoryCache)
        {
            _JWTTokenOptions = jwtTokenOptions.CurrentValue;
            _IMemoryCache = iMemoryCache;
        }

        /// <summary>
        /// 创建AccessToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract string CreateAccessToken(UserDto user);

        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract string CreateRefreshToken(UserDto user);

        /// <summary>
        /// 写入token信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual List<Claim> UserToClaim(UserDto user)
        {
            List<Claim> claimsArray = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid,user.UserId.ToString()),
                new Claim(ClaimTypes.MobilePhone,user.Mobile?? string.Empty),
                new Claim(ClaimTypes.OtherPhone,user.Phone?? string.Empty),
                new Claim(ClaimTypes.StreetAddress,user.Address?? string.Empty),
                new Claim(ClaimTypes.Email,user.Email?? string.Empty),
                new Claim("userName",user.Name?? string.Empty),
                new Claim("imageUrl",user.Imageurl?? string.Empty),
                new Claim("QQ",user.QQ.ToString()),
                new Claim("WeChat",user.WeChat?? string.Empty),
                new Claim("Sex",user.Sex.ToString()),
            };
            return claimsArray;
        }
    }
}
