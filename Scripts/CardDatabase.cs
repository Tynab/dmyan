using System.Collections.Generic;

namespace DMYAN.Scripts;

public partial class CardDatabase
{
    public readonly Dictionary<string, (int Atk, int Def, string Type)> CARDS = new()
    {
        { "02118022", (1500, 900, "Monster") },
        { "05901497", (350, 300, "Monster") },
        { "07805359",  (900, 800, "Monster") },
        { "08471389",  (1200, 1400, "Monster") }
    };
}
