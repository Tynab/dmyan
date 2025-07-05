using DMYAN.Scripts.Common.Enum;
using System.Collections.Generic;
using YANLib;
using static DMYAN.Scripts.Common.Constant;
using static Godot.FileAccess;
using static Godot.FileAccess.ModeFlags;

namespace DMYAN.Scripts.Common;

internal static class CardDatabase
{
    private static readonly Dictionary<string, CardData> _cardData = [];
    private static bool _isLoaded = false;

    internal static void LoadCardsData()
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

            if (line.IsNullWhiteSpace())
            {
                continue;
            }

            var parts = line.Split(',');

            var cardData = new CardData
            {
                Code = parts[1].Trim('"'),
                Name = parts[2].Trim('"'),
                Description = parts[3].Trim('"'),
                Type = parts[4].Parse<CardType>(CardType.None),
                Property = parts[5].Parse<CardProperty>(CardProperty.None),
                Attribute = parts[6].Parse<MonsterAttribute>(MonsterAttribute.None),
                Race = parts[7].Parse<MonsterRace>(MonsterRace.None),
                SummonType = parts[8].Parse<MonsterSummonType>(MonsterSummonType.None),
                Level = parts[9].Parse<int?>(),
                ATK = parts[10].Parse<int?>(),
                DEF = parts[11].Parse<int?>(),
                BanlistStatus = parts[12].Parse<CardBanlistStatus>(CardBanlistStatus.None),
                EffectType = parts[13].Parse<CardEffectType>(CardEffectType.None)
            };

            if (cardData.Code.IsNotNullWhiteSpace() && !_cardData.TryAdd(cardData.Code, cardData))
            {
                _cardData[cardData.Code] = cardData;
            }
        }

        _isLoaded = true;
    }

    internal static CardData GetCardData(string cardCode)
    {
        if (!_isLoaded)
        {
            LoadCardsData();
        }

        return _cardData.TryGetValue(cardCode, out var cardData) ? cardData : default;
    }
}

internal struct CardData
{
    internal string Code;
    internal string Name;
    internal string Description;
    internal CardType Type;
    internal CardProperty Property;
    internal MonsterAttribute Attribute;
    internal MonsterRace Race;
    internal MonsterSummonType SummonType;
    internal int? Level;
    internal int? ATK;
    internal int? DEF;
    internal CardBanlistStatus BanlistStatus;
    internal CardEffectType EffectType;
}
