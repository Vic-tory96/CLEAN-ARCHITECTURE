using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Domain
{
    public class Book : BaseEntity
    {
        public Author Author { get; set; }
        public string AuthorId { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Publisher {  get; set; }
    }
}
