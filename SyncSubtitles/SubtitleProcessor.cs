using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using System.Text.RegularExpressions;
using System.Globalization;
using SyncSubtitles.Interfaces;

namespace SyncSubtitles
{
    public class SubtitleProcessor : ISubtitleProcessor
    {
        public async Task ProcessAndSendSubtitleFile(ITelegramBotClient botClient, long chatId, string filePath, TimeSpan userStartTime, CancellationToken cancellationToken)
        {
            try
            {
                ProcessSubtitleFile(filePath, userStartTime);

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    await botClient.SendDocumentAsync(
                        chatId: chatId,
                        document: new InputFileStream(fileStream, Path.GetFileName(filePath)),
                        caption: "🎬 فایل زیرنویس هماهنگ‌شده شما آماده است!",
                        cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"❗️ خطا در پردازش فایل: {ex.Message}",
                    cancellationToken: cancellationToken);
            }
        }

        static void ProcessSubtitleFile(string filePath, TimeSpan userStartTime)
        {
            try
            {
                string[] subtitleLines = System.IO.File.ReadAllLines(filePath);
                TimeSpan firstDialogueTime = GetFirstDialogueTime(subtitleLines);
                TimeSpan syncTime = userStartTime - firstDialogueTime;

                for (int i = 0; i < subtitleLines.Length; i++)
                {
                    Match match = Regex.Match(subtitleLines[i], @"(\d{2}:\d{2}:\d{2},\d{3}) --> (\d{2}:\d{2}:\d{2},\d{3})");
                    if (match.Success)
                    {
                        TimeSpan start = AdjustTime(ParseTimeSpan(match.Groups[1].Value), syncTime);
                        TimeSpan end = AdjustTime(ParseTimeSpan(match.Groups[2].Value), syncTime);
                        subtitleLines[i] = $"{start:hh\\:mm\\:ss\\,fff} --> {end:hh\\:mm\\:ss\\,fff}";
                    }
                }

                System.IO.File.WriteAllLines(filePath, subtitleLines);
                Console.WriteLine("Subtitle file has been synchronized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the subtitle file: {ex.Message}");
            }
        }
        static TimeSpan GetFirstDialogueTime(string[] lines)
        {
            foreach (string line in lines)
            {
                Match match = Regex.Match(line, @"(\d{2}:\d{2}:\d{2},\d{3}) --> (\d{2}:\d{2}:\d{2},\d{3})");
                if (match.Success)
                {
                    return ParseTimeSpan(match.Groups[1].Value);
                }
            }
            return TimeSpan.Zero; // Return TimeSpan.Zero if no dialogue time is found
        }


        static TimeSpan ParseTimeSpan(string timeString)
        {
            timeString = timeString.Replace(',', '.');
            return TimeSpan.ParseExact(timeString, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
        }

        static TimeSpan AdjustTime(TimeSpan originalTime, TimeSpan adjustment)
        {
            TimeSpan adjustedTime = originalTime + adjustment;
            return (adjustedTime < TimeSpan.Zero) ? TimeSpan.Zero : adjustedTime; // Ensure time is not negative
        }

    }
}
