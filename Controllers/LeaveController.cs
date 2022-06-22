﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RPFBE.Auth;
using RPFBE.Model;
using RPFBE.Model.LeaveModels;
using RPFBE.Model.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;
        private readonly IOptions<WebserviceCreds> config;

        public LeaveController(
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
        //Get Leave Type and Balance
        [Authorize]
        [HttpGet]
        [Route("getstaffleavebalance")]
        public async Task<IActionResult> GetStaffLeaveBalance()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var leavetypelist = await codeUnitWebService.Client().EmployeeLeavesAsync(user.EmployeeId);
                List<LeaveTypes> leaveTypeList = new List<LeaveTypes>();

                dynamic leavetypelistSerial = JsonConvert.DeserializeObject(leavetypelist.return_value);

                foreach (var item in leavetypelistSerial)
                {
                    LeaveTypes ltyp = new LeaveTypes
                    {
                        Value = item.Value,
                        Label = item.Label,
                        Leavebalance = item.Leavebalance,
                        Allocationdays = item.Allocationdays,
                        Employeeno = item.Employeeno
                    };
                    leaveTypeList.Add(ltyp);
                }
                return Ok(new { leaveTypeList });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Document Read check failed: " + x.Message });
            }
        }

        //Leave Application List
        [Authorize]
        [HttpGet]
        [Route("getleaveapplicationlist")]
        public async Task<IActionResult> GetLeaveApplicationList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var leaveApplication = await codeUnitWebService.HRWS().GetleavesAsync("", user.EmployeeId);
                dynamic leaveApplicationSerial = JsonConvert.DeserializeObject(leaveApplication.return_value);
                List<LeaveApplicationList> leaveApplications = new List<LeaveApplicationList>();

                foreach (var item in leaveApplicationSerial)
                {
                    LeaveApplicationList lap = new LeaveApplicationList
                    {
                        
                        No = item.No,
                        EmployeeNo = item.EmployeeNo,
                        EmployeeName = item.EmployeeName,
                        LeaveType = item.LeaveType,
                        LeaveStartDate = item.LeaveStartDate,
                        LeaveBalance = item.LeaveBalance,
                        DaysApplied = item.DaysApplied,
                        DaysApproved = item.DaysApproved,
                        LeaveEndDate = item.LeaveEndDate,
                        LeaveReturnDate = item.LeaveReturnDate,
                        ReasonForLeave = item.ReasonForLeave,
                        SubstituteEmployeeNo = item.SubstituteEmployeeNo,
                        SubstituteEmployeeName = item.SubstituteEmployeeName,
                        GlobalDimension1Code = item.GlobalDimension1Code,
                        GlobalDimension2Code = item.GlobalDimension2Code,
                        ShortcutDimension3Code = item.ShortcutDimension3Code,
                        ShortcutDimension4Code = item.ShortcutDimension4Code,
                        ShortcutDimension5Code = item.ShortcutDimension5Code,
                        ShortcutDimension6Code = item.ShortcutDimension6Code,
                        ShortcutDimension7Code = item.ShortcutDimension7Code,
                        ShortcutDimension8Code = item.ShortcutDimension8Code,
                        ResponsibilityCenter = item.ResponsibilityCenter,
                        RejectionComments = item.RejectionComments,
                        Status = item.Status,
                    };
                    leaveApplications.Add(lap);
                }
                return Ok(new { leaveApplications });

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Leave Application List Failed: " + x.Message });
            }
        }

        //Create a New Leave
        [Authorize]
        [HttpGet]
        [Route("createnewleave")]
        public async Task<IActionResult> CreateNewLeave()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                //Create a record
                //var isLeaveOpen = false;
                var isLeaveOpen = await codeUnitWebService.HRWS().CheckOpenLeaveApplicationExistsAsync(user.EmployeeId);
                if (!isLeaveOpen.return_value)
                {
                    var leaveNo = await codeUnitWebService.HRWS().CreateNewLeaveApplicationAPIAsync(user.EmployeeId);
                    //var return_value = "LA00080";
                    if (leaveNo.return_value != "false")
                    {
                        List<EmployeeListModel> employeeListModels = new List<EmployeeListModel>();

                        var resEmp = await codeUnitWebService.Client().EmployeeListAsync();
                        dynamic resEmpSerial = JsonConvert.DeserializeObject(resEmp.return_value);

                        foreach (var emp in resEmpSerial)
                        {
                            EmployeeListModel e = new EmployeeListModel
                            {
                                Value = emp.No,
                                Label = emp.Fullname,
                            };
                            employeeListModels.Add(e);

                        }
                       
                        return Ok(new { leaveNo.return_value, employeeListModels });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"New  D365 Leave Initialization Failed" });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"The Employee {user.EmployeeId} has an Open Leave" });
                }

                //Get employees
                //Get Leave types , is attachment needed?

            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Leave Create New Failed: " + x.Message });
            }
        }

        //On select Leave Type
        [Authorize]
        [HttpGet]
        [Route("onselectleavetype/{LNO}/{LTYP}")]
        public async Task<IActionResult> OnSelectLeavetype(string LNO,string LTYP)
        {
            try
            {
                //upload the leave type to portal documents
                var res =await codeUnitWebService.HRWS().InsertLeaveApplicationDocumentsAsync(LNO, LTYP);
                if (res.return_value)
                {
                    //Check of selected leave type is attachment required
                    var isAttachementRequired = await codeUnitWebService.Client().GetLeaveAttachmentStatusAsync(LTYP);

                    //return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"The Leave {LNO} of {LTYP} has been recorded" });
                    return Ok(new { isAttachementRequired.return_value });
                }
                else
                {
                    var isAttachementRequired = await codeUnitWebService.Client().GetLeaveAttachmentStatusAsync(LTYP);

                    return Ok(new { isAttachementRequired.return_value });
                    //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = $"The Leave {LNO} of {LTYP} has not been recorded " });
                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "LeaveType Create New Record Failed: " + x.Message }); 
            }
        }

        //Get End Date
        [Authorize]
        [HttpPost]
        [Route("getleaveendreturndate")]
        public async Task<IActionResult> GetLeaveEndReturnDate([FromBody] LeaveEndDate leaveEndDate)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var endDate = await codeUnitWebService.HRWS().GetLeaveEndDateAsync(user.EmployeeId, leaveEndDate.LeaveType, leaveEndDate.LeaveStartDate, leaveEndDate.DaysApplied);
                var returnDate =await codeUnitWebService.HRWS().GetLeaveReturnDateAsync(user.EmployeeId, leaveEndDate.LeaveType, leaveEndDate.LeaveStartDate, leaveEndDate.DaysApplied);

                DateTime EndDate = endDate.return_value;
                DateTime ReturnDate = returnDate.return_value;
                var EndD= EndDate.ToString("MM/dd/yyyy");
                var ReturnD= ReturnDate.ToString("MM/dd/yyyy");
                return Ok(new { EndD, ReturnD });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Get Leave End Return Date Failed: " + x.Message });
            }
        }

        //Upload Leave Attachment
        [Authorize]
        [Route("uploadleaveattachment/{LNO}/{LTYP}")]
        [HttpPost]
        public async Task<IActionResult> UploadLeaveAttachment([FromForm] IFormFile formFile,string LNO,string LTYP)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);


                var subDirectory = "Files/LeaveAttachments";
                var target = Path.Combine(webHostEnvironment.ContentRootPath, subDirectory);
                // string fileName = new String(Path.GetFileNameWithoutExtension(formFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                // DateTime.Now.ToString("yymmssfff")
                string fileName = LNO+"_"+LTYP;
                fileName = fileName + Path.GetExtension(formFile.FileName);
                var path = Path.Combine(target, fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }

                //Update portal document record
                var updateDocRec = await codeUnitWebService.DOCMGT().ModifySystemFileURLAsync(LNO, LTYP, path);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Attachment Upload" });


            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = x.Message });
            }
        }

        //View Attachment
        [Authorize]
        [Route("viewleaveattachment/{LNO}")]
        [HttpGet]
        public async Task<IActionResult> ViewLeaveAttachment(string LNO)
        {
            //var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            try
            {
                var path = await codeUnitWebService.HRWS().GenerateSupportingDocumentLinkAsync(LNO);
                string ext = Path.GetExtension(path.return_value); // getting the file extension of uploaded file  

                var file = path.return_value;

                // Response...
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = file,
                    Inline = true // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                //Response.Headers.Add("Content-Disposition", cd.ToString());
                //Response.Headers.Add("X-Content-Type-Options", "nosniff");
                return File(System.IO.File.ReadAllBytes(file), "application/pdf");

                //return ext switch
                //{
                //    //".jpeg" => File(System.IO.File.ReadAllBytes(file), "image/jpeg"),
                //    //".jpg" => File(System.IO.File.ReadAllBytes(file), "image/jpg"),
                //    //".png" => File(System.IO.File.ReadAllBytes(file), "image/png"),
                //    ".pdf" => File(System.IO.File.ReadAllBytes(file), "application/pdf"),
                //    _ => Ok(""),
                //};



            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Attachment View failed " + x.Message});
            }
        }

        //Update Leave Card
        [Authorize]
        [HttpPost]
        [Route("uploadleaveform")]
        public async Task<IActionResult> UploadLeaveForm([FromBody] LeaveEndDate leaveEnd)
        {
            try
            {
                var updateRes = await codeUnitWebService.HRWS().ModifyLeaveApplicationAsync(leaveEnd.LeaveAppNo, leaveEnd.LeaveType,
                    leaveEnd.LeaveStartDate, leaveEnd.DaysApplied, leaveEnd.RelieverRemark, leaveEnd.RelieverNo);
                //check if the leave has a valid workflow
                var isWorkflowEnabled = await codeUnitWebService.HRWS().CheckLeaveApplicationApprovalWorkflowEnabledAsync(leaveEnd.LeaveAppNo);
                if (isWorkflowEnabled.return_value)
                {
                    //send for approval

                    try
                    {
                        var isApproved = await codeUnitWebService.HRWS().SendLeaveApplicationApprovalRequestAPIAsync(leaveEnd.LeaveAppNo);
                        if (isApproved.return_value=="true")
                        {
                            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Your leave application was successfully sent for approval. Once approved, you will receive an email containing your leave details." });
                        }
                        else
                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Leave Application Approval Request Failed." });
                        }
                        //return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"Your leave application was successfully sent for approval. Once approved, you will receive an email containing your leave details [sysmes:{isApproved.return_value} ]" });

                    }
                    catch (Exception x)
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Leave Application Approval Request Failed: " + x.Message });

                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Leave Application Approval Workflow is Disabled."  });
                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Leave Data Upload Failed: " + x.Message });
            }
        
        }

        //Get Employee List
        [Authorize]
        [HttpGet]
        [Route("getemployeelist")]
        public async Task<IActionResult> GetEmployeeList()
        {
            try
            {
                List<EmployeeListModel> employeeListModels = new List<EmployeeListModel>();

                var resEmp = await codeUnitWebService.Client().EmployeeListAsync();
                dynamic resEmpSerial = JsonConvert.DeserializeObject(resEmp.return_value);

                foreach (var emp in resEmpSerial)
                {
                    EmployeeListModel e = new EmployeeListModel
                    {
                        Value = emp.No,
                        Label = emp.Fullname,
                    };
                    employeeListModels.Add(e);

                }

                return Ok(new { employeeListModels });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Employee List Fetch Failed: " + x.Message });
            }
        }


    }


}
