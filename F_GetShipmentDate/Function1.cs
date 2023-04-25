using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace F_GetShipmentDate
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string orderId = req.Query["orderId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            orderId = orderId ?? data?.orderId;

            string responseMessage = string.IsNullOrEmpty(orderId)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {orderId}. This HTTP triggered function executed successfully.";

            var res = createResponse(orderId);

            return new OkObjectResult(responseMessage);
        }

        public static string createResponse(string orderId)
        {
            var client = new RestClient("https://orderstatusapi-dot-organization-project-311520.uc.r.appspot.com/api");
            var request = new RestRequest("getOrderStatus/");
            request.AddJsonBody("{\r\n \"orderId\": \"31312\"\r\n}");
            var response = client.Post(request);
            var content = response.Content; // Raw content as string

            return content;

        }
    }
}
