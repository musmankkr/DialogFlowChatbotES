// See https://aka.ms/new-console-template for more information

using RestSharp;

var client = new RestClient("https://orderstatusapi-dot-organization-project-311520.uc.r.appspot.com/api");

var request = new RestRequest("getOrderStatus/");

request.AddJsonBody("{\r\n \"orderId\": \"31312\"\r\n}");

var response = client.Post(request);

var content = response.Content; // Raw content as string

Console.WriteLine(content);