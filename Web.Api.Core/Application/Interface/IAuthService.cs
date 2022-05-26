using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Auth;

namespace Web.Api.Core.Application.Interface
{
    public interface IAuthService
    {
        Task<AuthResponse> GenerateAuth(string client_secret);
    }
}
