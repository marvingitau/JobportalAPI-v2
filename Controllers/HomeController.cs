using ClosedXML.Excel;
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
using RPFBE.Model.Repository;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
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
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public HomeController(
         UserManager<ApplicationUser> userManager,
         ApplicationDbContext dbContext,
         ILogger<HomeController> logger,
        IWebHostEnvironment webHostEnvironment,
        ICodeUnitWebService codeUnitWebService,
        IMailService mailService
      )
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.codeUnitWebService = codeUnitWebService;
            this.mailService = mailService;
        }

        [Route("posted-jobs")]
        // public async Task<IActionResult> GetPostedJobs()
        public async Task<IEnumerable<PostedJobModel>> GetPostedJobs()
        {
            List<PostedJobModel> postedJobsList = new List<PostedJobModel>();
            try
            {
                //List<PostedJobModel> postedJobsList = new List<PostedJobModel>();

                var result = await codeUnitWebService.Client().GetPostedJobsAsync();
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
                var jobMeta = codeUnitWebService.Client().GetJobMetaAsync(ReqNo).Result.return_value;
                JobMetaModel jobMetaDeserialize = JsonConvert.DeserializeObject<JobMetaModel>(jobMeta);

                try
                {
                    //Qualification
                    var jobQualification = codeUnitWebService.Client().GetPostedJobQualificationsAsync(jobMetaDeserialize.Jobno).Result.return_value;
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
                    var jobRequirement = codeUnitWebService.Client().GetPostedJobRequirementsAsync(jobMetaDeserialize.Jobno).Result.return_value;
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
                    var jobTask = codeUnitWebService.Client().GetJobTasksAsync(jobMetaDeserialize.Jobno).Result.return_value;
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
                    var jobChecklist = codeUnitWebService.Client().GetChecklistAsync(ReqNo).Result.return_value;
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
                var profileExist = dbContext.Users.Where(x => x.Id == user.Id && x.ProfileId != 0).Count();
                if (profileExist == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Success", Message = "Please Create your profile first" });
                }
                var subDirectory = "Files";
                var result = new List<FileUploadResult>();
                var target = Path.Combine(webHostEnvironment.ContentRootPath, subDirectory);
                var dbCount = dbContext.SpecFiles.Where(x => x.UserId == user.Id && x.JobId == jobNo).Count();
                if (dbCount <= 0)
                {
                    foreach (var file in forms)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
                        //string fileName = new String(Path.GetFileNameWithoutExtension(file.FileName).Take(17).ToArray()).Replace(' ', '-');
                        //fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(file.FileName);
                        var path = Path.Combine(target, fileName);
                        //var stream = new FileStream(path, FileMode.Create);
                        //await file.CopyToAsync(stream);
                        //result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });

                        using (FileStream stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
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

                    }
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "File Uploaded" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "File Uploaded Already" });
                }
               
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

        //Admin & use for users so as to delete
        [Route("viewattachment/{FID}")]
        [HttpGet]
        public IActionResult ViewAttachment(string FID)
        {
            //var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var dbres = dbContext.SpecFiles.Where(x => x.TagName == FID).FirstOrDefault();
                var file = dbres.FilePath;

                /*
                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file,
                    Inline = false // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return File(System.IO.File.ReadAllBytes(file), "application/pdf");
                */
                var stream = new FileStream(file, FileMode.Open);
                return new FileStreamResult(stream, "application/pdf");

                // return Ok(dbres.FilePath);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "Attachement View failed" });
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
        [Authorize]
        [Route("viewcv")]
        [HttpGet]
        public async Task<IActionResult> ViewCV()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var dbres = dbContext.UserCVs.Where(x => x.UserId == user.Id).FirstOrDefault();

                /*var bytes = await System.IO.File.ReadAllBytesAsync(dbres.FilePath);
                return File(bytes, "application/pdf", Path.GetFileName(dbres.FilePath));
                if (filename == null)
                    return Content("filename not present");

                var path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                               "wwwroot", filename);*/

                //var path = dbres.FilePath;

                //var memory = new MemoryStream();
                //using (var stream = new FileStream(path, FileMode.Open))
                //{
                //    await stream.CopyToAsync(memory);
                //}
                //memory.Position = 0;
                //return File(memory, "application/pdf", Path.GetFileName(path));

                var file = dbres.FilePath;

                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file,
                    Inline = true // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return File(System.IO.File.ReadAllBytes(file), "application/pdf");



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
                //var path = System.AppContext.BaseDirectory;
            
                var dbres = dbContext.UserCVs.Where(x => x.UserId == UID).FirstOrDefault();
                if(dbres == null)
                {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "CV Not Uploaded" });
                }
                var file = dbres.FilePath;
              
                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file,
                    Inline = true // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return File(System.IO.File.ReadAllBytes(file), "application/pdf");
                
                /*
                WebClient webclient = new WebClient();
                var byteArr = webclient.DownloadData(file);
                return File(byteArr, "application/pdf");
                */
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
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }
                //var stream = new FileStream(path, FileMode.Create);
                //await formFile.CopyToAsync(stream);
                //var respnuk = dbContext.UserCVs.Where(x => x.UserId == user.Id);

                /* 
                 * var host = HttpContext.Request.Host.ToUriComponent();
                 * var url = $"{HttpContext.Request.Scheme}://{host}/{path}";
                 * return Content(url);
                */

                if (dbContext.UserCVs.Where(x => x.UserId == user.Id).Count() > 0)
                {
                    var specificCV = dbContext.UserCVs.Where(x => x.UserId == user.Id).FirstOrDefault();
                    specificCV.FilePath = path;
                    specificCV.TagName = formFile.FileName;
                    await dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "CV Updated" });

                }
                else
                {
                    UserCV cvData = new UserCV
                    {
                        UserId = user.Id,
                        FilePath = path,
                        TagName = formFile.FileName
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


        //HOD Upload justification file
        [Authorize]
        [Route("justificationupload/{reqNo}")]
        [HttpPost]
        public async Task<IActionResult> JustificationFile([FromForm] IFormFile formFile, string reqNo)
        {

            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                var subDirectory = "Files/Reqfiles";
                var target = Path.Combine(webHostEnvironment.ContentRootPath, subDirectory);
                string fileName = new String(Path.GetFileNameWithoutExtension(formFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(formFile.FileName);
                var path = Path.Combine(target, fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }
      

                /* 
                 * var host = HttpContext.Request.Host.ToUriComponent();
                 * var url = $"{HttpContext.Request.Scheme}://{host}/{path}";
                 * return Content(url);
                */

                
                if (dbContext.JustificationFiles.Where(x => x.UserId == user.Id && x.ReqNo == reqNo).Count() > 0)
                {
                    var specificFile = dbContext.JustificationFiles.Where(x => x.UserId == user.Id && x.ReqNo == reqNo).FirstOrDefault();
                    specificFile.FilePath = path;
                    specificFile.TagName = formFile.FileName;
                    await dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "File Updated" });

                }
                else
                {
                    JustificationFile specData = new JustificationFile
                    {
                        UserId = user.Id,
                        ReqNo = reqNo,
                        FilePath = path,
                        TagName = fileName,

                    };
                    dbContext.JustificationFiles.Add(specData);
                    dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "File Uploaded" });
                }

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = x.Message });
            }


        }

        //Admin View Justification
        [Route("justificationfile/{reqID}")]
        [HttpGet]
        public IActionResult ViewJustificationFile(string reqID)
        {
            //var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                //var path = System.AppContext.BaseDirectory;

                var dbres = dbContext.JustificationFiles.Where(x => x.ReqNo == reqID).FirstOrDefault();
                if (dbres == null)
                {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "File Not Uploaded" });
                }
                var file = dbres.FilePath;

                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file,
                    Inline = true // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return File(System.IO.File.ReadAllBytes(file), "application/pdf");

                /*
                WebClient webclient = new WebClient();
                var byteArr = webclient.DownloadData(file);
                return File(byteArr, "application/pdf");
                */
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "File View failed" });
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
            try
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
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Applicants fetche failed " + x.Message });
            }
     
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

            var jobAppId = codeUnitWebService.Client().PostJobApplicationAsync(reqNo, employee.EmployeeId);
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
                var viewedCount = dbContext.AppliedJobs.Where(x => x.Viewed != true).Count();
                var pendingCount = dbContext.AppliedJobs.Where(x => x.Viewed == true).Count();

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

        [Route("env")]
        [HttpGet]
        public IActionResult GetDefault()
        {
            //string wwwPath = this.webHostEnvironment.WebRootPath;
            //string contentPath = this.webHostEnvironment.ContentRootPath;
            //return Ok(new { wwwPath, contentPath });
            WebClient webclient = new WebClient();
            var byteArr = webclient.DownloadData(@"A:\CODES\VS\RecruitmentPortalFolder\RPFBE\Files/CVs\Marvingita224125779.pdf");
                //await wc.DownloadDataTaskAsync(fileURL);
            return File(byteArr, "application/pdf");

           // var stream = new FileStream(@"A:\CODES\VS\RecruitmentPortalFolder\RPFBE\Files/CVs\Marvingita224125779.pdf", FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
        }

        [Route("getcv/{UID}")]
        [HttpGet]
        public IActionResult GetGlobalCV(string UID)
        {
            try
            {
                //var path = System.AppContext.BaseDirectory;
                var dbres = dbContext.UserCVs.Where(x => x.UserId == UID).FirstOrDefault();
                var file = dbres.FilePath;

                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file,
                    Inline = true // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return File(System.IO.File.ReadAllBytes(file), "application/pdf");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "CV View failed" });
            }
        }

        //Admin
        [Route("getspec/{UID}")]
        [HttpGet]
        public IActionResult GetGlobalSpec(string UID)
        {
            try
            {
                //var path = System.AppContext.BaseDirectory;
                var dbres = dbContext.SpecFiles.Where(x => x.TagName == UID).FirstOrDefault();
                var file = dbres.FilePath;

                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file,
                    Inline = true // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.Headers.Add("X-Content-Type-Options", "nosniff");

                return File(System.IO.File.ReadAllBytes(file), "application/pdf");
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "Supporting Doc View failed" });
            }
        }


        //Get Excel doc
        //Admin
        [Authorize]
        [Route("getexcel/{title}")]
        [HttpGet]
        public IActionResult getExcel(string title)
        {
            try
            {
                //var results = dbContext.AppliedJobs.Where(x => x.JobTitle == title).ToList();
                List<AppliedJob> appliedJobs= dbContext.AppliedJobs.ToList();
                List<Profile> profiles = dbContext.Profiles.ToList();
                List<JobSpecFile> jobSpecFiles = dbContext.SpecFiles.ToList();

                var query = from appjob in appliedJobs
                            join prof in profiles on appjob.UserId equals prof.UserId into tbl1
                            //from t1 in tbl1.ToList()
                            join jspec in jobSpecFiles on appjob.UserId equals jspec.UserId into tbl2
                            //from t2 in tbl2.ToList()
                            select new { appliedJobs = appjob, profiles = tbl1, jobSpecFiles = tbl2 };
                var results = query.Where(x => x.appliedJobs.JobTitle == title && x.appliedJobs.Viewed == false).ToList();

                //return Ok(results);
                /*
                var builder = new StringBuilder();
                builder.AppendLine("Job,UID,Resume");

                foreach (var result in results)
                {
                   //  = HYPERLINK("{HttpContext.Request.Host.ToUriComponent()}", "" / api / home / getcv / "")
                    builder.AppendLine($"{result.JobTitle},{result.UserId},=HYPERLINK({HttpContext.Request.Host.ToUriComponent()}/api/home/getcv/{result.UserId})");
                }

                return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "users.csv");

                //return Ok("dd")
                */

                
                //Excel Document
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Candidates");
                    var currentRow = 1;
                    worksheet.Cell(currentRow, 1).Value = "Job";
                    worksheet.Cell(currentRow, 2).Value = "First Name";
                    worksheet.Cell(currentRow, 3).Value = "Last Name";
                    worksheet.Cell(currentRow, 4).Value = "Application Date";
                    worksheet.Cell(currentRow, 5).Value = "Date of Birth";
                    worksheet.Cell(currentRow, 6).Value = "Country";
                    worksheet.Cell(currentRow, 7).Value = "Expected Salary";
                    worksheet.Cell(currentRow, 8).Value = "Current Salary";
                    worksheet.Cell(currentRow, 9).Value = "Highest Education Level";
                    worksheet.Cell(currentRow, 10).Value = "Willing to relocate";
                    worksheet.Cell(currentRow, 11).Value = "Gender";
                    worksheet.Cell(currentRow, 12).Value = "Disabled";
                    worksheet.Cell(currentRow, 13).Value = "Professional Experience (Y)";
                    worksheet.Cell(currentRow, 14).Value = "Resume";
                    worksheet.Cell(currentRow, 15).Value = "Other Qualification";

                    // From worksheet
                    //var rngTable = workbook.Range("1A:1K");
                    //rngTable.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //rngTable.Style.Font.Bold = true;
                    //rngTable.Style.Fill.BackgroundColor = XLColor.Aqua;

                    foreach (var r in results)
                    {
                        char[] delimiterChars = { '-', 'T' };
                        string text = r.profiles.FirstOrDefault().DOB;

                        string[] words = text.Split(delimiterChars);
                        string auxDate = words[1] + "/" + words[2] + "/" + words[0];

                        DateTime datetime = DateTime.ParseExact(auxDate, "MM/dd/yyyy", null);

                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = r.appliedJobs.JobTitle;
                        worksheet.Cell(currentRow, 2).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().FirstName : "";
                        worksheet.Cell(currentRow, 3).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().LastName : "";
                        worksheet.Cell(currentRow, 4).Value = r.appliedJobs.ApplicationDate;
                        worksheet.Cell(currentRow, 5).Value = auxDate;
                        worksheet.Cell(currentRow, 6).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().Country : "";
                        worksheet.Cell(currentRow, 7).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().ExpectedSalary: "";
                        worksheet.Cell(currentRow, 8).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().CurrentSalary: "";
                        worksheet.Cell(currentRow, 9).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().HighestEducation: "";
                        worksheet.Cell(currentRow, 10).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().WillingtoRelocate : "";
                        worksheet.Cell(currentRow, 11).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().Gender : "";
                        worksheet.Cell(currentRow, 12).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().PersonWithDisability : "";
                        worksheet.Cell(currentRow, 13).Value = r.profiles.FirstOrDefault() != null ? r.profiles.FirstOrDefault().Experience : "";
                        worksheet.Cell(currentRow, 14).Value = "Resume";
                        worksheet.Cell(currentRow, 14).Hyperlink = new XLHyperlink($"{HttpContext.Request.Host.ToUriComponent()}/api/home/getcv/{r.appliedJobs.UserId}", "Click to Open CV!");
                        foreach(var spec in r.jobSpecFiles)
                        {
                            worksheet.Cell(currentRow, 15).Value = spec.TagName;
                            worksheet.Cell(currentRow, 15).Hyperlink = new XLHyperlink($"{HttpContext.Request.Host.ToUriComponent()}/api/home/getspec/{spec.TagName}", "Click to Open");
                            currentRow++;
                        }
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "users.xlsx");
                    }
                }
                
                
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "Excel download failed" });
            }
           
        }

        [HttpGet]
        [Route("reset/{email}")]

        //Get token
        public async Task<IActionResult> RequestPasswordResetLink(string email)
        {
            var user = dbContext.Users.Where(x => x.Email == email).FirstOrDefault();
            string host = HttpContext.Request.Host.ToUriComponent();
            string protocol = HttpContext.Request.Scheme;
           // return Ok(user);
            if (user != null)
            {
                var code = await userManager.GeneratePasswordResetTokenAsync(user);

                var link = $"{code}";
                //var link = $"{protocol}/{host}/forgot?id={code}";

                try
                {
                     await mailService.SendEmailPasswordReset(user.Email, link);
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Link Mailed" });
                }
                catch (Exception x)
                {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "Reset Link email failed: "+x.Message });
                }
                //logger.LogInformation($"An password reset email was sent to {user.Email}");
            }
            else
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "User not found" });
            }

        }

        //Use Token
        [HttpPost]
        [Route("forgotten")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] ForgottenModel forgottenModel)
        {
            try
            {
                var user = dbContext.Users.Where(x => x.Email == forgottenModel.Email).FirstOrDefault();
                var resetPassResult = await userManager.ResetPasswordAsync(user, forgottenModel.Token, forgottenModel.Password);
                if (resetPassResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Passord reset Success" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "Password reset failed " });

                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "Password reset failed "+x.Message });

            }
        }


        //HOD  Upload supporting Documents

        [Route("hoduploadsupportingdocs/{ID}")]
        [HttpPost]
        public async Task<IActionResult> HODUploadSupportingDocs([FromForm] IFormFile formFile,string ID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);


                var subDirectory = "Files/Monitoring";
                var target = Path.Combine(webHostEnvironment.ContentRootPath, subDirectory);
                string fileName = new String(Path.GetFileNameWithoutExtension(formFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(formFile.FileName);
                var path = Path.Combine(target, fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }
                
                /* 
                 * var host = HttpContext.Request.Host.ToUriComponent();
                 * var url = $"{HttpContext.Request.Scheme}://{host}/{path}";
                 * return Content(url);
                */

                if (dbContext.UserCVs.Where(x => x.UserId == user.Id).Count() > 0)
                {
                    var specificCV = dbContext.UserCVs.Where(x => x.UserId == user.Id).FirstOrDefault();
                    specificCV.FilePath = path;
                    specificCV.TagName = formFile.FileName;
                    await dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "CV Updated" });

                }
                else
                {
                    UserCV cvData = new UserCV
                    {
                        UserId = user.Id,
                        FilePath = path,
                        TagName = formFile.FileName
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

    }
}
