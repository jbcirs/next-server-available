using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;

namespace NextServerAvailable
{
    public class FindServer
    {
        [Function("FindServer")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "findserver")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("FindServer");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<List<Servers>>(requestBody);
            var result = new Servers();

            result.number = Convert.ToInt32(FindNewServer(request));

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        private string FindNewServer(List<Servers> usedServers)
        {
            var result = string.Empty;
            var items = usedServers.OrderBy(t => t.number);

            if (usedServers.Count() > 0)
            {
                foreach (var item in items)
                {
                    if (item.number != 1 && item.number == items.FirstOrDefault().number)
                    {
                        result = 1.ToString();
                    }
                    else
                    {
                        var nextNubmer = item.number + 1;

                        if (items.Where(t => t.number == nextNubmer).Count() == 0)
                        {
                            result = nextNubmer.ToString();
                            break;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    result = items.LastOrDefault().number.ToString();
                }
            }
            else
            {
                result = 1.ToString();
            }

            return result;
        }
    }

    public class Servers
    {
        public int number { get; set; }
    }
}
