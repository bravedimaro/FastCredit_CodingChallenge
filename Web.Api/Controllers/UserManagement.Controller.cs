using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Application.Interface;
using Web.Domain.Accounts;

namespace Web.Api.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserManagement : ControllerBase
    {
        private IWebHostEnvironment _Environment;
        private IUserManagementService _userManagementService;
        public UserManagement(IWebHostEnvironment webHostEnvironment, IUserManagementService userManagementService)
        {
            _Environment = webHostEnvironment;
            _userManagementService = userManagementService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> UploadAddress([FromForm] CreateRequest model)
        {
            string contentPath = this._Environment.ContentRootPath;
            var obj = await _userManagementService.Create(model, contentPath);
            if (obj.ResponseCode=="00")
            {
                return Ok(obj);
            }
            else if(obj.ResponseCode == "01")
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError };
            }
           
        }
        [Authorize]
        [HttpGet("GetAll")]
        public async Task<IActionResult> AllUsers()
        {
           
            var obj = await _userManagementService.GetAll();
            if (obj.ResponseCode == "00")
            {
                return Ok(obj);
            }
            else if (obj.ResponseCode == "01")
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
        [Authorize]
        [HttpGet("GetAll/{Id}")]
        public async Task<IActionResult> GetAllById(int Id)
        {

            var obj = await _userManagementService.GetAllById(Id);
            if (obj.ResponseCode == "00")
            {
                return Ok(obj);
            }
            else if (obj.ResponseCode == "01")
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
        [Authorize]
        [HttpPost("Delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {

            var obj = await _userManagementService.Delete(Id);
            if (obj.ResponseCode == "00")
            {
                return Ok(obj);
            }
            else if (obj.ResponseCode == "01")
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
        [Authorize]
        [HttpPost("DeleteBySelected/{Ids}")]
        public async Task<IActionResult> DeleteBySelected(string Ids)
        {

            var obj = await _userManagementService.DeleteBySelected(Ids);
            if (obj.ResponseCode == "00")
            {
                return Ok(obj);
            }
            else if (obj.ResponseCode == "01")
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
       
        [Authorize]
        [HttpPost("Update/{Id}")]
        public async Task<IActionResult> UpdateRequest(int Id,[FromBody] UpdateRequest updateRequest)
        {

            var obj = await _userManagementService.Update(Id, updateRequest);
            if (obj.ResponseCode == "00")
            {
                return Ok(obj);
            }
            else if (obj.ResponseCode == "01")
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                return new ObjectResult(obj) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
    }
}
