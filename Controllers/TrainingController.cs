using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RPFBE.Auth;
using RPFBE.Model;
using RPFBE.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;
        private readonly IOptions<WebserviceCreds> config;
        private readonly RoleManager<IdentityRole> roleManager;

        public TrainingController(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext,
        ILogger<HomeController> logger,
        IWebHostEnvironment webHostEnvironment,
        ICodeUnitWebService codeUnitWebService,
        IMailService mailService,
        IOptions<WebserviceCreds> config,
        RoleManager<IdentityRole> roleManager
        )
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.codeUnitWebService = codeUnitWebService;
            this.mailService = mailService;
            this.config = config;
            this.roleManager = roleManager;
        }
    }
}
