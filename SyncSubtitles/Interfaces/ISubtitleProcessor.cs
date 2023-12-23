using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SyncSubtitles.Interfaces
{
    public interface ISubtitleProcessor : IAutoInjectable
    {
        Task ProcessAndSendSubtitleFile(ITelegramBotClient botClient, long chatId, string filePath, TimeSpan userStartTime, CancellationToken cancellationToken);
    }
}
