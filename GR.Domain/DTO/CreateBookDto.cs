using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Domain.DTO
{
    public class CreateBookDto
    {
        public string AuthorId { get; set; }  // This references the ID of the author.
        public string Name { get; set; }      // The name of the book.
        public string ISBN { get; set; }      // The ISBN of the book.
        public string Publisher { get; set; } // The publisher of the book.
    }
}
