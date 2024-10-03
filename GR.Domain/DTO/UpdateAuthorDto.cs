using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Domain.DTO
{
    public class UpdateAuthorDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
      

        //public UpdateAuthorDto()
        //{
        //    BookDto = new List<BookDto>();
        //}
    }
}
