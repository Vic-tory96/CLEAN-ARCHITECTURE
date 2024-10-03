using GR.Application.Respository;
using GR.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Infrastructure.Respository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ApplicationContext _context;
        public BaseRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<T> Create(T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }

           await _context.AddAsync(entity);
           await _context.SaveChangesAsync();
            return entity;
        }
       public async Task<bool>DeleteAuthorsWithBooks(T entity)
       {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var books = _context.Books.Where(b => b.AuthorId == entity.Id).ToList();
            if(books.Any())
            {
                _context.RemoveRange(books);
            }
            _context.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
       }
        public async Task<bool> Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Remove(entity);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<T> Get(string id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id ==id);
        }
         
        public async Task<IEnumerable<T>> GetAll() => await _context.Set<T>().ToListAsync();
       

        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
          await _context.SaveChangesAsync();
            return entity;
        }
    }
}
