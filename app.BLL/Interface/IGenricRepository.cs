﻿using app.DAL.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.BLL.Interface
{
    public interface IGenricRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        int Add(T item);
        int Update(T item);
        int Delete(T item);
    }
}
