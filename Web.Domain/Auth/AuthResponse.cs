using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Auth
{
    public class Data
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
    public class ErrorResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
   public class AuthResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public Data data { get; set; }
    }
}
