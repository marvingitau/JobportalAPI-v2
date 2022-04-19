//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using RPFBE.Auth;
//using RPFBE.Model;
//using RPFBE.Model.DBEntity;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RPFBE.Auth;
using RPFBE.Model;
using RPFBE.Model.DBEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    //[Authorize(Roles =UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
     

        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public HomeController(
         UserManager<ApplicationUser> userManager,
         ApplicationDbContext dbContext,
         ILogger<HomeController> logger,
        IWebHostEnvironment webHostEnvironment
      )
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        [Route("posted-jobs")]
        // public async Task<IActionResult> GetPostedJobs()
        public async Task<IEnumerable<PostedJobModel>> GetPostedJobs()
        {
            List<PostedJobModel> postedJobsList = new List<PostedJobModel>();
            try
            {
                //List<PostedJobModel> postedJobsList = new List<PostedJobModel>();

                var result = await CodeUnitWebService.Client().GetPostedJobsAsync();
                dynamic postedJobs = JsonConvert.DeserializeObject<List<PostedJobModel>>(result.return_value);

                foreach (var postedJob in postedJobs)
                {
                    PostedJobModel postedJobModel = new PostedJobModel();
                    postedJobModel.Jobno = postedJob.Jobno;
                    postedJobModel.Jobtitle = postedJob.Jobtitle;
                    postedJobModel.Closingdate = postedJob.Closingdate;

                    postedJobsList.Add(postedJobModel);
                }

                //return Content(postedJobs );
                return postedJobs;
            }
            catch (Exception)
            {
                return postedJobsList;
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = ex.Message });
            }

        }

        [HttpGet]
        [Route("jobdata/{ReqNo}")]
        public IActionResult GetJobData(string ReqNo)
        {
            List<JobQualificationModel> JobQualifList = new List<JobQualificationModel>();
            List<JobRequirementModel> JobRequireList = new List<JobRequirementModel>();
            List<TasksModel> JobTaskList = new List<TasksModel>();
            List<ChecklistModel> JobCheckList = new List<ChecklistModel>();

            try
            {
                //Job Metadata
                var jobMeta = CodeUnitWebService.Client().GetJobMetaAsync(ReqNo).Result.return_value;
                JobMetaModel jobMetaDeserialize = JsonConvert.DeserializeObject<JobMetaModel>(jobMeta);

                try
                {
                    //Qualification
                    var jobQualification = CodeUnitWebService.Client().GetPostedJobQualificationsAsync(jobMetaDeserialize.Jobno).Result.return_value;
                    dynamic jobQualificationDeserialize = JsonConvert.DeserializeObject(jobQualification);
                    foreach (var qualif in jobQualificationDeserialize)
                    {
                        JobQualificationModel qualificationModel = new JobQualificationModel
                        {
                            Jobno = qualif.Jobno,
                            Mandantory = qualif.Mandatory,
                            Description = qualif.Description,
                            Qficationcode = qualif.Qficationcode

                        };

                        JobQualifList.Add(qualificationModel);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Job Qualification deserialize failed" });
                }

                try
                {
                    //Requirements
                    var jobRequirement = CodeUnitWebService.Client().GetPostedJobRequirementsAsync(jobMetaDeserialize.Jobno).Result.return_value;
                    dynamic JobReqSerial = JsonConvert.DeserializeObject(jobRequirement);
                    foreach (var require in JobReqSerial)
                    {
                        JobRequirementModel requirementModel = new JobRequirementModel
                        {
                            Jobno = require.Jobno,
                            Mandatory = require.Mandatory,
                            Description = require.Description,
                            Rqmentcode = require.Rqmentcode
                        };
                        JobRequireList.Add(requirementModel);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Job Requirement deserialize failed" + ex.Message });
                }

                try
                {
                    //Task
                    var jobTask = CodeUnitWebService.Client().GetJobTasksAsync(jobMetaDeserialize.Jobno).Result.return_value;
                    dynamic JobTaskSerial = JsonConvert.DeserializeObject(jobTask);

                    foreach (var task in JobTaskSerial)
                    {
                        TasksModel tasksModel = new TasksModel
                        {
                            Jobno = task.Jobno,
                            Description = task.Description,
                            Taskcode = task.Taskcode
                        };
                        JobTaskList.Add(tasksModel);
                    }
                }
                catch (Exception)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Job Task deserialize failed" });
                }

                try
                {
                    //Checklist
                    var jobChecklist = CodeUnitWebService.Client().GetChecklistAsync(ReqNo).Result.return_value;
                    dynamic jobChecklistSerial = JsonConvert.DeserializeObject(jobChecklist);

                    foreach (var task in jobChecklistSerial)
                    {
                        ChecklistModel checkModel = new ChecklistModel
                        {
                            ReqNo = task.ReqNo,
                            Code = task.DocCode,
                            Description = task.Description
                        };
                        JobCheckList.Add(checkModel);
                    }
                }
                catch (Exception)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Job Check List deserialize failed" });
                }




                return Ok(new
                {
                    JobMeta = jobMetaDeserialize,
                    JobQualification = JobQualifList,
                    JobRequirement = JobRequireList,
                    JobTask = JobTaskList,
                    JobCheck = JobCheckList
                });
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Job metadata deserialize failed" });
            }
        }

        [HttpPost]
        [Route("applyjob")]
        public async Task<IActionResult> ApplyJob([FromBody] AppliedJob appliedJob)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);


                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Application Failed" });
                }
                appliedJob.UserId = user.Id;
                appliedJob.ApplicationDate = DateTime.Now;
                var profileExist = dbContext.Users.Where(x => x.Id == user.Id && x.ProfileId != 0).Count();
                if (profileExist == 0)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Please Create your profile first" });
                }
                var duplicateCheck = dbContext.AppliedJobs.Where(y => y.UserId == user.Id).Where(x => x.UserId == user.Id && x.JobReqNo == appliedJob.JobReqNo).FirstOrDefault();
                if (duplicateCheck == null)
                {
                    await dbContext.AppliedJobs.AddAsync(appliedJob);
                    await dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Job Applied" });

                }


                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Job Applied Already" });
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Logged out" });

            }

        }


        //Checklist Documents !(Roles="Admin")
        [Authorize]
        [Route("uploadcheck/{jobNo}")]
        [HttpPost]
        public async Task<IActionResult> SaveImage([FromForm] List<IFormFile> forms, string jobNo)
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
          
            try
            {
                var subDirectory = "Files";
                var result = new List<FileUploadResult>();
                var target = Path.Combine(webHostEnvironment.ContentRootPath, subDirectory);

                foreach (var file in forms)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                    //string fileName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(17).ToArray()).Replace(' ', '-');
                    //fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);
                    var path = Path.Combine(target, fileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });

                    JobSpecFile specData = new JobSpecFile
                    {
                        UserId = user.Id,
                        JobId = jobNo,
                        FilePath = path,
                        TagName = fileName,

                    };
                    dbContext.SpecFiles.Add(specData);
                    dbContext.SaveChanges();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "File upload failed :"+ex.Message });
                
            }

        }

        //User View Checklist Documents
        [Route("viewchecklist")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobSpecFile>>> ViewChecklist()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var dbres = dbContext.SpecFiles.Where(x => x.UserId == user.Id).ToList();
                return dbres;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "CV View failed" });
            }

        }

        //All users
        [Route("viewattachment/{FID}")]
        [HttpGet]
        public IActionResult ViewAttachment(string FID)
        {
            //var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var dbres = dbContext.SpecFiles.Where(x => x.TagName == FID).FirstOrDefault();
                return Ok(dbres.FilePath);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "CV View failed" });
            }
        }



        //Admin View Checklist Documents
        [Route("adminviewchecklist/{UID}")]
        [HttpGet]
        public ActionResult<IEnumerable<JobSpecFile>> AdminViewChecklist(string UID)
        {
            //var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var dbres = dbContext.SpecFiles.Where(x => x.UserId == UID).ToList();
                return dbres;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "CV View failed" });
            }

        }

        //User View CV
        [Route("viewcv")]
        [HttpGet]
        public async Task<IActionResult> ViewCV()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var dbres = dbContext.UserCVs.Where(x => x.UserId == user.Id).FirstOrDefault();
                //var bytes = await System.IO.File.ReadAllBytesAsync(dbres.FilePath);
                //return File(bytes, "application/pdf", Path.GetFileName(dbres.FilePath));
                //if (filename == null)
                //return Content("filename not present");

                //var path = Path.Combine(
                //               Directory.GetCurrentDirectory(),
                //               "wwwroot", filename);
                var path = dbres.FilePath;

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "application/pdf", Path.GetFileName(path));



            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "CV View failed" + x.Message });
            }

        }
        //Admin View CV
        [Route("viewcv/{UID}")]
        [HttpGet]
        public IActionResult AdminViewCV(string UID)
        {
            //var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var dbres = dbContext.UserCVs.Where(x => x.UserId == UID).FirstOrDefault();
                return Ok(dbres.FilePath);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "CV View failed" });
            }
        }

        //User Upload the CV
        [Route("uploadcv")]
        [HttpPost]
        public async Task<IActionResult> UploadCV([FromForm] IFormFile formFile)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);


                var subDirectory = "Files/CVs";
                var target = Path.Combine(webHostEnvironment.ContentRootPath, subDirectory);
                string fileName = new String(Path.GetFileNameWithoutExtension(formFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(formFile.FileName);
                var path = Path.Combine(target, fileName);
                var stream = new FileStream(path, FileMode.Create);
                await formFile.CopyToAsync(stream);
                var respnuk = dbContext.UserCVs.Where(x => x.UserId == user.Id);
                if (dbContext.UserCVs.Where(x => x.UserId == user.Id).Count() > 0)
                {
                    var specificCV = dbContext.UserCVs.Where(x => x.UserId == user.Id).FirstOrDefault();
                    specificCV.FilePath = path;
                    await dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "CV Updated" });

                }
                else
                {
                    UserCV cvData = new UserCV
                    {
                        UserId = user.Id,
                        FilePath = path,
                        TagName = fileName,

                    };
                    dbContext.UserCVs.Add(cvData);
                    dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "CV Uploaded" });
                }

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = x.Message });
            }
        }




        [Authorize]
        [Route("appliedjobs")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppliedJob>>> AppliedJobs()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            return dbContext.AppliedJobs.Where(y => y.UserId == user.Id).Where(x => x.UserId == user.Id).ToList();
        }
        [Authorize]
        [Route("applicants")]
        [HttpGet]
        public ActionResult GetApplicants()
        {
            var query = dbContext.AppliedJobs
            .Join(
                dbContext.Users,
                apps => apps.UserId,
                user => user.Id,
                (apps, user) => new
                {
                    UserId = user.Id,
                    Viewed = apps.Viewed,
                    Title = apps.JobTitle,
                    Name = user.Name,
                    AppDate = apps.ApplicationDate,
                    ReqNo = apps.JobReqNo

                }
                ).Where(x => x.Viewed != true).ToList();

            return Ok(query);
        }
        [Authorize]
        [Route("approvedapplicants")]
        [HttpGet]
        public ActionResult GetApprovedApplicants()
        {
            var query = dbContext.AppliedJobs
            .Join(
                dbContext.Users,
                apps => apps.UserId,
                user => user.Id,
                (apps, user) => new
                {
                    UserId = user.Id,
                    Viewed = apps.Viewed,
                    Title = apps.JobTitle,
                    Name = user.Name,
                    AppDate = apps.ApplicationDate,
                    ReqNo = apps.JobReqNo

                }
                ).Where(x => x.Viewed == true).ToList();

            return Ok(query);
        }


        //View Single Applicant i.e the CV
        //[Authorize]
        [Route("applicant/{id}")]
        [HttpGet]
        public ActionResult GetApplicant(string id)
        {
            var usr = dbContext.Users.Find(id);
            if (usr.ProfileId != 0)
            {
                var profile = dbContext.Profiles.Where(x => x.UserId == usr.Id);
                return Ok(new { profile, usr });
            }
            return Ok(usr);
        }

        // Create ERP PostJobApplication
        // Needs the Requision and Employee Code
        [Route("postjob/{reqNo}")]
        [HttpGet]
        public async Task<IActionResult> PushJobApp(string reqNo)
        {

            var employee = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var jobAppId = CodeUnitWebService.Client().PostJobApplicationAsync(reqNo, employee.EmployeeId);
            return Ok(jobAppId);
        }

        //Admin Dashboad
        [Authorize]
        [Route("adminstats")]
        [HttpGet]

        public IActionResult adminstats()
        {
            try
            {
                var viewedCount = dbContext.AppliedJobs.Where(x => x.Viewed == true).Count();
                var pendingCount = dbContext.AppliedJobs.Where(x => x.Viewed != true).Count();

                return Ok(new { viewedCount, pendingCount });
            }
            catch (Exception)
            {
                var viewedCount = 0;
                var pendingCount = 0;
                return Ok(new { viewedCount, pendingCount });

            }
        }

        [Authorize]
        [Route("userstats")]
        [HttpGet]

        public async Task<IActionResult> userstats()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var viewedCount = dbContext.AppliedJobs.Where(x => x.UserId == user.Id && x.Viewed == true).Count();
                var pendingCount = dbContext.AppliedJobs.Where(x => x.UserId == user.Id && x.Viewed != true).Count();

                return Ok(new { viewedCount, pendingCount });
            }
            catch (Exception)
            {
                var viewedCount = 0;
                var pendingCount = 0;
                return Ok(new { viewedCount, pendingCount });

            }
        }



    }
}
