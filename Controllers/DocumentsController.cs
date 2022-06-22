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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;
        private readonly IOptions<WebserviceCreds> config;

        public DocumentsController(
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

        //Get Employee Documents en filter according to payroll
        [Authorize]
        [HttpGet]
        [Route("employeedocuments")]
        public async Task<IActionResult> EmployeeDocuments()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                List<DocumentListModel> documentListAll = new List<DocumentListModel>();
                List<DocumentListModel> documentList = new List<DocumentListModel>();
                if (user.Payrollcode == null)
                {
                    

                    var uRes = await codeUnitWebService.Client().GetUserPayrollDataAsync(user.EmployeeId);
                    dynamic uResSerial = JsonConvert.DeserializeObject(uRes.return_value);

                    foreach (var item in uResSerial)
                    {
                        UserModel umodel = new UserModel
                        {
                            Payrollcode = item.Payrollcode,
                            Calculationscheme = item.Calculationschema
                        };
                        user.Calculationscheme = item.Calculationschema;
                        user.Payrollcode = item.Payrollcode;
                        dbContext.Users.Update(user);
                        await dbContext.SaveChangesAsync();
                    }

                    var documentRes = await codeUnitWebService.HRWS().GetEmployeeDocumentsListAsync(user.EmployeeId);
                    dynamic docOutSerialIn = JsonConvert.DeserializeObject(documentRes.return_value);
                    foreach (var item in docOutSerialIn)
                    {
                        DocumentListModel dlm = new DocumentListModel
                        {
                            EmployeeNo = item.EmployeeNo,
                            DocumentCode = item.DocumentCode,
                            DocumentName = item.DocumentName,
                            Read = item.Read,
                            DateTimeRead = item.DateTimeRead,
                            Payrollcode = item.Payrollcode,
                            URL = item.URL,

                        };
                        documentListAll.Add(dlm);

                    }
                    documentList = documentListAll.Where(x => x.Payrollcode == user.Payrollcode || x.Payrollcode == "").ToList();
                    return Ok(new { documentList });
                }
                var documentResOut = await codeUnitWebService.HRWS().GetEmployeeDocumentsListAsync(user.EmployeeId);
                dynamic docOutSerial = JsonConvert.DeserializeObject(documentResOut.return_value);
                foreach (var item in docOutSerial)
                {
                    DocumentListModel dlm = new DocumentListModel
                    {
                        EmployeeNo = item.EmployeeNo,
                        DocumentCode = item.DocumentCode,
                        DocumentName = item.DocumentName,
                        Read = item.Read,
                        DateTimeRead = item.DateTimeRead,
                        Payrollcode = item.Payrollcode,
                        URL = item.URL

                    };
                    documentListAll.Add(dlm);

                }
                documentList = documentListAll.Where(x => x.Payrollcode == user.Payrollcode || x.Payrollcode == "").ToList();
                return Ok(new { documentList });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Documents Fetch failed: " + x.Message });
            }
        }

        //Read Employee Document
        [Authorize]
        [HttpGet]
        [Route("reademployeedocument/{Filename}/{Doccode}")]
        public async Task<IActionResult> ReadEmployeeDocument(string Filename,string Doccode)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var file = config.Value.HRDocFilePath+Filename;

                //byte[] b = System.IO.File.ReadAllBytes(file);
                //return  Convert.ToBase64String(b);

            //// Response...
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

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Document Fetch failed: " + x.Message });
            }
        }

        //Mark as Read Document
        [Authorize]
        [HttpGet]
        [Route("viewedemployeedocument/{EID}/{DID}")]
        public async Task<IActionResult> ViewedEmployeeDocument(string EID,int DID)
        {
            try
            {
                var readStatus = await codeUnitWebService.HRWS().SignEmployeeDocumentsAsync(EID, DID);
                if (readStatus.return_value)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "Checked the Document D365" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Document Read check failed D365" });
                }
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Document Read check failed: " + x.Message });
            }
        }
    }
}
