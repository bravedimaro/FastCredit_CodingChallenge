using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Application.Interface;
using Web.Api.DataAccess;
using Web.Domain.Helpers;
using Web.UI.Models;

namespace Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserManagementService _userManagementService;
        public HomeController(ILogger<HomeController> logger, IUserManagementService userManagementService)
        {
            _logger = logger;
            _userManagementService = userManagementService;
        }

        public async Task<IActionResult> Index()
        {
            var obj = await _userManagementService.GetAll();
            //SerializeObject response
            var Resp= JsonConvert.SerializeObject(obj.data);
            var DeSerialize = JsonConvert.DeserializeObject<List<Users>>(Resp);
            UsersRep users = new UsersRep();
            users.users = DeSerialize;
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
