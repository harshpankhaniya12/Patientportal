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
using Patientportal.Model;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.Metrics;

namespace Patientportal.Pages.Patient
{
    [Authorize]
    [IgnoreAntiforgeryToken(Order = 2000)]  
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        [FromQuery(Name = "id")]
        public long? Id { get; set; }
        public ProfileListItem PatientData { get; set; }
        public List<Country> CountryLists { get; set; }
        public AppointmentListItem AppoinmentData { get; set; }
        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public IEnumerable<State> StateLists { get; set; }
        public IEnumerable<City> CityLists { get; set; }
        public List<string> ChangeRequests { get; set; } = new List<string>();
        public List<AppointmentListItem> Doctorblocktime { get; set; } = new List<AppointmentListItem>();
        public List<Holidays> Holidays { get; set; } = new List<Holidays>();
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
        }
        public async Task<JsonResult> OnPostAppointmentView([FromBody] DataManagerRequest dm)
        {
            var queryId = Request.Query["id"];
            if (queryId.Any())
            {
                Id = Convert.ToInt64(queryId);
            }
            if (dm == null)
            {
                return new JsonResult(new { result = new List<object>(), count = 0 });
            }

            string apiUrl = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/getPatientByAppointment?id={Id}";
            string apiUrl2 = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/getPatientByAppointmentRequest?id={Id}";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ";
            var appointments = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl, token);
            var appointmentsRequest = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token);
            if (appointments != null && appointments.Count > 0)
            {
                foreach (var appointment in appointments)
                {
                    //if (appointment.AppointmentStartTime.HasValue)
                    //{
                    //    appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                    //    appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
                    //}
                    //if (appointment.CreatedOn != null)
                    //{
                    //    appointment.CreatedOn = appointment.CreatedOn.Value.AddHours(-5).AddMinutes(-30);
                    //}

                    if (appointment.StatusName == "Rescheduled" || appointment.StatusName == "Booked")
                    {
                        appointment.StatusName = "Booked";
                    }
                    if (appointment.StatusName == "Released" || appointment.StatusName == "Completed")
                    {
                        appointment.StatusName = "Completed";
                    }
                    if (appointment.AppoinmentType == "Consultation")
                    {
                        appointment.AppoinmentType = "Appointment for Consultation";
                    }
                    if (appointment.StatusName == "Confirmed" ||
                      appointment.StatusName == "ReverseCheckin")
                    {
                        appointment.StatusName = "Confirmed";
                    }
                    if (appointment.StatusName == "Checked-In" || appointment.StatusName == "ReverseCheckout")
                    {
                        appointment.StatusName = "Checked-In";
                    }
                    if (appointment.StatusName == "Walked-Out")
                    {
                        appointment.StatusName = "Walked-Out";
                    }
                }
            }
            if (appointmentsRequest != null && appointmentsRequest.Count > 0)
            {
                foreach (var appointmentes in appointmentsRequest)
                {
                    //if (appointmentes.AppointmentStartTime.HasValue)
                    //{
                    //    appointmentes.AppointmentStartTime = appointmentes.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                    //}
                    if (appointmentes.StatusName == "Reschedule")
                    {
                        appointmentes.StatusName = "Booked";
                    }
                    if (appointmentes.AppoinmentType == "Consultation")
                    {
                        appointmentes.AppoinmentType = "Appointment for Consultation";
                    }
                }
            }

            IEnumerable<object> data = appointmentsRequest.Concat(appointments).AsEnumerable().OrderByDescending(x => x.CreatedOn);
            int count = data.Count();

            var dataOperations = new DataOperations();

            // Filtering
            if (dm.Where != null && dm.Where.Count > 0)
            {
                data = dataOperations.PerformFiltering(data, dm.Where, "and");
                count = data.Count();
            }

            // Searching
            if (dm.Search != null && dm.Search.Count > 0)
            {
                data = dataOperations.PerformSearching(data, dm.Search);
                count = data.Count();
            }

            // Sorting
            if (dm.Sorted != null && dm.Sorted.Count > 0)
            {
                data = dataOperations.PerformSorting(data, dm.Sorted);
            }

            // Paging
            if (dm.Skip != 0)
            {
                data = dataOperations.PerformSkip(data, dm.Skip);
            }
            if (dm.Take != 0)
            {
                data = dataOperations.PerformTake(data, dm.Take);
            }

            return new JsonResult(new { result = data, count });
        }


        public async Task<JsonResult> OnPostAppointmentViewCard([FromBody] DataManagerRequest dm)
        {
            var queryId = Request.Query["id"];
            if (queryId.Any())
            {
                Id = Convert.ToInt64(queryId);
            }


            string apiUrl = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/getPatientByAppointment?id={Id}";
            string apiUrl2 = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/getPatientByAppointmentRequest?id={Id}";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ";
            var appointments = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl, token);
            var appointmentsRequest = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl2, token);
            if (appointments != null && appointments.Count > 0)
            {
                foreach (var appointment in appointments)
                {
                    //if (appointment.AppointmentStartTime.HasValue)
                    //{
                    //    appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                    //    appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
                    //}
                    //if (appointment.CreatedOn != null)
                    //{
                    //    appointment.CreatedOn = appointment.CreatedOn.Value.AddHours(-5).AddMinutes(-30);
                    //}

                    if (appointment.StatusName == "Rescheduled" || appointment.StatusName == "Booked")
                    {
                        appointment.StatusName = "Booked";
                    }
                    if (appointment.StatusName == "Released" || appointment.StatusName == "Completed")
                    {
                        appointment.StatusName = "Completed";
                    }
                    if (appointment.AppoinmentType == "Consultation")
                    {
                        appointment.AppoinmentType = "Appointment for Consultation";
                    }
					if (appointment.StatusName == "Confirmed" ||
					  appointment.StatusName == "ReverseCheckin")
					{
						appointment.StatusName = "Confirmed";
					}
                    if (appointment.StatusName == "Checked-In" || appointment.StatusName == "ReverseCheckout")
					{
						appointment.StatusName = "Checked-In";
					}
                    if (appointment.StatusName == "Walked-Out")
					{
						appointment.StatusName = "Walked-Out";
					}
				}
            }
            if (appointmentsRequest != null && appointmentsRequest.Count > 0)
            {
                foreach (var appointmentes in appointmentsRequest)
                {
                    //if (appointmentes.AppointmentStartTime.HasValue)
                    //{
                    //    appointmentes.AppointmentStartTime = appointmentes.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                    //}

                    if (appointmentes.AppoinmentType == "Consultation")
                    {
                        appointmentes.AppoinmentType = "Appointment for Consultation";
                    }
                }
            }

            IEnumerable<object> data = appointmentsRequest.Concat(appointments).AsEnumerable().OrderByDescending(x => x.CreatedOn);
            int count = data.Count();


            return new JsonResult(new { result = data, count });
        }
        public async Task<IActionResult> OnGetStatesAsync(int countryId)
        {

            string apiUrl4 = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/CountryStateCity/GetStatesByCountry?countryId={countryId}";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; 

            var states = await _apiService.GetAsync<List<State>>(apiUrl4, token) ?? new List<State>();
            return new JsonResult(states) { StatusCode = 200 };
        }
        public async Task<IActionResult> OnGetCitiesAsync(int stateId)
        {
            string apiUrl = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/CountryStateCity/GetCitiesByState?stateId={stateId}";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ";


            var citiesdp = await _apiService.GetAsync<List<City>>(apiUrl, token) ?? new List<City>();
            return new JsonResult(citiesdp) { StatusCode = 200 };
        }
        public async Task<IActionResult> OnGetAsync()
        {

            var queryId = Request.Query["id"];
            if (queryId.Any())
            {
                Id = Convert.ToInt64(EncryptionHelper.DecryptId(queryId));
            }
            else
            {
                return RedirectToPage("/Account/Index"); // Ya phir Redirect("/Login");
            }

            string apiUrl = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/Profile/getProfileforpatientportal?id={Id}";
            string apiUrl2 = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/Profile/getDetailsChangesbyId?id={Id}";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; // Valid token yahan dalein
            string apiUrl3 = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/GetAppointmentsPortalByDoctor?id={Id}";
            string apiUrl4 = $"http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/GetInvoiceAmount?id={Id}";
            string apiUrl5 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Holiday/getHolidaysList";
            string apiUrl6 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/CountryStateCity/GetCountry";


            // API Response Fetch karein
            var invoiceResponse = await _apiService.GetAsync<InvoiceResponse>(apiUrl4, token);
            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl3, token) ?? new List<AppointmentListItem>();
            Holidays = await _apiService.GetAsync<List<Holidays>>(apiUrl5, token) ?? new List<Holidays>();
            CountryLists = await _apiService.GetAsync<List<Country>>(apiUrl6, token) ?? new List<Country>();
            //if (Doctorblocktime != null && Doctorblocktime.Count > 0 )
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

            PatientData = await _apiService.GetAsync<ProfileListItem>(apiUrl, token) ?? new ProfileListItem();


            if (PatientData != null)
            {
                ViewData["PatientName"] = PatientData.Name;
            }
            if (PatientData != null)
            {
                ViewData["Invoice"] = invoiceResponse?.Invoice;
            }
             if (PatientData != null)
            {
                ViewData["ProfileId"] = PatientData?.ProfileId;
            }

            ChangeRequests = await _apiService.GetAsync<List<string>>(apiUrl2, token) ?? new List<string>();
            return Page();
        }

        public async Task<IActionResult> OnPostSavePatientAsync()
        {
            try
            {


                using var reader = new StreamReader(HttpContext.Request.Body);
                var json = await reader.ReadToEndAsync();
                ProfileListItem viewModel = JSON.Deserialize<ProfileListItem>(json);


                string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/Profile/Addpatientportalchanges";
                string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; // Valid token yahan dalein

                var apiHelper = new ApiService(_httpClient);
                var response = await _apiService.PostAsync<ProfileListItem, ApiResponse>(apiUrl, viewModel, token);

                if (response != null && response.IsSuccess)
                {
                    return new JsonResult(new { isSuccess = true,  message = "Your change request has been submitted." });

                }
                else
                {
                    return BadRequest("Failed to save patient details.");
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();   
            }
        } 
        public async Task<IActionResult> OnPostPirescheduleAsync()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();
            AppointmentListItem viewModel = JSON.Deserialize<AppointmentListItem>(json);


            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/viewAppointmentButtonPatientPortal";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; // Valid token yahan dalein

            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<AppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

            if (response != null && response.IsSuccess)
            {
                return new JsonResult(new { isSuccess = true, message = "Your change request has been submitted." });

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
            AppointmentListItem viewModel = JSON.Deserialize<AppointmentListItem>(json);
            

            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/AddAppointmentbyportalAppointmentbyPatientId";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; // Valid token yahan dalein

            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<AppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

            if (response != null && response.IsSuccess)
            {
                return new JsonResult(new { isSuccess = true, message = "Your change request has been submitted." });

            }
            else
            {
                return BadRequest("Failed to save patient details.");
            }
        }
        public async Task<IActionResult> OnPostAddAppointmentRequestAsync()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();
            AppointmentListItem viewModel = JSON.Deserialize<AppointmentListItem>(json);
            

            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Appointment/UpsertAppointmentRequest";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyNyIsInN1YiI6IjI3IiwidW5pcXVlX25hbWUiOiJNZWh1bGkiLCJlbWFpbCI6Im1laHVsdUBpbnVyc2tuLmluIiwicm9sZSI6IkZyb250RGVza0JpbGxpbmdBZG1pbiIsIm5iZiI6MTc0OTUzODYyMywiZXhwIjoxNzUwMTQzNDIzLCJpYXQiOjE3NDk1Mzg2MjMsImlzcyI6IkNvbm5ldHdlbGxDSVMiLCJhdWQiOiJDb25uZXR3ZWxsQ0lTIn0.CT-ijEQkb_OCD0m15J6olFH8WGw2T24464fFRFO-XnvPvYlU3k4hqOmBOV1FepkiErBjbWUquR_XHgWbgrARxQ"; // Valid token yahan dalein
            string apiUrl5 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8889/api/v1/Holiday/getHolidaysList";

            Holidays = await _apiService.GetAsync<List<Holidays>>(apiUrl5, token) ?? new List<Holidays>();
            var appointmentDate = viewModel.AppointmentStartTime.Value.ToString("dd/MM/yyyy");
            var isHoliday = Holidays.Any(h => h.StartDate.HasValue &&
                                              h.StartDate.Value.ToString("dd/MM/yyyy") == appointmentDate);

            if (isHoliday)
            {
                return new JsonResult(new { isSuccess = false, errorMessage = "Appointments cannot be scheduled on holidays." });
            }
            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<AppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

            if (response != null && response.IsSuccess)
            {
                return new JsonResult(new { isSuccess = true, message = "Your change request has been submitted." });

            }
            else
            {
                return BadRequest("Failed to save patient details.");
            }
        }

    }
}
