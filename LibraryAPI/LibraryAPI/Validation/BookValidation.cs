using FluentValidation;
using LibraryAPI.DTOs.BookDTO;
using LibraryAPI.Models;
using System;

namespace LibraryAPI.Validation
{
    public class BookValidation : AbstractValidator<BookCreateDto>
    {
        public BookValidation()
        {
            RuleFor(b=>b.Name).NotEmpty()
                               .WithMessage("Values in the SubItems array cannot be empty.");
            RuleFor(b=>b.Price).NotEmpty()
                               .InclusiveBetween(1, 1000)
                               .WithMessage("Money must be bigger than one");
        }
    }
}
