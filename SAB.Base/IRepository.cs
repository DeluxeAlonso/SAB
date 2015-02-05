using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAB.Base
{
    public interface IRepository<T>
    {
        void Insert(T entity);
        int Delete(T entity);      
        T QueryById(int id);
        void Update(T entity);
        IEnumerable<T> QueryAll();
    }
}
