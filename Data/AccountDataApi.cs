using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.PeerToPeer;
using System.Text;
using System.Threading.Tasks;


namespace module_21_exercise_2_WPF
{
    public class AccountDataApi //управление логином и регистрацией
    {
        private HttpClient httpClient { get; set; }
        public AccountDataApi()
        {
            httpClient = new HttpClient();
        }

        //класс отправки пользователя на сервер
        public class UserForm
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        //логин
        public string Login(string userName, string password)
        {
            UserForm user = new UserForm()
            {
                UserName = userName,
                Password = password
            };            

            string url = @"https://localhost:44380/api/application/login";
            var result = httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {                
                return "ok";
            }
            return "fail";
        }

        //регистрация
        public string Registartion(string userName, string password)
        {
            UserForm user = new UserForm()
            {
                UserName = userName,
                Password = password
            };

            string url = @"https://localhost:44380/api/application/register";
            var result = httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return "ok";
            }
            return "fail";
        }

    }
}
