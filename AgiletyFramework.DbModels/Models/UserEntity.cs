namespace AgiletyFramework.DbModels.Models
{
    /// <summary>
    /// 用户信息---用作和数据库映射的实体对象
    /// </summary>
    public class UserEntity : BaseModel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }

        /// <summary>
        /// 用户类型--UserTypeEnum
        /// 1:管理员 系统默认生成
        /// 2:普通用户 添加的或者注册的用户都为普通用户
        /// </summary>
        public int UserType { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? QQ { get; set; }
        public string? WeChat { get; set; }
        public int Sex { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string? Imageurl { get; set; }
        public DateTime LastLoginTime { get; set; }
    }
}