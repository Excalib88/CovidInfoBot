using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovidTelegramBot
{
    public class CovidService
    {
        public async Task<CovidInfo> GetInfo()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://yandex.ru/web-maps/covid19");
            var regex = new Regex("(stat-item-value\">)(.*?)(</div>)");
            var content = await response.Content.ReadAsStringAsync();
            var matches = regex.Matches(content).ToList();

            var result = new CovidInfo
            {
                AllTime = matches[0].Groups[2].Value.ToInt(),
                Today = matches[1].Groups[2].Value.ToInt(),
                Recovered = matches[2].Groups[2].Value.ToInt(),
                Dead = matches[3].Groups[2].Value.ToInt()
            };

            return result;
        }

        public async Task<List<CovidRegion>> GetRegion()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://yandex.ru/web-maps/covid19");
            var pattern = "(<div class=\"covid-panel-view__item-name\">)(.*?)(</div><div class=\"covid-panel-view__item-cases\">)(.*?)(<div class=\"covid-panel-view__item-cases-diff\">)(.*?)(</div>)";
            var regex = new Regex(pattern);
            var content = await response.Content.ReadAsStringAsync();
            var matches = regex.Matches(content).ToList();

            var result = new List<CovidRegion>();
            
            foreach (var match in matches)
            {
                // тупа костылю посаны
                var totalString = match.Groups[4].Value;
                int total;
                var today = 0;
                
                if (totalString.Length > 10)
                {
                    var i = 0;
                    var temp = "";
                    while (char.IsDigit(totalString[i]))
                    {
                        temp += totalString[i];
                        i++;
                    }
                    total = int.Parse(temp);
                }
                else
                {
                    total = totalString.ToInt();
                    today = match.Groups[6].Value.ToInt();
                }
                
                result.Add(new CovidRegion
                {
                    Name = match.Groups[2].Value,
                    Total = total,
                    Today = today
                });
            }

            return result;
        }
    }
}