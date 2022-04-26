using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RPFBE.Auth;
using RPFBE.Model;
using System;
using System.Collections.Generic;
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

        [Authorize]
        [Route("updaterequisition/{Reqno}")]
        [HttpPost]
        public async Task<IActionResult> UpdateRequision([FromBody] RequisionModel requisionModel,string Reqno)
        {
            try
            {
                string[] reqArr = new string[10];
                reqArr[0] = string.IsNullOrEmpty(requisionModel.Requisiontype)?"": requisionModel.Requisiontype;
                reqArr[1] = string.IsNullOrEmpty(requisionModel.Contracttype)?"":requisionModel.Contracttype;
                reqArr[2] = string.IsNullOrEmpty(requisionModel.Department)?"": requisionModel.Department;
                reqArr[3] = string.IsNullOrEmpty(requisionModel.Employeereplaced)?"": requisionModel.Employeereplaced;
                reqArr[4] = string.IsNullOrEmpty(requisionModel.HOD)?"": requisionModel.HOD;
                reqArr[5] = string.IsNullOrEmpty(requisionModel.HRManager)?"":requisionModel.HRManager;
                reqArr[6] = string.IsNullOrEmpty(requisionModel.MD)?"": requisionModel.MD;
                reqArr[7] = string.IsNullOrEmpty(requisionModel.Description)?"": requisionModel.Description;
                reqArr[8] = string.IsNullOrEmpty(requisionModel.Reason)?"":requisionModel.Reason;
                reqArr[9] = string.IsNullOrEmpty(requisionModel.Comment)?"":requisionModel.Comment;

                //return Ok(reqArr);
                var response = await codeUnitWebService.Client().ModifyEmpRequisitionAsync(Reqno, requisionModel.Startdate, requisionModel.Enddate ,reqArr);
                return Ok(response.return_value);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Update failed "+x.Message });
            }
        }


        [Authorize]
        [Route("approveandpublish/{Reqno}")]
        [HttpGet]
        public async Task<IActionResult> Approveupdate(string Reqno)
        {
            try
            {
                //return Ok(Reqno);
                var responser = await codeUnitWebService.Client().SetChecklistMandatoryRequiredAsync(Reqno);
                return Ok(responser.return_value);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Approve Publish failed" });
            }
        }



    }
}
