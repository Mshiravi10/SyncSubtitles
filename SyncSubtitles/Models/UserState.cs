using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncSubtitles.Models
{
    class UserState
    {
        public string FilePath { get; set; }
        public bool AwaitingTimeInput { get; set; }
        public bool AwaitingFileInput { get; set; }

    }
}
