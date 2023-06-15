using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace module_21_exercise_2_WPF
{
    public class SubscriberDataApi //управление абонентами
    {
        private HttpClient httpClient { get; set; }

        public SubscriberDataApi()
        {
            httpClient = new HttpClient();
        }

        //добавление абонента
        public void AddSubscriber(Subscriber subscriber)
        {
            string url = @"https://localhost:44380/api/application/subscribers/";
            var r = httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(subscriber), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;
        }

        //изменение абонента
        public void UpdateSubscriber(Subscriber sub)
        {
            string url = $"https://localhost:44380/api/application/subscribers/";
            var r = httpClient.PutAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(sub), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;
        }

        //получение всех абонентов
        public IEnumerable<Subscriber> GetSubscribers()
        {
            string url = @"https://localhost:44380/api/application/subscribers/";
            string json = httpClient.GetStringAsync(url).Result;
            Console.WriteLine(json);
            return JsonConvert.DeserializeObject<IEnumerable<Subscriber>>(json);
        }

        //удаление абонента
        public void DeleteSubscriber(int id) 
        {            
            string url = @"https://localhost:44380/api/application/subscribers/";
            var result = httpClient.DeleteAsync(url+id).Result;            
        }
    }
}
