using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.DataAccess;

namespace Web.Domain.Helpers
{
    public class ResponseCodes
    {
        public static readonly string SUCCESS = "00";
        public static readonly string ERROR = "01";
        public static readonly string SYSTEM_ERROR = "99";
    }

    public class ResponseHandler
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object data { get; set; }
    }

    public class DeleteRespCount
    {
        public int count { get; set; }
    }
    public class UsersRep
    {
        public List<Users> users { get; set; }
    }
}
