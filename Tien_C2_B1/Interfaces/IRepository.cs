using System;
using System.Collections.Generic;

namespace Tien_C2_B1
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T Get(string id);
        List<T> Gets();
    }
}
