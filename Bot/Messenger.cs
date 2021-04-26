using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Bot
{
    public class Messenger
    {
        private ITelegramBotClient botClient;

        private CommandParser parser;

        public Messenger(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            parser = new CommandParser();

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            parser.AddCommand(new AddWordCommand(botClient));
            parser.AddCommand(new DeleteWordCommand());
            parser.AddCommand(new StartTrainingCommand(botClient));
            parser.AddCommand(new StopTrainingCommand());
            parser.AddCommand(new DictionaryCommand(botClient));
        }

        public async Task MakeAnswer(Conversation chat)
        {
            var lastMessage = chat.GetLastMessage();

            if (chat.IsTraningInProcess && !parser.IsTextCommand(lastMessage))
            {
                parser.ContinueTraining(lastMessage, chat);

                return;
            }

            if (chat.IsAddingInProcess)
            {
                parser.NextStage(lastMessage, chat);

                return;
            }

            if (parser.IsMessageCommand(lastMessage))
            {
                await ExecCommand(chat, lastMessage);
            }

            else
            {
                var text = CreateTextMessage();

                await SendText(chat, text);
            }
        }

        public async Task ExecCommand(Conversation chat, string command)
        {
            if (parser.IsTextCommand(command))
            {
                var text = parser.GetMessageText(command, chat);

                await SendText(chat, text);
            }

            if (parser.IsButtonCommand(command))
            {
                var keys = parser.GetKeyBoard(command);
                var text = parser.GetInformationalMeggase(command);
                parser.AddCallback(command, chat);

                await SendTextWithKeyBoard(chat, text, keys);
            }

            if (parser.AddingCommand(command))
            {
                parser.StartDictionary(command, chat);
            }

            if (parser.IsAddingCommand(command))
            {
                chat.IsAddingInProcess = true;
                parser.StartAddingWord(command, chat);
            }
        }

        private string CreateTextMessage()
        {
            var text = "Not a command";

            return text;
        }

        private async Task SendText(Conversation chat, string text)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

        private async Task SendTextWithKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }
    }
}
