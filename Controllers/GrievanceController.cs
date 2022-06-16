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
using RPFBE.Model.DBEntity;
using RPFBE.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrievanceController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;
        private readonly IOptions<WebserviceCreds> config;

        public GrievanceController(
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
        public IActionResult Index()
        {
            return Ok("");
        }

        [Authorize]
        [Route("getsystemdimensions")]
        [HttpGet]
        public async Task<IActionResult> GetSystemDim()
        {
            try { 


                List<DimensionModel> dimensionList = new List<DimensionModel>();
                var res = await codeUnitWebService.Client().GetSystemDimensionsAsync();
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                foreach (var item in resSerial)
                {
                    DimensionModel dm = new DimensionModel
                    {
                        Value = item.Value,
                        Label = item.Label,
                        Dimensionno = item.Dimensionno,
                        Dimensioncode = item.Dimensioncode
                    };
                    dimensionList.Add(dm);

                }

                //station;section;department
                var stationList = dimensionList.Where(x => x.Dimensioncode == config.Value.Station).ToList();
                var sectionList = dimensionList.Where(x => x.Dimensioncode == config.Value.Section).ToList();
                var departmentList = dimensionList.Where(x => x.Dimensioncode == config.Value.Department).ToList();


                List<EmployeeListModel> employeeList = new List<EmployeeListModel>();

                var resEmp = await codeUnitWebService.Client().EmployeeListAsync();
                dynamic resEmpSerial = JsonConvert.DeserializeObject(resEmp.return_value);

                foreach (var emp in resEmpSerial)
                {
                    EmployeeListModel e = new EmployeeListModel
                    {
                        Value = emp.No,
                        Label = emp.Fullname,
                    };
                    employeeList.Add(e);

                }


                return Ok(new { stationList, sectionList, departmentList, employeeList });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Dimension get failed: " + x.Message });
            }
        }
        //Staff create grievance and upload details
        [Authorize]
        [HttpPost]
        [Route("creategrievance")]
        public async Task<IActionResult> CreateGrievance([FromBody] GrievanceCard grievanceCard)
        {
            try
            {
                string[] textArr = new string[20];

                textArr[0] = grievanceCard.EmpID;
                textArr[1] = grievanceCard.Station;
                textArr[2] = grievanceCard.Section;
                textArr[3] = grievanceCard.Dept;
                textArr[4] = grievanceCard.Supervisor;
                textArr[5] = grievanceCard.CurrentStage;
                textArr[6] = grievanceCard.NextStage;
                textArr[7] = grievanceCard.GrievanceType;
                
                textArr[8] = grievanceCard.Subject;
                textArr[9] = grievanceCard.Description;
                textArr[10] = grievanceCard.StepTaken;
                textArr[11] = grievanceCard.Outcome;
                textArr[12] = grievanceCard.Comment;
                textArr[13] = grievanceCard.Recommendation;


                var res =await codeUnitWebService.Client().CreateGrievanceAsync(textArr, grievanceCard.GrievanceDate, grievanceCard.DateofIssue, grievanceCard.WorkEnv, grievanceCard.EmployeeRln);

                dynamic resSeria = JsonConvert.DeserializeObject(res.return_value);

               
                if (res.return_value == "false")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Initialization Failed D365 " });
                }
                else
                {
                    foreach (var item in resSeria)
                    {
                        GrievanceList grievanceList = new GrievanceList
                        {
                            GID = item.GID,
                            Employeeno = grievanceCard.EmpID,
                            Supervisor = grievanceCard.Supervisor,
                            Currentstage = grievanceCard.CurrentStage,
                            Nextstage = grievanceCard.NextStage,

                            Employeename = item.Employee,
                            Supervisorname = item.Supervisor,
                            GrievanceType = grievanceCard.GrievanceType,
                            Subject = grievanceCard.Subject,
                            Description = grievanceCard.Description,
                            StepTaken = grievanceCard.StepTaken,
                            Outcome = grievanceCard.Outcome,
                            Comment = grievanceCard.Comment,
                            Recommendation = grievanceCard.Recommendation,
                            Resolved= false
                            
                        };

                        dbContext.GrievanceList.Add(grievanceList);
                        await dbContext.SaveChangesAsync();
                        return StatusCode(StatusCodes.Status200OK, new { grievanceList.GID });
                    }
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Initialization Foreachloop Failed  " });

                }


                //dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);
                //foreach (var item in resSerial)
                //{
                //    GrievanceList grievanceList = new GrievanceList
                //    {
                //        GID = item.GID,
                //        Employeeno = item.Employeeno,
                //        Supervisor = item.Supervisor,
                //        Currentstage = item.Currentstage,
                //        Nextstage = item.Nextstage
                //    };

                //    dbContext.GrievanceList.Add(grievanceList);
                //    await dbContext.SaveChangesAsync();
                //}

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Creation Failed: " + x.Message });
            }
        }
        //Grieavance List
        [Authorize]
        [HttpGet]
        [Route("staffgrievancelist")]
        public IActionResult StaffGrievanceList()
        {
            try
            {
                var grievancelist = dbContext.GrievanceList.Where(x => x.GID != "").Select(x=>new { x.Supervisorname,x.Employeename,x.GID,x.Employeeno,x.Supervisor,x.Currentstage,x.Nextstage,x.Resolved}).ToList();
                return Ok(new { grievancelist });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance List Failed: " + x.Message });
            }
        }
        //Get Grievance Single Record
        [Authorize]
        [HttpGet]
        [Route("singlegrievance/{GID}")]
        public async Task<IActionResult> SingleGrievance(string GID)
        {
            try
            {
                var grievancesingle = dbContext.GrievanceList.Where(x => x.GID == GID).First();
                //Get the Rank Remarks.
                List<GrievanceRanksRemark> grievanceRanksRemarks = new List<GrievanceRanksRemark>();
                var rankRemaks = await codeUnitWebService.Client().GrievanceRankRemarksAsync(GID);
                dynamic rankRemarkSerial = JsonConvert.DeserializeObject(rankRemaks.return_value);
                foreach (var item in rankRemarkSerial)
                {
                    GrievanceRanksRemark grr = new GrievanceRanksRemark
                    {
                        HRrem = item.HRremark,
                        HRref = item.HRreference,

                        HODrem = item.HODremark,
                        HODref = item.HODreference,

                        HOSref = item.HOSreference,
                        HOSrem = item.HOSremark,

                        MDrem = item.MDremark,
                        MDref = item.MDreference

                    };
                    grievanceRanksRemarks.Add(grr);
                }
                return Ok(new { grievancesingle,grievanceRanksRemarks });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Single Failed: " + x.Message });
            }
        }

        //Modify/create the Rank value
        [Authorize]
        [HttpPost]
        [Route("modifyrankremarks/{GID}")]
        public async Task<IActionResult> ModifyRankRemark([FromBody] GrievanceRanksRemark ranksRemark,string GID)
        {
            try
            {
                var modifyRes = await codeUnitWebService.Client().GrievanceModifyRankRemarksAsync(GID, ranksRemark.HRrem, ranksRemark.HRref, ranksRemark.HOSrem,
                    ranksRemark.HOSref, ranksRemark.HODrem, ranksRemark.HODref, ranksRemark.MDrem, ranksRemark.MDref);
                if (modifyRes.return_value == "true")
                {
                    return StatusCode(StatusCodes.Status200OK, new { modifyRes.return_value });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Modify Single Rank Remark Failed: D365 "});
                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Modify Single Rank Remark Failed: " + x.Message });
            }
        }

        //Forward the grievance
        [Authorize]
        [HttpGet]
        [Route("forwardgrievance/{GID}")]

        public async Task<IActionResult> ForwardGrievance(string GID)
        {
            try
            {
                var res =await codeUnitWebService.Client().GrievanceForwardAsync(GID);
                dynamic resSeria = JsonConvert.DeserializeObject(res.return_value);

                var grievanceModel = dbContext.GrievanceList.Where(x => x.GID == GID).First();
                if (res.return_value == "false")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Forward Failed D365 " });
                }
                else
                {
                    foreach (var item in resSeria)
                    {


                        grievanceModel.Currentstage = item.Currentstage;
                        grievanceModel.Nextstage = item.Nextstage;

                        dbContext.GrievanceList.Update(grievanceModel);
                        await dbContext.SaveChangesAsync();

                        return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Forward Grievance Success " });
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { res.return_value });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Forward Grievance Failed: " + x.Message });
            }
        }

        //Resolve
        [Authorize]
        [HttpGet]
        [Route("resolvegrievance/{GID}")]
        public async Task<IActionResult> ResolveGrievance(string GID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var res = await codeUnitWebService.Client().GrievanceResolveAsync(GID);
                if (res.return_value=="false")
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Grievance Resolve Failed D365 " });
                }
                else
                {
                    var grievanceRecord = dbContext.GrievanceList.Where(x => x.GID == GID).First();
                    grievanceRecord.Resolver = user.Name;
                    grievanceRecord.ResolverID = user.Id;

                    dbContext.GrievanceList.Update(grievanceRecord);
                    await dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new { res.return_value });
                }
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Resolve Grievance Failed: " + x.Message });
            }
        }

        //
    }
}
