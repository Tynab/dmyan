using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMYAN.Scripts.Common.Enum;

internal enum DuelPhase
{
    None = 0,       // Vô
    Draw = 1,       // Rút bài
    Standby = 2,    // Chuẩn bị
    Main1 = 3,      // Chính 1
    Battle = 4,     // Giao tranh
    Main2 = 5,      // Chính 2
    End = 6         // Kết thúc
}
