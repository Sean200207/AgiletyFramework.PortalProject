using AgiletyFramework.DbModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.DbModels
{
    /// <summary>
    /// 数据访问层
    /// </summary>
    public class AgiletyDbContext:DbContext
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        private string ConnectionString;

        /// <summary>
        /// 带有数据库连接字符串的参数
        /// </summary>
        /// <param name="connectionString"></param>
        public AgiletyDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public AgiletyDbContext(DbContextOptions<AgiletyDbContext> options)
            :base(options)
        {

        }


        public virtual DbSet<UserEntity> UserEntities { get; set; }

        public virtual DbSet<RoleEntity> RoleEntities { get; set; }

        public virtual DbSet<UserRoleMapEntity> UserRoleMapEntities { get; set; }

        public virtual DbSet<MenuEntity> MenuEntities { get; set; }

        public virtual DbSet<RoleMenuMapEntity> RoleMenuMapEntities { get; set; }

        public virtual DbSet<SystemLog> SystemLogs { get; set; }


        /// <summary>
        /// 配置DbContext需要的参数--例如  数据库链接字符串
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        /// <summary>
        /// 配置数据库表映射关系
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("UserEntity").HasKey(u => u.UserId);
            });
            modelBuilder.Entity<MenuEntity>(entity =>
            {
                entity.ToTable("MenuEntity").HasKey(u => u.Id);
            });
            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.ToTable("RoleEntity").HasKey(u => u.RoleId);
            });
            modelBuilder.Entity<RoleMenuMapEntity>(entity =>
            {
                entity.ToTable("RoleMenuMapEntity").HasKey(u => u.Id);
            });
            modelBuilder.Entity<UserRoleMapEntity>(entity =>
            {
                entity.ToTable("UserRoleMapEntity").HasKey(u => u.Id);
            });
            modelBuilder.Entity<SystemLog>(entity =>
            {
                entity.ToTable("SystemLog").HasKey(u => u.Id);
            });
        }
    }
}
