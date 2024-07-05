using AgiletyFramework.IBusinessServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.BusinessServices
{
    public abstract class BaseService : IBaseService
    {
        protected DbContext Context { get; set; }

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="context"></param>
        public BaseService(DbContext context)
        {
            Context = context;
        }

        #region Query

        /// <summary>
        /// 主键查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : class
        {
            return this.Context.Set<T>().Find(id);
        }

        /// <summary>
        /// 不应该暴露给上端使用者，尽量少用
        /// 查询所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Set<T>() where T : class
        {
            return this.Context.Set<T>();
        }

        /// <summary>
        /// 这才是合理的做法，上端给条件，这里查询
        /// 查询所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return this.Context.Set<T>().Where<T>(funcWhere);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public PagingData<T> QueryPage<T, S>(Expression<Func<T, bool>>
            funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>>
            funcOrderby, bool isAsc = true) where T : class
        {
            var list = Set<T>();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }
            PagingData<T> result = new PagingData<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                RecordCount = list.Count()
            };
            return result;
        }

        #endregion

        #region Insert

        /// <summary>
        /// 即使保存 不需要用Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t) where T : class
        {
            this.Context.Set<T>().Add(t);
            this.Commit();//写在这里 就不需要单独commit 不写就需要
            return t;
        }

        /// <summary>
        /// 新增集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
        {
            this.Context.Set<T>().AddRange(tList);
            this.Commit();//一个链接 多个sql
            return tList;
        }
        #endregion

        #region Update

        /// <summary>
        /// 是没有实现查询，直接更新的，需要Attach和State
        /// 如果是已经在context，只能在封装一个(在具体的service)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public void Update<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");
            //将数据附加到上下文，支持实体修改和新实体，重置为Unchanged
            this.Context.Set<T>().Attach(t);
            this.Context.Entry<T>(t).State = EntityState.Modified;
            this.Commit();//保存 然后重置为UnChanged
        }

        /// <summary>
        /// 修改一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        public void Update<T>(IEnumerable<T> tList) where T : class
        {
            foreach(var t in tList)
            {
                this.Context.Set<T>().Attach(t);
                this.Context.Entry<T>(t).State = EntityState.Modified;
            }
            this.Commit();
        }

        #endregion

        #region Delete

        /// <summary>
        /// 先附加在删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public void Delete<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");
            //将数据附加到上下文，支持实体修改和新实体，重置为Unchanged
            this.Context.Set<T>().Attach(t);
            this.Context.Set<T>().Remove(t);
            this.Commit();//保存 然后重置为UnChanged
        }

        /// <summary>
        /// 还可以添加非即时commit版本的，做成protected
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public void Delete<T>(int Id) where T : class
        {
            T t = this.Find<T>(Id);//也可以附加
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Remove(t);
            this.Commit();//保存 然后重置为UnChanged
        }

        public void Delete<T>(IEnumerable<T> tList) where T : class
        {
            foreach(var t in tList)
            {
                this.Context.Set<T>().Attach(t);
            }
            this.Context.Set<T>().RemoveRange(tList);
            this.Commit();//保存 然后重置为UnChanged
        }

        #endregion

        #region Other
        public void Commit()
        {
            Context.SaveChanges();//EFCore中对于增删改，必须要执行这句话才能生效
        }

        /// <summary>
        /// 执行Sql语句，返回IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[] parameters) where T : class
        {
            return this.Context.Set<T>().FromSqlRaw(sql, parameters);
        }

        /// <summary>
        /// 执行Sql语句，返回实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public void Excute<T>(string sql, SqlParameter[] parameters) where T : class
        {
            IDbContextTransaction trans = null;
            try
            {
                trans = Context.Database.BeginTransaction();
                this.Context.Database.ExecuteSqlRaw(sql, parameters);
                trans.Commit();
            }
            catch (Exception)
            {

                if(trans!=null)
                    trans.Rollback();
                throw;
            }
        }
        #endregion

        #region 伪代码

        //public void Add()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Deleta()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Query()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Update()
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
    }
}
