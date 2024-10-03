using GR.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Application.Respository
{
    public interface IBaseRepository <T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> DeleteAuthorsWithBooks(T entity);
        Task<bool> Delete(T entity);
        Task<T> Get(string id);

    }
}
