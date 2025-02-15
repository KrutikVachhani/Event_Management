using Newtonsoft.Json;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace Event_Management.Controllers
{
    public class SmsService
    {
        private readonly string _authKey = "441531Aq2aQ85gadE867b06851P1"; // Replace with your MSG91 Auth Key
        private readonly string _senderId = "YourLove"; // Use your approved MSG91 Sender ID
        private readonly string _templateId = "67b06a0dd6fc0564e833deb2";
        

        public async Task SendSms(string phoneNumber, string message)
        {
            string apiUrl = "https://api.msg91.com/api/v5/otp";
            var requestData = new
            {
                template_id = _templateId,
                sender = _senderId,
                message = $"Your OTP is {message}",
                mobiles = phoneNumber
            };

            string jsonPayload = JsonConvert.SerializeObject(requestData);

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("authkey", _authKey);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("SMS Response: " + result);
            }
        }
    }
}
