using LibraryAPI.DAL;
using LibraryAPI.DTOs.BookDTO;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BookController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _context.Books.Where(b => b.IsDeleted == false && b.Id == id).FirstOrDefaultAsync();
            if (book is null) return StatusCode(404);
            return Ok(book);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books.Where(b => !b.IsDeleted).ToListAsync();
            if (books is null) return StatusCode(404);
            return Ok(books);
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookCreateDto bookDto)
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest);
            var book = new Book
            { 
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                Name = bookDto.Name,
                Price = bookDto.Price,
            };
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BookUpdateDto bookDto)
        {
            if (!ModelState.IsValid) return StatusCode(StatusCodes.Status400BadRequest);
            var dbBook = await _context.Books.Where(b => b.Id == id && !b.IsDeleted).FirstOrDefaultAsync();
            if (dbBook is null) return StatusCode(StatusCodes.Status404NotFound);
            dbBook.Name = bookDto.Name ?? dbBook.Name;
            dbBook.Price = bookDto.Price == 0 ? dbBook.Price : bookDto.Price;
            _context.Books.Update(dbBook);
            await _context.SaveChangesAsync();
            return Ok(dbBook);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dbBook = await _context.Books.Where(b => b.Id == id && !b.IsDeleted).FirstOrDefaultAsync();
            if (dbBook is null) return StatusCode(StatusCodes.Status404NotFound);
            dbBook.IsDeleted = true;
            _context.Update(dbBook);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatch(int id, JsonPatchDocument book)
        {
            var dbBook = await _context.Books.Where(b => b.Id == id && !b.IsDeleted).FirstOrDefaultAsync();
            if (dbBook is null) return StatusCode(StatusCodes.Status404NotFound);
            book.ApplyTo(dbBook);
            await _context.SaveChangesAsync();
            return Ok(dbBook);
        }
    }
}
