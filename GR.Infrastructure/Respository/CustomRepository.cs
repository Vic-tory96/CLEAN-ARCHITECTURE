using GR.Application.Respository;
using GR.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Infrastructure.Respository
{
    public class CustomRepository<T> : ICustomRepository<T> where T : BaseEntity
    {
        private readonly IBaseRepository<T> _baseRepository;
        public CustomRepository(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<bool> DeleteMany(List<string> ids)
        {
            if(ids is null  && ids.Count < 1)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            foreach(var id in ids)
            {
                var entity = await _baseRepository.Get(id);
                if(entity is not null)
                {
                    await _baseRepository.DeleteAuthorsWithBooks(entity);
                }
            }
            return true;
        }
        public async Task<bool> DeleteManys(List<string> ids)
        {
            if (ids is null && ids.Count < 1)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            foreach (var id in ids)
            {
                var entity = await _baseRepository.Get(id);
                if (entity is not null)
                {
                    await _baseRepository.Delete(entity);
                }
            }
            return true;
        }
    }
}
