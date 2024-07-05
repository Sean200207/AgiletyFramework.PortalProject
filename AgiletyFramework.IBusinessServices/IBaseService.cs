using AgiletyFramework.IBusinessServices;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AgiletyFramework.BusinessServices
{
    public interface IBaseService
    {

        #region Query

        /// <summary>
        /// 主键查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Find<T>(int id) where T : class;

        /// <summary>
        /// 不应该暴露给上端使用者，尽量少用
        /// 查询所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Set<T>() where T : class;

        /// <summary>
        /// 这才是合理的做法，上端给条件，这里查询
        /// 查询所有集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere)
            where T : class;

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
            funcOrderby, bool isAsc = true) where T : class;

        #endregion

        #region Insert

        /// <summary>
        /// 即使保存 不需要用Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t) where T : class;

        /// <summary>
        /// 新增集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class;

        #endregion

        #region Update

        /// <summary>
        /// 是没有实现查询，直接更新的，需要Attach和State
        /// 如果是已经在context，只能在封装一个(在具体的service)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public void Update<T>(T t) where T : class;

        /// <summary>
        /// 修改一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tList"></param>
        /// <returns></returns>
        public void Update<T>(IEnumerable<T> tList) where T : class;

        #endregion

        #region Delete

        /// <summary>
        /// 先附加在删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public void Delete<T>(T t) where T : class;

        /// <summary>
        /// 还可以添加非即时commit版本的，做成protected
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public void Delete<T>(int Id) where T : class;

        public void Delete<T>(IEnumerable<T> tList) where T : class;
        #endregion

        #region Other

        /// <summary>
        /// 执行Sql语句，返回IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[]
            parameters) where T : class;

        /// <summary>
        /// 执行Sql语句，返回实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public void Excute<T>(string sql, SqlParameter[] parameters) where T : class;

        #endregion

        #region 伪代码
        //public void Add();
        //public void Deleta();
        //public void Update();
        //public void Query();
        #endregion
    }
}
