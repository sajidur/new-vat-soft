using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace RiceMill_MVC.BAL
{
    public class DBService <T> where T : class
    {
        DbContextRepository<T> entity = new DbContextRepository<T>();
        public List<T> GetAll()
        {
            return entity.GetAll().ToList();
        }
        public List<T> ExecuteProcedure(string name, List<SqlParameter> param)
        {
            return entity.ExecuteQuery(name,param).ToList();
        }

        public DataTable ExecuteProcedure(string name, List<SqlParameter> param,bool isDatataTable)
        {
            return entity.ExecuteQuery(name, param, true);
        }
        public List<T> ExecuteProcedure(string name)
        {
            return entity.ExecuteQuery(name).ToList();
        }

        public int ExecuteNonQuery(string name)
        {
            return entity.ExecuteNonQuery(name);
        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return entity.Query(predicate);
        }
        public IQueryable<T> LastRow()
        {
            return entity.GetAll();
        }

        public T GetById(int? id = 0)
        {
            return entity.GetById(id);
        }

        public T Save(T cat)
        {
            entity.Add(cat);
            this.entity.SaveChanges();
            return cat;

        }
        public T Update(T t, int id)
        {
            entity.Update(t, id);
            this.entity.SaveChanges();
            return t;

        }
        public int Delete(int id)
        {
            var entitybyid= GetById(id);
            entity.Delete(entitybyid);
            return entity.SaveChanges();
        }
    }
}