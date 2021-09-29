using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using NineBizlogistics.Config;
using NineBizlogistics.Model;

namespace NineBizlogistics.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return Redirect("web/index.html");
        }
    }
}
