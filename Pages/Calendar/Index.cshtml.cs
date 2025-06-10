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
        public List<Holidays> Holidays { get; set; } = new List<Holidays>();


        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
        }
        public async Task OnGet()
        {
            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Holiday/getHolidaysList";
            string apiUrl2 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/GetAppointmentsByDoctor";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; // Valid token yahan dalein

            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
            Holidays = await _apiService.GetAsync<List<Holidays>>(apiUrl, token) ?? new List<Holidays>();
            //foreach (var appointment in Doctorblocktime)
            //{
            //    if (appointment.AppointmentStartTime != null)
            //    {
            //        appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
            //    }

            //    if (appointment.AppointmentEndDateTime != null)
            //    {
            //        appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
            //    }
            //}

        
        
        
        
        
        
        }
        public async Task OnGetCalendarAsync()
        {
            string apiUrl2 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/GetAppointmentsByDoctor";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; // Valid token yahan dalein

            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token) ?? new List<AppointmentListItem>();
            //if (Doctorblocktime != null && Doctorblocktime.Count > 0)
            //{
            //    foreach (var appointment in Doctorblocktime)
            //    {
            //        if (appointment.AppointmentStartTime != null)
            //        {
            //            appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
            //        }

            //        if (appointment.AppointmentEndDateTime != null)
            //        {
            //            appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
            //        }
            //    }
            //}
        }
    }
}
