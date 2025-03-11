using System.Net.Http;

namespace Patientportal.AllApicall
{
    public class OTPService
    {
        private readonly Dictionary<string, (int Attempts, DateTime BlockTime)> _otpAttempts = new();

        public OTPService()
        {
           
        }
        public bool CanSendOTP(string phoneNumber)
        {
            if (_otpAttempts.ContainsKey(phoneNumber))
            {
                var (attempts, blockTime) = _otpAttempts[phoneNumber];

                if (attempts >= 3 && DateTime.UtcNow < blockTime)
                {
                    return false; // Blocked for 24 hours
                }
            }

            return true;
        }

        public void RecordOTPAttempt(string phoneNumber)
        {
            if (_otpAttempts.ContainsKey(phoneNumber))
            {
                var (attempts, blockTime) = _otpAttempts[phoneNumber];

                if (attempts >= 3)
                {
                    _otpAttempts[phoneNumber] = (attempts, DateTime.UtcNow.AddHours(24)); // Block for 24 hours
                }
                else
                {
                    _otpAttempts[phoneNumber] = (attempts + 1, blockTime);
                }
            }
            else
            {
                _otpAttempts[phoneNumber] = (1, DateTime.UtcNow);
            }
        }
    }
}
