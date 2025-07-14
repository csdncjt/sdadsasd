using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETHotfix
{
    public enum CardType
    {
        RoyalFlush = 1,     // 皇家同花顺
        StraightFlush = 2,  // 同花顺
        FourOfAKind = 3,    // 四条
        FullHouse = 4,      // 葫芦
        Flush = 5,          // 同花
        Straight = 6,       // 顺子
        ThreeOfAKind = 7,   // 三条
        TwoPair = 8,        // 两对
        OnePair = 9,        // 一对
        None = 10,          // 高牌
    }

    public static class CardTypeUtil
    {

        //普通局测试代码
        public static void RunTest()
        {
            sbyte[][] cardss = new sbyte[][]
            {
            //皇家同花顺        
            new sbyte[] { 22, 21, 23, 10, 11, 25, 24 },
            //同花顺
            new sbyte[] { 22, 26, 28, 29, 30, 27, 7 },
            //四条
            new sbyte[] { 25, 12, 23, 10, 11, 38, 51 },
            //葫芦
            new sbyte[] { 17, 28, 15, 41, 30, 36, 21 },
            //同花
            new sbyte[] { 15, 17, 13, 23, 50, 36, 21 },
            //顺子
            new sbyte[] { 13, 41, 29, 17, 22, 36, 27 },
            //三条
            new sbyte[] { 15, 17, 28, 32, 37, 35, 41 },
            //两对
            new sbyte[] { 15, 17, 28, 32, 35, 22, 36 },
            //一对
            new sbyte[] { 17, 28, 32, 35, 14, 22, 36 },
            //高牌
            new sbyte[] { 17, 28, 32, 35, 14, 36, 21 }
            };

            foreach (sbyte[] cards in cardss)
            {
                //获取普通局牌型示例
                List<sbyte> highlightCards;
                CardType cardType = CardTypeUtil.GetCardType(new List<sbyte>(cards), out highlightCards);


                Log.Debug($"cardType={cardType},highlightCards={highlightCards.Count}");
                foreach (sbyte card in highlightCards)
                {
                    Log.Debug($"{card}");
                }
            }
        }

        //奥马哈牌型测试代码
        public static void RunOmahaTest()
        {      
            sbyte[] handCards = new sbyte[] { 51, 24, 38, 6 };
            sbyte[] publicCards = new sbyte[] { 0, 30, 17, 19, 11};
            //获取奥马哈牌型示例
            List<sbyte> highlightCards;
            CardType cardType = CardTypeUtil.GetOmahaCardType(new List<sbyte>(handCards), new List<sbyte>(publicCards), out highlightCards);

            //Debug.Log($"cardType={cardType},highlightCards={highlightCards.Count}");
            foreach (sbyte card in highlightCards)
            {
                 //Debug.Log($"{card}");
            }
        }


        //获取普通局牌型
        public static CardType GetCardType(List<sbyte> cards, out List<sbyte> highlightCards)
        {
            List<sbyte> mCards = new List<sbyte>();
            mCards.AddRange(cards);

            if (mCards.Count < 7)
            {
                //不够7张牌，先补齐
                for (int i = mCards.Count; i < 7; i++)
                {
                    mCards.Add(-1);
                }
            }
            highlightCards = new List<sbyte>();
            int carTypeNum = 100;
            int lastMin = 100;
            List<sbyte> card = new List<sbyte>(new sbyte[5]);
            //7选5
            for (int i = 0; i < 3; i++)
            {
                card[0] = mCards[i];
                for (int j = i + 1; j < 4; j++)
                {
                    card[1] = mCards[j];
                    for (int a = j + 1; a < 5; a++)
                    {
                        card[2] = mCards[a];
                        for (int b = a + 1; b < 6; b++)
                        {
                            card[3] = mCards[b];
                            for (int c = b + 1; c < 7; c++)
                            {
                                card[4] = mCards[c];

                                CardTypeUtil.CompareCardType(card, ref carTypeNum, ref lastMin, ref highlightCards);
                            }
                        }
                    }
                }
            }
            return (CardType)carTypeNum;
        }

        //获取奥马哈牌型
        public static CardType GetOmahaCardType(List<sbyte> handCard, List<sbyte> publicCard, out List<sbyte> highlightCards)
        {
            List<sbyte> mHandCard = new List<sbyte>();
            mHandCard.AddRange(handCard);
            List<sbyte> mPublicCard = new List<sbyte>();
            mPublicCard.AddRange(publicCard);

            if (mPublicCard.Count < 5)
            {
                //公共牌不够5张牌，先补齐
                for (int i = mPublicCard.Count; i < 5; i++)
                {
                    mPublicCard.Add(-1);
                }
            }
            highlightCards = new List<sbyte>();
            int carTypeNum = 100;
            int lastMin = 100;
            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 4; j++)
                {
                    List<sbyte> card = new List<sbyte>(new sbyte[5]);
                    //手牌挑两张
                    card[0] = mHandCard[i];
                    card[1] = mHandCard[j];

                    //公共牌挑3张
                    for (int a = 0; a < 5 - 2; a++)
                    {
                        for (int b = a + 1; b < 5 - 1; b++)
                        {
                            for (int c = b + 1; c < 5; c++)
                            {
                                card[2] = mPublicCard[a];
                                card[3] = mPublicCard[b];
                                card[4] = mPublicCard[c];

                                //Debug.Log($"cur={card[0]},{card[1]},{card[2]},{card[3]},{card[4]}");
                                CardTypeUtil.CompareCardType(card, ref carTypeNum, ref lastMin, ref highlightCards);
                            }
                        }
                    }
                }
            }
            return (CardType)carTypeNum;
        }

        public static string GetCardTypeName(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.RoyalFlush:
                    return CPErrorCode.LanguageDescription(10053);
                case CardType.StraightFlush:
                    return CPErrorCode.LanguageDescription(10054);
                case CardType.FourOfAKind:
                    return CPErrorCode.LanguageDescription(10055);
                case CardType.FullHouse:
                    return CPErrorCode.LanguageDescription(10056);
                case CardType.Flush:
                    return CPErrorCode.LanguageDescription(10057);
                case CardType.Straight:
                    return CPErrorCode.LanguageDescription(10058);
                case CardType.ThreeOfAKind:
                    return CPErrorCode.LanguageDescription(10059);
                case CardType.TwoPair:
                    return CPErrorCode.LanguageDescription(10060);
                case CardType.OnePair:
                    return CPErrorCode.LanguageDescription(10061);
                case CardType.None:
                    return CPErrorCode.LanguageDescription(10062);
                default:
                    return "";
            }
        }

        //根据牌型代号获取牌型名称
        public static string GetCardTypeName(int cardTypeNum)
        {
            CardType cardType = (CardType)cardTypeNum;
            return GetCardTypeName(cardType);
        }

        public static void CompareCardType(List<sbyte> card, ref int carTypeNum, ref int lastMin, ref List<sbyte> highlightCards)
        {
            CardType cardType = CardTypeUtil.GetExactCardType(card);
            int num = (int)cardType;
            if (num < carTypeNum)
            {
                carTypeNum = num;

                //记录不同牌型需要记录的来比较
                if (num == 2 || num == 6)
                {
                    //同花顺或者顺子
                    for (int i = 0; i < 5; i++)
                    {
                        if ((card[i] % 13) < lastMin)
                        {
                            //取牌型最小一张
                            lastMin = card[i] % 13;
                        }
                    }
                    if (lastMin == 0)
                    {
                        //有2要判断是否有1，因为1到5最小
                        for (int i = 0; i < 5; i++)
                        {
                            if ((card[i] % 13) == 12)
                            {
                                //1到5
                                lastMin = -1;
                            }
                        }
                    }
                }
                else if (num == 3)
                {
                    //四条
                    for (int i = 0; i < 4; i++)
                    {//四条不可能遍历完
                        if ((card[i] % 13) == (card[i + 1] % 13))
                        {
                            //取四条牌型
                            lastMin = card[i] % 13;
                            break;
                        }
                    }
                }
                else if (num == 4)
                {
                    //葫芦
                    lastMin = card[0] % 13 + card[1] % 13 + card[2] % 13 + card[3] % 13 + card[4] % 13;
                }
                else if (num == 5)
                {
                    //同花
                    lastMin = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        lastMin += (card[i] % 13);
                    }
                }
                else if (num == 7 || num == 9)
                {
                    //三条或者一对(手牌两对才可能平行变三条或者一对)
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = i + 1; j < 5; j++)
                        {
                            if ((card[i] % 13) == (card[j] % 13))
                            {
                                //取牌型
                                lastMin = card[i] % 13;
                            }
                        }
                    }
                }
                else if (num == 8)
                {
                    //两对
                    int first = -1;
                    int second = -1;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = i + 1; j < 5; j++)
                        {
                            if ((card[i] % 13) == (card[j] % 13))
                            {
                                //取两个对子牌型
                                if (first == -1)
                                {
                                    first = card[i] % 13;
                                }
                                else
                                {
                                    second = card[i] % 13;
                                }
                            }
                        }
                    }
                    int maxOne = Mathf.Max(first, second);
                    int minOne = Mathf.Min(first, second);
                    //两对判断大小，以大牌的大小优先，所以乘一个加权
                    lastMin = maxOne * 20 + minOne;
                }
                highlightCards = CardTypeUtil.GetHighlightCard(card, (CardType)num);
            }
            else if (num == carTypeNum)
            {
                //牌型相同时候部分牌型需要比较大小
                if (num == 2 || num == 6)
                {
                    //同花顺或者顺子
                    bool bigger = true;
                    for (int i = 0; i < 5; i++)
                    {
                        if ((card[i] % 13) < lastMin)
                        {
                            //有一张小于就是小于
                            bigger = false;
                        }
                    }
                    if (bigger)
                    {
                        lastMin = 100;
                        for (int i = 0; i < 5; i++)
                        {
                            if ((card[i] % 13) < lastMin)
                            {
                                //取新的牌型最小一张
                                lastMin = card[i] % 13;
                            }
                        }
                        if (lastMin == 0)
                        {
                            //有2要判断是否有1，因为1到5最小
                            for (int i = 0; i < 5; i++)
                            {
                                if ((card[i] % 13) == 12)
                                {
                                    //1到5
                                    lastMin = -1;
                                }
                            }
                        }
                        //显示更大
                        highlightCards = CardTypeUtil.GetHighlightCard(card, (CardType)num);
                    }
                }
                else if (num == 3)
                {
                    //四条
                    int currentMin = 0;
                    for (int i = 0; i < 4; i++)
                    {//四条不可能遍历完
                        if ((card[i] % 13) == (card[i + 1] % 13))
                        {
                            //取四条牌型
                            currentMin = card[i] % 13;
                            break;
                        }
                    }
                    if (currentMin > lastMin)
                    {
                        lastMin = currentMin;
                        //显示更大
                        highlightCards = CardTypeUtil.GetHighlightCard(card, (CardType)num);
                    }
                }
                else if (num == 4)
                {
                    //葫芦
                    int newNum = card[0] % 13 + card[1] % 13 + card[2] % 13 + card[3] % 13 + card[4] % 13;
                    if (lastMin < newNum)
                    {
                        lastMin = newNum;
                        //显示更大
                        highlightCards = CardTypeUtil.GetHighlightCard(card, (CardType)num);
                    }
                }
                else if (num == 5)
                {
                    //同花
                    int currentMin = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        currentMin += (card[i] % 13);
                    }
                    if (currentMin > lastMin)
                    {
                        lastMin = currentMin;
                        //显示更大
                        highlightCards = CardTypeUtil.GetHighlightCard(card, (CardType)num);
                    }
                }
                else if (num == 7 || num == 9)
                {
                    //三条或者一对
                    int currentMin = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = i + 1; j < 5; j++)
                        {
                            if ((card[i] % 13) == (card[j] % 13))
                            {
                                //取牌型
                                currentMin = card[i] % 13;
                            }
                        }
                    }
                    if (currentMin > lastMin)
                    {
                        lastMin = currentMin;
                        //显示更大
                        highlightCards = CardTypeUtil.GetHighlightCard(card, (CardType)num);
                    }
                }
                else if (num == 8)
                {
                    //两对
                    int first = -1;
                    int second = -1;
                    int currentMin = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = i + 1; j < 5; j++)
                        {
                            if ((card[i] % 13) == (card[j] % 13))
                            {
                                //取两个对子牌型
                                if (first == -1)
                                {
                                    first = card[i] % 13;
                                }
                                else
                                {
                                    second = card[i] % 13;
                                }
                            }
                        }
                    }
                    int maxOne = Mathf.Max(first, second);
                    int minOne = Mathf.Min(first, second);
                    //两对判断大小，以大牌的大小优先，所以乘一个加权
                    currentMin = maxOne * 20 + minOne;
                    if (currentMin > lastMin)
                    {
                        lastMin = currentMin;
                        //显示更大
                        highlightCards = CardTypeUtil.GetHighlightCard(card, (CardType)num);
                    }
                }
            }
        }

        //5张确定牌
        private static CardType GetExactCardType(List<sbyte> cards)
        {
            if (cards.Count < 5)
            {
                return CardType.None;
            }
            CardType cardType = CardType.None;
            List<sbyte> CardColor = new List<sbyte>(new sbyte[5]);
            List<sbyte> CardNumber = new List<sbyte>(new sbyte[5]);
            List<sbyte> array = new List<sbyte>(new sbyte[5]);
            for (int i = 0; i < 5; i++)
            {
                if (cards[i] == -1)
                {
                    //空的公共牌处理
                    return CardType.None;
                }
                CardColor[i] = (sbyte)((cards[i] / 13) + 1);
                CardNumber[i] = (sbyte)(cards[i] % 13);
            }

            bool bsameColorOverFive = false;
            bsameColorOverFive = true;
            for (int i = 0, n = 4; i < n; i++)
            {
                if (CardColor[i] != CardColor[i + 1])
                {
                    bsameColorOverFive = false;
                    break;
                }
            }

            if (bsameColorOverFive)
            {
                for (int i = 0, n = array.Count; i < n; i++)
                {
                    array[i] = CardNumber[i];
                }
            }

            if (bsameColorOverFive)
            {
                List<sbyte> arraySorted = new List<sbyte>(array.ToArray());
                arraySorted.Sort();
                if (CardTypeUtil.isShunzi(arraySorted, array))
                {
                    if (array[0] == 12 && array[1] == 11)
                    {
                        cardType = CardType.RoyalFlush;
                    }
                    else
                    {
                        cardType = CardType.StraightFlush;
                    }
                }
                else

                {
                    cardType = CardType.Flush;
                }

            }
            else
            {

                array.Clear();
                for (int i = 0; i < 5; i++)
                {
                    array.Add(CardNumber[i]);
                }
                List<sbyte> arrInput = new List<sbyte>(array.ToArray());
                arrInput.Sort();
                if (CardTypeUtil.isShunzi(arrInput, null))
                {
                    cardType = CardType.Straight;
                }
                else
                {
                    int type = CardTypeUtil.getTempCardType(array);
                    if (type != -1)
                    {
                        // 0  hulu  1 duizi 2  santiao  3 liangdui  -1 gaopai  4 sitiao
                        switch (type)
                        {
                            case 0:
                                cardType = CardType.FullHouse;
                                break;
                            case 1:
                                cardType = CardType.OnePair;
                                break;
                            case 2:
                                cardType = CardType.ThreeOfAKind;
                                break;
                            case 3:
                                cardType = CardType.TwoPair;
                                break;
                            case 4:
                                cardType = CardType.FourOfAKind;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        cardType = CardType.None;
                    }
                }
            }

            return cardType;
        }

        private static bool isShunzi(List<sbyte> array, List<sbyte> arrayStoreShunzi)
        {
            int count = 0;
            if (arrayStoreShunzi == null)
            {
                arrayStoreShunzi = new List<sbyte>();
            }
            arrayStoreShunzi.Clear();
            for (int i = array.Count - 1; i > 0; i--)
            {
                if (array[i] - 1 == array[i - 1])
                {
                    count++;
                    arrayStoreShunzi.Add(array[i]);
                }
                else
                {
                    count = 0;
                    arrayStoreShunzi.Clear();
                }
                if (count >= 4)
                {
                    arrayStoreShunzi.Add(array[i - 1]);
                    return true;
                }
            }
            if (count == 3)
            {
                if (arrayStoreShunzi[0] == 3)
                {
                    foreach (sbyte number in array)
                    {
                        if (number == 12)
                        {
                            return true;
                        }
                    }
                }
            }
            if (GameCache.Instance.room_path == 21 || GameCache.Instance.room_path == 23)//短牌A当5用
            {
                if (count == 3)
                {
                    if (arrayStoreShunzi[0] == 7)
                    {
                        foreach (sbyte number in array)
                        {
                            if (number == 12)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            
            return false;
        }

        private static int getTempCardType(List<sbyte> array)
        {
            List<sbyte> arrInfo = new List<sbyte>(array.ToArray());
            int countOfDuiZi = 0;
            int countOfThree = 0;
            int countOfFour = 0;
            for (int i = 0; i < arrInfo.Count; i++)
            {
                int count = 0;
                int value = arrInfo[i];
                for (int j = 0; j < arrInfo.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (value == arrInfo[j])
                    {
                        count++;
                        arrInfo.RemoveAt(j);
                        j--;
                        if (arrInfo.Count <= 1)
                        {
                            break;
                        }
                    }
                }
                arrInfo.RemoveAt(i);
                i--;
                if (count == 2)
                {
                    countOfThree++;
                }
                if (count == 1)
                {
                    countOfDuiZi++;
                }
                if (count == 3)
                {
                    countOfFour++;
                }
                if (arrInfo.Count <= 1)
                {
                    break;
                }
            }

            if (countOfFour >= 1)
            {
                return 4;
            }
            if ((countOfDuiZi >= 1 && countOfThree >= 1) || countOfThree == 2)
            {
                return 0;
            }
            if (countOfThree >= 1)
            {
                return 2;
            }
            if (countOfDuiZi >= 2)
            {
                return 3;
            }
            if (countOfDuiZi == 1)
            {
                return 1;
            }

            return -1;

        }


        private static List<sbyte> GetHighlightCard(List<sbyte> cards, CardType cardType)
        {
            List<sbyte> highlightCard = new List<sbyte>();
            if (cardType == CardType.RoyalFlush
             || cardType == CardType.StraightFlush
             || cardType == CardType.FullHouse
             || cardType == CardType.Flush
             || cardType == CardType.Straight)
            {
                //5张都高亮
                highlightCard.AddRange(cards);
            }
            else
            {
                //找相同牌高亮
                List<sbyte> arrInfo = new List<sbyte>(cards.ToArray());
                for (int i = 0; i < arrInfo.Count; i++)
                {
                    sbyte valueCard = arrInfo[i];
                    sbyte valueCardNum = (sbyte)(valueCard % 13);
                    for (int j = 0; j < arrInfo.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }
                        sbyte comparedCard = arrInfo[j];
                        sbyte comparedCardNum = (sbyte)(comparedCard % 13);
                        if (valueCardNum == comparedCardNum && valueCardNum != -1)
                        {
                            if (!highlightCard.Contains(valueCard))
                            {
                                highlightCard.Add(valueCard);
                            }
                            if (!highlightCard.Contains(comparedCard))
                            {
                                highlightCard.Add(comparedCard);
                            }
                            arrInfo.RemoveAt(j);
                            j--;
                            if (arrInfo.Count <= 1)
                            {
                                break;
                            }
                        }
                    }
                    arrInfo.RemoveAt(i);
                    i--;
                    if (arrInfo.Count <= 1)
                    {
                        break;
                    }
                }
            }

            return highlightCard;
        }

    }
}
