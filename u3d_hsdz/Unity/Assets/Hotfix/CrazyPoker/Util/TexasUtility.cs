using System.Collections.Generic;

namespace ETHotfix
{
    public class TexasUtility
    {
        // 方块(Diamond) : 0 1 2 3 4 5 6 7 8 9 10 11 12  方块2-10 J Q K A
        // 梅花(Club) : 13 14 15 16 17 18 19 20 21 22 23 24 25  梅花2-10 J Q K A
        // 红桃(Heart) : 26 27 28 29 30 31 32 33 34 35 36 37 38  红桃2-10 J Q K A
        // 黑桃(Spade) : 39 40 41 42 43 44 45 46 47 48 49 50 51  黑桃2-10 J Q K A
        // 王(Joker) : 52 53 大小王

        public static int changeIdToHex(int id)
        {
            int kind = id / 13;
            int point = id % 13;

            switch (kind)
            {
                case 0:
                    kind = 0x00;
                    break;
                case 1:
                    kind = 0x10;
                    break;
                case 2:
                    kind = 0x20;
                    break;
                case 3:
                    kind = 0x30;
                    break;
                case 4:
                    kind = 0x40;
                    break;
            }

            if (point == 12)
                point = -1;

            point += 2;

            return kind + point;
        }

        public static PokerUtility.PokerType getPokerType(sbyte[] cards, out List<sbyte> outCards)
        {
            outCards = new List<sbyte>();

            if (null == cards)
                return PokerUtility.PokerType.t_none;

            int len = cards.Length;
            if (len == 0)
                return PokerUtility.PokerType.t_none;
            if (len == 1)
                return PokerUtility.PokerType.t_1;
            if(len > 7)
                return PokerUtility.PokerType.t_none;

            if (len > 5)
                len = 5;

            PokerUtility.PokerType t = PokerUtility.PokerType.t_none;
            List<sbyte[]> list = MathUtility.GetCombination(cards, len);
            List<byte> inlist = new List<byte>();
            Dictionary<PokerUtility.PokerType, List<List<sbyte>>> dic = new Dictionary<PokerUtility.PokerType, List<List<sbyte>>>();
            for (int i = 0, n = list.Count; i < n; i++)
            {
                inlist.Clear();
                for (int j = 0, m = list[i].Length; j < m; j++)
                {
                    inlist.Add((byte)changeIdToHex(list[i][j]));
                }
                t = PokerUtility.getType(inlist);
                if (t != PokerUtility.PokerType.t_none)
                {
                    if (dic.ContainsKey(t))
                    {
                        dic[t].Add(new List<sbyte>(list[i]));
                    }
                }
            }

            if (dic.Count > 0)
            {
                List<sbyte> mCacheMax = null;
                foreach (KeyValuePair<PokerUtility.PokerType, List<List<sbyte>>> keyValuePair in dic)
                {
                    switch (keyValuePair.Key)
                    {
                        case PokerUtility.PokerType.t_1xn:
                            foreach (List<sbyte> sbytes in keyValuePair.Value)
                            {
                                sbytes.Sort();
                                if (null == mCacheMax)
                                {
                                    mCacheMax = sbytes;
                                    continue;
                                }

                                int mPoint0 = mCacheMax[4] % 13;
                                int mPoint1 = sbytes[4] % 13;
                                if (mPoint0 != 12 && mPoint1 != 12)
                                {
                                    if (sbytes[0] > mCacheMax[0])
                                    {
                                        mCacheMax = sbytes;
                                    }
                                }
                                else
                                {
                                    if (mPoint0 == 12 && mPoint1 == 12)
                                    {

                                    }
                                    else if (mPoint0 == 12 && mPoint1 != 12)
                                    {
                                        if (mCacheMax[0] % 13 == 0)
                                        {
                                            mCacheMax = sbytes;
                                        }
                                    }
                                    else if (mPoint0 != 12 && mPoint1 == 12)
                                    {
                                        if (sbytes[0] % 13 != 0)
                                        {
                                            mCacheMax = sbytes;
                                        }
                                    }
                                }
                            }
                            break;
                        case PokerUtility.PokerType.t_41:
                            foreach (List<sbyte> sbytes in keyValuePair.Value)
                            {
                                sbytes.Sort();
                                if (null == mCacheMax)
                                {
                                    mCacheMax = sbytes;
                                    continue;
                                }

                                int m40 = -1;
                                int mMaxId0 = -1;
                                for (int i = 0, n = mCacheMax.Count-2; i < n; i++)
                                {
                                    int mPoint = mCacheMax[i] % 13;
                                    int mNextPoint = mCacheMax[i + 1] % 13;
                                    int mNextNextPoint = mCacheMax[i + 2] % 13;
                                    if (mPoint == mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        continue;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        m40 = mCacheMax[i + 1];
                                        mMaxId0 = mCacheMax[i];
                                        break;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m40 = mCacheMax[i];
                                        mMaxId0 = mCacheMax[i + 1];
                                        break;
                                    }

                                    if (mPoint == mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m40 = mCacheMax[i];
                                        mMaxId0 = mCacheMax[i + 2];
                                        break;
                                    }
                                }

                                int m41 = -1;
                                int mMaxId1 = -1;
                                for (int i = 0, n = sbytes.Count - 2; i < n; i++)
                                {
                                    int mPoint = sbytes[i] % 13;
                                    int mNextPoint = sbytes[i + 1] % 13;
                                    int mNextNextPoint = sbytes[i + 2] % 13;
                                    if (mPoint == mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        continue;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        m41 = sbytes[i + 1];
                                        mMaxId1 = sbytes[i];
                                        break;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m41 = sbytes[i];
                                        mMaxId1 = sbytes[i + 1];
                                        break;
                                    }

                                    if (mPoint == mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m41 = sbytes[i];
                                        mMaxId1 = sbytes[i + 2];
                                        break;
                                    } 
                                }
                                int m4Point0 = m40 % 13;
                                int mMaxIdType0 = mMaxId0 / 13;
                                int mMaxIdPoint0 = mMaxId0 % 13;
                                int m4Point1 = m41 % 13;
                                int mMaxIdType1 = mMaxId1 / 13;
                                int mMaxIdPoint1 = mMaxId1 % 13;
                                if (m4Point0 > m4Point1)
                                    continue;
                                if (m4Point0 == m4Point1 && mMaxIdPoint0 > mMaxIdPoint1)
                                    continue;
                                if(m4Point0 == m4Point1 && mMaxIdPoint0 == mMaxIdPoint1 && mMaxIdType0 > mMaxIdType1)
                                    continue;

                                mCacheMax = sbytes;
                            }
                            break;
                        case PokerUtility.PokerType.t_32:
                            foreach (List<sbyte> sbytes in keyValuePair.Value)
                            {
                                sbytes.Sort();
                                if (null == mCacheMax)
                                {
                                    mCacheMax = sbytes;
                                    continue;
                                }

                                int m30 = -1;
                                int m20 = -1;
                                for (int i = 0, n = mCacheMax.Count - 2; i < n; i++)
                                {
                                    int mPoint = mCacheMax[i] % 13;
                                    int mNextPoint = mCacheMax[i + 1] % 13;
                                    int mNextNextPoint = mCacheMax[i + 2] % 13;
                                    if (mPoint == mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        continue;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        m30 = mCacheMax[i + 1];
                                        m20 = mCacheMax[i];
                                        break;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m30 = mCacheMax[i];
                                        m20 = mCacheMax[i + 1];
                                        break;
                                    }

                                    if (mPoint == mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m30 = mCacheMax[i];
                                        m20 = mCacheMax[i + 2];
                                        break;
                                    }
                                }

                                int m31 = -1;
                                int m21 = -1;
                                for (int i = 0, n = sbytes.Count - 2; i < n; i++)
                                {
                                    int mPoint = sbytes[i] % 13;
                                    int mNextPoint = sbytes[i + 1] % 13;
                                    int mNextNextPoint = sbytes[i + 2] % 13;
                                    if (mPoint == mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        continue;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint == mNextNextPoint)
                                    {
                                        m31 = sbytes[i + 1];
                                        m21 = sbytes[i];
                                        break;
                                    }

                                    if (mPoint != mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m31 = sbytes[i];
                                        m21 = sbytes[i + 1];
                                        break;
                                    }

                                    if (mPoint == mNextPoint && mNextPoint != mNextNextPoint)
                                    {
                                        m31 = sbytes[i];
                                        m21 = sbytes[i + 2];
                                        break;
                                    }
                                }
                                int m4Point0 = m30 % 13;
                                int mMaxIdType0 = m20 / 13;
                                int mMaxIdPoint0 = m20 % 13;
                                int m4Point1 = m31 % 13;
                                int mMaxIdType1 = m21 / 13;
                                int mMaxIdPoint1 = m21 % 13;
                                if (m4Point0 > m4Point1)
                                    continue;
                                if (m4Point0 == m4Point1 && mMaxIdPoint0 > mMaxIdPoint1)
                                    continue;
                                if (m4Point0 == m4Point1 && mMaxIdPoint0 == mMaxIdPoint1 && mMaxIdType0 > mMaxIdType1)
                                    continue;

                                mCacheMax = sbytes;
                            }
                                break;
                        case PokerUtility.PokerType.t_1x:
                            foreach (List<sbyte> sbytes in keyValuePair.Value)
                            {
                                sbytes.Sort();
                                if (null == mCacheMax)
                                {
                                    mCacheMax = sbytes;
                                    continue;
                                }

                                int mMaxPoint0 = 0;
                                int mMaxPoint1 = 0;
                                bool mCanChange = true;
                                for (int i = 0, n = mCacheMax.Count; i < n; i++)
                                {
                                    mMaxPoint0 = mCacheMax[i] % 13;
                                    mMaxPoint1 = sbytes[i] % 13;
                                    if (mMaxPoint0 > mMaxPoint1)
                                    {
                                        mCanChange = false;
                                        break;
                                    }
                                }
                                if(mCanChange)
                                    mCacheMax = sbytes;
                            }
                            break;
                        case PokerUtility.PokerType.t_1n:
                            foreach (List<sbyte> sbytes in keyValuePair.Value)
                            {
                                sbytes.Sort();
                                if (null == mCacheMax)
                                {
                                    mCacheMax = sbytes;
                                    continue;
                                }

                                int mMaxPoint0 = 0;
                                int mMaxType0 = 0;
                                int mMaxPoint1 = 0;
                                int mMaxType1 = 0;
                                bool mCanChange = true;
                                for (int i = 0, n = mCacheMax.Count; i < n; i++)
                                {
                                    mMaxPoint0 = mCacheMax[4] % 13;
                                    mMaxType0 = mCacheMax[4] / 13;

                                    mMaxPoint1 = sbytes[4] % 13;
                                    mMaxType1 = sbytes[4] / 13;

                                    if (mCacheMax[0] % 13 != 0 && sbytes[0] % 13 != 0)
                                    {
                                        if (mMaxPoint0 > mMaxPoint1)
                                        {
                                            mCanChange = false;
                                            break;
                                        }
                                    }
                                    else if (mCacheMax[0] % 13 == 0 && sbytes[0] % 13 != 0)
                                    {
                                        continue;
                                    }
                                }
                                if (mCanChange)
                                    mCacheMax = sbytes;
                            }
                            break;
                    }
                }

                outCards = mCacheMax;
                return t;
            }

            len--;

            if (len > 2)
            {
                // 三条比两对大
                list = MathUtility.GetCombination(cards, 3);
                for (int i = 0, n = list.Count; i < n; i++)
                {
                    inlist.Clear();
                    for (int j = 0, m = list[i].Length; j < m; j++)
                    {
                        inlist.Add((byte)changeIdToHex(list[i][j]));
                    }
                    t = PokerUtility.getType(inlist);
                    if (t != PokerUtility.PokerType.t_none)
                    {
                        if (dic.ContainsKey(t))
                        {
                            dic[t].Add(new List<sbyte>(list[i]));
                        }
                    }
                }
            }

            if (dic.Count == 0)
            {
                while (len > 1)
                {
                    if (len == 3)
                    {
                        len--;
                        continue;
                    }

                    list = MathUtility.GetCombination(cards, len);
                    for (int i = 0, n = list.Count; i < n; i++)
                    {
                        inlist.Clear();
                        for (int j = 0, m = list[i].Length; j < m; j++)
                        {
                            inlist.Add((byte)changeIdToHex(list[i][j]));
                        }
                        t = PokerUtility.getType(inlist);
                        if (t != PokerUtility.PokerType.t_none)
                        {
                            if (dic.ContainsKey(t))
                            {
                                dic[t].Add(new List<sbyte>(list[i]));
                            }
                        }
                    }

                    if (dic.Count > 0)
                        break;

                    len--;
                }
            }

            if (dic.Count > 0)
            {
                List<sbyte> mCacheMax = null;
                foreach (KeyValuePair<PokerUtility.PokerType, List<List<sbyte>>> keyValuePair in dic)
                {
                    switch (keyValuePair.Key)
                    {
                        case PokerUtility.PokerType.t_3:
                            break;
                        case PokerUtility.PokerType.t_22:
                            break;
                        case PokerUtility.PokerType.t_2:
                            break;
                        case PokerUtility.PokerType.t_1:
                            break;
                    }
                }

                outCards = mCacheMax;
                return t;
            }

            return t;
        }
    }
}
