using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using SyncSubtitles.Models;
using SyncSubtitles.Interfaces;

namespace SyncSubtitles
{
    public class CallbackQueryHandler : ICallbackQueryHandler
    {
        static Dictionary<long, TimeSpan> userTimes = new Dictionary<long, TimeSpan>();
        static Dictionary<long, UserState> userStates = new Dictionary<long, UserState>();
        public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var chatId = callbackQuery.Message.Chat.Id;
            var selectedData = callbackQuery.Data;
            var parts = selectedData.Split('_');

            // بررسی انتخاب بازه دقیقه
            if (selectedData.StartsWith("range_"))
            {
                int rangeStart = int.Parse(parts[1]);
                var minuteButtons = new List<InlineKeyboardButton[]>();
                for (int minute = rangeStart; minute < rangeStart + 10 && minute < 60; minute++)
                {
                    var buttonLabel = $"{minute:00}";
                    var buttonData = $"minute_{minute:00}";
                    minuteButtons.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonLabel, buttonData) });
                }

                var inlineKeyboard = new InlineKeyboardMarkup(minuteButtons);

                await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: callbackQuery.Message.MessageId,
                    text: "حالا دقیقه را انتخاب کنید:",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }
            // بررسی انتخاب دقیقه
            else if (parts[0] == "minute" && parts.Length == 2)
            {
                // نمایش دکمه‌های انتخاب بازه ثانیه
                var rangeSecondButtons = new List<InlineKeyboardButton[]>();
                for (int rangeStart = 0; rangeStart < 60; rangeStart += 10)
                {
                    var buttonLabel = $"{rangeStart}-{rangeStart + 9}";
                    var buttonData = $"{selectedData}_range_{rangeStart}";
                    rangeSecondButtons.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonLabel, buttonData) });
                }

                var inlineKeyboard = new InlineKeyboardMarkup(rangeSecondButtons);

                await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: callbackQuery.Message.MessageId,
                    text: "حالا بازه ثانیه را انتخاب کنید:",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }
            // بررسی انتخاب بازه ثانیه
            else if (parts.Length >= 4 && parts[2] == "range")
            {
                int minute = int.Parse(parts[1]);
                int rangeStartSecond = int.Parse(parts[3]);

                var secondButtons = new List<InlineKeyboardButton[]>();
                for (int second = rangeStartSecond; second < rangeStartSecond + 10 && second < 60; second++)
                {
                    var buttonLabel = $"{second:00}";
                    var buttonData = $"minute_{minute}_second_{second}";
                    secondButtons.Add(new[] { InlineKeyboardButton.WithCallbackData(buttonLabel, buttonData) });
                }

                var inlineKeyboard = new InlineKeyboardMarkup(secondButtons);

                await botClient.EditMessageTextAsync(
                    chatId: chatId,
                    messageId: callbackQuery.Message.MessageId,
                    text: "حالا ثانیه را انتخاب کنید:",
                    replyMarkup: inlineKeyboard,
                    cancellationToken: cancellationToken);
            }
            // بررسی انتخاب نهایی دقیقه و ثانیه
            else if (parts[0] == "minute" && parts.Length == 4 && parts[2] == "second")
            {
                var minute = int.Parse(parts[1]);
                var second = int.Parse(parts[3]);
                var userStartTime = new TimeSpan(0, minute, second);

                userTimes[chatId] = userStartTime; // ذخیره زمان انتخابی کاربر

                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "لطفا فایل زیرنویس خود را ارسال کنید",
                    cancellationToken: cancellationToken);
                userStates[chatId].AwaitingFileInput = true;
                userStates[chatId].AwaitingTimeInput = false;
            }
        }
    }
}
