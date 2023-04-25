using Google.Cloud.Dialogflow.V2;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Google.Protobuf;
using System.Text;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class orderController : ControllerBase
    {

        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

        [HttpPost]
        public async Task<JsonResult> Post()
        {
            WebhookRequest request;
            using (var reader = new StreamReader(Request.Body))
            {
                request = jsonParser.Parse<WebhookRequest>(reader);
            }

            var pas = request.QueryResult.Parameters;
            var orderId = pas.Fields["orderId"].StringValue;

            var client = new RestClient("https://orderstatusapi-dot-organization-project-311520.uc.r.appspot.com/api");
            var requestShipmentDate = new RestRequest("getOrderStatus/");
            string jsonBody = "{\r\n \"orderId\": \"value\"\r\n}";
            jsonBody = jsonBody.Replace("value", orderId);
            requestShipmentDate.AddJsonBody("{\r\n \"orderId\": \"31312\"\r\n}");

            var response = client.Post(requestShipmentDate).Content;
            string dateString = response.Substring(response.IndexOf(":\"") + 2, 19);

            DateTime shipmentDate = DateTime.Parse(dateString);
            string formatedDate = shipmentDate.ToString("dddd, dd MMM yyyy");

            var WebHookResponse = new WebhookResponse();
            StringBuilder sb = new StringBuilder();
            sb.Append("Your order " + orderId + " will be shipped on "+formatedDate);

            WebHookResponse.FulfillmentText = sb.ToString();

            JsonResult jsonResult = new JsonResult(WebHookResponse);
            return jsonResult;
        }

    }

}
