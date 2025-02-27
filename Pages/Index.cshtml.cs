﻿using Microsoft.AspNetCore.Http.Extensions;
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

namespace Patientportal.Pages
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        public ProfileListItem PatientData { get; set; }
        public AppointmentListItem AppoinmentData { get; set; }
        public string? EjsDateTimePattern = "dd/MM/yyyy hh:mm:ss a";
        public List<string> ChangeRequests { get; set; } = new List<string>();
        public List<AppointmentListItem> Doctorblocktime { get; set; } = new List<AppointmentListItem>();
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
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ";
            var appointments = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl, token);
            if (appointments != null && appointments.Count > 0)
            {
                foreach (var appointment in appointments)
                {
                    if (appointment.AppointmentStartTime.HasValue)
                    {
                        appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                        appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
                    }
                }
            }

            IEnumerable<object> data = appointments;
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
            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/getPatientByAppointment?id=575";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiMTliN2Y1NTgtZDdhNS00NGE2LThmZGUtNjQ2MzgwMmQ4ZmZiIiwibmJmIjoxNzQwMDU1OTIzLCJleHAiOjE3NzE1OTE5MjMsImlhdCI6MTc0MDA1NTkyMywiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.tW5vy8tSKQNHBZlcFg7nB0luLBipQ18xyCLLbp1ifv5Hvt8vUrU1ejuSekvLku1ebZnUrL0PA6N-_iALHfh5RQ"; // सही टोकन डालें
            var appointments =  await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl, token);
            if (appointments != null && appointments.Count > 0)
            {
                foreach (var appointment in appointments)
                {
                    if (appointment.AppointmentStartTime.HasValue)
                    {
                        appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                        appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
                    }
                }
            }
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
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein
            string apiUrl3 = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/GetAppointmentsByDoctor";
          
            Doctorblocktime = await _apiService.GetAsync<List<AppointmentListItem>>(apiUrl3, token) ?? new List<AppointmentListItem>();
            if (Doctorblocktime != null && Doctorblocktime.Count > 0)
            {
                foreach (var appointment in Doctorblocktime)
                {
                    if (appointment.AppointmentStartTime.HasValue)
                    {
                        appointment.AppointmentStartTime = appointment.AppointmentStartTime.Value.AddHours(-5).AddMinutes(-30);
                        appointment.AppointmentEndDateTime = appointment.AppointmentEndDateTime.Value.AddHours(-5).AddMinutes(-30);
                    }
                }
            }

            PatientData = await _apiService.GetAsync<ProfileListItem>(apiUrl, token) ?? new ProfileListItem();

            ChangeRequests = await _apiService.GetAsync<List<string>>(apiUrl2, token) ?? new List<string>();
        }

        public async Task<IActionResult> OnPostSavePatientAsync()
        {
            try
            {


                using var reader = new StreamReader(HttpContext.Request.Body);
                var json = await reader.ReadToEndAsync();
                ProfileListItem viewModel = JSON.Deserialize<ProfileListItem>(json);


                string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/Profile/Addpatientportalchanges";
                string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI3NDUiLCJqdGkiOiI0N2FiZjA0NC1kYjQzLTQwZGItYWQ3MC03NWMzMDY3M2U1MjciLCJuYmYiOjE3NDA2NDgyODgsImV4cCI6MTc3MjE4NDI4NywiaWF0IjoxNzQwNjQ4Mjg4LCJpc3MiOiJDb25uZXR3ZWxsQ0lTIiwiYXVkIjoiQ29ubmV0d2VsbENJUyJ9.QoJcFwTUCrVEQWfe3zjXFnduo7nSUOwok5lCZUqrjJcK4bMt9R9pvU3UOla7XfI6cj8tHvHAZVZykexi19GpQQ"; // Valid token yahan dalein

                var apiHelper = new ApiService(_httpClient);
                var response = await _apiService.PostAsync<ProfileListItem, ApiResponse>(apiUrl, viewModel, token);

                if (response != null && response.IsSuccess)
                {
                    return new JsonResult(new { message = "Your change request has been submitted." });

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


            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/viewAppointmentButton";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein

            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<AppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

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
            AppointmentListItem viewModel = JSON.Deserialize<AppointmentListItem>(json);
            

            string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/AddAppointmentbyportalAppointmentbyPatientId";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein

            var apiHelper = new ApiService(_httpClient);
            var response = await _apiService.PostAsync<AppointmentListItem, ApiResponse>(apiUrl, viewModel, token);

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
