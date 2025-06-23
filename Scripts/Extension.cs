using System;
using System.Collections.Generic;

namespace DMYAN.Scripts;

public static class Extension
{
    private static readonly Random _random = new();

    public static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;

        while (n > 1)
        {
            n--;

            var k = _random.Next(n + 1);

            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static string GetCardAssetPathByCode(this string code) => $"res://Assets/Card/{code}.jpg";

    public static string ViewPower(this int value)
    {
        var power = value switch
        {
            < -1 => "?",
            -1 => "X000",
            int.MaxValue => "∞",
            _ => value.ToString()
        };

        return power.PadLeft(5, ' ');
    }

    public static string VietTranslation(this CardType cardType) => cardType switch
    {
        CardType.Monster => "Quái",
        CardType.Spell => "Phép",
        CardType.Trap => "Bẫy",
        _ => string.Empty
    };

    public static string VietTranslation(this CardProperty cardProperty) => cardProperty switch
    {
        CardProperty.Continuous => "Liên Tục",
        CardProperty.Equip => "Trang Bị",
        CardProperty.Field => "Sân Dã",
        CardProperty.QuickPlay => "Cấp Tốc",
        CardProperty.Ritual => "Nghi Lễ",
        CardProperty.Counter => "Phản Kích",
        CardProperty.Synchro => "Đồng Bộ",
        CardProperty.Xyz => "Chồng Lớp",
        CardProperty.Pendulum => "Dao Động",
        CardProperty.Link => "Liên Kết",
        CardProperty.DarkSynchro => "Hắc Đồng Bộ",
        CardProperty.Token => "Ảo Ảnh",
        CardProperty.LegendaryDragon => "Truyền Kỳ Long",
        _ => string.Empty
    };

    public static string VietTranslation(this MonsterAttribute monsterAttribute) => monsterAttribute switch
    {
        MonsterAttribute.Dark => "Ám",
        MonsterAttribute.Divine => "Thần",
        MonsterAttribute.Earth => "Địa",
        MonsterAttribute.Fire => "Hỏa",
        MonsterAttribute.Light => "Quang",
        MonsterAttribute.Water => "Thủy",
        MonsterAttribute.Wind => "Phong",
        MonsterAttribute.Spell => "Phép",
        MonsterAttribute.Trap => "Bẫy",
        _ => string.Empty
    };

    public static string VietTranslation(this MonsterRace monsterRace) => monsterRace switch
    {
        MonsterRace.Aqua => "Thủy Sinh",
        MonsterRace.Beast => "Thú",
        MonsterRace.BeastWarrior => "Thú Chiến",
        MonsterRace.Creator => "Tạo Giả",
        MonsterRace.CreatorGod => "Sáng Thế Thần",
        MonsterRace.Cyberse => "Không Gian Mạng",
        MonsterRace.Dinosaur => "Cự Long",
        MonsterRace.DivineBeast => "Thần Thú",
        MonsterRace.Dragon => "Long",
        MonsterRace.Fairy => "Tiên",
        MonsterRace.Fiend => "Ác Ma",
        MonsterRace.Fish => "Ngư",
        MonsterRace.Insect => "Trùng",
        MonsterRace.Illusion => "Ảo Ảnh",
        MonsterRace.Machine => "Cơ",
        MonsterRace.Plant => "Thực Vật",
        MonsterRace.Psychic => "Tâm Linh",
        MonsterRace.Pyro => "Viêm",
        MonsterRace.Reptile => "Bò Sát",
        MonsterRace.Rock => "Thạch",
        MonsterRace.SeaSerpent => "Hải Xà",
        MonsterRace.Spellcaster => "Pháp Sư",
        MonsterRace.Thunder => "Lôi Điện",
        MonsterRace.Warrior => "Chiến Binh",
        MonsterRace.WingedBeast => "Dực Thú",
        MonsterRace.Wyrm => "Á Long",
        MonsterRace.Zombie => "Thây Ma",
        _ => string.Empty
    };
}
