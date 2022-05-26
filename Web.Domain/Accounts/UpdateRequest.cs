using System.ComponentModel.DataAnnotations;
using Web.Api.DataAccess;

namespace Web.Domain.Accounts
{
    public class UpdateRequest
    {
        
        private string _role;
        private string _email;
        
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string nationality { get; set; }
     
        [EnumDataType(typeof(Role))]
        public string Role
        {
            get => _role;
            set => _role = replaceEmptyWithNull(value);
        }

        [EmailAddress]
        public string Email
        {
            get => _email;
            set => _email = replaceEmptyWithNull(value);
        }

       

        // helpers

        private string replaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}