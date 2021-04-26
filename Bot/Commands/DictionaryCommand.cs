using Bot.English_Trainer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Bot
{
    public class DictionaryCommand : AbstractCommand
    {
        private ITelegramBotClient botClient;

        public DictionaryCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;

            CommandText = "/dictionary";
        }

        public async void ProcessAsync(Conversation chat)
        {
            var text = "";
            foreach (var word in chat.dictionary)
            {
                text = word.Value.Russian + " - " + word.Value.English + " - " + word.Value.Theme;
                await SendText(text, chat.GetId());
            }

            
        }

        private async Task SendText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }
    }
}
