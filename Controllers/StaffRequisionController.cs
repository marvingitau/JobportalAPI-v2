using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
   // [EnableCors("CorsPolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class StaffRequisionController : Controller
    {
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IMailService mailService;
        private readonly ILogger<StaffRequisionController> logger;

        public StaffRequisionController( 
            ICodeUnitWebService codeUnitWebService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            IMailService mailService,
            ILogger<StaffRequisionController> logger
        )
        {
            this.codeUnitWebService = codeUnitWebService;
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.mailService = mailService;
            this.logger = logger;
        }
        [Authorize]
        [Route("jobslist")]
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                List<JobList> jobLists = new List<JobList>();
                var res = await codeUnitWebService.Client().GetJobsAsync();
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                foreach (var item in resSerial)
                {
                    JobList jl = new JobList
                    {
                        Label = item.Title,
                        Value = item.No,
                    };
                    jobLists.Add(jl);
                }

                return Ok(new { jobLists });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "No Jobs" });
            }
        }

        [Authorize]
        [Route("getempreqcode/{Jobcode}")]
        [HttpGet]
        public async Task<IActionResult> GetEmpReqCode(string Jobcode)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            //create level 0 at this stage
            try
            {
                var res = await codeUnitWebService.Client().PostEmpRequisitionAsync(Jobcode, user.EmployeeId);
                return Ok(res.return_value);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "No Employee Requsition created" });
            }
           
        }

        [Authorize]
        [Route("getempreqsourcedata/{Reqcode}")]
        [HttpGet]
        public async Task<IActionResult> GetSourcedata(string Reqcode)
        {
            try
            {
                List<ChecklistModel> jobCheckList = new List<ChecklistModel>();
                List<EmployeeListModel> employeeListModels = new List<EmployeeListModel>();
                List<ContractListModel> contractListModels = new List<ContractListModel>();
                List<DepartmentListModel> departmentListModels = new List<DepartmentListModel>();

                //Req card data
                var reqcard = await codeUnitWebService.Client().GetRequisitionCardAsync(Reqcode);
                dynamic reqcardSerial = JsonConvert.DeserializeObject(reqcard.return_value);
                RequsitionCardModel requsitionGeneral = new RequsitionCardModel
                {
                    No = reqcardSerial.No,
                    Jobno = reqcardSerial.Jobno,
                    Jobtitle = reqcardSerial.Jobtitle,
                    Description = reqcardSerial.Description,
                    Jobgrade = reqcardSerial.Jobgrade,
                    Maxposition = reqcardSerial.Maxposition,
                    Occupiedposition = reqcardSerial.Occupiedposition,
                    Vacantposition = reqcardSerial.Vacantposition,
                    Requestedemployees = reqcardSerial.Requestedemployees,
                    Closingdate = reqcardSerial.Closingdate,
                    Requisitiontype = reqcardSerial.Requisitiontype,
                    Contractcode = reqcardSerial.Contractcode,
                    Reason = reqcardSerial.Reason,
                    Branchcode = reqcardSerial.Branchcode,
                    Jobadvertised = reqcardSerial.Jobadvertised,
                    Jobadvertiseddropped = reqcardSerial.Jobadvertiseddropped,
                    Status = reqcardSerial.Status,
                    // Userid = reqcardSerial.Userid,
                    Comments = reqcardSerial.Comments,
                    Documentdate = reqcardSerial.Documentdate,
                    Desiredstartdate = reqcardSerial.Desiredstartdate,
                    Employeetoreplace = reqcardSerial.Employeetoreplace,
                    HOD = reqcardSerial.HOD,
                    HR = reqcardSerial.HR,
                    MD = reqcardSerial.MD
                };

                //Get Checklist

                var checkList = await codeUnitWebService.Client().GetChecklistAsync(Reqcode);
                dynamic ChecklistSerial = JsonConvert.DeserializeObject(checkList.return_value);

                foreach (var task in ChecklistSerial)
                {
                    ChecklistModel checkModel = new ChecklistModel
                    {
                        ReqNo = task.ReqNo,
                        Code = task.DocCode,
                        Description = task.Description
                    };
                    jobCheckList.Add(checkModel);
                }

                //Get Contract codes
                var contractList = await codeUnitWebService.Client().GetContractCodeAsync();
                dynamic contractListSerial = JsonConvert.DeserializeObject(contractList.return_value);
                foreach(var contr in contractListSerial)
                {
                    ContractListModel listModel = new ContractListModel
                    {
                        Value = contr.Code,
                        Label = contr.Description
                    };
                    contractListModels.Add(listModel);
                }
                //Get Departments
                var departmentList = await codeUnitWebService.Client().GetDepartmentAsync();
                dynamic departmentListSerial = JsonConvert.DeserializeObject(departmentList.return_value);
                foreach (var dep in departmentListSerial)
                {
                    DepartmentListModel department = new DepartmentListModel()
                    {
                        Value = dep.Code,
                        Label = dep.Description
                    };
                    departmentListModels.Add(department);
                }
                //Get Employee list
                var employeeList = await codeUnitWebService.Client().EmployeeListAsync();
                dynamic employeeListSerial = JsonConvert.DeserializeObject(employeeList.return_value);
                foreach(var emp in employeeListSerial)
                {
                    EmployeeListModel employee = new EmployeeListModel
                    {
                        Value = emp.No,
                        Label = emp.Fullname
                    };
                    employeeListModels.Add(employee);
                }

                return Ok(new { jobCheckList, employeeListModels,contractListModels, departmentListModels, requsitionGeneral });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Issue when getting requisition sdata" });

            }
        }

        //Get Category 5 as bulk
        [Authorize]
        [Route("getcategoryfive/{Jobno}/{Reqno}")]
        [HttpGet]
        public async Task<IActionResult> GetCatfive(string Jobno,string Reqno)
        {
            try
            {
                List<JobQualificationModel> qualificationModels = new List<JobQualificationModel>();
                List<JobRequirementModel> requirementModels = new List<JobRequirementModel>();
                List<JobResponsibilityModel> responsibilityModels = new List<JobResponsibilityModel>();
                List<JobChecklistModel> checklistModels = new List<JobChecklistModel>();

                var qualif = await codeUnitWebService.Client().GetJobQualificationAsync(Jobno);
                dynamic qualifSerial = JsonConvert.DeserializeObject(qualif.return_value);

                foreach (var qua in qualifSerial)
                {
                    JobQualificationModel job = new JobQualificationModel
                    {
                        Jobno = qua.No,
                        Lineno = qua.Lineno,
                        Description = qua.Description,
                        Qficationcode = qua.Qualificationcode,
                        Mandantory = qua.Mandatory
                    };
                    qualificationModels.Add(job);

                }

                var requems = await codeUnitWebService.Client().GetJobRequirementAsync(Jobno);
                dynamic requemSerial = JsonConvert.DeserializeObject(requems.return_value);

                foreach (var req in requemSerial)
                {
                    JobRequirementModel jobRequirement = new JobRequirementModel
                    {
                       // Id = "",
                        Jobno = req.No,
                        Lineno = req.Lineno,
                        Rqmentcode = req.Requirementcode,
                        Description = req.Description,
                        Mandatory = req.Mandatory
                    };

                    requirementModels.Add(jobRequirement);

                }

                var responsibility = await codeUnitWebService.Client().GetJobResponsibilityAsync(Jobno);
                dynamic respSerial = JsonConvert.DeserializeObject(responsibility.return_value);

                foreach (var respb in respSerial)
                {
                    JobResponsibilityModel jobResponsibility = new JobResponsibilityModel
                    {
                        Jobno = respb.No,
                        Lineno = respb.Lineno,
                        Responsibilitycode = respb.Responsibilitycode,
                        Description = respb.Description
                    };

                    responsibilityModels.Add(jobResponsibility);

                }

                var checklistCode = await codeUnitWebService.Client().GetChecklistAsync(Reqno);
                dynamic checkistSerial = JsonConvert.DeserializeObject(checklistCode.return_value);
                foreach (var check in checkistSerial)
                {
                    JobChecklistModel job = new JobChecklistModel
                    {
                        Lineno = check.Lineno,
                        Reqno = check.ReqNo,
                        Code = check.DocCode,
                        Description = check.Description,
                    };
                    checklistModels.Add(job);
                }

                var checklistInitCode = await codeUnitWebService.Client().GetChecklistCodeAsync();
                var checklistInitCodeAux = checklistInitCode.return_value;

                return Ok(new { qualificationModels, requirementModels , responsibilityModels, checklistModels, checklistInitCodeAux});
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Issue when getting category five" });
            }
        }


        //Kisha individual methods on modification

        //remove qualification
        [Authorize]
        [Route("removequalification/{No}")]
        [HttpGet]
        public async Task<IActionResult> Removequalification(int No)
        {
            try
            {
                var response = await codeUnitWebService.Client().RemJobQualificationAsync(No);
                return Ok(response.return_value);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Delete failed" });
            }
        }

        //Add a new qualification
        [Authorize]
        [Route("addqualification/{Jobno}")]
        [HttpPost]
        public async Task<IActionResult> Addqualification([FromBody] JobQualificationModel job,string Jobno)
        {
            try
            {
                bool auxMandatory = job.Mandantory == "Yes" ? true : false;
                if(!string.IsNullOrEmpty(job.Lineno))
                {
                    //Update
                    var response = await codeUnitWebService.Client().UpdateJobQualificationAsync(job.Description, int.Parse(job.Lineno), auxMandatory);
                    return Ok(response.return_value);
                }
                else
                {
                   // new
                    var response = await codeUnitWebService.Client().SetJobQualificationAsync(job.Description, Jobno, auxMandatory, job.Qficationcode);
                    return Ok(response.return_value);

                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Uploading Failed "+x });
            }
        }

        //remove requirement
        [Authorize]
        [Route("removerequirement/{No}")]
        [HttpGet]
        public async Task<IActionResult> Removerequirement(int No)
        {
            try
            {
                var response = await codeUnitWebService.Client().RemJobRequirementAsync(No);
                return Ok(response.return_value);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Delete failed" });
            }
        }

        //Add a new add requirement
        [Authorize]
        [Route("addrequirement/{Jobno}")]
        [HttpPost]
        public async Task<IActionResult> Addrequirement([FromBody] JobRequirementModel job,string Jobno)
        {
            try
            {
                bool auxMandatory = job.Mandatory == "Yes" ? true : false;
                if (string.IsNullOrEmpty(job.Lineno))
                {
                    //create new
                    var response = await codeUnitWebService.Client().SetJobRequirementAsync(job.Description, job.Rqmentcode, auxMandatory, Jobno);
                    return Ok(response.return_value);
                }
                else
                {
                    //update
                    var response = await codeUnitWebService.Client().UpdateJobRequirementAsync(job.Description,int.Parse(job.Lineno),auxMandatory);
                    return Ok(response.return_value);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Uploading Failed" });
            }
        }


        //remove responsibility
        [Authorize]
        [Route("removeresponsibility/{No}")]
        [HttpGet]
        public async Task<IActionResult> Removeresponsibility(int No)
        {
            try
            {
                var response = await codeUnitWebService.Client().RemJobResponsibilityAsync(No);
                return Ok(response.return_value);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Delete failed" });
            }
        }

        //Add a new add responsibility
        [Authorize]
        [Route("addresponsibility")]
        [HttpPost]
        public async Task<IActionResult> Addresponsibiity([FromBody] JobResponsibilityModel job)
        {
            try
            {
                //bool auxMandatory = job.Mandatory == "Yes" ? true : false;
                if (job.Lineno == "" || job.Lineno == "NA")
                {
                    //create new
                    var response = await codeUnitWebService.Client().SetJobResponsibilityAsync(job.Description, job.Jobno);
                    return Ok(response.return_value);
                }
                else
                {
                    //update
                    var response = await codeUnitWebService.Client().UpdateJobResponsibilityAsync(job.Description, int.Parse(job.Lineno));
                    return Ok(response.return_value);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Uploading Failed" });
            }
        }


        //get checklist code
        //

        [Authorize]
        [Route("getchecklistcode")]
        [HttpGet]
        public async Task<IActionResult> Getcheckcode()
        {
            try
            {
                var response = await codeUnitWebService.Client().GetChecklistCodeAsync();
                return Ok(response.return_value);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Delete failed" });
            }
        }
        //remove checklist
        [Authorize]
        [Route("removereschecklist/{No}")]
        [HttpGet]
        public async Task<IActionResult> Removechecklist(int No)
        {
            try
            {
                var response = await codeUnitWebService.Client().RemMandatoryDocsAsync(No);
                return Ok(response.return_value);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Delete failed" });
            }
        }

        //Add a new add responsibility
        [Authorize]
        [Route("addcheck")]
        [HttpPost]
        public async Task<IActionResult> Addcheckist([FromBody] JobChecklistModel job)
        {
            //return Ok(job);
            try
            {
                //bool auxMandatory = job.Mandatory == "Yes" ? true : false;
                if (job.Lineno == null || job.Lineno == "")
                {
                    //create new
                    var response = await codeUnitWebService.Client().SetMandatoryDocsAsync(job.Code, job.Description, job.Reqno);
                    return Ok(response.return_value);
                    //return Ok(job);
                }
                else
                {
                    //update
                    var response = await codeUnitWebService.Client().ModifyMandatoryDocsAsync(int.Parse(job.Lineno),job.Description);
                    return Ok(response.return_value);
                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Uploading Failed: " +x.Message});
            }
        }

        //Update the Checbox indicating checks
        //enable attachement
        [Authorize]
        [Route("enableattachment/{Reqno}")]
        [HttpGet]
        public async Task<IActionResult> Enablecheck(string Reqno)
        {
            try
            {
                var res =await  codeUnitWebService.Client().SetChecklistMandatoryRequiredAsync(Reqno);
                return Ok(res.return_value);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "File Attachement Enabling Failed" });
            }
        }

        //HOD function
        [Authorize]
        [Route("updaterequisition/{Reqno}")]
        [HttpPost]
        public async Task<IActionResult> UpdateRequision([FromBody] RequisionModel requisionModel,string Reqno)
        {
            //return Ok(requisionModel);
            try
            {
                string[] reqArr = new string[20];
                int[] reqArrInt = new int[10];
                reqArr[0] = string.IsNullOrEmpty(requisionModel.Requisiontype)?"": requisionModel.Requisiontype;
                reqArr[1] = string.IsNullOrEmpty(requisionModel.Contracttype)?"":requisionModel.Contracttype;
                reqArr[2] = string.IsNullOrEmpty(requisionModel.Department)?"": requisionModel.Department;
                reqArr[3] = string.IsNullOrEmpty(requisionModel.Employeereplaced)?"": requisionModel.Employeereplaced;
                reqArr[4] = string.IsNullOrEmpty(requisionModel.HOD)?"": requisionModel.HOD;
                reqArr[5] = string.IsNullOrEmpty(requisionModel.HRManager)?"":requisionModel.HRManager;
                reqArr[6] = string.IsNullOrEmpty(requisionModel.MD)?"": requisionModel.MD;
                reqArr[7] = string.IsNullOrEmpty(requisionModel.Description) ?"": requisionModel.Description;
                reqArr[8] = string.IsNullOrEmpty(requisionModel.Reason)?"":requisionModel.Reason;
                reqArr[9] = string.IsNullOrEmpty(requisionModel.Comment)?"":requisionModel.Comment;
                reqArrInt[0]= string.IsNullOrEmpty(requisionModel.RequestedNo) ? 0 : Int32.Parse(requisionModel.RequestedNo);

                //return Ok(reqArr); 
                //return Ok(reqArr);
                var response = await codeUnitWebService.Client().ModifyEmpRequisitionAsync(Reqno, requisionModel.Startdate, requisionModel.Enddate ,reqArr, reqArrInt);

                return Ok(response.return_value);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Update failed "+x.Message });
            }
        }
        //HOD Dashboard
        [Authorize]
        [Route("hoddashboard")]
        [HttpGet]
        public IActionResult HODdashboard()
        {
            try
            {
                var pendingCount = dbContext.RequisitionProgress.Where(x => x.ProgressStatus <= 3).Count();
                var viewedCount = dbContext.RequisitionProgress.Where(x => x.ProgressStatus == 4).Count();

                return Ok(new { viewedCount, pendingCount });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "MD Dashboard failed " + x.Message });
            }
        }
        //HOD Req list
        [Authorize]
        [Route("hodgetreqlist")]
        [HttpGet]
        public IActionResult HODReqList()
        {
            try
            {
                var list = dbContext.RequisitionProgress.Where(x => x.ProgressStatus >= 0).ToList();
                return Ok(list);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "MD Stage Requsition List failed " + x.Message });
            }
        }


        //HOD push to HR
        [Authorize]
        [Route("mdpushtohr")]
        [HttpPost]
        public async Task<IActionResult> MDPushtoHR(PushtoHRModel pushtoHR)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;
                var result = await codeUnitWebService.Client().GetJobDetailsAsync(pushtoHR.Jobno);
                dynamic resSerial = JsonConvert.DeserializeObject(result.return_value);

                int exist = dbContext.RequisitionProgress.Where(x => x.ReqID == pushtoHR.Reqno).Count();
                if(exist == 0) {
                    RequisitionProgress requisitionProgress = new RequisitionProgress
                    {
                        UID = user.Id,
                        ReqID = pushtoHR.Reqno,
                        JobNo = resSerial.Jobno,
                        JobTitle = resSerial.Jobtitle,
                        JobGrade = resSerial.Jobgrade,
                        RequestedEmployees = pushtoHR.RequestedEmployees,
                        ClosingDate = pushtoHR.ClosingDate,
                        Status = resSerial.Status,
                        ProgressStatus = 1,
                        UIDComment = pushtoHR.HODcomment,
                    };


                    dbContext.RequisitionProgress.Add(requisitionProgress);
                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    var reqProg = dbContext.RequisitionProgress.Where(x => x.ReqID == pushtoHR.Reqno).FirstOrDefault();
                    reqProg.JobNo = resSerial.Jobno;
                    reqProg.JobTitle = resSerial.Jobtitle;
                    reqProg.JobGrade = resSerial.Jobgrade;
                    reqProg.RequestedEmployees = pushtoHR.RequestedEmployees;
                    reqProg.ClosingDate = pushtoHR.ClosingDate;
                    reqProg.Status = resSerial.Status;
                    reqProg.ProgressStatus = 1;
                    reqProg.UIDComment = pushtoHR.HODcomment;
                    dbContext.RequisitionProgress.Update(reqProg);
                    await dbContext.SaveChangesAsync();
                }
          

                //@email
                var mailHRRes = await codeUnitWebService.WSMailer().StaffRequisitiontoHRfromHODAsync(pushtoHR.Reqno);
                logger.LogInformation($"User:{user.EmployeeId},Verb:{verb},Path:HOD Pushed Requsition to HEAD-HR Success");

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requisition pushed" });
            }
            catch (Exception x)
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;

                logger.LogError($"User:{user.EmployeeId},Verb:{verb},Action:HOD Pushed Requsition to HEAD-HR failed,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Requsition Progression failed " + x.Message });
            }
        }

        /*HR to MD
         * Get list 
         * Get single
         */
        [Authorize]
        [Route("headhrgetreqlist")]
        [HttpGet]
        public  async Task<IActionResult> HeadHrReqList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var list = dbContext.RequisitionProgress.Where(x => x.ProgressStatus == 1 || x.ProgressStatus >= 3).ToList();
                logger.LogInformation($"User:{user.EmployeeId},Verb:GET,Path:HEAD HR Get Requsition List Success");
                return Ok(list);
            }
            catch (Exception x)
            {
                logger.LogError($"User:NAp,Verb:GET,Action:HEAD HR Stage Requsition List failed,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "HEAD HR Stage Requsition List failed " + x.Message });
            }
        }

        [Authorize]
        [Route("hrgetreqlist")]
        [HttpGet]
        public async Task<IActionResult> HrReqList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var list = dbContext.RequisitionProgress.Where(x => x.ProgressStatus >= 3).ToList();
                logger.LogInformation($"User:{user.EmployeeId},Verb:GET,Path:HR Get Requsition List Success");
                return Ok(list);
            }
            catch (Exception x)
            {
                logger.LogError($"User:NAp,Verb:GET,Action:HR Stage Requsition List failed,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "HR Stage Requsition List failed " + x.Message });
            }
        }

        [Authorize]
        [Route("hrgetreqsingle/{Reqno}/{Jobno}")]
        [HttpGet]
        public async Task<IActionResult> HrReqSingle(string Reqno,string Jobno)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;
                //get all about the req card
                var statusProgress = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).FirstOrDefault().ProgressStatus;

                //Geneneral Data
                List<EmployeeListModel> employeeListModels = new List<EmployeeListModel>();
                List<JobQualificationModel> qualificationModels = new List<JobQualificationModel>();
                List<JobRequirementModel> requirementModels = new List<JobRequirementModel>();
                List<JobResponsibilityModel> responsibilityModels = new List<JobResponsibilityModel>();
                List<JobChecklistModel> checklistModels = new List<JobChecklistModel>();

                //Req card data
                var reqcard = await codeUnitWebService.Client().GetRequisitionCardAsync(Reqno);
                dynamic reqcardSerial = JsonConvert.DeserializeObject(reqcard.return_value);
                RequsitionCardModel requsitionCard = new RequsitionCardModel
                {
                    No = reqcardSerial.No,
                    Jobno = reqcardSerial.Jobno,
                    Jobtitle = reqcardSerial.Jobtitle,
                    Description = reqcardSerial.Description,
                    Jobgrade = reqcardSerial.Jobgrade,
                    Maxposition = reqcardSerial.Maxposition,
                    Occupiedposition = reqcardSerial.Occupiedposition,
                    Vacantposition = reqcardSerial.Vacantposition,
                    Requestedemployees = reqcardSerial.Requestedemployees,
                    Closingdate = reqcardSerial.Closingdate,
                    Requisitiontype = reqcardSerial.Requisitiontype,
                    Contractcode = reqcardSerial.Contractcode,
                    Reason = reqcardSerial.Reason,
                    Branchcode = reqcardSerial.Branchcode,
                    Jobadvertised = reqcardSerial.Jobadvertised,
                    Jobadvertiseddropped = reqcardSerial.Jobadvertiseddropped,
                    Status = reqcardSerial.Status,
                   // Userid = reqcardSerial.Userid,
                    Comments = reqcardSerial.Comments,
                    Documentdate = reqcardSerial.Documentdate,
                    Desiredstartdate = reqcardSerial.Desiredstartdate,
                    Employeetoreplace = reqcardSerial.Employeetoreplace,
                    HOD = reqcardSerial.HOD,
                    HR = reqcardSerial.HR,
                    MD = reqcardSerial.MD
                };



                //Get Employee list
                var employeeList = await codeUnitWebService.Client().EmployeeListAsync();
                dynamic employeeListSerial = JsonConvert.DeserializeObject(employeeList.return_value);
                foreach (var emp in employeeListSerial)
                {
                    EmployeeListModel employee = new EmployeeListModel
                    {
                        Value = emp.No,
                        Label = emp.Fullname
                    };
                    employeeListModels.Add(employee);
                }



                //Cat5 data
                var qualif = await codeUnitWebService.Client().GetJobQualificationAsync(Jobno);
                dynamic qualifSerial = JsonConvert.DeserializeObject(qualif.return_value);

                foreach (var qua in qualifSerial)
                {
                    JobQualificationModel job = new JobQualificationModel
                    {
                        Jobno = qua.No,
                        Lineno = qua.Lineno,
                        Description = qua.Description,
                        Qficationcode = qua.Qualificationcode,
                        Mandantory = qua.Mandatory
                    };
                    qualificationModels.Add(job);

                }

                var requems = await codeUnitWebService.Client().GetJobRequirementAsync(Jobno);
                dynamic requemSerial = JsonConvert.DeserializeObject(requems.return_value);

                foreach (var req in requemSerial)
                {
                    JobRequirementModel jobRequirement = new JobRequirementModel
                    {
                        // Id = "",
                        Jobno = req.No,
                        Lineno = req.Lineno,
                        Rqmentcode = req.Requirementcode,
                        Description = req.Description,
                        Mandatory = req.Mandatory
                    };

                    requirementModels.Add(jobRequirement);

                }

                var responsibility = await codeUnitWebService.Client().GetJobResponsibilityAsync(Jobno);
                dynamic respSerial = JsonConvert.DeserializeObject(responsibility.return_value);

                foreach (var respb in respSerial)
                {
                    JobResponsibilityModel jobResponsibility = new JobResponsibilityModel
                    {
                        Jobno = respb.No,
                        Lineno = respb.Lineno,
                        Responsibilitycode = respb.Responsibilitycode,
                        Description = respb.Description
                    };

                    responsibilityModels.Add(jobResponsibility);

                }

                var checklistCode = await codeUnitWebService.Client().GetChecklistAsync(Reqno);
                dynamic checkistSerial = JsonConvert.DeserializeObject(checklistCode.return_value);
                foreach (var check in checkistSerial)
                {
                    JobChecklistModel job = new JobChecklistModel
                    {
                        Lineno = check.Lineno,
                        Reqno = check.ReqNo,
                        Code = check.DocCode,
                        Description = check.Description,
                    };
                    checklistModels.Add(job);
                }

                var checklistInitCode = await codeUnitWebService.Client().GetChecklistCodeAsync();
                var checklistInitCodeAux = checklistInitCode.return_value;

                var EmployeeReplaced = employeeListModels.Where(x => x.Value == requsitionCard.Employeetoreplace).FirstOrDefault();
                var HR = employeeListModels.Where(x => x.Value == requsitionCard.HR).FirstOrDefault();
                var HOD = employeeListModels.Where(x => x.Value == requsitionCard.HOD).FirstOrDefault();
                var MD = employeeListModels.Where(x => x.Value == requsitionCard.MD).FirstOrDefault();


                logger.LogInformation($"User:{user.EmployeeId},Verb:{verb},Path:HR Get Requsition Card Success");
                return Ok(new { statusProgress, EmployeeReplaced, HR, HOD,MD,qualificationModels, requirementModels, responsibilityModels, checklistModels, checklistInitCodeAux, requsitionCard });
            }
            catch (Exception x)
            {
                logger.LogError($"User:NAp,Verb:GET,Action:HR Stage Requsition Single failed,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Requsition Single failed " + x.Message });
            }
        }


        //HR Push to MD
        [Authorize]
        [Route("hrsendmd/{Reqno}")]
        [HttpPost]
        public async Task<IActionResult> HrSendMD([FromBody] PushtoHRModel pushtoHR,string Reqno)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;

                RequisitionProgress reqModel = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).FirstOrDefault();
                //RequisitionProgress reqModel = new RequisitionProgress
                //{
                //    ReqID = Reqno,
                //    ProgressStatus = 2
                //};
                reqModel.ProgressStatus = 2;
                reqModel.UIDTwo = user.Id;
                reqModel.UIDTwoComment = pushtoHR.HRcomment;
                dbContext.RequisitionProgress.Update(reqModel);
                await dbContext.SaveChangesAsync();

                //@email

                var emailMD = await codeUnitWebService.WSMailer().StaffRequisitiontoMDfromHRAsync(Reqno);
                //send Email to MD
                //var mdUser = dbContext.Users.Where(x => x.Id == reqModel.UIDTwo).First();
                //Requisitionrequest requisitionrequest = new Requisitionrequest
                //{
                //    RequisionNo = Reqno,
                //    ToEmail = hrUser.Email,
                //    Username = hrUser.UserName
                //};
                //await mailService.RequisitionRequestAsync(requisitionrequest);

                logger.LogInformation($"User:{user.EmployeeId},Verb:{verb},Path:Requisition Card Pushed to MD Success");
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requisition pushed to MD" });
            }
            catch (Exception x)
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;
                logger.LogError($"User:{user.EmployeeId},Verb:{verb},Action:Head HR Requisition Card Pushed to MD failed,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "HR Send to MD failed " + x.Message });
            }
        }
        //HR function
        [Authorize]
        [Route("approveandpublish/{Reqno}")]
        [HttpPost]
        public async Task<IActionResult> Approveupdate([FromBody] PushtoHRModel pushtoHR,string Reqno)
        {
            try
            {
                //return Ok(Reqno);
                var responser = await codeUnitWebService.Client().ApprovePublishAsync(Reqno);
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;
                if(responser.return_value == "Yes")
                {
                    RequisitionProgress reqModel = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).First();
                    reqModel.ProgressStatus = 4;
                    reqModel.UIDFour = user.Id;
                    reqModel.UIDTwoComment = pushtoHR.HRcomment;
                    dbContext.RequisitionProgress.Update(reqModel);
                    await dbContext.SaveChangesAsync();

                    //@email
                    var hrApprovalEmail = await codeUnitWebService .WSMailer().StaffRequisitionHRApprovalAsync(Reqno);
                    logger.LogInformation($"User:{user.EmployeeId},Verb:{verb},Path:Job Publish Success");
                    return Ok(responser.return_value);
                }
                else
                {
                    logger.LogInformation($"User:{user.EmployeeId},Verb:{verb},Path:Job Publish Failure");
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Approve Publish failed" });
                }
               
            }
            catch (Exception x)
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                logger.LogError($"User:{user.EmployeeId},Verb:POST,Action:Job Publish Failure,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Approve Publish failed: "+x.Message });
            }
        }

        //MD Rejected
        [Authorize]
        [Route("rejectreq/{Reqno}")]
        [HttpPost]
        public async Task<IActionResult> RejectReq([FromBody] PushtoHRModel rejecttoHR,string Reqno)
        {
            try
            {
                //return Ok(Reqno);
                var responser = await codeUnitWebService.Client().RejectRequisitionAsync(Reqno);
                var verb = Request.HttpContext.Request.Method;

                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (responser.return_value == "Yes")
                {
                    RequisitionProgress reqModel = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).FirstOrDefault();
                    reqModel.ProgressStatus = 5;
                    reqModel.UIDFour = user.Id;
                    reqModel.Status = "Rejected";
                    reqModel.UIDThreeComment = rejecttoHR.MDcomment;
                    reqModel.UIDFourComment = rejecttoHR.MDcomment;
                    dbContext.RequisitionProgress.Update(reqModel);
                    await dbContext.SaveChangesAsync();

                    //@email
                    var mdRejectMail = await codeUnitWebService.WSMailer().StaffRequisitionMDRejectionAsync(Reqno);
                    logger.LogInformation($"User:{user.EmployeeId},Verb:{verb},Path:MD Reject Requisition Failure");
                    return Ok(responser.return_value);
                }
                else
                {
                    logger.LogWarning($"User:{user.EmployeeId},Verb:{verb},Path:MD Reject Requisition Failure");
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Rejection failed" });
                }

            }
            catch (Exception x)
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;
                logger.LogError($"User:{user.EmployeeId},Verb:{verb},Action:MD Reject Requisition Failure,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Rejection  failed"+x.Message });
            }
        }

        //MD Dashboade
        [Authorize]
        [Route("mddashboard")]
        [HttpGet]
        public IActionResult MDdashboard()
        {
            try
            {
                var viewedCount = dbContext.RequisitionProgress.Where(x => x.ProgressStatus >= 3).Count();
                var pendingCount = dbContext.RequisitionProgress.Where(x => x.ProgressStatus == 2).Count();

                return Ok(new { viewedCount, pendingCount });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "MD Dashboard failed "+x.Message });
            }
        }
        //MD Req list
        [Authorize]
        [Route("mdgetreqlist")]
        [HttpGet]
        public IActionResult MDReqList()
        {
            try
            {
                var list = dbContext.RequisitionProgress.Where(x => x.ProgressStatus == 2).ToList();
                return Ok(list);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "MD Stage Requsition List failed " + x.Message });
            }
        }
        //MD Req card
        /*----USE HR ONE----*/
        //MD Approving action
        //Send back to HR
        [Authorize]
        [Route("mdsendhr/{Reqno}")]
        [HttpPost]
        public async Task<IActionResult> MdSendHr([FromBody] PushtoHRModel pushtoHR,string Reqno)
        {
            
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;
                RequisitionProgress reqModel = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).FirstOrDefault();
                reqModel.ProgressStatus = 3;
                reqModel.UIDThree = user.Id;
                reqModel.UIDThreeComment = pushtoHR.MDcomment;
                dbContext.RequisitionProgress.Update(reqModel);
                await dbContext.SaveChangesAsync();

                //@email
                //send Email to HR
                var mdMailHr = await codeUnitWebService.WSMailer().StaffRequisitiontoHRfromMDAsync(Reqno);
                /*var hrUser = dbContext.Users.Where(x => x.Id == reqModel.UIDTwo).First();
                Requisitionrequest requisitionrequest = new Requisitionrequest
                {
                    RequisionNo = Reqno,
                    ToEmail = hrUser.Email,
                    Username = hrUser.UserName
                };
                await mailService.RequisitionRequestAsync(requisitionrequest);*/

                logger.LogInformation($"User:{user.EmployeeId},Verb:{verb},Path:MD Requisition Approved & Pushed to HR");
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requisition Approved & Pushed to HR" });
            }
            catch (Exception x)
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var verb = Request.HttpContext.Request.Method;
                logger.LogError($"User:{user.EmployeeId},Verb:POST,Action:MD Requisition Send to HR failed,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "MD Send to HR failed " + x.Message });
            }
        }

        //Staff Requsion Reversal
        [Authorize]
        [HttpPost]
        [Route("reverserequisition")]
        public async Task<IActionResult> ReverseRequision([FromBody] PushtoHRModel reqmodel)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var reqrec = dbContext.RequisitionProgress.Where(x => x.ReqID == reqmodel.Reqno).FirstOrDefault();

                reqrec.ProgressStatus = int.Parse(reqmodel.Stage);
                dbContext.RequisitionProgress.Update(reqrec);
                await dbContext.SaveChangesAsync();

                if (int.Parse(reqmodel.Stage) == 0)
                {
                    //1 (HOD)
                    var hodId = dbContext.Users.Where(x => x.Id == reqrec.UID).First();
                    var mailresp = await codeUnitWebService.Client().RequsitionReversalAsync(reqmodel.Reqno, reqmodel.HRcomment,hodId.EmployeeId,1);
                    logger.LogInformation($"User:{user.EmployeeId},Verb:POST,Path:Requisition Card Reversal to HOD:{hodId.EmployeeId} Success");
                    return Ok(new { mailresp.return_value });
                }
                else
                {
                    //2 (HR)
                    var hrId = dbContext.Users.Where(x => x.Id == reqrec.UID).First();
                    var mailresp = await codeUnitWebService.Client().RequsitionReversalAsync(reqmodel.Reqno, reqmodel.HRcomment, hrId.EmployeeId, 2);
                    logger.LogInformation($"User:{user.EmployeeId},Verb:POST,Path:Requisition Card Reversal to HR:{hrId.EmployeeId} Success");
                    return Ok(new { mailresp.return_value });
                }
          
            }
            catch (Exception x)
            {
                var user1 = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                logger.LogError($"User:{user1.EmployeeId},Verb:POST,Action:Requisition Card Reversal failed,Message:{x.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Requsition Reversal Failed " + x.Message });
            }
        }

        /*
         * ######################################### PERFORMANCE MONITORING
         */

        //get performance employeses
        [HttpGet]
        [Route("getperformancesourcedata")]

        public async Task<IActionResult> PSourcedata()
        {
            try
            {
                List<EmployeeListModel> employeeListModels = new List<EmployeeListModel>();

                var employeeList =await  codeUnitWebService.Client().EmployeeListAsync();
                dynamic employeeListSerial = JsonConvert.DeserializeObject(employeeList.return_value);
                foreach (var emp in employeeListSerial)
                {
                    EmployeeListModel employee = new EmployeeListModel
                    {
                        Value = emp.No,
                        Label = emp.Fullname
                    };
                    employeeListModels.Add(employee);
                }

                return Ok(new { employeeListModels });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Source Data failed " + x.Message });
            }
        }

        //Create Performance monitoring
        [HttpPost]
        [Route("createmonitoring")]

        public async Task<IActionResult> CreateMonitoring([FromBody] MonitoringHeadModel monitoringHead)
        {
            try
            {
                var res = await  codeUnitWebService.Client().InsertPerformanceMonitoringAsync(monitoringHead.Manager, monitoringHead.Staff, monitoringHead.Attendee);

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Create Monitoring Done" });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Monitoring creation failed " + x.Message });
            }
        }

        //Get Monitor List
        [HttpGet]
        [Route("monitoringlist")]
        public async Task<IActionResult> MonitoringList()
        {
            try
            {
                List<MonitoringHeadModel> monitoringHeadModels = new List<MonitoringHeadModel>();

                //Add HOD employee no
                var res = await codeUnitWebService.Client().GetPerformanceHeaderAsync();
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                foreach (var vv in resSerial)
                {
                    MonitoringHeadModel monitoringHead = new MonitoringHeadModel
                    {
                        MonitorNo = vv.MonitorNo,
                        Date = vv.Date,
                        Staff = vv.StaffName,
                        StaffName = vv.StaffName,
                        Manager = vv.ManagerName,
                        ManagerName = vv.ManagerName,
                        Attendee = vv.AttendeeName,
                        AttendeeName = vv.AttendeeName,
                        AreasofSupport = vv.AreasofSupport,
                        AreasofSupport2 = vv.AreasofSupport2,
                        Recommendations = vv.Recommendations,
                        Approvalstatus = vv.ApprovalStatus,
                        HRRemarks = vv.HRRemarks,
                    };

                    monitoringHeadModels.Add(monitoringHead);
                }

                return Ok(monitoringHeadModels);

            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Monitoring List failed " + x.Message });
            }
        }

        //Get Line data HR and HOD

        [HttpGet]
        [Route("getdataline/{ID}")]

        public async Task<IActionResult> Getheaderlines(string ID)
        {
            try
            {
                List<PerformanceLineModel> performanceLineModels = new List<PerformanceLineModel>();

                var res = await  codeUnitWebService.Client().GetPerformanceLineAsync(ID);
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                //Get the status of the card
                var dbPerMonitor = dbContext.PerformanceMonitoring.Where(x => x.PerformanceId == ID).FirstOrDefault();
                


               
                    foreach (var pm in resSerial)
                    {
                        PerformanceLineModel lineModel = new PerformanceLineModel
                        {
                            Monitorno = pm.MonitorNo,
                            Performanceparameter = pm.PerformanceParameter,
                            Currentperformance = pm.CurrentPerformance,
                            Month1 = pm.Month1,
                            Month2 = pm.Month2,
                            Month3 = pm.Month3,
                            Original = pm.MonitorNo==""?false:true,

                        };
                        performanceLineModels.Add(lineModel);
                    }

                //Get Monitoring Invoked PIP Head & Line
                List<MonitoringInvokePIP> pipHeader = new List<MonitoringInvokePIP>();
                List<MonitoringInvokePIP> pipLines = new List<MonitoringInvokePIP>();

                var resPIPHeader = await codeUnitWebService.Client().GetPIPHeaderAsync(ID);
                dynamic resPIPHeaderSerial = JsonConvert.DeserializeObject(resPIPHeader.return_value);
                foreach (var h in resPIPHeaderSerial)
                {
                    MonitoringInvokePIP piph = new MonitoringInvokePIP
                    {
                        MonitorNo = h.MonitorNo,
                        //Lineno = h.Lineno,
                        PerformanceMonths = h.PerformanceMonth,
                        ReviewDate = h.ReviewDate
                    };
                    pipHeader.Add(piph);
                }

                var resPIPLine = await codeUnitWebService.Client().GetPIPLinesAsync(ID);
                dynamic resPIPLineSerial = JsonConvert.DeserializeObject(resPIPLine.return_value);
                foreach (var l in resPIPLineSerial)
                {
                   

                    MonitoringInvokePIP pipl = new MonitoringInvokePIP
                    {
                        MonitorNo = l.HeaderNo,
                        Lineno = l.Lineno,
                        Month = l.Month,
                        SalesTarget = l.SalesTarget,
                        SalesRep = l.SalesRep
                    };
                    pipLines.Add(pipl);
                }

                return Ok(new { performanceLineModels, dbPerMonitor, pipHeader,pipLines });
               
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Monitoring Lines failed " + x.Message });
            }
        }


        // Add Monitoring Line 
        [Authorize]
        [HttpPost]
        [Route("addmonitoringline")]
        public async Task<IActionResult> Addplines([FromBody] PerformanceLineModel lineModel)
        {
            try
            {
                if(lineModel.Original == false)
                {
                    var res = await codeUnitWebService.Client().InsertPerformanceMonitoringLinesAsync(lineModel.Monitorno, lineModel.Performanceparameter, lineModel.Currentperformance,
                   lineModel.Month1, lineModel.Month2, lineModel.Month3);

                    var mailStaffOnM1 = await codeUnitWebService.WSMailer().PerformanceMonitorMonth1Async(lineModel.Monitorno);
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Create Monitoring Line Done" });
                }
                else
                {
                    var res = await codeUnitWebService.Client().ModifyPerformanceMonitoringLinesAsync(lineModel.Monitorno, lineModel.Performanceparameter, lineModel.Currentperformance,
                                       lineModel.Month1, lineModel.Month2, lineModel.Month3);
                    var mailStaffOnM2 = await codeUnitWebService.WSMailer().PerformanceMonitorMonth2Async(lineModel.Monitorno);

                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Update Monitoring Line Done" });
                }
               
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Monitoring Lines Upload Failed " + x.Message });
            }
        }

        // Modify Monitoring Header
        [Authorize]
        [HttpPost]
        [Route("modifymonitoringheader")]
        public async Task<IActionResult> Modifypheader([FromBody] MonitoringHeadModel monitoringHead)
        {
            try
            {
                var res = await codeUnitWebService.Client().ModifyPerformanceMonitoringAsync(monitoringHead.MonitorNo,
                    monitoringHead.AreasofSupport, monitoringHead.AreasofSupport2, monitoringHead.Recommendations);

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Modify Monitoring Done" });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Monitoring Lines Upload Failed " + x.Message });
            }
        }
       
        //Delete Monitoring Line
        [Authorize]
        [HttpPost]
        [Route("deletemonitoringline")]
        public async Task<IActionResult> Deleteplines([FromBody] PerformanceLineModel lineModel)
        {
            try
            {

                var res = await codeUnitWebService.Client().DeleteMonitoringLineAsync(lineModel.Monitorno,lineModel.Performanceparameter,
                    lineModel.Currentperformance);
                if (res.return_value == "TRUE")
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Line Delete Success" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Line Delete Failed" });
                }
                

            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Monitoring Lines Delete Failed " + x.Message });
            }
        }

        //Approve Monitoring
        [Authorize]
        [HttpGet]
        [Route("approvemonitoring/{ID}")]
        public async Task<IActionResult> ApproveMonitoring(string ID)
        {
            try
            {
                var res = await codeUnitWebService.Client().ApprovePerformanceMonitoringAsync(ID);
                if (res.return_value == "TRUE")
                {
                    var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var monModel = dbContext.PerformanceMonitoring.Where(x => x.PerformanceId == ID).FirstOrDefault();
                    monModel.Progresscode = 2;
                    monModel.HRId = user.Id;
                    monModel.ApprovalStatus = "Approved";

                    dbContext.PerformanceMonitoring.Update(monModel);
                    await dbContext.SaveChangesAsync();

                    //Mail Staff
                    var hrApproveEmail = await codeUnitWebService.WSMailer().PerformanceMonitorApprovalAsync(ID);

                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Approve Success" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Approve Failed" });
                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Approving Failed " + x.Message });
            }
        }

        //Reject Monitoring
        [Authorize]
        [HttpGet]
        [Route("rejectmonitoring/{ID}")]
        public async Task<IActionResult> RejectMonitoring(string ID)
        {
            try
            {
                var res = await codeUnitWebService.Client().RejectPerformanceMonitoringAsync(ID);
                if (res.return_value == "TRUE")
                {
                    var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                    var monModel = dbContext.PerformanceMonitoring.Where(x => x.PerformanceId == ID).FirstOrDefault();
                    monModel.Progresscode = 3;
                    monModel.HRId = user.Id;
                    monModel.ApprovalStatus = "Rejected";

                    dbContext.PerformanceMonitoring.Update(monModel);
                    await dbContext.SaveChangesAsync();

                    /*
                     *  Emails to send to  staff
                     * **/
                    var rejectMail = await codeUnitWebService.WSMailer().PerformanceMonitorRejectionAsync(ID);

                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Reject Success" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Reject Failed" });
                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Reject Failed " + x.Message });
            }
        }


        //HOD Push to HR
        [Authorize]
        [HttpPost]
        [Route("monitoringpushtohr")]

        public async Task<IActionResult> MonitoringPushToHR([FromBody] PerformanceMonitoring monitoring)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if(dbContext.PerformanceMonitoring.Where(x=>x.PerformanceId == monitoring.PerformanceId).Count() > 0)
                {
                    var duplModel = dbContext.PerformanceMonitoring.Where(x => x.PerformanceId == monitoring.PerformanceId).FirstOrDefault();
                    duplModel.Progresscode = 1;
                    duplModel.HODId = user.Id;

                    dbContext.PerformanceMonitoring.Update(duplModel);
                    await dbContext.SaveChangesAsync();

                    var mailHRMonitoring = await codeUnitWebService.WSMailer().PerformanceMonitortoHRfromHODAsync(monitoring.PerformanceId);
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Pushing Update Success" });
                }
                else
                {
                    monitoring.Progresscode = 1;
                    monitoring.HODId = user.Id;

                    dbContext.PerformanceMonitoring.Add(monitoring);
                    await dbContext.SaveChangesAsync();

                    // var hodPushMail = await codeUnitWebService.WSMailer().
                    //@check on this
                    var mailHRMonitoring2 = await codeUnitWebService.WSMailer().PerformanceMonitortoHRfromHODAsync(monitoring.PerformanceId);
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Pushing Success" });
                }
               
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Pushing Failed " + x.Message });
            }
        }


        //HR Monitoring
        [Authorize]
        [HttpGet]
        [Route("hrmonitoring")]

        public IActionResult GetHRMonitoringList()
        {
            try
            {
                var HRMonitoringList = dbContext.PerformanceMonitoring.Where(x => x.Progresscode >= 1).ToList();
                return Ok(new { HRMonitoringList });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Pull Failed " + x.Message });
            }

        }

        //Get Monitor List
        [HttpGet]
        [Route("monitoringspecificheader/{ID}")]
        public async Task<IActionResult> MonitoringSpecificHeader(string ID)
        {
            try
            {
                List<MonitoringHeadModel> monitoringHeadModels = new List<MonitoringHeadModel>();

                var res = await codeUnitWebService.Client().GetPerformanceSpecificHeaderAsync(ID);
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                foreach (var vv in resSerial)
                {
                    MonitoringHeadModel monitoringHead = new MonitoringHeadModel
                    {
                        MonitorNo = vv.MonitorNo,
                        Date = vv.Date,
                        Staff = vv.StaffNo,
                        StaffName = vv.StaffName,
                        Manager = vv.ManagerName,
                        ManagerName = vv.ManagerName,
                        Attendee = vv.AttendeeName,
                        AttendeeName = vv.AttendeeName,
                        AreasofSupport = vv.AreasofSupport,
                        AreasofSupport2 = vv.AreasofSupport2,
                        Recommendations = vv.Recommendations,
                        Approvalstatus = vv.ApprovalStatus,
                        HRRemarks = vv.HRRemarks,
                    };

                    monitoringHeadModels.Add(monitoringHead);
                }

                return Ok(new { monitoringHeadModels });

            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Monitoring Header failed " + x.Message });
            }
        }


        //Push HR Monitoring Remark
        [HttpPost]
        [Route("hrmonitoringremark/{ID}")]
        public async Task<IActionResult> HRmonitoring([FromBody] MonitoringHeadModel headModel,string ID)
        {
            try
            {
                var res = await codeUnitWebService.Client().InsertMonitoringHRRemarksAsync(ID, headModel.HRRemarks);
                return Ok(res.return_value);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Remark Push Failed " + x.Message });
            }
        }
    
        //***************************  HR Generate PIP ********************************
        //CreateUpdate PIP Header
        [Authorize]
        [HttpPost]
        [Route("createupdatepipheader")]
        public async Task<IActionResult> CreateUpdatePIPHeader([FromBody] MonitoringInvokePIP invokePIP)
        {
            try
            {
                var res = await codeUnitWebService.Client().CreateModifyPIPHeaderAsync(
                    invokePIP.MonitorNo, 
                    invokePIP.PerformanceMonths, 
                    invokePIP.ReviewDate);

                return Ok(res.return_value);
                //return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = "Create/Update PIP Header, Success "});
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create/Update PIP Header Failed " + x.Message });
            }
        }
        //CreateUpdate PIP Line
        [Authorize]
        [HttpPost]
        [Route("createupdatepipline")]
        public async Task<IActionResult> CreateUpdatePIPLine([FromBody] MonitoringInvokePIP invokePIP)
        {
            try
            {

                //int auxLineno = String.IsNullOrEmpty(invokePIP.Lineno.ToString()) ? -1 : invokePIP.Lineno;

                var res = await codeUnitWebService.Client().CreateModifyPIPLineAsync(
                    invokePIP.MonitorNo,
                    invokePIP.Lineno,
                    invokePIP.Month,
                    invokePIP.SalesTarget,
                    invokePIP.SalesRep);

                //return Ok(res.return_value);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = "Create/Update PIP Header, Success ",ExtMessage= res.return_value });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create/Update PIP Line Failed " + x.Message });
            }
        }
        //Delete PIP Line
        [Authorize]
        [HttpPost]
        [Route("deletepipline")]
        public async Task<IActionResult> DeletePIPLine([FromBody] MonitoringInvokePIP invokePIP)
        {
            try
            {
                var res = await codeUnitWebService.Client().DeletePIPLineAsync(invokePIP.Lineno);
                return Ok(res.return_value);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create/Update PIP Line Failed " + x.Message });
            }
        }

        //Alert The Staff in PIP
        [Authorize]
        [HttpGet]
        [Route("alertstaffforpip/{monitorNo}")]
        public async Task<IActionResult> AlertStaffForPIP(string monitorNo)
        {
            try
            {
                var alertStaff = await codeUnitWebService.WSMailer().PerformanceMonitoringSendPIPFromHRAsync(monitorNo);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Error", Message = "Alert Staff for PIP, Success" });

            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Alert Staff for PIP Failed " + x.Message });
            }
        }
    
      

   


    }
}
