using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ParkManagement2.Helpers
{
    public class SMSHelper
    {
        public static void SendMessage(string message_body, string mobile)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings["SMSGateWayUrl"] + "api/sms/SubmitSms?message=" + message_body + "&mobile=" + mobile + "&gate_id=1");
                HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();


                //var temp = ConfigurationManager.AppSettings["SMSGateWayUrl"] + "api/sms/SubmitSms?message=" + message_body + "&mobile=" + mobile + "&gate_id=1";
                //var temp = ConfigurationManager.AppSettings["SMSGateWayUrl"] + "sms/SubmitSms?message=" + message_body + "&mobile=" + mobile + "&gate_id=1";
                //// message, string mobile, string gate_id
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["SMSGateWayUrl"] + "api/sms/SubmitSms?message=" + message_body + "&mobile=" + mobile + "&gate_id=1");
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //// If  you need to add token with request, 
                //// uncomment the below code and replace 'token_value' with actual Token.
                //// client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "token_value");

                //var response = client.GetAsync("route").Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    string responseString = response.Content.ReadAsStringAsync().Result;
                //}
            }

        }
    }
}