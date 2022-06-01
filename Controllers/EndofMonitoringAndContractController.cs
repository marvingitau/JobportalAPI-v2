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
    public class EndofMonitoringAndContractController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<HomeController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICodeUnitWebService codeUnitWebService;
        private readonly IMailService mailService;
        private readonly IOptions<WebserviceCreds> config;

        public EndofMonitoringAndContractController(
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
        //Used for probation and endof contract

        //[Authorize]
        [HttpGet]
        [Route("createprobationview")]
        public async Task<IActionResult> EmployeeProbationProgress()
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

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create Probation View Failed: "+x.Message });
            }
           
        }

        [Authorize]
        [HttpPost]
        [Route("storeprobationcreate")]
        public async Task<IActionResult> StoreProbationCreate([FromBody] EmployeeEndofForm employeeEndofForm)
        {
            try
            {
                List<ProbationProgress> probationProgresses = new List<ProbationProgress>();
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var res = await codeUnitWebService.Client().CreateProbationProgressGeneralAsync(employeeEndofForm.EmpID,
                    employeeEndofForm.MgrID, employeeEndofForm.SupervisionTime, employeeEndofForm.ImportantSkills
                    );

                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);


                foreach (var item in resSerial)
                {
                    ProbationProgress pp = new ProbationProgress
                    {
                        UID = user.Id,
                        ProbationStatus = 0,
                        ProbationNo = item.Probationno,

                        EmpID = item.Employeeno,
                        EmpName = item.Employeename,
                        MgrID = item.Managerno,
                        MgrName = item.Managername,
                        CreationDate = item.Creationdate,
                        Department = item.Department,
                        Status = item.Status,
                        Position = item.Position,
                    };
                    dbContext.ProbationProgress.Add(pp);
                    await dbContext.SaveChangesAsync();

                    return Ok(true);

                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "D365 Create Probation Store Failed" });
               
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create Probation Store Failed: " + x.Message });
            }
        }

        //Get the individual(manager) list of created probations
        [Authorize]
        [HttpGet]
        [Route("getprobationlist")]
        public async Task<IActionResult> GetProbationList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                //var objRes = await codeUnitWebService.Client().GetProbationProgressGeneralListAsync(user.EmployeeId.ToUpper());
                //dynamic objSerial = JsonConvert.DeserializeObject(objRes.return_value);

                //List<EmployeeEndofForm> employeeEndofs = new List<EmployeeEndofForm>();

                //foreach (var item in objSerial)
                //{
                //    EmployeeEndofForm endofForm = new EmployeeEndofForm
                //    {
                //        EmpID = item.Employeeno,
                //        EmpName = item.Employeename,
                //        CreationDate = item.Creationdate,
                //        Department = item.Department,
                //        Status = item.Status,
                //        Position = item.Position,
                //        Probationno = item.Probationno
                //    };
                //    employeeEndofs.Add(endofForm);


                //}
                var employeeEndofs = dbContext.ProbationProgress.Where(x => x.MgrID == user.EmployeeId && x.ProbationStatus==0).ToList();

                return Ok(new { employeeEndofs });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation List Failed: " + x.Message });
            }
        }

        // Get individual single probation
        [Authorize]
        [HttpGet]
        [Route("getprobationcard/{CardID}")]
        public async Task<IActionResult> GetProbationCard(string CardID)
        {
            try
            {
                var res = await codeUnitWebService.Client().GetProbationProgressGeneralAsync(CardID);
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);
                List<EmployeeEndofForm> employeeEndofForms = new List<EmployeeEndofForm>();

                foreach (var item in resSerial)
                {
                    EmployeeEndofForm endofForm = new EmployeeEndofForm
                    {
                        Probationno = item.Probationno,
                        EmpName = item.Employeename,
                        CreationDate = item.Creationdate,
                        Department = item.Department,
                        Status = item.Status,
                        Position = item.Position
                    };

                    employeeEndofForms.Add(endofForm);
                }

                    return Ok(new { employeeEndofForms });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Failed: " + x.Message });
            }
        }

        //Upload Probation Section One Data
        [Authorize]
        [HttpPost]
        [Route("uploadprobationsectionone/{PID}")]
        public async Task<IActionResult> UploadProbationSectionOne([FromBody] ProbationFirstSection probationFirstSection, string PID)
        {
            try
            {
                bool[] boolData = new bool[100];
                string[] commentArr = new string[20];

                commentArr[0] = probationFirstSection.PerformanceComment;
                commentArr[1] = probationFirstSection.AttendanceComment;
                commentArr[2] = probationFirstSection.AttitudeComment;
                commentArr[3] = probationFirstSection.AppearanceComment;
                commentArr[4] = probationFirstSection.InitiativeComment;
                commentArr[5] = probationFirstSection.DependabilityComment;
                commentArr[6] = probationFirstSection.JudmentComment;
                commentArr[7] = probationFirstSection.AttentionToDetailComment;
                commentArr[8] = probationFirstSection.InterpersonalComment;
                commentArr[9] = probationFirstSection.MannersComment;
                commentArr[10] = probationFirstSection.ResponsiblityComment;
                commentArr[11] = probationFirstSection.LearningCampacityComment;
                commentArr[12] = probationFirstSection.OutputComment;
                commentArr[13] = probationFirstSection.LeadershipComment;
                commentArr[14] = probationFirstSection.PressureComment;

                boolData[0] = probationFirstSection.Outstanding;
                boolData[1] = probationFirstSection.AboveAverage;
                boolData[2] = probationFirstSection.Satisfactory;
                boolData[3] = probationFirstSection.Marginal;
                boolData[4] = probationFirstSection.Unsatisfactory;

                boolData[5] = probationFirstSection.ExcellentAttendance;
                boolData[6] = probationFirstSection.OccasionalAbsence;
                boolData[7] = probationFirstSection.RepeatedAbsence;
                boolData[8] = probationFirstSection.UnjustifiedAbsence;

                boolData[9] = probationFirstSection.AlwaysInterested;
                boolData[10] = probationFirstSection.ReasonablyDevoted;
                boolData[11] = probationFirstSection.PassiveAttitude;
                boolData[12] = probationFirstSection.ActiveDislikeofWork;

                boolData[13] = probationFirstSection.AlwaysNeat;
                boolData[14] = probationFirstSection.GenerallyNeat;
                boolData[15] = probationFirstSection.SometimesCareles;
                boolData[16] = probationFirstSection.AttirenotSuitable;

                boolData[17] = probationFirstSection.SelfStarter;
                boolData[18] = probationFirstSection.NeedsStimilus;
                boolData[19] = probationFirstSection.NeedsCSupervision;
                boolData[20] = probationFirstSection.ShowNoInitiative;

                boolData[21] = probationFirstSection.AlwayOnTime;
                boolData[22] = probationFirstSection.OccasionallyLate;
                boolData[23] = probationFirstSection.RepeatedLate;
                boolData[24] = probationFirstSection.RarelyOnTime;

                boolData[25] = probationFirstSection.DecisionLogical;
                boolData[26] = probationFirstSection.GenSoundJudgment;
                boolData[27] = probationFirstSection.ReqFreqCorrection;
                boolData[28] = probationFirstSection.JudgmentOftenFaulty;

                boolData[29] = probationFirstSection.RarelyMakesErrs;
                boolData[30] = probationFirstSection.FewErrThanMost;
                boolData[31] = probationFirstSection.AvgAccuracy;
                boolData[32] = probationFirstSection.UnacceptablyErratic;

                boolData[33] = probationFirstSection.FriendlyOutgoing;
                boolData[34] = probationFirstSection.SomewhatBusinesslike;
                boolData[35] = probationFirstSection.GregariousToPoint;
                boolData[36] = probationFirstSection.SullenAndWithdrawn;

                boolData[37] = probationFirstSection.AlwayscourteousTactful;
                boolData[38] = probationFirstSection.GenCourteous;
                boolData[39] = probationFirstSection.SometimesIncosiderate;
                boolData[40] = probationFirstSection.ArouseAntagonism;

                boolData[41] = probationFirstSection.SeeksAddResponsibility;
                boolData[42] = probationFirstSection.WillinglyAcceptResp;
                boolData[43] = probationFirstSection.AssumesWhenUnavoidable;
                boolData[44] = probationFirstSection.AlwaysAvoidResponsibility;

                boolData[45] = probationFirstSection.GraspImmediately;
                boolData[46] = probationFirstSection.QuickerThanAvg;
                boolData[47] = probationFirstSection.AvgLearning;
                boolData[48] = probationFirstSection.SlowLearner;
                boolData[49] = probationFirstSection.UnableToGraspNew;

                boolData[50] = probationFirstSection.ExcepHighProductivity;
                boolData[51] = probationFirstSection.CompleteMoreThanAvg;
                boolData[52] = probationFirstSection.AdequatePerHr;
                boolData[53] = probationFirstSection.InadequateOutput;

                boolData[54] = probationFirstSection.AssumesLeadershipInit;
                boolData[55] = probationFirstSection.WillLeadEncouraged;
                boolData[56] = probationFirstSection.CanLeadifNecessary;
                boolData[57] = probationFirstSection.RefusesLeadership;
                boolData[58] = probationFirstSection.AttemptbutInefficient;

                boolData[59] = probationFirstSection.NeverFalter;
                boolData[60] = probationFirstSection.MaintainPoise;
                boolData[61] = probationFirstSection.DependableExcUnderPress;
                boolData[62] = probationFirstSection.CantTakePressure;

                var res = await codeUnitWebService.Client().UpdateProbationProgressFirstSectionAsync(PID, boolData, commentArr);
                

                return Ok(res.return_value);
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Section One Upload Failed: " + x.Message });
            }
        }
        //Upload Probation Recommendation Section
        [Authorize]
        [HttpPost]
        [Route("uploadprobationrecommendation/{PID}")]
        public async Task<IActionResult> UploadProbationRecommendation([FromBody] ProbationRecommendation probationRecommendation,string PID)
        {
            try
            {
                string[] textArr = new string[10];
                bool[] boolArr = new bool[5];

                textArr[0] = probationRecommendation.EmployeeStrongestPoint;
                textArr[1] = probationRecommendation.EmployeeWeakestPoint;
                textArr[2] = probationRecommendation.EmployeeQualifiedForPromo;
                textArr[3] = probationRecommendation.PromoPosition;
                textArr[4] = probationRecommendation.PromotableInTheFuture;
                textArr[5] = probationRecommendation.EffectiveDifferentAssignment;
                textArr[6] = probationRecommendation.WhichAssignment;
                textArr[7] = probationRecommendation.AdditionalComment;

                boolArr[0] = probationRecommendation.confirm;
                boolArr[1] = probationRecommendation.Extend;
                boolArr[2] = probationRecommendation.Terminate;

                var res = await codeUnitWebService.Client().UpdateProbationRecommendationSectionAsync(PID,textArr, boolArr);
                return Ok(res.return_value);

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Recommendation Upload Failed: " + x.Message });
            }
        }

        //Move Probation To HR
        [Authorize]
        [HttpGet]
        [Route("moveprobationfrommanagertohr/{PID}")]
        public async Task<IActionResult> MoveProbationFromManagerToHR(string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                var probModel = dbContext.ProbationProgress.Where(p => p.ProbationNo == PID && p.ProbationStatus ==0).First();
                probModel.UID = user.Id;
                probModel.ProbationStatus = 1;

                dbContext.ProbationProgress.Update(probModel);
                await dbContext.SaveChangesAsync();

                //Mail HR
                var emailArr = dbContext.Users.Where(x => x.Rank == "HR")
                    .Select(t => t.Email).ToArray();

                var unameArr = dbContext.Users.Where(x => x.Rank == "HR")
                    .Select(t => t.UserName).ToArray();

                List<ProbationProgressMail> v = new List<ProbationProgressMail>();
               // v.AddRange(userList);
               mailService.SendEmail(emailArr, unameArr, PID);

               // return Ok(userList);

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Probation Moved: "});
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Move Failed: " + x.Message });
            }
        }

        //Probation Card Data
        [Authorize]
        [HttpGet]
        [Route("probationcarddata/{PID}")]
        public async Task<IActionResult> ProbationCardData(string PID)
        {
            try
            {
                List<ProbationFirstSection> probationFirstList = new List<ProbationFirstSection>();
                var res = await codeUnitWebService.Client().GetProbationCardDataAsync(PID);
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                foreach (var item in resSerial)
                {
                    ProbationFirstSection probationFirstSection = new ProbationFirstSection
                    {

                        Probationno=item.Probationno,
                        Employeeno = item.Employeeno,
                        Employeename =item.Employeename,
                        Creationdate =item.Creationdate,
                        Department =item.Department,
                        Status =item.Status,
                        Position =item.Position,
                        Managername =item.Manangername,
                        Skill = item.Skill,


                        Outstanding = item.Outstanding == "Yes" ? true : false,
                        AboveAverage = item.Aboveaverage == "Yes" ? true : false,
                        Satisfactory = item.Satisfactory == "Yes" ? true : false,
                        Marginal = item.Marginal == "Yes" ? true : false,
                        Unsatisfactory = item.Unsatisfactory == "Yes" ? true : false,
                        PerformanceComment = "",

                        ExcellentAttendance = item.ExcellentAttendance == "Yes" ? true : false,
                        OccasionalAbsence = item.OccasionalAbsence == "Yes" ? true : false,
                        RepeatedAbsence = item.RepeatedAbsence == "Yes" ? true : false,
                        UnjustifiedAbsence = item.UnjustifiedAbsence == "Yes" ? true : false,
                        AttendanceComment = item.AttendanceComment,

                        AlwaysInterested = item.AlwaysInterested == "Yes" ? true : false,
                        ReasonablyDevoted = item.ReasonablyDevoted == "Yes" ? true : false,
                        PassiveAttitude = item.PassiveAttitude == "Yes" ? true : false,
                        ActiveDislikeofWork = item.ActiveDislikeofWork == "Yes" ? true : false,
                        AttitudeComment = item.AttitudeComment,

                        AlwaysNeat = item.AlwaysNeat == "Yes" ? true : false,
                        GenerallyNeat = item.GenerallyNeat == "Yes" ? true : false,
                        SometimesCareles = item.SometimesCareles == "Yes" ? true : false,
                        AttirenotSuitable = item.AttirenotSuitable == "Yes" ? true : false,
                        AppearanceComment = item.AppearanceComment,


                        SelfStarter = item.SelfStarter == "Yes" ? true : false,
                        NeedsStimilus = item.NeedsStimilus == "Yes" ? true : false,
                        NeedsCSupervision = item.NeedsCSupervision == "Yes" ? true : false,
                        ShowNoInitiative = item.ShowNoInitiative == "Yes" ? true : false,
                        InitiativeComment = item.InitiativeComment,

                        AlwayOnTime = item.AlwayOnTime == "Yes" ? true : false,
                        OccasionallyLate = item.OccasionallyLate == "Yes" ? true : false,
                        RepeatedLate = item.RepeatedLate == "Yes" ? true : false,
                        RarelyOnTime = item.RarelyOnTime == "Yes" ? true : false,
                        DependabilityComment = item.DependabilityComment,

                        DecisionLogical = item.DecisionLogical == "Yes" ? true : false,
                        GenSoundJudgment = item.GenSoundJudgment == "Yes" ? true : false,
                        ReqFreqCorrection = item.ReqFreqCorrection == "Yes" ? true : false,
                        JudgmentOftenFaulty = item.JudgmentOftenFaulty == "Yes" ? true : false,
                        JudmentComment = item.JudmentComment,

                        RarelyMakesErrs = item.RarelyMakesErrs == "Yes" ? true : false,
                        FewErrThanMost = item.FewErrThanMost == "Yes" ? true : false,
                        AvgAccuracy = item.AvgAccuracy == "Yes" ? true : false,
                        UnacceptablyErratic = item.UnacceptablyErratic == "Yes" ? true : false,
                        AttentionToDetailComment = item.AttentionToDetailComment,

                        FriendlyOutgoing = item.FriendlyOutgoing == "Yes" ? true : false,
                        SomewhatBusinesslike = item.SomewhatBusinesslike == "Yes" ? true : false,
                        GregariousToPoint = item.GregariousToPoint == "Yes" ? true : false,
                        SullenAndWithdrawn = item.SullenAndWithdrawn == "Yes" ? true : false,
                        InterpersonalComment = item.InterpersonalComment,

                        AlwayscourteousTactful = item.AlwayscourteousTactful == "Yes" ? true : false,
                        GenCourteous = item.GenCourteous == "Yes" ? true : false,
                        SometimesIncosiderate = item.SometimesIncosiderate == "Yes" ? true : false,
                        ArouseAntagonism = item.ArouseAntagonism == "Yes" ? true : false,
                        MannersComment = item.MannersComment,

                        SeeksAddResponsibility = item.SeeksAddResponsibility == "Yes" ? true : false,
                        WillinglyAcceptResp = item.WillinglyAcceptResp == "Yes" ? true : false,
                        AssumesWhenUnavoidable = item.AssumesWhenUnavoidable == "Yes" ? true : false,
                        AlwaysAvoidResponsibility = item.AlwaysAvoidResponsibility == "Yes" ? true : false,
                        ResponsiblityComment = item.ResponsiblityComment,

                        GraspImmediately = item.GraspImmediately == "Yes" ? true : false,
                        QuickerThanAvg = item.QuickerThanAvg == "Yes" ? true : false,
                        AvgLearning = item.AvgLearning == "Yes" ? true : false,
                        SlowLearner = item.SlowLearner == "Yes" ? true : false,
                        UnableToGraspNew = item.UnableToGraspNew == "Yes" ? true : false,
                        LearningCampacityComment = item.LearningCampacityComment,

                        ExcepHighProductivity = item.ExcepHighProductivity == "Yes" ? true : false,
                        CompleteMoreThanAvg = item.CompleteMoreThanAvg == "Yes" ? true : false,
                        AdequatePerHr = item.AdequatePerHr == "Yes" ? true : false,
                        InadequateOutput = item.InadequateOutput == "Yes" ? true : false,
                        OutputComment = item.OutputComment,

                        AssumesLeadershipInit = item.AssumesLeadershipInit == "Yes" ? true : false,
                        WillLeadEncouraged = item.WillLeadEncouraged == "Yes" ? true : false,
                        CanLeadifNecessary = item.CanLeadifNecessary == "Yes" ? true : false,
                        RefusesLeadership = item.RefusesLeadership == "Yes" ? true : false,
                        AttemptbutInefficient = item.AttemptbutInefficient == "Yes" ? true : false,
                        LeadershipComment = item.LeadershipComment,

                        NeverFalter = item.NeverFalter == "Yes" ? true : false,
                        MaintainPoise = item.MaintainPoise == "Yes" ? true : false,
                        DependableExcUnderPress = item.DependableExcUnderPress == "Yes" ? true : false,
                        CantTakePressure = item.CantTakePressure == "Yes" ? true : false,
                        PressureComment = item.PressureComment,


                        HRcomment = item.HRcomment,
                        MDcomment = item.MDcomment,

                        empStrongestpt = item.empStrongestpt,
                        empWeakestPt = item.empWeakestPt,
                        qualifiedPromo = item.qualifiedPromo,
                        promoPstn = item.promoPstn,
                        promotable = item.promotable,
                        effectiveWithDifferent = item.effectiveWithDifferent,
                        differentAssingment = item.differentAssingment,
                        recommendationSectionComment = item.recommendationSectionComment,
                        empRecConfirm = item.empRecConfirm,
                        empRecExtProb = item.empRecExtProb,
                        empRecTerminate = item.empRecTerminate,


                    };

                    probationFirstList.Add(probationFirstSection);

                }
                return Ok(new { probationFirstList });
              
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Failed: " + x.Message });
            }
        }



        /**
         * *****************************************************************************************************************
         *                                                      HR SECTION
         * 
         * ************************************************************************************************************************
         */


        //Get the HR list of created probations
        [Authorize]
        [HttpGet]
        [Route("gethrprobationlist")]
        public async Task<IActionResult> GetHRProbationList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var employeeEndofs = dbContext.ProbationProgress.Where(x=>x.ProbationStatus == 1 || x.ProbationStatus == 3).ToList();

                return Ok(new { employeeEndofs });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation List Failed: " + x.Message });
            }
        }

        //HR Push the comment
        [Authorize]
        [HttpPost]
        [Route("hrpushtomdfd/{PID}")]
        public async Task<IActionResult> HRPushToMDFD([FromBody] ProbationRecommendationModel probationFirst,string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var resRemarks = await codeUnitWebService.Client().UpdateProbationHRremarkAsync(PID, probationFirst.HRcomment);
                if (bool.Parse(resRemarks.return_value))
                {
                   var probModel = dbContext.ProbationProgress.Where(x => x.ProbationNo == PID).FirstOrDefault();
                    probModel.ProbationStatus = 2;
                    probModel.UIDTwo = user.Id;
                    probModel.UIDTwoComment = probationFirst.HRcomment;

                    dbContext.ProbationProgress.Update(probModel);
                    await dbContext.SaveChangesAsync();

                    ////Mail MD/FD
                    ///@email
                    //var emailArr = dbContext.Users.Where(x => x.Rank == "MD" || x.Rank =="FD")
                    //    .Select(t => t.Email).ToArray();



                    //var unameArr = dbContext.Users.Where(x => x.Rank == "MD" || x.Rank == "FD")
                    //    .Select(t => t.UserName).ToArray();

                    //List<ProbationProgressMail> v = new List<ProbationProgressMail>();
                    //// v.AddRange(userList);
                    //mailService.SendEmail(emailArr, unameArr, PID);

                    return Ok(bool.Parse(resRemarks.return_value));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: D365 failed "});
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: " + x.Message });
            }
        }

        //HR Approves
        [Authorize]
        [HttpGet]
        [Route("hrapproveprobation/{PID}")]
        public async Task<IActionResult> HRApproveProbation(string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var resRemarks = await codeUnitWebService.Client().ApproveProbationHRAsync(PID);
                if (bool.Parse(resRemarks.return_value))
                {
                    var probModel = dbContext.ProbationProgress.Where(x => x.ProbationNo == PID).First();
                    probModel.Status = "Approved";
                    dbContext.ProbationProgress.Update(probModel);
                    await dbContext.SaveChangesAsync();

                    ////Mail MD/FD
                    //@email

                    //var emailArr = dbContext.Users.Where(x => x.Rank == "HR")
                    //    .Select(t => t.Email).ToArray();



                    //var unameArr = dbContext.Users.Where(x => x.Rank == "HR")
                    //    .Select(t => t.UserName).ToArray();

                    //List<ProbationProgressMail> v = new List<ProbationProgressMail>();
                    //// v.AddRange(userList);
                    //mailService.SendEmail(emailArr, unameArr, PID);

                    return Ok(bool.Parse(resRemarks.return_value));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: D365 failed " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: " + x.Message });
            }
        }




        /**
         * *****************************************************************************************************************
         *                                                      FD SECTION
         * 
         * ************************************************************************************************************************
         */

        //FD Dashboard
        [Authorize]
        [HttpGet]
        [Route("fddashboard")]
        public async Task<IActionResult> FDDashboard()
        {
            try
            {
                var usr = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var pendingCount = dbContext.ProbationProgress.Where(x => x.ProbationStatus == 2).Count();
                var doneCount = dbContext.ProbationProgress.Where(x => x.ProbationStatus > 2).Count();

                return Ok(new { pendingCount, doneCount });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "FD Failed " + x.Message });
            }
        }

        //Get the FD list of created probations
        [Authorize]
        [HttpGet]
        [Route("getfdprobationlist")]
        public async Task<IActionResult> GetFDProbationList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var employeeEndofs = dbContext.ProbationProgress.Where(x => x.ProbationStatus == 2).ToList();

                return Ok(new { employeeEndofs });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation List Failed: " + x.Message });
            }
        }

        //FD Approves
        [Authorize]
        [HttpPost]
        [Route("fdapproveprobation/{PID}")]
        public async Task<IActionResult> FDApproveProbation([FromBody] ProbationRecommendationModel probationRecommendationModel,string PID)
        {
            try { 
                       var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var resRemarks = await codeUnitWebService.Client().UpdateProbationMFDremarkAsync(PID, probationRecommendationModel.MDcomment);
            if (bool.Parse(resRemarks.return_value))
            {
                var probModel = dbContext.ProbationProgress.Where(x => x.ProbationNo == PID).First();
                probModel.ProbationStatus = 3;
                probModel.UIDThree = user.Id;
                probModel.UIDThreeComment = probationRecommendationModel.MDcomment;

                dbContext.ProbationProgress.Update(probModel);
                await dbContext.SaveChangesAsync();

                //Mail MD/FD
                //@email
                //var emailArr = dbContext.Users.Where(x => x.Rank == "HR")
                //    .Select(t => t.Email).ToArray();



                //var unameArr = dbContext.Users.Where(x => x.Rank == "HR")
                //    .Select(t => t.UserName).ToArray();

                //List<ProbationProgressMail> v = new List<ProbationProgressMail>();
                //// v.AddRange(userList);
                //mailService.SendEmail(emailArr, unameArr, PID);

                return Ok(bool.Parse(resRemarks.return_value));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: D365 failed " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: " + x.Message });
            }
        }

        //FD Approves
        [Authorize]
        [HttpPost]
        [Route("fdrejectprobation/{PID}")]
        public async Task<IActionResult> FDRejectProbation([FromBody] ProbationRecommendationModel probationRecommendationModel, string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var resRemarks = await codeUnitWebService.Client().UpdateProbationMFDremarkAsync(PID, probationRecommendationModel.MDcomment);
                if (bool.Parse(resRemarks.return_value))
                {
                    var rejectRes = await codeUnitWebService.Client().RejectProbationMFDAsync(PID);
                    var probModel = dbContext.ProbationProgress.Where(x => x.ProbationNo == PID).First();
                    probModel.ProbationStatus = 3;
                    probModel.UIDThree = user.Id;
                    probModel.Status = "Rejected";
                    probModel.UIDThreeComment = probationRecommendationModel.MDcomment;

                    dbContext.ProbationProgress.Update(probModel);
                    await dbContext.SaveChangesAsync();

                    //Mail MD/FD
                    //@email
                    //var emailArr = dbContext.Users.Where(x => x.Rank == "HR")
                    //    .Select(t => t.Email).ToArray();



                    //var unameArr = dbContext.Users.Where(x => x.Rank == "HR")
                    //    .Select(t => t.UserName).ToArray();

                    //List<ProbationProgressMail> v = new List<ProbationProgressMail>();
                    //// v.AddRange(userList);
                    //mailService.SendEmail(emailArr, unameArr, PID,false);

                    return Ok(bool.Parse(resRemarks.return_value));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: D365 failed " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Update Failed: " + x.Message });
            }
        }



        /*******************************************************************************************************************
        *-- -------------------------------------------------                                                      
        *------------------------------------------------------END OF CONTRACT SECTION
        * ----------------------------------------------------
        * ************************************************************************************************************************
        */

        [Authorize]
        [HttpPost]
        [Route("storeendofcontractcreate")]
        public async Task<IActionResult> StoreEndofContractCreate([FromBody] EndofContractProgress  endofContract )
        {
            try
            {
                List<EndofContractProgress> endofContracts = new List<EndofContractProgress>();
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var res = await codeUnitWebService.Client().CreateEndofContractGeneralAsync(
                    endofContract.EmpID,
                    endofContract.MgrID, 
                    endofContract.SupervisionTime, 
                    endofContract.DoRenew,
                    endofContract.RenewReason,
                    endofContract.Howlong
                    );

                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);


                foreach (var item in resSerial)
                {
                    EndofContractProgress pp = new EndofContractProgress
                    {
                        UID = user.Id,
                        ContractStatus = 0,
                        ContractNo = item.Probationno,

                        EmpID = item.Employeeno,
                        EmpName = item.Employeename,
                        MgrID = item.Managerno,
                        MgrName = item.Managername,
                        CreationDate = item.Creationdate,
                        Department = item.Department,
                        Status = item.Status,
                        Position = item.Position,
                        RenewReason= endofContract.RenewReason,
                        Howlong= endofContract.Howlong,
                        SupervisionTime = item.Supervisiontime,
                        DoRenew = item.Dorenew,
                    };
                    dbContext.EndofContractProgress.Add(pp);
                    await dbContext.SaveChangesAsync();

                    return Ok(true);

                }
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "D365 Create Contract Store Failed" });

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Create Contract Store Failed: " + x.Message });
            }
        }

        //Get the Staff/Imediate Manager list of created contracts
        [Authorize]
        [HttpGet]
        [Route("getstaffcontractlist")]
        public async Task<IActionResult> GetStaffContractList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var employeeContracts = dbContext.EndofContractProgress.Where(x => x.ContractStatus == 0 && x.MgrID == user.EmployeeId).ToList();

                return Ok(new { employeeContracts });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract List Failed: " + x.Message });
            }
        }

        //Upload Contact Section One Data
        [Authorize]
        [HttpPost]
        [Route("uploadcontractsectionone/{PID}")]
        public async Task<IActionResult> UploadContractSectionOne([FromBody] ProbationFirstSection probationFirstSection, string PID)
        {
            try
            {
                bool[] boolData = new bool[50];
                string[] commentArr = new string[25];

                commentArr[0] = probationFirstSection.PerformanceComment;
                commentArr[1] = probationFirstSection.AttendanceComment;
                commentArr[2] = probationFirstSection.AttitudeComment;
                commentArr[3] = probationFirstSection.AppearanceComment;
             

                boolData[0] = probationFirstSection.Outstanding;
                boolData[1] = probationFirstSection.AboveAverage;
                boolData[2] = probationFirstSection.Satisfactory;
                boolData[3] = probationFirstSection.Marginal;
                boolData[4] = probationFirstSection.Unsatisfactory;

                boolData[5] = probationFirstSection.ExcellentAttendance;
                boolData[6] = probationFirstSection.OccasionalAbsence;
                boolData[7] = probationFirstSection.RepeatedAbsence;
                boolData[8] = probationFirstSection.UnjustifiedAbsence;

                boolData[9] = probationFirstSection.AlwaysInterested;
                boolData[10] = probationFirstSection.ReasonablyDevoted;
                boolData[11] = probationFirstSection.PassiveAttitude;
                boolData[12] = probationFirstSection.ActiveDislikeofWork;

                boolData[13] = probationFirstSection.AlwaysNeat;
                boolData[14] = probationFirstSection.GenerallyNeat;
                boolData[15] = probationFirstSection.SometimesCareles;
                boolData[16] = probationFirstSection.AttirenotSuitable;

              

                var res = await codeUnitWebService.Client().UpdateContractProgressFirstSectionAsync(PID, boolData, commentArr);


                return Ok(res.return_value);
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Section One Upload Failed: " + x.Message });
            }
        }
        //Upload Contact Recommendation Section
        [Authorize]
        [HttpPost]
        [Route("uploadcontractrecommendation/{PID}")]
        public async Task<IActionResult> UploadContractRecommendation([FromBody] ProbationRecommendation probationRecommendation, string PID)
        {
            try
            {
                string[] textArr = new string[10];

                textArr[0] = probationRecommendation.EmployeeStrongestPoint;
                textArr[1] = probationRecommendation.EmployeeWeakestPoint;
                textArr[2] = probationRecommendation.EmployeeQualifiedForPromo;
                textArr[3] = probationRecommendation.PromoPosition;
                textArr[4] = probationRecommendation.PromotableInTheFuture;
                textArr[5] = probationRecommendation.EffectiveDifferentAssignment;
                textArr[6] = probationRecommendation.WhichAssignment;
                textArr[7] = probationRecommendation.AdditionalComment;

              

                var res = await codeUnitWebService.Client().UpdateContractRecommendationSectionAsync(PID, textArr);
                return Ok(res.return_value);

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Recommendation Upload Failed: " + x.Message });
            }
        }

        //Move Probation To HR
        [Authorize]
        [HttpGet]
        [Route("movecontractfrommanagertohr/{PID}")]
        public async Task<IActionResult> MoveContractFromManagerToHR(string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);

                var cModel = dbContext.EndofContractProgress.Where(p => p.ContractNo == PID && p.ContractStatus == 0).First();
                cModel.UID = user.Id;
                cModel.ContractStatus = 1;

                dbContext.EndofContractProgress.Update(cModel);
                await dbContext.SaveChangesAsync();

                //Mail HR
                //@email
                //var emailArr = dbContext.Users.Where(x => x.Rank == "HR")
                //    .Select(t => t.Email).ToArray();

                //var unameArr = dbContext.Users.Where(x => x.Rank == "HR")
                //    .Select(t => t.UserName).ToArray();

                //List<ProbationProgressMail> v = new List<ProbationProgressMail>();
                //// v.AddRange(userList);
                //mailService.SendEmail(emailArr, unameArr, PID);

                // return Ok(userList);

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Contract Moved: " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Move Failed: " + x.Message });
            }
        }

        //Contact Card Data
        [Authorize]
        [HttpGet]
        [Route("contractcarddata/{PID}")]
        public async Task<IActionResult> ContractCardData(string PID)
        {
            try
            {
                List<ProbationFirstSection> probationFirstList = new List<ProbationFirstSection>();
                var res = await codeUnitWebService.Client().GetContractCardDataAsync(PID);
                dynamic resSerial = JsonConvert.DeserializeObject(res.return_value);

                foreach (var item in resSerial)
                {
                    ProbationFirstSection contractFirstSection = new ProbationFirstSection
                    {

                        Probationno = item.Contractno,
                        Employeeno = item.Employeeno,
                        Employeename = item.Employeename,
                        Creationdate = item.Creationdate,
                        Department = item.Department,
                        Status = item.Status,
                        Position = item.Position,
                        Managername = item.Manangername,


                        Outstanding = item.Outstanding == "Yes" ? true : false,
                        AboveAverage = item.Aboveaverage == "Yes" ? true : false,
                        Satisfactory = item.Satisfactory == "Yes" ? true : false,
                        Marginal = item.Marginal == "Yes" ? true : false,
                        Unsatisfactory = item.Unsatisfactory == "Yes" ? true : false,
                        PerformanceComment = "",

                        ExcellentAttendance = item.ExcellentAttendance == "Yes" ? true : false,
                        OccasionalAbsence = item.OccasionalAbsence == "Yes" ? true : false,
                        RepeatedAbsence = item.RepeatedAbsence == "Yes" ? true : false,
                        UnjustifiedAbsence = item.UnjustifiedAbsence == "Yes" ? true : false,
                        AttendanceComment = item.AttendanceComment,

                        AlwaysInterested = item.AlwaysInterested == "Yes" ? true : false,
                        ReasonablyDevoted = item.ReasonablyDevoted == "Yes" ? true : false,
                        PassiveAttitude = item.PassiveAttitude == "Yes" ? true : false,
                        ActiveDislikeofWork = item.ActiveDislikeofWork == "Yes" ? true : false,
                        AttitudeComment = item.AttitudeComment,

                        AlwaysNeat = item.AlwaysNeat == "Yes" ? true : false,
                        GenerallyNeat = item.GenerallyNeat == "Yes" ? true : false,
                        SometimesCareles = item.SometimesCareles == "Yes" ? true : false,
                        AttirenotSuitable = item.AttirenotSuitable == "Yes" ? true : false,
                        AppearanceComment = item.AppearanceComment,



                 


                        HRcomment = item.HRcomment,
                        MDcomment = item.MDcomment,

                        empStrongestpt = item.empStrongestpt,
                        empWeakestPt = item.emprovementArea,
                        qualifiedPromo = item.qualifiedPromo,
                        promoPstn = item.promoPstn,
                        promotable = item.promotable,
                        effectiveWithDifferent = item.effectiveWithDifferent,
                        differentAssingment = item.differentAssingment,
                        recommendationSectionComment = item.recommendationSectionComment,
                        empRecConfirm = item.empRecConfirm,
                        empRecExtProb = item.empRecExtProb,
                        empRecTerminate = item.empRecTerminate,


                    };

                    probationFirstList.Add(contractFirstSection);

                }
                return Ok(new { probationFirstList });

            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Probation Card Failed: " + x.Message });
            }
        }

        /**
        * *****************************************************************************************************************
        *                                                      HR SECTION
        * 
        * ************************************************************************************************************************
        */


        //Get the HR list of created contracts
        [Authorize]
        [HttpGet]
        [Route("gethrcontractlist")]
        public async Task<IActionResult> GetHRContractList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var employeeEndofs = dbContext.EndofContractProgress.Where(x => x.ContractStatus == 1 || x.ContractStatus == 3).ToList();

                return Ok(new { employeeEndofs });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract List Failed: " + x.Message });
            }
        }

        //HR Push the comment
        [Authorize]
        [HttpPost]
        [Route("hrpushcontracttomdfd/{PID}")]
        public async Task<IActionResult> HRPushContractToMDFD([FromBody] ProbationRecommendationModel probationFirst, string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var resRemarks = await codeUnitWebService.Client().UpdateContractHRremarkAsync(PID, probationFirst.HRcomment);
                if (bool.Parse(resRemarks.return_value))
                {
                    var contModel = dbContext.EndofContractProgress.Where(x => x.ContractNo == PID).First();
                    contModel.ContractStatus = 2;
                    contModel.UIDTwo = user.Id;
                    contModel.UIDTwoComment = probationFirst.HRcomment;

                    dbContext.EndofContractProgress.Update(contModel);
                    await dbContext.SaveChangesAsync();

                    ////Mail MD/FD
                    ///@email
                    

                    return Ok(bool.Parse(resRemarks.return_value));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: D365 failed " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: " + x.Message });
            }
        }

        //HR Approves
        [Authorize]
        [HttpGet]
        [Route("hrapprovecontract/{PID}")]
        public async Task<IActionResult> HRApproveContract(string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var resRemarks = await codeUnitWebService.Client().ApproveContractHRAsync(PID);
                if (bool.Parse(resRemarks.return_value))
                {
                    var contModel = dbContext.EndofContractProgress.Where(x => x.ContractNo == PID).First();
                    contModel.Status = "Approved";
                    dbContext.EndofContractProgress.Update(contModel);
                    await dbContext.SaveChangesAsync();

                    ////Mail MD/FD
                    //@email


                    return Ok(bool.Parse(resRemarks.return_value));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: D365 failed " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: " + x.Message });
            }
        }


         /**
         * *****************************************************************************************************************
         *                                                      FD SECTION
         * 
         * ************************************************************************************************************************
         */

        //Get the FD list of created probations
        [Authorize]
        [HttpGet]
        [Route("getfdcontractlist")]
        public async Task<IActionResult> GetFDContractList()
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var employeeEndofs = dbContext.EndofContractProgress.Where(x => x.ContractStatus == 2).ToList();

                return Ok(new { employeeEndofs });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract List Failed: " + x.Message });
            }
        }

        //FD Approves
        [Authorize]
        [HttpPost]
        [Route("fdapprovecontract/{PID}")]
        public async Task<IActionResult> FDApproveContract([FromBody] ProbationRecommendationModel probationRecommendationModel, string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var resRemarks = await codeUnitWebService.Client().UpdateContractMFDremarkAsync(PID, probationRecommendationModel.MDcomment);
                if (bool.Parse(resRemarks.return_value))
                {
                    var contModel = dbContext.EndofContractProgress.Where(x => x.ContractNo == PID).First();
                    contModel.ContractStatus = 3;
                    contModel.UIDThree = user.Id;
                    contModel.UIDThreeComment = probationRecommendationModel.MDcomment;

                    dbContext.EndofContractProgress.Update(contModel);
                    await dbContext.SaveChangesAsync();

                    //Mail MD/FD
                    //@email
                    

                    return Ok(bool.Parse(resRemarks.return_value));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: D365 failed " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: " + x.Message });
            }
        }

        //FD Reject
        [Authorize]
        [HttpPost]
        [Route("fdrejectcontract/{PID}")]
        public async Task<IActionResult> FDRejectContract([FromBody] ProbationRecommendationModel probationRecommendationModel, string PID)
        {
            try
            {
                var user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                var resRemarks = await codeUnitWebService.Client().UpdateContractMFDremarkAsync(PID, probationRecommendationModel.MDcomment);
                if (bool.Parse(resRemarks.return_value))
                {
                    var rejectRes = await codeUnitWebService.Client().RejectContractMFDAsync(PID);
                    var contModel = dbContext.EndofContractProgress.Where(x => x.ContractNo == PID).First();
                    contModel.ContractStatus = 3;
                    contModel.UIDThree = user.Id;
                    contModel.Status = "Rejected";
                    contModel.UIDThreeComment = probationRecommendationModel.MDcomment;

                    dbContext.EndofContractProgress.Update(contModel);
                    await dbContext.SaveChangesAsync();

                    //Mail MD/FD
                    //@email
                   

                    return Ok(bool.Parse(resRemarks.return_value));
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: D365 failed " });
            }
            catch (Exception x)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Contract Card Update Failed: " + x.Message });
            }
        }



    }
}
