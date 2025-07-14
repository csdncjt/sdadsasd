using System;
using System.Collections.Generic;
using BestHTTP;
using ETModel;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace ETHotfix
{
    public enum MTT_GameType
    {
        MTT_NORMALGAME = 0,
        MTT_QUICKGAME = 1,
    }

    public class MTTGameUtil
    {
        static private int[] blindTable = { };
        static private int[] anteTable = { };

        //static private int[] blindTableQuick = {1000,2000,4000,8000,12000,16000,20000,32000,40000,60000,80000,120000,160000,200000,320000,400000,600000,800000,1200000,1600000,2000000,3200000,4000000,6000000,8000000,12000000,16000000,20000000,32000000,40000000};
        //static private int[] anteTableQuick = {0,0,0,0,0,0,4000,8000,12000,20000,20000,40000,60000,60000,80000, 120000, 200000, 200000, 400000, 600000, 600000, 800000, 1200000, 2000000, 2000000, 4000000, 6000000, 6000000, 8000000, 12000000 };
        //static private int[] blindTableNormal = {1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 10000, 12000, 16000, 20000, 24000, 32000, 40000, 48000, 60000, 80000, 100000, 120000, 160000, 200000, 240000, 320000, 400000, 480000, 600000, 800000, 1200000, 1600000, 2000000, 2400000, 3200000, 4000000, 6000000, 8000000, 12000000, 16000000, 20000000, 24000000, 32000000, 40000000 };
        //static private int[] anteTableNormal = {0,0,0,0, 1000, 1000, 2000, 2000, 3000, 4000, 5000, 6000, 8000, 10000, 12000, 16000, 20000, 24000, 32000, 40000, 52000, 64000, 80000, 100000, 120000, 160000, 200000, 260000, 400000, 520000, 600000, 800000, 1000000, 1200000, 2000000, 2400000, 3600000, 4800000, 6000000, 7200000, 9600000, 12000000 };

        private static int[] BlindTable(MTT_GameType gameType)
        {
            return blindTable;
            //switch (gameType)
            //{
            //    case MTT_GameType.MTT_QUICKGAME:
            //        return blindTableQuick;
            //    case MTT_GameType.MTT_NORMALGAME:
            //        return blindTableNormal;
            //}
            //return new int[] { };
        }

        private static int[] AnteTable(MTT_GameType gameType)
        {
            return anteTable;
            //switch (gameType)
            //{
            //    case MTT_GameType.MTT_QUICKGAME:
            //        return anteTableQuick;
            //    case MTT_GameType.MTT_NORMALGAME:
            //        return anteTableNormal;
            //}
            //return new int[] { };
        }

        public static int BlindAtLevel(int level, MTT_GameType gameType, float scale)
        {
            return (int)(BlindTable(gameType)[level] * scale);
        }

        public static int AnteAtLevel(int level, MTT_GameType gameType, float scale)
        {
            return (int)(AnteTable(gameType)[level] * scale);
        }

        public static int LeveForBlind(int blind, MTT_GameType gameType, float scale)
        {
            return Array.IndexOf(BlindTable(gameType), (int)(blind / scale));
        }

        public static int numOfLevel(MTT_GameType gameType)
        {
            return BlindTable(gameType).Length;
        }

        public static void initList(List<int> blinds, List<int> ante)
        {
            blindTable = blinds.ToArray();
            anteTable = ante.ToArray();
        }

    }
}
