using GR.Application.Respository;
using GR.Domain;
using GR.Domain.DTO;
using GR.Infrastructure.Respository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace GR.Data.Controllers
{
    [Route("api/[controller]")]
    // [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBaseRepository<Book> _baseRepository;
        private readonly ICustomRepository<Book> _customRepository;
        public BookController(IBaseRepository<Book> baseRepository, ICustomRepository<Book> customRepository)
        {
            _baseRepository = baseRepository;
            _customRepository = customRepository;
        }

        [HttpPost("create-book")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var book = new Book()
            {
                Name = createBookDto.Name,
                AuthorId = createBookDto.AuthorId,
                ISBN = createBookDto.ISBN,
                Publisher = createBookDto.Publisher
            };
            var result = await _baseRepository.Create(book);

            var response = new BookResponseDto()
            {
                AuthorId = book.AuthorId,
                ISBN    = book.ISBN,
                Publisher = book.Publisher,
                Name = book.Name,
            };

            if (result is not null)
            {
                return CreatedAtAction(nameof(GetBookById), new {id = book.Id}, response);
            }

            return BadRequest("Failed in creating book");

        }

        [HttpGet("single{id}")]
        public async Task<IActionResult> GetBookById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id cannot be null or empty!");
            }
            var result = await _baseRepository.Get(id);

            if (result is null)
            {
                return NotFound("Book not found!");
               
            }
            return Ok(result);
        }

        [HttpGet("get-many")]
        public async Task<IActionResult> GetBooks()
        {
            var result = await _baseRepository.GetAll();
            return Ok(result);
        }

        [HttpPut("update-book")]
        public async Task<IActionResult> UpdateBook([FromBody] UpdateBookDto updateBookDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var getBook = await _baseRepository.Get(updateBookDto.Id);
            if (getBook is null)
            {
               return NotFound("Book not found"!);
            }
            getBook.Id = updateBookDto.Id;
            getBook.Name = updateBookDto.Name;
            getBook.ISBN = updateBookDto.ISBN;
            getBook.AuthorId = updateBookDto.AuthorId;
            getBook.Publisher = updateBookDto.Publisher;

            var result = await _baseRepository.Update(getBook);
            return Ok($"The book:{result} was updated successfully!");
        }

        [HttpDelete("single-delete")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id cannot be null!");
            }

            var getId = await _baseRepository.Get(id);
            if (getId is not null)
            {
                var result = await _baseRepository.Delete(getId);
                return Ok($"Book with Id:{id} successfully deleted!");
            }
            return NotFound("Book not found");

        }

        [HttpDelete("delete-many")]
        public async Task<IActionResult> DeleteMany([FromBody] DeleteBooksDto deleteBooksDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(deleteBooksDto.Id.Count() < 1)
            {
                return NotFound("No Ids found!");
            }
            var list = new List<string>();

            foreach(var id in deleteBooksDto.Id)
            {
                var result = await _baseRepository.Get(id);
                list.Add(result.Id);
            }
            if(list.Count > 0)
            {
                var delete = await _customRepository.DeleteManys(list);
                if(delete is true)
                {
                    return Ok("Books successfully deleted!");
                }
            }
            return BadRequest("Failed in deleting books!");
        }
    }
}
