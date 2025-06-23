using System.Collections.Generic;
using static DMYAN.Scripts.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;
using static System.Enum;

namespace DMYAN.Scripts;

public static class CardDatabase
{
    private static readonly Dictionary<string, CardData> _cardData = [];
    private static bool _isLoaded = false;

    public static void LoadCards()
    {
        if (_isLoaded)
        {
            return;
        }

        using var file = Open(CARDS_DATA_PATH, Read);

        if (!file.EofReached())
        {
            _ = file.GetLine();
        }

        while (!file.EofReached())
        {
            var line = file.GetLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(',');

            var cardData = new CardData
            {
                Id = int.TryParse(parts[0], out var idVal) ? idVal : 0,
                Code = parts[1].Trim('"'),
                Name = parts[2].Trim('"'),
                Description = parts[3].Trim('"'),
                Type = TryParse(parts[4], true, out CardType typeVal) ? typeVal : CardType.None,
                Property = TryParse(parts[5], true, out CardProperty propVal) ? propVal : CardProperty.None,
                Attribute = TryParse(parts[6], true, out MonsterAttribute attrVal) ? attrVal : MonsterAttribute.None,
                Race = TryParse(parts[7], true, out MonsterRace raceVal) ? raceVal : MonsterRace.None,
                SummonType = TryParse(parts[8], true, out MonsterSummonType summonVal) ? summonVal : MonsterSummonType.None,
                Level = int.TryParse(parts[9], out var levelVal) ? levelVal : 0,
                ATK = int.TryParse(parts[10], out var atkVal) ? atkVal : -1,
                DEF = int.TryParse(parts[11], out var defVal) ? defVal : -1,
                BanlistStatus = TryParse(parts[12], true, out CardBanlistStatus banVal) ? banVal : CardBanlistStatus.None,
                EffectType = TryParse(parts[13], true, out CardEffectType effectVal) ? effectVal : CardEffectType.None
            };

            if (!string.IsNullOrWhiteSpace(cardData.Code) && !_cardData.TryAdd(cardData.Code, cardData))
            {
                _cardData[cardData.Code] = cardData;
            }
        }

        _isLoaded = true;
    }

    public static CardData GetCardData(string cardCode)
    {
        if (!_isLoaded)
        {
            LoadCards();
        }

        return _cardData.TryGetValue(cardCode, out var cardData) ? cardData : default;
    }
}

public struct CardData
{
    public int Id;
    public string Code;
    public string Name;
    public string Description;
    public CardType Type;
    public CardProperty Property;
    public MonsterAttribute Attribute;
    public MonsterRace Race;
    public MonsterSummonType SummonType;
    public int Level;
    public int ATK;
    public int DEF;
    public CardBanlistStatus BanlistStatus;
    public CardEffectType EffectType;
}
