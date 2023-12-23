using SyncSubtitles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using SyncSubtitles.Interfaces;

namespace SyncSubtitles
{
    public class UpdateHandler : IUpdateHandler
    {
        static Dictionary<long, TimeSpan> userTimes = new Dictionary<long, TimeSpan>();
        static Dictionary<long, UserState> userStates = new Dictionary<long, UserState>();
        private readonly ISubtitleProcessor _subtitleProcessor;
        private readonly ICallbackQueryHandler _callbackQueryHandler;

        public UpdateHandler(ISubtitleProcessor subtitleProcessor, ICallbackQueryHandler callbackQueryHandler)
        {
            _subtitleProcessor = subtitleProcessor;
            _callbackQueryHandler = callbackQueryHandler;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            var chatId = update.Message?.Chat.Id;

            switch (update.Type)
            {
                case UpdateType.Message:
                    var message = update.Message;
                    if (message.Type == MessageType.Text && message.Text == "/sync")
                    {
                        await SendTimeSelectionButtons(botClient, message.Chat.Id, cancellationToken);
                        userStates[chatId.Value] = new UserState { AwaitingTimeInput = true };
                    }
                    else if (message.Type == MessageType.Document && userStates.ContainsKey((long)chatId) && userStates[chatId.Value].AwaitingFileInput)
                    {
                        var fileInfo = await botClient.GetFileAsync(message.Document.FileId, cancellationToken);
                        var filePath = $"./{message.Document.FileName}";
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await botClient.DownloadFileAsync(fileInfo.FilePath, fileStream, cancellationToken);
                        }

                        if (userTimes.ContainsKey((long)chatId))
                        {
                            TimeSpan userStartTime = userTimes[(long)chatId];
                            await _subtitleProcessor.ProcessAndSendSubtitleFile(botClient, (long)chatId, filePath, userStartTime, cancellationToken);
                            userStates.Remove((long)chatId);
                            userTimes.Remove((long)chatId);
                        }
                    }
                    break;

                case UpdateType.CallbackQuery:
                    await _callbackQueryHandler.HandleCallbackQueryAsync(botClient, update.CallbackQuery, cancellationToken);
                    break;
            }
        }

        static async Task SendTimeSelectionButtons(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            // ایجاد دکمه‌های Inline برای انتخاب بازه دقیقه
            var rangeButtons = new List<InlineKeyboardButton[]>();
            for (int rangeStart = 0; rangeStart < 60; rangeStart += 10)
            {
                var buttonLabel = $"{rangeStart}-{rangeStart + 9}";
                var buttonData = $"range_{rangeStart}";
                rangeButtons.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonLabel, buttonData) });
            }

            var inlineKeyboard = new InlineKeyboardMarkup(rangeButtons);

            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "لطفا بازه دقیقه را انتخاب کنید:",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

    }
}
