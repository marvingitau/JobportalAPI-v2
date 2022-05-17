using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RPFBE.Auth;
using RPFBE.Model.DBEntity;
using RPFBE.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPFBE.Controllers
{
    public enum ScaleOne
    {
        Poor = 1,
        BelowAverage,
        Average,
        AboveAverage,
        Excellent
    }
    public enum ScaleTwo
    {
        Never = 1,
        Seldom,
        Often,
        Usually,
        Always
    }

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;

        public EmployeeController(
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
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.codeUnitWebService = codeUnitWebService;
            this.mailService = mailService;
        }

        [Authorize]
        [HttpGet]
        [Route("employeedashboard")]
        public async Task<IActionResult> EmployeeDashboard()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var exitCount = dbContext.ExitInterviewCard.Where(x => x.EID == user.EmployeeId).Count();

                return Ok(new { exitCount });
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Employee Dashboard err" + x.Message });
            }
           
        }

        //Get the Form Meta data
        [Authorize]
        [HttpGet]
        [Route("employeeexitform")]
        public async Task<IActionResult> EmployeeExitFormMeta()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var exitModel = dbContext.ExitInterviewCard.Where(x => x.EID == user.EmployeeId).FirstOrDefault();
                if(exitModel != null)
                {
                    return Ok(new { exitModel });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Employee Exit Meta Null "});

                }
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Employee Exit Meta err " + x.Message });
            }
        }

        //Push the Exit Form Data
        [Authorize]
        [HttpPost]
        [Route("employeepushform")]
        public async Task<IActionResult> EmployeePushForm ([FromBody] ExitInterviewForm exitInterview)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                var duplicate = dbContext.ExitInterviewForm.Where(x => x.UID == user.Id).Count();
                if (duplicate > 0)
                {

                    //update
                    var mode = dbContext.ExitInterviewForm.Where(x => x.UID == user.Id).FirstOrDefault();
                    mode.Typeofwork = exitInterview.Typeofwork;
                    mode.Workingcondition = exitInterview.Workingcondition;
                    mode.Payment = exitInterview.Payment;
                    mode.Manager = exitInterview.Manager;

                    /*mode.Fairnessofworkload = ((ScaleOne)int.Parse(exitInterview.Fairnessofworkload)).ToString();
                      mode.Salary = ((ScaleOne)int.Parse(exitInterview.Salary)).ToString();
                      mode.WorkingconditionOne = ((ScaleOne)int.Parse(exitInterview.WorkingconditionOne)).ToString();
                      mode.Toolsprovided = ((ScaleOne)int.Parse(exitInterview.Toolsprovided)).ToString();
                      mode.Trainingreceived = ((ScaleOne)int.Parse(exitInterview.Trainingreceived)).ToString();
                      mode.Rxtioncoworker = ((ScaleOne)int.Parse(exitInterview.Rxtioncoworker)).ToString();
                      mode.Typeworkperformed = ((ScaleOne)int.Parse(exitInterview.Typeworkperformed)).ToString();
                      mode.Supervisonreceived = ((ScaleOne)int.Parse(exitInterview.Supervisonreceived)).ToString();
                      mode.Decisionaffected = ((ScaleOne)int.Parse(exitInterview.Decisionaffected)).ToString();
                      mode.Recruitmentprocess = ((ScaleOne)int.Parse(exitInterview.Recruitmentprocess)).ToString();
                      mode.Employeeorientation = ((ScaleOne)int.Parse(exitInterview.Employeeorientation)).ToString();
                      mode.Trainingopportunity = ((ScaleOne)int.Parse(exitInterview.Trainingopportunity)).ToString();
                      mode.Careerdevops = ((ScaleOne)int.Parse(exitInterview.Careerdevops)).ToString();
                      mode.Employeemorale = ((ScaleOne)int.Parse(exitInterview.Employeemorale)).ToString();
                      mode.Fairtreatment = ((ScaleOne)int.Parse(exitInterview.Fairtreatment)).ToString();
                      mode.Recognitionofwelldone = ((ScaleOne)int.Parse(exitInterview.Recognitionofwelldone)).ToString();
                      mode.Suportofworklifebal = ((ScaleOne)int.Parse(exitInterview.Suportofworklifebal)).ToString();
                      mode.Cooperationinoffice = ((ScaleOne)int.Parse(exitInterview.Cooperationinoffice)).ToString();
                      mode.Communicationmgtemp = ((ScaleOne)int.Parse(exitInterview.Communicationmgtemp)).ToString();
                      mode.Performancedevplan = ((ScaleOne)int.Parse(exitInterview.Performancedevplan)).ToString();
                      mode.Interestinvemp = ((ScaleOne)int.Parse(exitInterview.Interestinvemp)).ToString();
                      mode.CommitmentCustServ = ((ScaleOne)int.Parse(exitInterview.CommitmentCustServ)).ToString();
                      mode.ConcernedQualityExcellence = ((ScaleOne)int.Parse(exitInterview.ConcernedQualityExcellence)).ToString();
                      mode.AdminPolicy = ((ScaleOne)int.Parse(exitInterview.AdminPolicy)).ToString();
                      mode.RecognitionAccomp = ((ScaleTwo)int.Parse(exitInterview.RecognitionAccomp)).ToString();
                      mode.ClearlyCommExpectation = ((ScaleTwo)int.Parse(exitInterview.ClearlyCommExpectation)).ToString();
                      mode.TreatedFairly = ((ScaleTwo)int.Parse(exitInterview.TreatedFairly)).ToString();
                      mode.CoarchedTrainedDev = ((ScaleTwo)int.Parse(exitInterview.CoarchedTrainedDev)).ToString();
                      mode.ProvidedLeadership = ((ScaleTwo)int.Parse(exitInterview.ProvidedLeadership)).ToString();
                      mode.EncouragedTeamworkCoop = ((ScaleTwo)int.Parse(exitInterview.EncouragedTeamworkCoop)).ToString();
                      mode.ResolvedConcerns = ((ScaleTwo)int.Parse(exitInterview.ResolvedConcerns)).ToString();
                      mode.ListeningToSuggetions = ((ScaleTwo)int.Parse(exitInterview.ListeningToSuggetions)).ToString();
                      mode.KeptTeamInfo = ((ScaleTwo)int.Parse(exitInterview.KeptTeamInfo)).ToString();
                      mode.SupportedWorkLifeBal = ((ScaleTwo)int.Parse(exitInterview.SupportedWorkLifeBal)).ToString();
                      mode.AppropriateChallengingAssignments = ((ScaleTwo)int.Parse(exitInterview.AppropriateChallengingAssignments)).ToString();*/

                    mode.Fairnessofworkload = exitInterview.Fairnessofworkload.ToString();
                    mode.Salary = exitInterview.Salary.ToString();
                    mode.WorkingconditionOne = exitInterview.WorkingconditionOne.ToString();
                    mode.Toolsprovided = exitInterview.Toolsprovided.ToString();
                    mode.Trainingreceived = exitInterview.Trainingreceived.ToString();
                    mode.Rxtioncoworker = exitInterview.Rxtioncoworker.ToString();
                    mode.Typeworkperformed = exitInterview.Typeworkperformed.ToString();
                    mode.Supervisonreceived = exitInterview.Supervisonreceived.ToString();
                    mode.Decisionaffected = exitInterview.Decisionaffected.ToString();
                    mode.Recruitmentprocess = exitInterview.Recruitmentprocess.ToString();
                    mode.Employeeorientation = exitInterview.Employeeorientation.ToString();
                    mode.Trainingopportunity = exitInterview.Trainingopportunity.ToString();
                    mode.Careerdevops = exitInterview.Careerdevops.ToString();
                    mode.Employeemorale = exitInterview.Employeemorale.ToString();
                    mode.Fairtreatment = exitInterview.Fairtreatment.ToString();
                    mode.Recognitionofwelldone = exitInterview.Recognitionofwelldone.ToString();
                    mode.Suportofworklifebal = exitInterview.Suportofworklifebal.ToString();
                    mode.Cooperationinoffice = exitInterview.Cooperationinoffice.ToString();
                    mode.Communicationmgtemp = exitInterview.Communicationmgtemp.ToString();
                    mode.Performancedevplan = exitInterview.Performancedevplan.ToString();
                    mode.Interestinvemp = exitInterview.Interestinvemp.ToString();
                    mode.CommitmentCustServ = exitInterview.CommitmentCustServ.ToString();
                    mode.ConcernedQualityExcellence = exitInterview.ConcernedQualityExcellence.ToString();
                    mode.AdminPolicy = exitInterview.AdminPolicy.ToString();
                    mode.RecognitionAccomp = exitInterview.RecognitionAccomp.ToString();
                    mode.ClearlyCommExpectation = exitInterview.ClearlyCommExpectation.ToString();
                    mode.TreatedFairly = exitInterview.TreatedFairly.ToString();
                    mode.CoarchedTrainedDev = exitInterview.CoarchedTrainedDev.ToString();
                    mode.ProvidedLeadership = exitInterview.ProvidedLeadership.ToString();
                    mode.EncouragedTeamworkCoop = exitInterview.EncouragedTeamworkCoop.ToString();
                    mode.ResolvedConcerns = exitInterview.ResolvedConcerns.ToString();
                    mode.ListeningToSuggetions = exitInterview.ListeningToSuggetions.ToString();
                    mode.KeptTeamInfo = exitInterview.KeptTeamInfo.ToString();
                    mode.SupportedWorkLifeBal = exitInterview.SupportedWorkLifeBal.ToString();
                    mode.AppropriateChallengingAssignments = exitInterview.AppropriateChallengingAssignments.ToString();

                    mode.Whatulldosummarydous = exitInterview.Whatulldosummarydous;
                    mode.TheJobLeaving = exitInterview.TheJobLeaving;
                    mode.TheOrgoverla = exitInterview.TheOrgoverla;
                    mode.YourSupervisorMgr = exitInterview.YourSupervisorMgr;
                    mode.AnyOtherSuggetionQ = exitInterview.AnyOtherSuggetionQ;
                    mode.NowDate = exitInterview.NowDate;
                    mode.ExitCardRef = exitInterview.ExitCardRef;

                    dbContext.ExitInterviewForm.Update(mode);
                    await dbContext.SaveChangesAsync();

                    //Update card
                    var rec = dbContext.ExitInterviewCard.Where(x => x.Id == exitInterview.ExitCardRef).FirstOrDefault();
                    rec.FormUploaded = 1;
                    dbContext.ExitInterviewCard.Update(rec);
                    await dbContext.SaveChangesAsync();


                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "Exit form Updated" });
                }
                else
                {
                    //create
                    ExitInterviewForm aux = new ExitInterviewForm
                    {
                        UID = user.Id,
                        Typeofwork = exitInterview.Typeofwork,
                        Workingcondition = exitInterview.Workingcondition,
                        Payment = exitInterview.Payment,
                        Manager = exitInterview.Manager,

                        /*Fairnessofworkload = ((ScaleOne)int.Parse(exitInterview.Fairnessofworkload)).ToString(),
                         Salary = ((ScaleOne)int.Parse(exitInterview.Salary)).ToString(),
                         WorkingconditionOne = ((ScaleOne)int.Parse(exitInterview.WorkingconditionOne)).ToString(),
                         Toolsprovided = ((ScaleOne)int.Parse(exitInterview.Toolsprovided)).ToString(),
                         Trainingreceived = ((ScaleOne)int.Parse(exitInterview.Trainingreceived)).ToString(),
                         Rxtioncoworker = ((ScaleOne)int.Parse(exitInterview.Rxtioncoworker)).ToString(),
                         Typeworkperformed = ((ScaleOne)int.Parse(exitInterview.Typeworkperformed)).ToString(),
                         Supervisonreceived = ((ScaleOne)int.Parse(exitInterview.Supervisonreceived)).ToString(),
                         Decisionaffected = ((ScaleOne)int.Parse(exitInterview.Decisionaffected)).ToString(),
                         Recruitmentprocess = ((ScaleOne)int.Parse(exitInterview.Recruitmentprocess)).ToString(),
                         Employeeorientation = ((ScaleOne)int.Parse(exitInterview.Employeeorientation)).ToString(),
                         Trainingopportunity = ((ScaleOne)int.Parse(exitInterview.Trainingopportunity)).ToString(),
                         Careerdevops = ((ScaleOne)int.Parse(exitInterview.Careerdevops)).ToString(),
                         Employeemorale = ((ScaleOne)int.Parse(exitInterview.Employeemorale)).ToString(),
                         Fairtreatment = ((ScaleOne)int.Parse(exitInterview.Fairtreatment)).ToString(),
                         Recognitionofwelldone = ((ScaleOne)int.Parse(exitInterview.Recognitionofwelldone)).ToString(),
                         Suportofworklifebal = ((ScaleOne)int.Parse(exitInterview.Suportofworklifebal)).ToString(),
                         Cooperationinoffice = ((ScaleOne)int.Parse(exitInterview.Cooperationinoffice)).ToString(),
                         Communicationmgtemp = ((ScaleOne)int.Parse(exitInterview.Communicationmgtemp)).ToString(),
                         Performancedevplan = ((ScaleOne)int.Parse(exitInterview.Performancedevplan)).ToString(),
                         Interestinvemp = ((ScaleOne)int.Parse(exitInterview.Interestinvemp)).ToString(),
                         CommitmentCustServ = ((ScaleOne)int.Parse(exitInterview.CommitmentCustServ)).ToString(),
                         ConcernedQualityExcellence = ((ScaleOne)int.Parse(exitInterview.ConcernedQualityExcellence)).ToString(),
                         AdminPolicy = ((ScaleOne)int.Parse(exitInterview.AdminPolicy)).ToString(),
                         RecognitionAccomp = ((ScaleTwo)int.Parse(exitInterview.RecognitionAccomp)).ToString(),
                         ClearlyCommExpectation = ((ScaleTwo)int.Parse(exitInterview.ClearlyCommExpectation)).ToString(),
                         TreatedFairly = ((ScaleTwo)int.Parse(exitInterview.TreatedFairly)).ToString(),
                         CoarchedTrainedDev = ((ScaleTwo)int.Parse(exitInterview.CoarchedTrainedDev)).ToString(),
                         ProvidedLeadership = ((ScaleTwo)int.Parse(exitInterview.ProvidedLeadership)).ToString(),
                         EncouragedTeamworkCoop = ((ScaleTwo)int.Parse(exitInterview.EncouragedTeamworkCoop)).ToString(),
                         ResolvedConcerns = ((ScaleTwo)int.Parse(exitInterview.ResolvedConcerns)).ToString(),
                         ListeningToSuggetions = ((ScaleTwo)int.Parse(exitInterview.ListeningToSuggetions)).ToString(),
                         KeptTeamInfo = ((ScaleTwo)int.Parse(exitInterview.KeptTeamInfo)).ToString(),
                         SupportedWorkLifeBal = ((ScaleTwo)int.Parse(exitInterview.SupportedWorkLifeBal)).ToString(),
                         AppropriateChallengingAssignments = ((ScaleTwo)int.Parse(exitInterview.AppropriateChallengingAssignments)).ToString(),*/

                        Fairnessofworkload = exitInterview.Fairnessofworkload.ToString(),
                        Salary = exitInterview.Salary.ToString(),
                        WorkingconditionOne = exitInterview.WorkingconditionOne.ToString(),
                        Toolsprovided = exitInterview.Toolsprovided.ToString(),
                        Trainingreceived = exitInterview.Trainingreceived.ToString(),
                        Rxtioncoworker = exitInterview.Rxtioncoworker.ToString(),
                        Typeworkperformed = exitInterview.Typeworkperformed.ToString(),
                        Supervisonreceived = exitInterview.Supervisonreceived.ToString(),
                        Decisionaffected = exitInterview.Decisionaffected.ToString(),
                        Recruitmentprocess = exitInterview.Recruitmentprocess.ToString(),
                        Employeeorientation = exitInterview.Employeeorientation.ToString(),
                        Trainingopportunity = exitInterview.Trainingopportunity.ToString(),
                        Careerdevops = exitInterview.Careerdevops.ToString(),
                        Employeemorale = exitInterview.Employeemorale.ToString(),
                        Fairtreatment = exitInterview.Fairtreatment.ToString(),
                        Recognitionofwelldone = exitInterview.Recognitionofwelldone.ToString(),
                        Suportofworklifebal = exitInterview.Suportofworklifebal.ToString(),
                        Cooperationinoffice = exitInterview.Cooperationinoffice.ToString(),
                        Communicationmgtemp = exitInterview.Communicationmgtemp.ToString(),
                        Performancedevplan = exitInterview.Performancedevplan.ToString(),
                        Interestinvemp = exitInterview.Interestinvemp.ToString(),
                        CommitmentCustServ = exitInterview.CommitmentCustServ.ToString(),
                        ConcernedQualityExcellence = exitInterview.ConcernedQualityExcellence.ToString(),
                        AdminPolicy = exitInterview.AdminPolicy.ToString(),
                        RecognitionAccomp = exitInterview.RecognitionAccomp.ToString(),
                        ClearlyCommExpectation = exitInterview.ClearlyCommExpectation.ToString(),
                        TreatedFairly = exitInterview.TreatedFairly.ToString(),
                        CoarchedTrainedDev = exitInterview.CoarchedTrainedDev.ToString(),
                        ProvidedLeadership = exitInterview.ProvidedLeadership.ToString(),
                        EncouragedTeamworkCoop = exitInterview.EncouragedTeamworkCoop.ToString(),
                        ResolvedConcerns = exitInterview.ResolvedConcerns.ToString(),
                        ListeningToSuggetions = exitInterview.ListeningToSuggetions.ToString(),
                        KeptTeamInfo = exitInterview.KeptTeamInfo.ToString(),
                        SupportedWorkLifeBal = exitInterview.SupportedWorkLifeBal.ToString(),
                        AppropriateChallengingAssignments = exitInterview.AppropriateChallengingAssignments.ToString(),

                        Whatulldosummarydous = exitInterview.Whatulldosummarydous,
                        TheJobLeaving = exitInterview.TheJobLeaving,
                        TheOrgoverla = exitInterview.TheOrgoverla,
                        YourSupervisorMgr = exitInterview.YourSupervisorMgr,
                        AnyOtherSuggetionQ = exitInterview.AnyOtherSuggetionQ,
                        NowDate = exitInterview.NowDate,
                        ExitCardRef = exitInterview.ExitCardRef

                    };

                    dbContext.ExitInterviewForm.Add(aux);
                    await dbContext.SaveChangesAsync();

                    //Update card
                    var rec= dbContext.ExitInterviewCard.Where(x => x.Id == exitInterview.ExitCardRef).FirstOrDefault();
                    rec.FormUploaded = 1;
                    dbContext.ExitInterviewCard.Update(rec);
                    await dbContext.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Succes", Message = "Exit form Uploaded" });
                }
               

                //return Ok(aux);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Employee Exit Form Push err " + x.Message });
            }
        }
    
        //HR Get the Form Data
        [Authorize]
        [HttpGet]
        [Route("hrgetexitform/{ID}")]

        public IActionResult HRGetExitForm(string ID)
        {
            try
            {
                var formModel = dbContext.ExitInterviewForm.Where(x => x.ExitCardRef == int.Parse(ID)).FirstOrDefault();
                return Ok(formModel);
            }
            catch (Exception x)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Employee Exit Form err " + x.Message });
            }
        }
    
    }
}
