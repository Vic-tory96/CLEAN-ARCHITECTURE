using GR.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Infrastructure.Respository
{
    public interface ICustomRepository<T> where T : BaseEntity
    
    {
        Task<bool> DeleteMany(List<string> ids);
        Task<bool> DeleteManys(List<string> ids);
    }
}
