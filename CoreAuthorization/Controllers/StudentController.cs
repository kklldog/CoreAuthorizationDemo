﻿using System;
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
    [Authorize(Policy = "女老师")]
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return Content("Student index");
        }

    }
}
