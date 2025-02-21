using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Patientportal.AllApicall;
using Syncfusion.EJ2.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text.Json;
using Innovura.CSharp.Core;
using System.ComponentModel.DataAnnotations;

namespace Patientportal.Pages
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        public vwProfileListItem PatientData { get; set; }
        public vwAppointmentListItem AppoinmentData { get; set; }
        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public List<string> ChangeRequests { get; set; } = new List<string>();
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
        }
        public async Task<JsonResult> OnPostAppointmentView([FromBody] DataManagerRequest dm)
        {
            if (dm == null)
            {
                return new JsonResult(new { result = new List<object>(), count = 0 });
            }
            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/getPatientByAppointment?id=575";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // सही टोकन डालें
            var appointments = await _apiService.GetAsync<List<vwAppointmentListItem>>(apiUrl, token);


            IEnumerable<object> data = appointments;
            int count = data.Count();

            if (dm.Where != null && dm.Where.Count > 0)
            {
                var dataOperations = new DataOperations();
                data = dataOperations.PerformFiltering(data, dm.Where, "and");
                count = data.Count();
            }
            if (dm.Search != null && dm.Search.Count > 0)
            {
                var dataOperations = new DataOperations(); // ✅ DataOperations का एक instance बनाएं
                data = dataOperations.PerformSearching(data, dm.Search);
            }

            return new JsonResult(new { result = data, count });
        }
        public  async Task<JsonResult> OnPostAppointmentViewCard([FromBody] DataManagerRequest dm)
        {
            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/getPatientByAppointment?id=575";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // सही टोकन डालें
            var appointments =  await _apiService.GetAsync<List<vwAppointmentListItem>>(apiUrl, token);

            if (appointments == null || !appointments.Any())
            {
                return new JsonResult(new { result = new List<object>(), count = 0 });
            }

            IEnumerable<object> data = appointments;
            int dataCount = data.Count();

            return new JsonResult(new { result = appointments, count = dataCount });
        }

        public async Task OnGetAsync()
        {
            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/Profile/getProfile?id=575";
            string apiUrl2 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/Profile/getDetailsChangesbyId?id=575";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // Valid token yahan dalein

            PatientData = await _apiService.GetAsync<vwProfileListItem>(apiUrl, token) ?? new vwProfileListItem();

            ChangeRequests = await _apiService.GetAsync<List<string>>(apiUrl2, token) ?? new List<string>();
        }

        public async Task<IActionResult> OnPostSavePatientAsync()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();
            vwProfileListItem viewModel = JSON.Deserialize<vwProfileListItem>(json);


            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/Profile/Addpatientportalchanges";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // Valid token yahan dalein

            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<vwProfileListItem, ApiResponse>(apiUrl, viewModel, token);

            if (response != null && response.IsSuccess)
            {
                return new JsonResult(new {  message = "Your change request has been submitted." });

            }
            else
            {
                return BadRequest("Failed to save patient details.");
            }
        } 
        public async Task<IActionResult> OnPostPirescheduleAsync()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();
            vwAppointmentListItem viewModel = JSON.Deserialize<vwAppointmentListItem>(json);


            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/viewAppointmentButton";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // Valid token yahan dalein

            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<vwAppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

            if (response != null && response.IsSuccess)
            {
                return new JsonResult(new {  message = "Your change request has been submitted." });

            }
            else
            {
                return BadRequest("Failed to save patient details.");
            }
        } 
        public async Task<IActionResult> OnPostAddaptallAsync()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();
            vwAppointmentListItem viewModel = JSON.Deserialize<vwAppointmentListItem>(json);


            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/AddAppointmentbyPatientPortal";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // Valid token yahan dalein

            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<vwAppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

            if (response != null && response.IsSuccess)
            {
                return new JsonResult(new {  message = "Your change request has been submitted." });

            }
            else
            {
                return BadRequest("Failed to save patient details.");
            }
        }

    }
}
public class vwProfileListItem
{
    public string? Name { get; set; }
    public string? Gender { get; set; }
    public string? Type { get; set; }
    public DateTimeOffset? Dob { get; set; }
    public int? Age { get; set; }
    public string? Mobile { get; set; }

    [RegularExpression(@"^[\w\.-]+@[\w\.-]+\.\w+$", ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }
    public string? Address { get; set; }

    public string? MaritalStatus { get; set; }
    public long? Country { get; set; }

    public string? CountryName { get; set; }
    public string? Category { get; set; }
    public string? InterestIndicator { get; set; }
    public string? ActivityLevel { get; set; }

    public string? CityName { get; set; }

    public string? StateName { get; set; }
    public long? City { get; set; }

    public long? State { get; set; }

    public string? Pincode { get; set; }
    public string? Locality { get; set; }
    public string? ProfileId { get; set; }
    public bool IsMergeProfileto { get; set; }

    public int? Leadcount { get; set; }
    public int? LqlCount { get; set; }

    //public long? ProfileNumber { get; set; }

    public long? LeadId { get; set; }
    public string? Leadname { get; set; }
    public string? CreatedByUserName { get; set; }
    public string? ModifiedByUserName { get; set; }

    public long? MergeId { get; set; }
    [NotMapped]
    public long? DependentProfileId { get; set; }


    public int? NewEnquiryCount { get; set; }
    public int? RecMedicaalCount { get; set; }
    public int? RecProcedureCount { get; set; }
    public int? SingleProceCount { get; set; }
    public int? Lastsessionfollowup { get; set; }
    public int? InactiveFollowup { get; set; }
    public int? CancelationFollowup { get; set; }
    public int? PlannedPatientFup { get; set; }
    public int? RecProduct { get; set; }


    public short? Year { get; set; }
    public short? YearNo { get; set; }
    public short? Month { get; set; }
    public int? SourceId { get; set; }
    public string? SourceName { get; set; }
    public int? ChannelId { get; set; }
    public string? Channelname { get; set; }
    public long Id { get; set; }

    //public virtual string? PincodeName { get; set; }

    //public virtual string? LocalityName { get; set; }

    public string? ConcernGroups { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }

    public long? CreatedBy { get; set; }
    //public string? CreatedByUserName { get; set; }
    public long? ModifiedBy { get; set; }
    //public string? ModifiedByUserName { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }

    public string? Mobiles { get; set; }
    public long? MergeFrom { get; set; }
    public bool Isdependent { get; set; }
    public bool IsMainProfile { get; set; }

    public bool IsMergeProfile { get; set; }
    public string? OldProfileId { get; set; }
    public string? Emails { get; set; }
    public int? numberOfOpenAppointment { get; set; }
    public int? numberOfcancelAppointment { get; set; }
    public int? numberOfCloseAppointment { get; set; }
    //public int? numberOfCancelProfile {  get; set; }

    public int? numberOfOpenProfile { get; set; }
    public int? numberOfCompletedProfile { get; set; }

    public string? ProfileImage { get; set; }
}
public class ApiResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
}
public class vwAppointmentListItem
{
    //public string? Location { get; set; }
    public int? DoctoreId { get; set; }
    public int? SourceId { get; set; }
    public DateTime? AppointmentStartTime { get; set; }
    public DateTime? AppointmentEndDateTime { get; set; }

    public int? StatusId { get; set; }
    public string? AppoinmentType { get; set; }
    //[NotMapped]
    public long? LeadId { get; set; }
    //[NotMapped]
    //public string? Name { get; set; }
    //[NotMapped]
    public string? Mobile { get; set; }
    public int? Age { get; set; }
    public string? Location { get; set; }
    //[NotMapped]
    public string? Gender { get; set; }

    //[NotMapped]
    public string? Email { get; set; }
    //[NotMapped]
    public long? PatientId { get; set; }
    public string? PatientName { get; set; }

    public string? ProfileImage { get; set; }

    //[NotMapped]
    public string? AppointmentNo { get; set; }

    public short? Year { get; set; }
    public string? StatusName { get; set; }

    public string? AppointmentForm { get; set; }
    public string? Comment { get; set; }
    public string? SourceName { get; set; }
    public string? DoctorName { get; set; }


    //[NotMapped]
    public string? ConcernGroups { get; set; }
    [NotMapped]
    public List<string>? NotAllowedAction { get; set; }
    public long Id { get; set; }
    public long? CreatedBy { get; set; }
    //public string? CreatedByUserName { get; set; }
    public long? ModifiedBy { get; set; }
    //public string? ModifiedByUserName { get; set; }
    public DateTimeOffset? CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }
}