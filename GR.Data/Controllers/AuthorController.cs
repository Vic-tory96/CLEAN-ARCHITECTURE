using GR.Application.Respository;
using GR.Domain;
using GR.Domain.DTO;
using GR.Infrastructure.Respository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GR.Data.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IBaseRepository<Author> _baseRepository;
        private readonly ICustomRepository<Author> _customRepository;
        public AuthorController(IBaseRepository<Author> baseRepository, ICustomRepository<Author> customRepository)
        {
            _baseRepository = baseRepository;
            _customRepository = customRepository;
        }

        [HttpPost("create-author")]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorDto createAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var author = new Author()
            {
                FirstName = createAuthor.FirstName,
                LastName = createAuthor.LastName,
                Email = createAuthor.Email
               
            };

            var result = await _baseRepository.Create(author);

            var response = new CreateAuthorResponseDto()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email
            };

            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, response);
        }

        [HttpGet("single/{id}")]
        public async Task<IActionResult> GetAuthorById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound("This user is not found!");
            }

            var result = await _baseRepository.Get(id);
            return Ok(result);
        }

        [HttpGet("get-all-authors")]

        public async Task<IActionResult> GetAllAuthor()
        {
            var authors = await _baseRepository.GetAll();
            return Ok(authors);
        }

        [HttpPut("update-author")]
        public async Task<IActionResult> UpdateAuthor([FromBody] UpdateAuthorDto updateAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateExistingAuthor = await _baseRepository.Get(updateAuthor.Id);

            if (updateExistingAuthor is null)
            {
                return NotFound($"Author with Id {updateAuthor.Id} was not found");
            }

            updateExistingAuthor.Id = updateAuthor.Id;
            updateExistingAuthor.FirstName = updateAuthor.FirstName;
            updateExistingAuthor.LastName = updateAuthor.LastName;
            updateExistingAuthor.Email = updateAuthor.Email;


            var result = _baseRepository.Update(updateExistingAuthor);
            return Ok(result);

        }
        [HttpDelete("single-delete/{id}")]
        public async Task<IActionResult> DeleteAuthor (string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id cannot be null or empty");
            }

            var result = await _baseRepository.Get(id);

            if(result != null)
            {
                var delete = await _baseRepository.DeleteAuthorsWithBooks(result);

                if (delete)
                {
                    return Ok("Author successfully deleted!");
                }
              
            }

            return NotFound("Author not found!");
        }

        [HttpDelete("delete-many")]
        public async Task<IActionResult> DeleteAuthors([FromBody] DeleteAuthorDto deleteAuthor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(deleteAuthor.Id.Count < 1)
            {
                return BadRequest("Id cannot be null");
            }
            var list = new List<string>();
            foreach(var id in deleteAuthor.Id)
            {
                var result = await _baseRepository.Get(id);
                if (result is not null)
                {
                    list.Add(result.Id);
                }
              
            }
            if(list.Count > 0)
            {
                var delete = await _customRepository.DeleteMany(list);
                if(delete is true)
                {
                    return Ok("Authors sucessfully deleted"!);
                }
            }
            return NotFound("No authors found to delete!");
        }
    }
}
