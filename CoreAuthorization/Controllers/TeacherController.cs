using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreAuthorization.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreAuthorization.Controllers
{
    [Authorize(Roles = "老师,校长")]
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return Content("Teacher index");
        }

        [Authorize(Policy="王老师")]
        public IActionResult Edit()
        {
            return Content("Edit success");
        }

    }
}
