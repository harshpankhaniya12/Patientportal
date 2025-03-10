using Innovura.CSharp.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Patientportal.AllApicall;
using Patientportal.Model;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Patientportal.Pages.Account
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        //private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        [BindProperty]
        public InputModel Input { get; set; }
        public IndexModel(ILogger<IndexModel> logger, HttpClient httpClientFactory, ApiService apiService)
        {
            _logger = logger;
            _httpClient = httpClientFactory;
            _apiService = apiService;
        }
        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                OnPostAsync(); // Already logged in user ko home page pe bheje
            }
            return Page();
        }
        public async Task<JsonResult> OnPostSendOTPAsync([FromBody] InputModel request)
        {
            if (string.IsNullOrEmpty(request.Mobile) || request.Mobile.Length < 10)
            {
                return new JsonResult(new { success = false, message = "Invalid phone number" });
            }

            string apiUrl = $"http://localhost:5165/api/v1/Account/PatientportalSendAuthToken/{request.Mobile}.json";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDUwIiwianRpIjoiMGZiNmI4NGEtNWQxNS00Y2JlLWIyY2ItODg3MjA5M2M0YTc5IiwibmJmIjoxNzQxNjMwMTcyLCJleHAiOjE3NzMxNjYxNzIsImlhdCI6MTc0MTYzMDE3MiwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.vnwTeZDidK0VS1HgdhGki_8MtMQRxyU_Hpr1QV5pwPTMqkNICMw0cczvQkJqe2_QmjUyzOfmwF56cgaIBBkqbw"; // Valid token yahan dalein

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var payload = new { phone = request.Mobile };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            try
            {
              
                var response = await _apiService.GetAsync<List<InputModel>>(apiUrl, token);
                //var result = await response.Content.ReadAsStringAsync();

                
                    return new JsonResult(new { success = true, message = "OTP Sent Successfully" });
               
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = "Server error: " + ex.Message });
            }
        }


        //public async Task<IActionResult> OnPostSendOTPAsync([FromBody] InputModel request)
        //{
        //    using var reader = new StreamReader(HttpContext.Request.Body);
        //    var json = await reader.ReadToEndAsync();
        //    InputModel viewModel = JSON.Deserialize<InputModel>(json);
        //    if (string.IsNullOrEmpty(request.Mobile) || request.Mobile.Length < 10)
        //    {
        //        return new JsonResult(new { success = false, message = "Invalid phone number" });
        //    }

        //    string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/SendOTP";
        //    string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein

        //    var payload = new { phone = request.Mobile };
        
        //        return new JsonResult(new { success = true, message = "OTP Sent Successfully" });
       
        //}
       
        public async Task<JsonResult> OnPostVerifyotpAsync([FromBody] InputModel request)
        {
           
            if (string.IsNullOrEmpty(request.OTP) || request.OTP.Length < 4)
            {
                return new JsonResult(new { success = false, message = "Invalid phone number" });
            }
            if (string.IsNullOrEmpty(request.Mobile) || string.IsNullOrEmpty(request.OTP) || request.OTP.Length < 4)
            {
                return new JsonResult(new { success = false, error = "Invalid OTP or Mobile Number" });
            }

            string apiUrl = "http://localhost:5165/api/v1/Account/verify-otp";
            string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMDUwIiwianRpIjoiMGZiNmI4NGEtNWQxNS00Y2JlLWIyY2ItODg3MjA5M2M0YTc5IiwibmJmIjoxNzQxNjMwMTcyLCJleHAiOjE3NzMxNjYxNzIsImlhdCI6MTc0MTYzMDE3MiwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.vnwTeZDidK0VS1HgdhGki_8MtMQRxyU_Hpr1QV5pwPTMqkNICMw0cczvQkJqe2_QmjUyzOfmwF56cgaIBBkqbw"; // Valid token yahan dalein
            var payload = new { mobile = request.Mobile, otp = request.OTP};
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _apiService.PostAsync<InputModel, ApiResponse>(apiUrl, request, token);
            //var result = await response..ReadAsStringAsync();

            if (response.IsSuccess)
            {
                // ✅ User authenticated, create session
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Mobile),
                new Claim("OTPVerified", "true")
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return new JsonResult(new { success = true, message = "OTP Verified Successfully" });
            }
            else
            {
                return new JsonResult(new { success = false, error = response });
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync(); // Logout user
            return new JsonResult(new { success = true }); // Success response
        }


        //public async Task<IActionResult> OnPostVerifyotpAsync([FromBody] InputModel request)
        //{
        //    using var reader = new StreamReader(HttpContext.Request.Body);
        //    var json = await reader.ReadToEndAsync();
        //    InputModel viewModel = JSON.Deserialize<InputModel>(json);
        //    if (string.IsNullOrEmpty(request.OTP) || request.OTP.Length < 4)
        //    {
        //        return new JsonResult(new { success = false, message = "Invalid phone number" });
        //    }

        //    string apiUrl = "http://ec2-13-200-161-197.ap-south-1.compute.amazonaws.com:8888/api/v1/Appointment/AddAppointmentbyPatientPortal";
        //    string token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwianRpIjoiYjk5MDM2ZWItNTRhZS00ZWE0LWI1MjMtNThmYThlM2UzMzdkIiwibmJmIjoxNzQwNTU2NDQ1LCJleHAiOjE3NzIwOTI0NDUsImlhdCI6MTc0MDU1NjQ0NSwiaXNzIjoiQ29ubmV0d2VsbENJUyIsImF1ZCI6IkNvbm5ldHdlbGxDSVMifQ.yex9R3CP67Mkp715Y61FEIUIFhtiQhGJa8X01V_vEd_c9PuKw4uZbEi3_bQtpzQpwukb5uS_SPi4TN2HGh_JBQ"; // Valid token yahan dalein
        //    var apiHelper = new ApiService(_httpClient);
        //    var response = await _apiService.PostAsync<InputModel, ApiResponse>(apiUrl, viewModel, token);

        //    var payload = new { OTP = request.Mobile };
        //    //var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        //    //var response = await _httpClient.PostAsync(apiUrl, content);

        //    //if (response.IsSuccessStatusCode)
        //    //{
        //        return new JsonResult(new { success = true, message = "OTP Sent Successfully" });
        //    //}
        //    //else
        //    //{
        //    //    return new JsonResult(new { success = false, message = "Failed to send OTP" });
        //    //}
        //}




        //public async Task<JsonResult> OnPostVerifyOtpAsync()
        //{
        //    var apiUrl = "https://your-api.com/verify-otp"; // Replace with your API URL

        //    var payload = new { mobile = MobileNumber, otp = OtpCode };
        //    var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(apiUrl, content);
        //    var result = await response.Content.ReadAsStringAsync();

        //    if (response.IsSuccessStatusCode)
        //    {
        //        // If OTP is correct, create user session
        //        var claims = new List<Claim> { new Claim(ClaimTypes.Name, MobileNumber) };
        //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        var principal = new ClaimsPrincipal(identity);

        //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //        return new JsonResult(new { success = true });
        //    }
        //    else
        //    {
        //        return new JsonResult(new { success = false, error = result });
        //    }
        //}
    }
    public class InputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        [Required]
        [RegularExpression(@"^(\+?1?\d{10}|\d{10}|\d{12}|\d{13})$", ErrorMessage = "Invalid mobile number")]
        [MaxLength(13)]
        [MinLength(10)]
        public string Mobile { get; set; }
        public string OTP { get; set; }
        public string OTP1 { get; set; }
        public string OTP2 { get; set; }
        public string OTP3 { get; set; }
    }
}
