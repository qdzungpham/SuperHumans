using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Plugin.RestClient
{
    /// <summary>
    /// RestClient implements methods for calling CRUD operations
    /// using HTTP.
    /// </summary>
    public class RestClient<T>
    {
        //private string WebServiceUrl;  //
        private string baseUri = "http://www.hojunanum.com/api/";  //http://localhost:58455

        public RestClient() // string WebServiceUrl
        {
            //this.WebServiceUrl = WebServiceUrl;
        }
        // 데이터를 받고있구요
        public async Task<List<T>> GetAsync(string ControllerName)
        {
            var httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(baseUri);
            //var json = await httpClient.GetStringAsync(WebServiceUrl);
            //var taskModels = JsonConvert.DeserializeObject<List<T>>(json);
            var json = await httpClient.GetStringAsync(baseUri+ ControllerName);
            var taskModels = JsonConvert.DeserializeObject<List<T>>(json);
            return taskModels;
        }

        public async Task<List<T>> GetListByIDAsync(string ControllerName,string _id)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUri + ControllerName + "/");
            var json = await httpClient.GetStringAsync(_id);
            var taskModels = JsonConvert.DeserializeObject<List<T>>(json);
            return taskModels;
        }

        public async Task<T> GetOneObjAsync(string ControllerName,string _id)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUri+ ControllerName+"/");
            var json = await httpClient.GetStringAsync(_id);
            var taskModels = JsonConvert.DeserializeObject<T>(json);

            return taskModels;
        }

        // 데이터를 create 하고있죠
        public async Task<bool> PostAsync(T t,string ControllerName)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(baseUri+ ControllerName, httpContent);

            return result.IsSuccessStatusCode;
        }

        //데이터를 업데이트(id)를 가지고.
        public async Task<bool> PutAsync(int id, T t,string ControllerName)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PutAsync(baseUri + ControllerName + "/"+ id, httpContent);

            return result.IsSuccessStatusCode;
        }

        //삭제하고 있네요
        public async Task<bool> DeleteAsync(int id, string ControllerName)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.DeleteAsync(baseUri + ControllerName + "/" + id);

            return response.IsSuccessStatusCode;
        }
    }
}
