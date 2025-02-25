using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Patientportal.AllApicall;
using Patientportal.Model;

namespace Patientportal.Pages.Calendar
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        public List<AppointmentListItem> Doctorblocktime { get; set; } = new List<AppointmentListItem>();
   

        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
        }
        public async Task OnGet()
        
        {
            string apiUrl2 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/GetAppointmentsByDoctor";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // Valid token yahan dalein

            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
        }
        public async Task OnGetCalendarAsync()
        {
            string apiUrl2 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/GetAppointmentsByDoctor";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // Valid token yahan dalein

            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
        }
    }
}
