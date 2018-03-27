using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StockApp.Models;

namespace StockApp.Controllers
{
    public class HomeController : Controller
    {
        private const string Base = "https://api.iextrading.com/1.0/";
        private List<string> Symbols = new List<string>{ "RAVN", "WFC"};
        public IActionResult Index()
        {
            JObject json = new JObject();
            var _ = Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    //https://api.iextrading.com/1.0/
                    //https://api.iextrading.com/1.0/stock/market/batch?symbols=RAVN,WFC,tsla&types=quote,news,chart&range=1m&last=5
                    //https://api.iextrading.com/1.0/stock/market/batch?symbols=RAVN,WFC,tsla&types=price
                    //https://api.iextrading.com/1.0/stock/market/batch?symbols=RAVN,WFC,tsla&types=price,ohlc,news,company
                    var url = $"{Base}stock/market/batch?symbols={string.Join(",", Symbols)}&types=price,ohlc,news,company";
                    using (var request = await client.GetAsync(url))
                    {
                        if (request.IsSuccessStatusCode)
                        {
                            var data = await request.Content.ReadAsStringAsync();
                            json = JObject.Parse(data);
                            //var t = JsonConvert.DeserializeObject<ApiResult<GeocodedAddress>>(await request.Content.ReadAsStringAsync());
                        }
                    }
                }
            });

            _.Wait();

            return View(json);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
