using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// 08/23/2021 07:46 am - SSN - [20210822-1222] - [016] - M04-06 - Demo: Enhancing the application's routing features

namespace BethanysPieShopHRM.Shared
{

    public class FeedbackMessage
    {
        public string MessageText { get; set; }
        public bool ErrorMessage { get; set; }

        //public static void Add(List<FeedbackMessage> feedbackMessages, string message, bool isErrorMessage)
        //{
        //    feedbackMessages.Add(new FeedbackMessage { MessageText = message, ErrorMessage = isErrorMessage });

        //}
    }

    public static class FeedbackMessageUtil
    {
        public static void Add_2(this List<FeedbackMessage> feedbackMessages, string message, bool isErrorMessage)
        {
            feedbackMessages.Add(new FeedbackMessage { MessageText = message, ErrorMessage = isErrorMessage });

        }

        public static string GetFeedbackMessagesAsHTML(this List<FeedbackMessage> feedbackMessages)
        {
            StringBuilder sb = new StringBuilder();

            foreach (FeedbackMessage message in feedbackMessages)
            {
                sb.AppendLine(string.Format("<p class='{0}'>{1}</p>", message.ErrorMessage ? "alert-danger" : "alert-info", message.MessageText));
            }

            return sb.ToString();
        }

    }

    public class APIBag<T>
    {
        public T ModelRecord { get; set; }
        public List<FeedbackMessage> FeedbackMessages { get; set; } = new List<FeedbackMessage>();


        public  void parseRespose(  Stream response)
        {
            StreamReader sr = new StreamReader(response);
            StringBuilder sb = new StringBuilder();

            while (sr.Peek() >= 0)
            {
                sb.AppendLine(sr.ReadLine());
            }


            try
            {
                // result.ModelRecord = await JsonSerializer.DeserializeAsync<Employee>(response);
                this.ModelRecord = JsonSerializer.Deserialize<T>(sb.ToString());

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            try
            {
                // result.FeedbackMessages = await JsonSerializer.DeserializeAsync<List<FeedbackMessage>>(response);
                this.FeedbackMessages = JsonSerializer.Deserialize<List<FeedbackMessage>>(sb.ToString());


            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

            //        result.FeedbackMessages = JsonSerializer.Deserialize<List<FeedbackMessage>>(await response.Content.ReadAsStringAsync());
        }


    }

}
