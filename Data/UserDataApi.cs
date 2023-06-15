using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static module_21_exercise_2_WPF.AccountDataApi;

namespace module_21_exercise_2_WPF
{
    public class UserDataApi //уплавление пользователями
    {
        private HttpClient httpClient { get; set; }

        public UserDataApi()
        {
            httpClient = new HttpClient();
        }

        //создание класса IdentityUser, который используется в БД на сервере     
        public class IdentityUser : IdentityUser<string>
        {
            public IdentityUser()
            {
                Id = Guid.NewGuid().ToString();
                SecurityStamp = Guid.NewGuid().ToString();
            }

            public IdentityUser(string userName) : this()
            {
                UserName = userName;
            }
        }

        //создание класса User
        public class User : IdentityUser
        {
        }

        //получение всех пользователей
        public List<User> GetUsers()
        {
            string url = @"https://localhost:44380/api/application/users/";

            string json = httpClient.GetStringAsync(url).Result;
            Console.WriteLine(json);
            return JsonConvert.DeserializeObject<List<User>>(json);
        }

        //добавление пользователя
        public string AddUser(UserForm user)
        {
            string url = @"https://localhost:44380/api/application/users/";

            var r = httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;
            if (r.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return "ok";
            }
            return "fail";
        }

        //изменение пользователя
        public void UpdateUser(User user)
        {
            string url = $"https://localhost:44380/api/application/users/";

            var r = httpClient.PutAsync(
                requestUri: url,
                content: new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8,
                mediaType: "application/json")
                ).Result;
        }

        //удаление пользователя
        public void DeleteUser(string id)
        {
            string url = @"https://localhost:44380/api/application/users/";
            var result = httpClient.DeleteAsync(url + id).Result;            
        }
    }
}
