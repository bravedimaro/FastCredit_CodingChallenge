using KissLog;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Application.Interface;
using Web.Api.DataAccess;
using Web.Domain.Accounts;
using Web.Domain.Helpers;

namespace Web.Api.Core.Application.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IKLogger _logger;

        private readonly IConfiguration _config;
        private readonly DataAccessContext _context;
        
        public UserManagementService(IKLogger kLogger, IConfiguration configuration, DataAccessContext dataAccessContext)
        {
            _config = configuration;
            _logger = kLogger;
            _context = dataAccessContext;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DBConn"));
            }
        }

        public async Task<ResponseHandler> Create(CreateRequest model, string contentPath)
        {
            ResponseHandler responseHandler = new ResponseHandler();
            var MaxFileSize = 1 * 1024 * 1024;
            string fileName;
            var supportedTypes = new[] { "jpg", "png", "svg" };
            var fileExt = System.IO.Path.GetExtension(model.photo.FileName).Substring(1);
            try
            {
                if (model.photo == null && model.photo.Length == 0)
                {
                    responseHandler.ResponseMessage = "Uploaded file is empty or null";
                    responseHandler.ResponseCode = ResponseCodes.ERROR;
                    return responseHandler;
                }
                // Upload the file if Greater than 1 MB
                if (model.photo.Length >= MaxFileSize)
                {
                    responseHandler.ResponseMessage = "Image file Exceeded maximum size 1MB";
                    responseHandler.ResponseCode = ResponseCodes.ERROR;
                    return responseHandler;
                }
                //check if uploaded file support image file type
                 if (!supportedTypes.Contains(fileExt))
                {
                    responseHandler.ResponseMessage = "Image file  Extension is Invalid - Only Upload jpg/png/svg File";
                    responseHandler.ResponseCode = ResponseCodes.ERROR;
                    return responseHandler;
                }
                // validate User 
                if (_context.Users.Any(x => x.Email == model.Email))
                {
                    responseHandler.ResponseMessage = $"Email '{model.Email}' is already registered";
                    responseHandler.ResponseCode = ResponseCodes.ERROR;
                    return responseHandler;
                }
                //Upload service
                var folderName = Path.Combine("Resources", "Images");
                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string cloudPath = string.Empty;
                byte[] Content;
                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }
                //System saves image file locally
                using (var memoryStream = new MemoryStream())
                {

                  
                    await model.photo.CopyToAsync(memoryStream);
                    Content = memoryStream.ToArray();
                    //stream.Close();
                   
                }
             
                string base64ImageRepresentation = Convert.ToBase64String(Content);
                // map model to new User object
                Users users = new Users();
                users.photo = base64ImageRepresentation.Trim();
                users.LastName = model.LastName.Trim();
                users.nationality = model.Nationality.Trim();
                users.FirstName = model.FirstName.Trim();
                users.gender = model.Gender.Trim();
                users.dob = model.Dob.Trim();
                users.Email = model.Email.Trim();
               
                users.Role = (Role)model.Role;
                // save account
                _context.Users.Add(users);
                _context.SaveChanges();
                responseHandler.ResponseMessage = "New User Added Successfully";
                responseHandler.ResponseCode = ResponseCodes.SUCCESS;
                return responseHandler;
            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][Create User][Response] => {ex.Message}");

            }
            responseHandler.ResponseCode = ResponseCodes.SYSTEM_ERROR;
            responseHandler.ResponseMessage = "An error occoured. Please try again later or contact admin for resolution";
            return responseHandler;
        }

        public async Task<ResponseHandler> Delete(int id)
        {
            ResponseHandler responseHandler = new ResponseHandler();
            try
            {
                var account = getAccount(id);
                _context.Users.Remove(account);
                _context.SaveChanges();
                responseHandler.ResponseMessage = "User Deleted Successfully";
                responseHandler.ResponseCode = ResponseCodes.SUCCESS;
                return responseHandler;
            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][DeleteBySelected][Response] => {ex.Message}");

            }
            responseHandler.ResponseCode = ResponseCodes.SYSTEM_ERROR;
            responseHandler.ResponseMessage = "An error occoured. Please try again later or contact admin for resolution";
            return responseHandler;
        }

        public async Task<ResponseHandler> DeleteBySelected(string Id)
        {
            ResponseHandler responseHandler = new ResponseHandler();
            try
            {
                // Batch Deletion of Users from the Database
                List<string> Ids = Id.Split(',').ToList();
                List<DeleteRespCount> deleteRespCounts = new List<DeleteRespCount>();
                foreach (var id in Ids)
                {
                    var account = getAccount(Convert.ToInt32(id));
                    if (account==null)
                    {
                        continue;
                    }
                    _context.Users.Remove(account);
                    _context.SaveChanges();
                    DeleteRespCount deleteRespCount = new DeleteRespCount();
                    deleteRespCount.count = Convert.ToInt32(id);
                    deleteRespCounts.Add(deleteRespCount);
                }
                if (deleteRespCounts.Any())
                {
                    responseHandler.ResponseMessage = "Seleted Users Deleted Successfully";
                    responseHandler.ResponseCode = ResponseCodes.SUCCESS;
                    return responseHandler;
                }
                else
                {
                    responseHandler.ResponseMessage = "Nothing to delete";
                    responseHandler.ResponseCode = ResponseCodes.ERROR;
                    return responseHandler;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][DeleteBySelected][Response] => {ex.Message}");

            }
            responseHandler.ResponseCode = ResponseCodes.SYSTEM_ERROR;
            responseHandler.ResponseMessage = "An error occoured. Please try again later or contact admin for resolution";
            return responseHandler;
        }

        public async Task<ResponseHandler> GetAll()
        {
            ResponseHandler responseHandler = new ResponseHandler();
            try
            {

                var GetAllUsers = _context.Users.ToList();
                //var Serialize= JsonConvert.SerializeObject(GetAllUsers);
                //var GetContent = JsonConvert.DeserializeObject<List<Users>>(Serialize);
                responseHandler.ResponseCode = ResponseCodes.SUCCESS;
                responseHandler.data = GetAllUsers;
                return responseHandler;

            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][GetAll Users][Response] => {ex.Message}");

            }
            responseHandler.ResponseCode = ResponseCodes.SYSTEM_ERROR;
            responseHandler.ResponseMessage = "An error occoured. Please try again later or contact admin for resolution";
            return responseHandler;
        }
        // helper methods
        private Users getAccount(int id)
        {
            ResponseHandler responseHandler = new ResponseHandler();
            try
            {
                var users = _context.Users.Find(id);
                if (users != null)
                {
                    
                    return users;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][getAccount helper method][Response] => {ex.Message}");

            }
            return null;
        }
        public  async Task<ResponseHandler> GetAllById(int id)
        {
            ResponseHandler responseHandler = new ResponseHandler();
            try
            {

                var account = getAccount(id);
                responseHandler.ResponseCode = ResponseCodes.SUCCESS;
                responseHandler.data = account;
                return responseHandler;

            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][GetAllById Users][Response] => {ex.Message}");

            }
            responseHandler.ResponseCode = ResponseCodes.SYSTEM_ERROR;
            responseHandler.ResponseMessage = "An error occoured. Please try again later or contact admin for resolution";
            return responseHandler;
        }

        public async Task<ResponseHandler> Update(int id, UpdateRequest model)
         {
            ResponseHandler responseHandler = new ResponseHandler();
            try
            {

                var account = getAccount(id);

                // validate
                if (account.Email != model.Email && _context.Users.Any(x => x.Email == model.Email))
                {
                    responseHandler.ResponseMessage = $"Email '{model.Email}' is already taken";
                    responseHandler.ResponseCode = ResponseCodes.ERROR;
                    return responseHandler;
                }

                Users users = new Users();
                users.LastName = model.LastName.Trim();
                users.nationality = model.nationality.Trim();
                users.FirstName = model.FirstName.Trim();
                users.gender = model.gender.Trim();
                users.dob = model.dob.Trim();
                users.Email = model.Email.Trim();
                // copy model to account and save
                _context.Users.Update(account);
                _context.SaveChanges();
                responseHandler.ResponseMessage = "User Updated Successfully";
                responseHandler.ResponseCode = ResponseCodes.SUCCESS;
                return responseHandler;
            }
            catch (Exception ex)
            {
                _logger.Error($"[FastCredit_CodingChallenge][Update Users][Response] => {ex.Message}");

            }
            responseHandler.ResponseCode = ResponseCodes.SYSTEM_ERROR;
            responseHandler.ResponseMessage = "An error occoured. Please try again later or contact admin for resolution";
            return responseHandler;
        }
    }
}
