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

            var socks = new HttpToSocks5Proxy("185.233.82.111", 9694, "6L4E69", "aJcf5a");
            
            _bot = new TelegramBotClient("1212762438:AAFUFtjX85bpnIGY6Cvc1LzpuUp03gDLJ6Q", socks);
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