using System;
using System.Collections.Generic;

namespace DMYAN.Scripts;

public static class Extension
{
    #region Definitions

    private static readonly Random _random = new();

    #endregion

    #region Methods

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

    #endregion
}
