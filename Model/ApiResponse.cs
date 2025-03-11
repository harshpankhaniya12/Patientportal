namespace Patientportal.Model
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public bool IsSucceeded { get; set; }
        public string[] Errors { get; set; }
    }
}
