namespace AgiletyFramework.ModelDto
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }

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
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime ModifyTime { get; set; }
        public int Status { get; set; }

        public List<int>? RoleIdList { get; set; }

        public List<int>? MenuIdList { get; set; }

    }
}