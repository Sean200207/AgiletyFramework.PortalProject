using AgiletyFramework.DbModels;
using AgiletyFramework.DbModels.Models;

namespace AgiletyFramework.InitDatabase1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-TGSR1TT;Initial Catalog=AgiletyFramework.DB;" +
                "Persist Security Info=True;User ID=sa;Password=;TrustServerCertificate=true";
            using (AgiletyDbContext context = new AgiletyDbContext(connectionString))
            {
                //根据数据库连接字符串的配置删除数据库，如果不存在就不操作
                context.Database.EnsureDeleted();
                //根据数据库连接字符串的配置创建数据库，如果存在就不创建
                context.Database.EnsureCreated();

                #region MyRegion
                #region 这里是默认匹配的前端菜单展示的数据，为系统初始化必备数据，这里是Json格式
                string menuJson = "[{\"Id\":\"a76cac65-86c2-42a3-b155-901256ed1ede\",\"ParentId\":\"69b6bde4-3a40-4d25-88ef-f5885643e9c5\",\"MenuText\":\"用户管理\"," +
                    "\"MenuType\":1,\"Icon\":\"Edit\",\"WebUrlName\":\"user\",\"WebUrl\":\"/user\",\"VueFilePath\":\"../views/user/index.vue\",\"IsLeafNode\":true," +
                    "\"OrderBy\":1,\"CreateTime\":\"2023-04-04T14:46:30\",\"ModifyTime\":\"2023-04-05T22:06:30.2633333\",\"Status\":1}" +
                    ",{\"Id\":\"123dc79e-a69c-443e-8f00-d160693a1a59\",\"ParentId\":\"69b6bde4-3a40-4d25-88ef-f5885643e9c5\",\"MenuText\":\"角色管理\"," +
                    "\"MenuType\":1,\"Icon\":\"User\",\"WebUrlName\":\"role\",\"WebUrl\":\"/role\",\"VueFilePath\":\"../views/role/index.vue\",\"IsLeafNode\":true," +
                    "\"OrderBy\":2,\"CreateTime\":\"2023-04-04T14:46:30\",\"ModifyTime\":\"2023-04-04T18:03:55.41\",\"Status\":1}" +
                    ",{\"Id\":\"d6d0cb8d-0aba-45c5-97e3-f7ad1457c502\",\"ParentId\":\"69b6bde4-3a40-4d25-88ef-f5885643e9c5\",\"MenuText\":\"日志管理\"," +
                    "\"MenuType\":1,\"Icon\":\"Menu\",\"WebUrlName\":\"log\",\"WebUrl\":\"/log\",\"VueFilePath\":\"../views/system/log.vue\",\"IsLeafNode\":true," +
                    "\"OrderBy\":3,\"CreateTime\":\"2023-04-04T14:46:30\",\"ModifyTime\":\"2023-04-04T18:04:01.9366667\",\"Status\":1}" +
                    ",{\"Id\":\"d6d0cb8d-0aba-45c5-97e3-f7ad1557c502\",\"ParentId\":\"69b6bde4-3a40-4d25-88ef-f5885643e9c5\",\"MenuText\":\"菜单管理\"," +
                    "\"MenuType\":1,\"Icon\":\"Menu\",\"WebUrlName\":\"menu\",\"WebUrl\":\"/menu\",\"VueFilePath\":\"../views/menu/index.vue\",\"IsLeafNode\":true," +
                    "\"OrderBy\":3,\"CreateTime\":\"2023-04-04T14:46:30\",\"ModifyTime\":\"2023-04-04T18:04:01.9366667\",\"Status\":1}]";
                #endregion
                //反序列化成集合
                List<MenuEntity>? menuList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MenuEntity>>(menuJson);

                //保存到数据库
                context.Set<MenuEntity>().AddRange(menuList);
                context.SaveChanges();
                #endregion

                //添加一条数据
                var adduser = new DbModels.Models.UserEntity()
                {
                    Address = "北京市",
                    Email = "wenzhiwei646@163.com",
                    Imageurl = "",
                    LastLoginTime = DateTime.Now,
                    Mobile = "13272493813",
                    Name = "Sean",
                    Password = "123456",
                    QQ = "2208044559",
                    Phone = "13272493813",
                    Sex = 1,
                    UserType = 1,
                    WeChat = ""
                };
                context.UserEntities.Add(adduser);
                context.SaveChanges();


                //修改
                UserEntity user = context.UserEntities.OrderByDescending(c => c.UserId).FirstOrDefault();
                user.Name = "Peter";
                context.SaveChanges();

                //删除数据
                //context.Remove(user);
                //context.SaveChanges();
            }
        }
    }
}