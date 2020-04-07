using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace CovidTelegramBot
{
    class Program
    {
        private static TelegramBotClient _bot;
        
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var socks = new HttpToSocks5Proxy("host", 9694, "username", "password");
            
            _bot = new TelegramBotClient("token", socks);
            StartBot();
            
            Thread.Sleep(Timeout.Infinite);
        }

        static void StartBot()
        {
            _bot.OnUpdate += OnUpdate;
            _bot.StartReceiving();
        }

        public static async void OnUpdate(object sender, UpdateEventArgs e)
        {
            var chat = e.Update.Message.Chat;
            var message = e.Update.Message;
            
            if(chat == null || message == null || string.IsNullOrEmpty(message.Text)) return;

            switch (message.Text)
            {
                case var text when text.Contains("region"):
                {
                    var covidService = new CovidService();
                    var models = await covidService.GetRegion();

                    var sb = new StringBuilder();
                
                    foreach (var model in models)
                    {
                        sb.Append(model);                
                    }
                
                    await _bot.SendTextMessageAsync(chat.Id, sb.ToString());
                    
                    break;
                }
                case var text when text.Contains("info"):
                {
                    var covidService = new CovidService();
                    var model = await covidService.GetInfo();

                    await _bot.SendTextMessageAsync(chat.Id, model.ToString());
                    
                    break;
                }
            }
        }
    }
}