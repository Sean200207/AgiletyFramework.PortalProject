using AgiletyFramework.ModelDto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgiletyFramework.AuthenticationApi.Utility.JwtService
{
    /// <summary>
    /// 对称可逆加密实现
    /// </summary>
    public class JwtHSService : AbstractJwtService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtTokenOptions"></param>
        /// <param name="iMemoryCache"></param>
        public JwtHSService(IOptionsMonitor<JWTTokenOptions> jwtTokenOptions, IMemoryCache iMemoryCache) : base(jwtTokenOptions, iMemoryCache)
        {
        }

        /// <summary>
        /// 创建AccessToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override string CreateAccessToken(UserDto user)
        {
            //准备有效载荷
            List<Claim> claimslist = base.UserToClaim(user);
            //准备加密key
            string accesstoken = WriteToken(claimslist.ToArray(), TimeSpan.FromMinutes(10));
            return accesstoken;
        }

        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override string CreateRefreshToken(UserDto user)
        {
            //生成refreshToken
            string refreshtokenGuid = Guid.NewGuid().ToString();
            Claim[] claimslist = new Claim[]
            {
                new Claim("refreshtokenGuid", refreshtokenGuid)
            };
            string refreshToken = WriteToken(claimslist, TimeSpan.FromDays(5));
            //五天内有效
            _IMemoryCache.Set(refreshtokenGuid, user, DateTime.Now.AddDays(5));
            return refreshToken;
        }

        /// <summary>
        /// 对称可逆加密实现
        /// </summary>
        /// <param name="claimslist"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        private string WriteToken(Claim[] claimslist, TimeSpan timeSpan)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTTokenOptions.SecurityKey));
            //Sha256 加密方式
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken JwtRefreshToken = new JwtSecurityToken(
                issuer: _JWTTokenOptions.Issuer,
                audience: _JWTTokenOptions.Audience,
                claims: claimslist,
                expires: DateTime.Now.Add(timeSpan),//五天内有效
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(JwtRefreshToken);
        }
    }
}
