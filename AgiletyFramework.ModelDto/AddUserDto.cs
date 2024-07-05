using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.ModelDto
{
    public class AddUserDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? ImageUrl { get; set; }
        public long? Phone { get; set; }
        public long? Mobile { get; set; }
        public string? Email { get; set; }
        public string? QQ { get; set; }
        public string? WeChat { get; set; }
        public string? Sex { get; set; }
        public string? Address { get; set; }
        public bool IsEnabled { get; set; }
    }
}
