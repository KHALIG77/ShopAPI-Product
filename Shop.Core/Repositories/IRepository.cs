﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        IQueryable<T> GetAllQueryable(Expression<Func<T,bool>> expression, params string[] includes);
        List<T> GetAll( params string[] includes);
        bool IsExist(Expression<Func<T, bool>> expression, params string[] includes);
        T Get(Expression<Func<T,bool>> expression, params string[] includes);
        void Remove(T entity);
        int Commit();
    }
}
