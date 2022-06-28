using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RPFBE.Auth;
using RPFBE.Model;
using RPFBE.Model.Performance;
using RPFBE.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformanceEvaluationController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;
        private readonly IOptions<WebserviceCreds> config;

        public PerformanceEvaluationController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment,
            ICodeUnitWebService codeUnitWebService,
            IMailService mailService,
            IOptions<WebserviceCreds> config

        )
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.codeUnitWebService = codeUnitWebService;
            this.mailService = mailService;
            this.config = config;
        }

        //Employee Appraisals
        [Authorize]
        [HttpGet]
        [Route("getemployeeappraisals")]
        public async Task<IActionResult> GetEmployeeAppraisal()
        {
            try
            {
                List<EmployeeAppraisalList> employeeAppraisalLists = new List<EmployeeAppraisalList>();
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var apprRes = await codeUnitWebService.HRWS().GetEmployeePerformanceAppraisalEmployeeNoAsync(user.EmployeeId);
                dynamic apprSerial = JsonConvert.DeserializeObject(apprRes.return_value);
                foreach (var item in apprSerial.EmployeeAppraisals)
                {
                    EmployeeAppraisalList eal = new EmployeeAppraisalList
                    {
                        No = item.No,
                        EmployeeNo = item.EmployeeNo,
                        KPICode = item.KPICode,
                        EmployeeName = item.EmployeeName,
                        EmployeeDesgnation = item.EmployeeDesgnation,
                        JobTitle = item.JobTitle,
                        ManagerNo = item.ManagerNo,
                        ManagerName = item.ManagerName,
                        ManagerDesignation = item.ManagerDesignation,
                        AppraisalPeriod = item.AppraisalPeriod,
                        AppraisalStartPeriod = item.AppraisalStartPeriod,
                        AppraisalEndPeriod = item.AppraisalEndPeriod,
                        AppraisalLevel = item.AppraisalLevel,
                    };
                    employeeAppraisalLists.Add(eal);
                }
                return Ok(new { employeeAppraisalLists });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Get Job Performance KPIs failed: " + x.Message });

            }
        }

        //Get Employee Appraisal Performance Standard List
        // All the KPI stds
        [Authorize]
        [HttpGet]
        [Route("getappraisalperformancestd/{AppraisalNo}")]
        public async Task<IActionResult> GetAppraisalPerformancestd(string AppraisalNo)
        {
            try
            {
                List<EmployeeAppraisalStandard> employeeAppraisalStandards = new List<EmployeeAppraisalStandard>();
                var resL = await codeUnitWebService.HRWS().GetEmployeePerformanceAppraisalIndicatorsAsync(AppraisalNo);
                dynamic esSer = JsonConvert.DeserializeObject(resL.return_value);

                foreach (var item in esSer)
                {
                    EmployeeAppraisalStandard eas = new EmployeeAppraisalStandard
                    {
                        CriteriaCode = item.CriteriaCode,
                        TargetCode = item.TargetCode,
                        KPIDescription = item.KPIDescription,
                        HeaderNo = item.HeaderNo,
                        StandardCode = item.StandardCode,
                        IndicatorCode = item.IndicatorCode,
                        StandardDescription = item.StandardDescription,
                        StandardWeighting = item.StandardWeighting,
                        Timelines = item.Timelines,
                        ActivityDescription = item.ActivityDescription,
                        TargetedScore = item.TargetedScore,
                        AchievedScoreEmployee = item.AchievedScoreEmployee,
                        EmployeeComments = item.EmployeeComments,
                    };
                    employeeAppraisalStandards.Add(eas);
                }
                return Ok(new { employeeAppraisalStandards });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Get Employee Appraisal Perfomance Std failed: " + x.Message });

            }
        }

        //Get Employee Appraisal Performance Standard List
        // Filtere the KPI stds

        [Authorize]
        [HttpGet]
        [Route("getappraisalperformancestdperkpi/{KPICode}/{HeaderNo}")]
        public async Task<IActionResult> GetAppraisalPerformancestdPerKPI(int KPICode, string HeaderNo)
        {
            try
            {
                List<EmployeeAppraisalStandard> employeeAppraisalStandards = new List<EmployeeAppraisalStandard>();
                var resL = await codeUnitWebService.HRWS().GetEmployeePerformanceAppraisalIndicatorsPerKPIAsync(KPICode, HeaderNo);
                dynamic esSer = JsonConvert.DeserializeObject(resL.return_value);

                foreach (var item in esSer)
                {
                    EmployeeAppraisalStandard eas = new EmployeeAppraisalStandard
                    {
                        CriteriaCode = item.CriteriaCode,
                        TargetCode = item.TargetCode,
                        KPIDescription = item.KPIDescription,
                        HeaderNo = item.HeaderNo,
                        StandardCode = item.StandardCode,
                        IndicatorCode = item.IndicatorCode,
                        StandardDescription = item.StandardDescription,
                        StandardWeighting = item.StandardWeighting,
                        Timelines = item.Timelines,
                        ActivityDescription = item.ActivityDescription,
                        TargetedScore = item.TargetedScore,
                        AchievedScoreEmployee = item.AchievedScoreEmployee,
                        EmployeeComments = item.EmployeeComments,
                    };
                    employeeAppraisalStandards.Add(eas);
                }
                return Ok(new { employeeAppraisalStandards });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Get Employee Appraisal Perfomance Std failed: " + x.Message });

            }
        }

    }

}
