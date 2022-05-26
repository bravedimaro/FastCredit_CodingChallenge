using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Web.Api.DataAccess;

namespace Web.Domain.Accounts
{
    public class CreateRequest
    {
      
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EnumDataType(typeof(Role))]
        public int Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Dob { get; set; }
        [Required]
        public string Nationality { get; set; }
        public IFormFile photo { get; set; }

    }
}