using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Api.DataAccess
{
    [Table("Users_Tbl")]
    public class Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string nationality { get; set; }
        public string photo { get; set; }
        public Role Role { get; set; }
       
       
    }
}