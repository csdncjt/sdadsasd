using System.Collections.Generic;

public class PokerUtility
{
    // 方块(Diamond) : 0x01 0x02 0x03 0x04 0x05 0x06 0x07 0x08 0x09 0x0a 0x0b 0x0c 0x0d 方块A-10 J Q K
    // 梅花(Club) : 0x11 0x12 0x13 0x14 0x15 0x16 0x17 0x18 0x19 0x1a 0x1b 0x1c 0x1d 梅花A-10 J Q K
    // 红桃(Heart) : 0x21 0x22 0x23 0x24 0x25 0x26 0x27 0x28 0x29 0x2a 0x2b 0x2c 0x2d 红桃A-10 J Q K
    // 黑桃(Spade) : 0x31 0x32 0x33 0x34 0x35 0x36 0x37 0x38 0x39 0x3a 0x3b 0x3c 0x3d 黑桃A-10 J Q K
    // 王(Joker) : 0x4E 0x4F 大小王
    // 牌型定义
    public enum PokerType
    {
        t_none,
        /// <summary>
        /// 单张
        /// </summary>
        t_1,
        /// <summary>
        /// 顺子
        /// </summary>
        t_1n,
        /// <summary>
        /// 同花
        /// </summary>
        t_1x,
        /// <summary>
        /// 同花顺
        /// </summary>
        t_1xn,
        /// <summary>
        /// 对子
        /// </summary>
        t_2,
        /// <summary>
        /// 两对
        /// </summary>
        t_22,
        /// <summary>
        /// 连对
        /// </summary>
        t_2n,
        /// <summary>
        /// 豹子
        /// </summary>
        t_3,
        /// <summary>
        /// 多组豹子
        /// </summary>
        t_3n,
        /// <summary>
        /// 三带一
        /// </summary>
        t_31,
        /// <summary>
        /// 三带一飞机
        /// </summary>
        t_31n,
        /// <summary>
        /// 三代二
        /// </summary>
        t_32,
        /// <summary>
        /// 三代二飞机
        /// </summary>
        t_32n,
        /// <summary>
        /// 炸弹
        /// </summary>
        t_4,
        /// <summary>
        /// 四带一
        /// </summary>
        t_41,
        /// <summary>
        /// 四带二
        /// </summary>
        t_42,
        /// <summary>
        /// 四带三
        /// </summary>
        t_43,
        /// <summary>
        /// 王炸
        /// </summary>
        t_king
    }

    //    private static readonly Dictionary<int, List<int>> cardPool = new Dictionary<int, List<int>>()
    //    {
    //        [45] = new List<int>()
    //        {
    //            0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c,
    //            0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d,
    //            0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d,
    //            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d
    //        },
    //        [48] = new List<int>()
    //        {
    //            0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d,
    //            0x11, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d,
    //            0x21, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d,
    //            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d
    //        },
    //        [54] = new List<int>()
    //        {
    //            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d,
    //            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d,
    //            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d,
    //            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d,
    //            0x4E, 0x4F
    //        }
    //    };

    public static string pokerTypeToName(PokerType pokerType)
    {
        string name = string.Empty;
        switch (pokerType)
        {
            case PokerType.t_none:
                name = "高牌";
                break;
            case PokerType.t_1:
                name = "单张";
                break;
            case PokerType.t_1n:
                name = "顺子";
                break;
            case PokerType.t_1x:
                name = "同花";
                break;
            case PokerType.t_1xn:
                name = "同花顺";
                break;
            case PokerType.t_2:
                name = "对子";
                break;
            case PokerType.t_22:
                name = "两对";
                break;
            case PokerType.t_2n:
                name = "连对";
                break;
            case PokerType.t_3:
                name = "豹子";
                break;
            case PokerType.t_3n:
                name = "连豹子";
                break;
            case PokerType.t_31:
                name = "三带一";
                break;
            case PokerType.t_31n:
                name = "三带一飞机";
                break;
            case PokerType.t_32:
                name = "三带二";
                break;
            case PokerType.t_32n:
                name = "三带二飞机";
                break;
            case PokerType.t_4:
                name = "炸弹";
                break;
            case PokerType.t_41:
                name = "四带一";
                break;
            case PokerType.t_42:
                name = "四带二";
                break;
            case PokerType.t_43:
                name = "四带三";
                break;
            case PokerType.t_king:
                name = "王炸";
                break;
        }

        return name;
    }

    private static int getCardIndex(byte card, bool point2IsMax = false)
    {
        int index = card & 0x0f;
        if (index == 0x01)
        {
            // A
            index = 14;
        }
        else if (index == 0x02)
        {
            // 2
            if (point2IsMax)
                index = 16;
        }
        else if (index == 0x0e)
        {
            // 小王
            index = 17;
        }
        else if (index == 0x0f)
        {
            // 大王
            index = 18;
        }

        return index - 1;
    }

    /// <summary>
    /// 获取花色
    /// </summary>
    /// <param name="card"></param>
    /// <returns>1方块/2梅花/3红桃/4黑桃/5王</returns>
    private static int getCardKind(byte card)
    {
        int kind = card & 0x70; // 0方块 16梅花 32红桃 48黑桃 64王
        switch (kind)
        {
            case 0x00:
                kind = 1;
                break;
            case 0x10:
                kind = 2;
                break;
            case 0x20:
                kind = 3;
                break;
            case 0x30:
                kind = 4;
                break;
            case 0x40:
                kind = 5;
                break;
        }

        return kind;
    }

    // 每个点数的数量(点数 A-K A _ 2 王 王)
    private static int[] cacheCards = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    // 单张 对子 三条 四张 的数量
    private static int[] cacheCounts = new int[] { 0, 0, 0, 0 };
    // 单张 对子 三条 四张 的点数
    private static List<List<int>> cacheTypeCards = new List<List<int>>() { new List<int>(), new List<int>(), new List<int>(), new List<int>() };

    public static PokerType getType(List<byte> outCards)
    {
        for (int i = 0, n = cacheCards.Length; i < n; i++)
        {
            cacheCards[i] = 0;
        }

        for (int i = 0, n = outCards.Count; i < n; i++)
        {
            int index = getCardIndex(outCards[i]);
            cacheCards[index]++;
        }

        for (int i = 0, n = cacheCounts.Length; i < n; i++)
        {
            cacheCounts[i] = 0;
        }

        for (int i = 0, n = cacheTypeCards.Count; i < n; i++)
        {
            if (null != cacheTypeCards[i])
            {
                cacheTypeCards[i].Clear();
            }
            else
            {
                cacheTypeCards[i] = new List<int>();
            }
        }

        for (int i = 0, n = cacheCards.Length; i < n; i++)
        {
            int c = cacheCards[i];
            // 该点数有牌
            if (c != 0)
            {
                cacheCounts[c - 1]++;
                cacheTypeCards[c - 1].Add(i);
            }
        }

        if (cacheCounts[3] != 0)
        {
            return getType4(cacheCounts);
        }
        else if (cacheCounts[2] != 0)
        {
            return getType3(cacheCounts, cacheTypeCards);
        }
        else if (cacheCounts[1] != 0)
        {
            return getType2(cacheCounts, cacheTypeCards);
        }
        else if (cacheCounts[0] != 0)
        {
            return getType1(cacheCounts, cacheTypeCards, outCards);
        }
        return PokerType.t_none;
    }

    private static PokerType getType4(int[] counts)
    {
        if (counts[3] > 1)
        {
            // 两个炸弹不能一起出 2222 3333
            return PokerType.t_none;
        }

        int sum = counts[2] + counts[1] + counts[0];
        if (sum > 1)
        {
            return PokerType.t_none;
        }

        if (sum == 0)
        {
            return PokerType.t_4;
        }

        if (counts[2] == 1)
        {
            return PokerType.t_43;
        }

        if (counts[1] == 1)
        {
            return PokerType.t_42;
        }

        if (counts[0] == 1)
        {
            return PokerType.t_41;
        }

        return PokerType.t_none;
    }

    private static PokerType getType3(int[] counts, List<List<int>> cards)
    {
        int count3 = counts[2];
        int count2 = counts[1];
        int count1 = counts[0];

        if (count3 > 1)
        {
            if (isContinue(cards[2]))
            {
                if (count3 == count2 && count1 == 0)
                {
                    return PokerType.t_32n;
                }

                if (count3 == (count2 * 2 + count1 * 1))
                {
                    return PokerType.t_31n;
                }
            }
        }
        else if (count3 == 1)
        {
            if (count2 == 1 && count1 == 0)
            {
                return PokerType.t_32;
            }

            if (count2 == 0 && count1 == 1)
            {
                return PokerType.t_31;
            }

            if (count2 == 0 && count1 == 0)
            {
                return PokerType.t_3;
            }
        }

        return PokerType.t_none;
    }

    private static PokerType getType2(int[] counts, List<List<int>> cards)
    {
        if (counts[0] > 0)
        {
            return PokerType.t_none;
        }

        if (counts[1] == 1)
        {
            return PokerType.t_2;
        }

        if (counts[1] == 2)
        {
            return PokerType.t_22;
        }

        if (isContinue(cards[1]))
        {
            return PokerType.t_2n;
        }

        return PokerType.t_none;
    }

    private static PokerType getType1(int[] counts, List<List<int>> cards, List<byte> outCards)
    {
        int count = counts[0];
        if (count == 1)
        {
            return PokerType.t_1;
        }

        if (count == 2)
        {
            // 王炸
            if (cards[0][0] == 16 && cards[0][1] == 17)
            {
                return PokerType.t_king;
            }
        }

        if (count < 5)
        {
            return PokerType.t_none;
        }

        bool isC = isContinue(cards[0]);
        bool isSame = isSameKind(outCards);

        if (!isC)
        {
            bool isChangeA = false;
            int indexA = -1;
            for (int i = 0, n = cards[0].Count; i < n; i++)
            {
                if (cards[0][i] == 13 /*13=14-1*/)
                {
                    indexA = i;
                    isChangeA = true;
                    break;
                }
            }

            if (isChangeA)
            {
                List<int> tmp = new List<int>();
                for (int i = 0, n = cards[0].Count; i < n; i++)
                {
                    tmp.Add(cards[0][i]);
                }

                tmp.RemoveAt(indexA);
                tmp.Insert(0, 0);
                bool isContinueChangeA = isContinue(tmp);
                if (!isContinueChangeA)
                {
                    if (isSame)
                    {
                        return PokerType.t_1x;
                    }
                    else
                    {
                        return PokerType.t_none;
                    }
                }
                else
                {
                    if (isSame)
                    {
                        return PokerType.t_1xn;
                    }
                    else
                    {
                        return PokerType.t_1n;
                    }
                }
            }
        }

        if (!isC && isSame)
        {
            return PokerType.t_1x;
        }

        if (isC && !isSame)
        {
            return PokerType.t_1n;
        }

        if (isC && isSame)
        {
            return PokerType.t_1xn;
        }

        return PokerType.t_none;
    }

    private static bool isContinue(List<int> cards)
    {
        int last = -1;
        for (int i = 0, n = cards.Count; i < n; i++)
        {
            if (last != -1 && last + 1 != cards[i])
            {
                return false;
            }

            last = cards[i];
        }

        return true;
    }

    private static bool isSameKind(List<byte> cards)
    {
        int last = -1;
        for (int i = 0, n = cards.Count; i < n; i++)
        {
            if (last == -1)
            {
                last = getCardKind(cards[i]);
            }
            else
            {
                if (last != getCardKind(cards[i]))
                {
                    return false;
                }
            }
        }

        return true;
    }
}

