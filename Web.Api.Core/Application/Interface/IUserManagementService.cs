using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Accounts;
using Web.Domain.Helpers;

namespace Web.Api.Core.Application.Interface
{
   public interface IUserManagementService
    {
        Task<ResponseHandler> Create(CreateRequest model, string contentPath);
        Task<ResponseHandler> GetAll();
        Task<ResponseHandler> GetAllById(int id);
        Task<ResponseHandler> Delete(int id);
        Task<ResponseHandler> DeleteBySelected(string Id);
        Task<ResponseHandler> Update(int id, UpdateRequest model);

    }
}
