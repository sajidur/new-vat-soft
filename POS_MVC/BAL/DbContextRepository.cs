﻿using Microsoft.Ajax.Utilities;
using RiceMill_MVC.BAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data;

namespace RiceMill_MVC.BAL
{                                   
     public class DbContextRepository<T> : GenericRepository<T>
        where T : class
    {
        protected DbContext Context;
        protected DbSet<T> DbSet;

        public DbContextRepository()
        {
            DbContext context = new DbContext("Entities");
            if (context == null)
                throw new ArgumentException("context");

            Context = context;
            DbSet = Context.Set<T>();
           // context.Configuration.ProxyCreationEnabled = false;
        }

        public override void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot add a null entity.");

            DbSet.Add(entity);
        }

        public override void Update(T entity, int id)
        {
            if (entity == null)
                throw new ArgumentException("Cannot update a null entity.");

            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var attachedEntity = DbSet.Find(id); // Need to have access to key

                if (attachedEntity != null)
                {
                    var attachedEntry = Context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified; // This should attach entity
                }
            }
        }

        public override T GetById(object key)
        {
            return DbSet.Find(key);
        }

        public override IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public override int SaveChanges(bool validateEntities)
        {
            Context.Configuration.ValidateOnSaveEnabled = validateEntities;
            return Context.SaveChanges();
        }

        #region IDisposable implementation

        public override void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        public override IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
        public  IEnumerable<T> ExecuteQuery(string name,List<SqlParameter> param)
        {
           // return Context.Database.SqlQuery<T>(name).AsQueryable<T>();
            return Context.Database.SqlQuery<T>(name, param.ToArray());
        }
        public DataTable ExecuteQuery(string name, List<SqlParameter> param, bool returnDataTable)
        {
            DataSet dset = new DataSet();

            var dt = new DataTable();
            var conn = Context.Database.Connection;
            var connectionState = conn.State;
            try
            {
                if (connectionState != ConnectionState.Open) conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    SqlDataAdapter da = new SqlDataAdapter();
                    cmd.CommandText = name;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(param);
                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                // error handling
                throw;
            }
            finally
            {
                if (connectionState != ConnectionState.Closed) conn.Close();
            }
            return dt;

        }

        public int ExecuteNonQuery(string name)
        {
            int rowsAffected = 0;
            var conn = Context.Database.Connection;
            var connectionState = conn.State;
            try
            {
                if (connectionState != ConnectionState.Open) conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = name;
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // error handling
                throw;
            }
            finally
            {
                if (connectionState != ConnectionState.Closed) conn.Close();
            }
            return rowsAffected;

        }
        public IEnumerable<T> ExecuteQuery(string name)
        {
            // return Context.Database.SqlQuery<T>(name).AsQueryable<T>();
            return Context.Database.SqlQuery<T>(name);
        }

        public override void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Cannot Delete a null entity.");
            DbSet.Remove(entity);
        } 
        #endregion IDisposable implementation
    }
}