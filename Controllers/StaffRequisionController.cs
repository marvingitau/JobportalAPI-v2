using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    public class StaffRequisionController : Controller
    {
        private readonly ICodeUnitWebService codeUnitWebService;

        public StaffRequisionController( 
            ICodeUnitWebService codeUnitWebService
            )
        {
            this.codeUnitWebService = codeUnitWebService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
