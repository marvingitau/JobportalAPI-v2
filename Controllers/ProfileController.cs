using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RPFBE.Auth;
using RPFBE.Model;
using RPFBE.Model.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext dbContext;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ICodeUnitWebService codeUnitWebService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ICodeUnitWebService codeUnitWebService
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.codeUnitWebService = codeUnitWebService;
        }
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var user =await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var userRoles = await userManager.GetRolesAsync(user);

            return Ok(user);
        }

        //Get the profile date of usr id

        //Skills
        [Route("getskills")]
        [HttpGet]
        public async Task<IActionResult> GetSkill()
        {
            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var res = dbContext.Skills.Where(x => x.UserId == user.Id).ToList();
            return Ok(res);
        }
        [Route("setskills")]
        [HttpPost]
        public async Task<IActionResult> SetSkill([FromBody] List<Skill> Skills)
        {
            List<Skill> skills = new List<Skill>();

            var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            if(dbContext.Skills.Where(x => x.UserId == user.Id).Count()>0)
            {
                dbContext.Skills.RemoveRange(dbContext.Skills.Where(x => x.UserId == user.Id));
                await dbContext.SaveChangesAsync();
            }

            foreach (var item in Skills)
            {
                var aux = new Skill
                {
                    UserId = user.Id,
                    Title = item.Title,
                    ExperienceYears = ""

                };
                skills.Add(aux);
            }

            await dbContext.Skills.AddRangeAsync(skills);
            await dbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Skill uploaded" });
        }
        

        
        [Route("profile")]
        [HttpGet]
        public async Task<ActionResult> GetProfile()
        {
            //List<Profile> profile = new List<Profile>();

            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (user.ProfileId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User dont have profile" });
                }
                var usrModel = dbContext.Users.Where(x => x.Id == user.Id).FirstOrDefault(); //.Where(y => y.ProfileId != 0)
                var profileModel = dbContext.Profiles.Where(x=>x.UserId== user.Id).FirstOrDefault();
                var skillList = dbContext.Skills.Where(x => x.UserId == user.Id).ToList();
                var userCV = dbContext.UserCVs.Where(x => x.UserId == user.Id).FirstOrDefault();

                //get bankcode
                List<BankModel> bankModels = new List<BankModel>();

                var bankres = await codeUnitWebService.Client().GetBanksAsync();
                dynamic bankserial = JsonConvert.DeserializeObject(bankres.return_value);

                foreach (var bb in bankserial)
                {
                    BankModel bankModel = new BankModel
                    {
                        Value = bb.Code,
                        Label = bb.Name,
                    };
                    bankModels.Add(bankModel);
                }
                
                //get branch
                return Ok(new { usrModel, profileModel, skillList, userCV , bankModels });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "try failed"+ex.Message });

            }
        }

        //get bank branch codes
        [Route("getbranch/{code}")]
        [HttpGet]
        public async Task<IActionResult> Getbranchbank(string code ="0000")
        {
            try
            {
                List<BankBranchModel> bankBranches = new List<BankBranchModel>();
                //bank branch list
                var res = await codeUnitWebService.Client().GetBranchAsync(code);
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                foreach (var bbranch in resSerial)
                {
                    BankBranchModel bank = new BankBranchModel
                    {
                        Value = bbranch.Branchcode,
                        Label = bbranch.Branchname,
                    };
                    bankBranches.Add(bank);

                }

                return Ok(new { bankBranches });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "get branch failed" + ex.Message });
            }
        }
        //Admin
        [Route("profile/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetProfile(string id)
        {
            //List<Profile> profile = new List<Profile>();

            try
            {
                var user = await userManager.FindByIdAsync(id);
                if (user.ProfileId == 0)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User dont have profile" });
                }
                var userModel = dbContext.Users.Where(x => x.Id == user.Id).Where(y => y.ProfileId != 0).FirstOrDefault();
                var profileModel = dbContext.Profiles.Where(x => x.UserId == user.Id).FirstOrDefault();
                var skillList = dbContext.Skills.Where(x => x.UserId == user.Id).ToList();
                var checkList = dbContext.SpecFiles.Where(x => x.UserId == user.Id).ToList();
                return Ok(new { userModel,profileModel, skillList, checkList });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "try failed" + ex.Message });

            }
        }
        [Route("setprofile")]
        [HttpPost]
        public async Task<IActionResult> SetProfile([FromBody] Profile profile)
        {

            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (user.ProfileId == 0)
                {
                    profile.UserId = user.Id;
                    dbContext.Profiles.Add(profile);
                    await dbContext.SaveChangesAsync();
                    var pid = profile.Id;

                    var usrrec = dbContext.Users.Where(x => x.Id == user.Id).FirstOrDefault();

                    if (usrrec != null)
                    {
                        usrrec.ProfileId = pid;
                        await dbContext.SaveChangesAsync();

                        return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "User profile created" });
                    }
                    return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found" });
                }
                else
                {
                    //Update
                    var current = dbContext.Profiles.First(x => x.Id == user.ProfileId);
                    try
                    {
                        //dbContext.Entry(current).CurrentValues.SetValues(profile);
                        current.Gender = profile.Gender;
                        current.PersonWithDisability = profile.PersonWithDisability;
                        current.FirstName = profile.FirstName;
                        current.SurName = profile.SurName;
                        current.LastName = profile.LastName;
                        current.DOB = profile.DOB;
                        current.Age = profile.Age;
                        current.PostalAddress = profile.PostalAddress;
                        current.PostCode = profile.PostCode;
                        current.City = profile.City;
                        current.Country = profile.Country;
                        current.County = profile.County;
                        current.SubCounty = profile.SubCounty;
                        current.ResidentialAddress = profile.ResidentialAddress;
                        current.MobilePhoneNo = profile.MobilePhoneNo;
                        current.MobilePhoneNoAlt = profile.MobilePhoneNoAlt;
                        current.BirthCertificateNo = profile.BirthCertificateNo;
                        current.NationalIDNo = profile.NationalIDNo;
                        current.HudumaNo = profile.HudumaNo;
                        current.PassPortNo = profile.PassPortNo;
                        current.PinNo = profile.PinNo;
                        current.NHIFNo = profile.NHIFNo;
                        current.NSSFNo = profile.NSSFNo;
                        current.DriverLincenceNo = profile.DriverLincenceNo;
                        current.MaritalStatus = profile.MaritalStatus;
                        current.Citizenship = profile.Citizenship;
                        current.Ethnicgroup = profile.Ethnicgroup;
                        current.Religion = profile.Religion;
                        current.BankCode = profile.BankCode;
                        current.BankName = profile.BankName;
                        current.BankBranchCode = profile.BankBranchCode;
                        current.Experience = profile.Experience;
                        current.BankBranchName = profile.BankBranchName;
                        current.UserId = user.Id;

                        current.WillingtoRelocate = profile.WillingtoRelocate;
                        current.HighestEducation = profile.HighestEducation;
                        current.CurrentSalary = profile.CurrentSalary;
                        current.ExpectedSalary = profile.ExpectedSalary;

                        dbContext.Profiles.Update(current);

                        await dbContext.SaveChangesAsync();
                        return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "User profile updated" });
                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Success", Message = "User profile not updated" });
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = ex.Message });

            }
        }
        [Route("createapp/{reqNo}/{UID}")]
        [HttpGet]
        public async Task<IActionResult> CreateJobApplication(string reqNo,string UID )
        {
            var employee = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            string JobAppCode = "";
            if (ModelState.IsValid)
            {
                try
                {
                    JobAppCode = codeUnitWebService.Client().PostJobApplicationAsync(reqNo, employee.EmployeeId).Result.return_value;
                    // var jobModel = dbContext.AppliedJobs.First(x => x.JobReqNo == reqNo);
                    var jobModel = dbContext.AppliedJobs.Where(x => x.JobReqNo == reqNo && x.UserId ==UID).FirstOrDefault();

                    jobModel.Viewed = true;
                    jobModel.JobAppplicationNo = JobAppCode;
                    // await dbContext.SaveChangesAsync();
                    dbContext.AppliedJobs.Update(jobModel);
                    await dbContext.SaveChangesAsync();
                    return Ok(JobAppCode);
                }
                catch(Exception x)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Job Application Failed "+x.Message });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "Job Application Failed" });
            }
           
        }


        [Route("modifyapp/{jobAppNo}/{UID}")]
        [HttpGet]
        public async Task<IActionResult> ModifyJobApplication(string jobAppNo,string UID)
        {
            try
            {
                var user = await userManager.FindByIdAsync(UID);
                Profile current = dbContext.Profiles.First(x => x.Id == user.ProfileId);
                var Client = await userManager.FindByIdAsync(current.UserId);
        
               

                try
                {
                    //check DateTime.ParseExact(LeaveApplicationObj.LeaveStartDate, "MM/dd/yy", null)
                    //Push primary User data
                 

                    //String Array
                    string[] textUserData = new string[35];
                    textUserData[0] = current.Gender;
                    textUserData[1] = current.PersonWithDisability;
                    textUserData[2] = "";

                    textUserData[3] = current.City;
                    textUserData[4] = current.Country;
                    textUserData[5] = current.County;
                    textUserData[6] = current.SubCounty;
                    textUserData[7] = current.ResidentialAddress;
                    textUserData[8] = current.MobilePhoneNo;

                    textUserData[9] = "";//current.MobilePhoneNoAlt;
                    textUserData[10] = current.BirthCertificateNo;
                    textUserData[11] = current.HudumaNo;
                    textUserData[12] = current.PassPortNo;
                    textUserData[13] = current.PinNo;
                    textUserData[14] = current.NHIFNo;
                    textUserData[15] = current.NSSFNo;
                    textUserData[16] = current.DriverLincenceNo;

                    textUserData[17] = current.MaritalStatus;
                    textUserData[18] = current.Citizenship;
                    textUserData[19] = current.Ethnicgroup;
                    textUserData[20] = current.Religion;
                    textUserData[21] = current.BankName;
                    textUserData[22] = current.BankBranchName;

                    textUserData[23] = current.Age;
                    textUserData[24] = current.PostalAddress;
                    textUserData[25] = current.PostCode;
                    textUserData[26] = current.NationalIDNo;
                    textUserData[27] = current.BankCode;
                    textUserData[28] = "";
                    textUserData[29] = current.BankBranchCode;

                    textUserData[30] = current.SurName;
                    textUserData[31] = current.FirstName;
                    textUserData[32] = current.LastName;
                    textUserData[33] = Client.Email;



                    char[] delimiterChars = { '-', 'T' };
                    string text = current.DOB;

                    string[] words = text.Split(delimiterChars);
                    string auxDate = words[1] + "/" + words[2] + "/" + words[0];

                    DateTime datetime = DateTime.ParseExact(auxDate, "MM/dd/yyyy", null);

                    var res = codeUnitWebService.Client().JobApplicationModifiedAsync(jobAppNo, textUserData, datetime).Result.return_value;
                    return Ok(res);
                }
                catch (Exception x)
                {

                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "Job creation Update failed "+x.Message });
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status503ServiceUnavailable, new Response { Status = "Error", Message = "User  not found" });
            }
           

        }
    
}
}
