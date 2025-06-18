using System.Collections.Generic;

namespace DMYAN.Scripts;

public partial class CardDatabase
{
    public readonly Dictionary<string, (int Atk, int Def)> CARDS = new()
    {
        { "02118022", (1500, 900) },
        { "05901497", (350, 300) },
        { "07805359",  (900, 800) },
        { "08471389",  (1200, 1400) }
    };
}
