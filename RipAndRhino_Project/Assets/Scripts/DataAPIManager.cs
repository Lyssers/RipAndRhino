using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;

public class DataAPIManager : MonoBehaviour
{
    private static readonly HttpClient httpClient = new HttpClient();
    private const string baseURL = "https://jsonplaceholder.typicode.com/";

    public void SendRequest()
    {
        var comments = GetComments();
        PrintComments(comments);
    }

    public async void SendRequestAsync()
    {
        var comments = await GetCommentsAsync();
        PrintComments(comments);
    }

    public async Task<List<CommentModel>> GetCommentsAsync()
    {
        httpClient.BaseAddress = new Uri(baseURL);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await httpClient.GetAsync("comments/");
        if (response.StatusCode != HttpStatusCode.OK)
            return null;
        var resourceJson = await response.Content.ReadAsStringAsync();
        await Task.Delay(3000);
        return JsonConvert.DeserializeObject<List<CommentModel>>(resourceJson);
    }

    public List<CommentModel> GetComments()
    {
        httpClient.BaseAddress = new Uri(baseURL);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = httpClient.GetAsync("comments/");
        if (response.Result.StatusCode != HttpStatusCode.OK)
            return null;
        var resourceJson = response.Result.Content.ReadAsStringAsync();
        System.Threading.Thread.Sleep(3000);
        return JsonConvert.DeserializeObject<List<CommentModel>>(resourceJson.Result);
    }

    private void PrintComments(List<CommentModel> modelList)
    {
        string result = "";
        foreach (var item in modelList)
        {
            result += "\n" + "id:" +  item.id;
            result += "\n" + "post id:" + item.postId;
            result += "\n" + "name:" + item.name;
            result += "\n" + "email:" +  item.email;
            result += "\n" + "body:" + item.body;
            result += "\n";
        }
        Debug.Log("<b>API Response:</b> \n " + result);
    }
}
