using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RPFBE.Auth;
using RPFBE.Model;
using RPFBE.Model.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffRequisionController : Controller
    {
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public StaffRequisionController( 
            ICodeUnitWebService codeUnitWebService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext
        )
        {
            this.codeUnitWebService = codeUnitWebService;
            this.userManager = userManager;
            this.dbContext = dbContext;
        }
        [Authorize]
        [Route("jobslist")]
        [HttpGet]
        public async Task<IActionResult> GetJobs()
        {
            try
            {
                var res = await codeUnitWebService.Client().GetJobsAsync();
                return Ok(res.return_value);
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

                return Ok(new { jobCheckList, employeeListModels,contractListModels, departmentListModels });
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
                var list = dbContext.RequisitionProgress.Where(x => x.ProgressStatus >= 1).ToList();
                return Ok(list);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "MD Stage Requsition List failed " + x.Message });
            }
        }


        //HOD push to HR
        [Authorize]
        [Route("pushtohr")]
        [HttpPost]
        public async Task<IActionResult> PushtoHR(PushtoHRModel pushtoHR)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var result = await codeUnitWebService.Client().GetJobDetailsAsync(pushtoHR.Jobno);
                dynamic resSerial = JsonConvert.DeserializeObject(result.return_value);

                RequisitionProgress requisitionProgress = new RequisitionProgress
                {
                    UID = user.Id,
                    ReqID = pushtoHR.Reqno,
                    JobNo= resSerial.Jobno,
                    JobTitle = resSerial.Jobtitle,
                    JobGrade = resSerial.Jobgrade,
                    RequestedEmployees = pushtoHR.RequestedEmployees,
                    ClosingDate = pushtoHR.ClosingDate,
                    Status= resSerial.Status,
                    ProgressStatus = 1,
                };


                dbContext.RequisitionProgress.Add(requisitionProgress);
                await dbContext.SaveChangesAsync();

               return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requisition pushed" });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Requsition Progression failed " + x.Message });
            }
        }

        /*HR to MD
         * Get list 
         * Get single
         */
        [Authorize]
        [Route("hrgetreqlist")]
        [HttpGet]
        public  IActionResult HrReqList()
        {
            try
            {
                var list = dbContext.RequisitionProgress.Where(x => x.ProgressStatus == 1 || x.ProgressStatus >= 3).ToList();
                return Ok(list);
            }
            catch (Exception x)
            {
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

                    

                return Ok(new { statusProgress, EmployeeReplaced, HR, HOD,MD,qualificationModels, requirementModels, responsibilityModels, checklistModels, checklistInitCodeAux, requsitionCard });

               
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Requsition Single failed " + x.Message });
            }
        }


        //HR Push to MD
        [Authorize]
        [Route("hrsendmd/{Reqno}")]
        [HttpGet]
        public async Task<IActionResult> HrSendMD(string Reqno)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                RequisitionProgress reqModel = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).FirstOrDefault();
                //RequisitionProgress reqModel = new RequisitionProgress
                //{
                //    ReqID = Reqno,
                //    ProgressStatus = 2
                //};
                reqModel.ProgressStatus = 2;
                reqModel.UIDTwo = user.Id;
                dbContext.RequisitionProgress.Update(reqModel);
                await dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requisition pushed to MD" });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "HR Send to MD failed " + x.Message });
            }
        }
        //HR function
        [Authorize]
        [Route("approveandpublish/{Reqno}")]
        [HttpGet]
        public async Task<IActionResult> Approveupdate(string Reqno)
        {
            try
            {
                //return Ok(Reqno);
                var responser = await codeUnitWebService.Client().SetChecklistMandatoryRequiredAsync(Reqno);
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if(responser.return_value == "Yes")
                {
                    RequisitionProgress reqModel = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).FirstOrDefault();
                    reqModel.ProgressStatus = 4;
                    reqModel.UIDFour = user.Id;
                    dbContext.RequisitionProgress.Update(reqModel);
                    await dbContext.SaveChangesAsync();
                    return Ok(responser.return_value);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Approve Publish failed" });
                }
               
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Approve Publish failed" });
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
        [HttpGet]
        public async Task<IActionResult> MdSendHr(string Reqno)
        {
            
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                RequisitionProgress reqModel = dbContext.RequisitionProgress.Where(x => x.ReqID == Reqno).FirstOrDefault();
                reqModel.ProgressStatus = 3;
                reqModel.UIDThree = user.Id;
                dbContext.RequisitionProgress.Update(reqModel);
                await dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Requisition Approved & Pushed to HR" });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "MD Send to HR failed " + x.Message });
            }
        }



    }
}
