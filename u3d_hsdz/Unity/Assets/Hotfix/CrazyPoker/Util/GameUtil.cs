using System;
using System.Collections.Generic;
using BestHTTP;
using ETModel;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

namespace ETHotfix
{
    public enum RoomPath
    {
        DP = 21,
        DPAof=23,
        PAP = 51,//大菠萝
        Normal = 61,//普通局 德州--
        NormalThanOff = 62,//普通局必下场
        NormalAof = 63,//普通局AOF
        MTT = 71,//锦标赛
        SNG = 81,//坐满即玩  
        Omaha = 91,//奥马哈--
        OmahaThanOff = 92,//奥马哈必下场
        OmahaAof = 93,//奥马哈AOF
    }

    public class GameUtil
    {
        /// <summary>
        /// 获取正整数位数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int IntLength(int num)
        {
            int res = 0;
            while (num > 0)
            {
                num /= 10;
                res++;
            }
            return res;
        }

        public static HTTPRequest HttpsPost(string uri, string json, OnRequestFinishedDelegate onRequestFinishedDelegate)
        {
            HTTPRequest httpRequest = new HTTPRequest(new Uri(uri), HTTPMethods.Post, onRequestFinishedDelegate);
            httpRequest.UseAlternateSSL = true;

            httpRequest.AddHeader("Content-Type", "application/json");
            httpRequest.AddHeader("did", SystemInfoUtil.getDeviceUniqueIdentifier());
            httpRequest.AddHeader("md5at", GameCache.Instance.strToken);
            byte[] bytes = json.ToUtf8();
            string sh2sg = encrypt(bytes);
            httpRequest.AddHeader("sh2sg", sh2sg);
            httpRequest.RawData = bytes;
            
            // Log.Debug(httpRequest.DumpHeaders());
            return httpRequest.Send();
        }

        public static HTTPRequest HttpsImagePost(string uri, byte[] data, OnRequestFinishedDelegate onRequestFinishedDelegate)
        {
            HTTPRequest httpRequest = new HTTPRequest(new Uri(uri), HTTPMethods.Post, onRequestFinishedDelegate);
            httpRequest.UseAlternateSSL = true;
            httpRequest.StreamFragmentSize = 100 * 1024;

            httpRequest.AddHeader("did", SystemInfoUtil.getDeviceUniqueIdentifier());
            httpRequest.AddHeader("md5at", GameCache.Instance.strToken);
            httpRequest.AddField("user_id", $"{GameCache.Instance.nUserId}");
            httpRequest.AddBinaryData("file", data, "picture.png", "image/png");
            // Log.Debug(httpRequest.DumpHeaders());
            return httpRequest.Send();
        }

        const string KEY = "0fea4d0fbeda6195f1cba0ff6413e16887ee9de46613e9092d659a9fe212e273";
        const string HEX_META_ARRAY = "0123456789abcdef";
        /**
         * 将二进制数组转换为十六进制字符串
         * @param bytes 二进制数组
         * @return
         */
        public static string encode(byte[] bytes)
        {
            if (null == bytes || bytes.Length == 0)
                return "";

            StringBuilder result = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                //字节高4位
                result.Append(HEX_META_ARRAY.Substring((bytes[i] & 0xF0) >> 4, 1));
                //字节低4位
                result.Append(HEX_META_ARRAY.Substring(bytes[i] & 0x0F, 1));
            }
            return result.ToString();
        }

        /**
         * 将十六进制转换为二进制字节数组
         * @param hexString 16进制字符串
         * @return
         */
        public static byte[] decode(string hexString)
        {
            if (null == hexString || hexString.Trim().Length == 0)
                return new byte[0];

            if (hexString.Length % 2 != 0)
            {
                Log.Error("Invalid String,it must be format of hex!");
            }

            int len = hexString.Length / 2;
            byte[] bytes = new byte[len];
            int high = 0;//字节高四位
            int low = 0;//字节低四位
            for (int i = 0; i < len; i++)
            {
                high = HEX_META_ARRAY.IndexOf(hexString.Substring(2 * i, 1));
                if (high == -1)
                {
                    Log.Error("Invalid String,it must be format of hex!");
                }

                low = HEX_META_ARRAY.IndexOf(hexString.Substring(2 * i + 1, 1));
                if (low == -1)
                {
                    Log.Error("Invalid String,it must be format of hex!");
                }

                //右移四位得到高位
                high = (byte)(high << 4);
                low = (byte)low;
                //高地位做或运算
                bytes[i] = (byte)(high | low);
            }
            return bytes;
        }

        public static string encrypt(byte[] bytes)
        {

            using (var hmacsha256 = new HMACSHA256())
            {
                hmacsha256.Key = decode(KEY);
                byte[] hashmessage = hmacsha256.ComputeHash(bytes);
                byte[] final = hmacsha256.ComputeHash(bytes);
                return encode(final);
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(string toEncrypt, string key)
        {
            if (key == null || key.Length != 16)
            {
                return "key 不能为空并且需要 16 位长度";
            }
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            // 将 + 替换为 %2B
            string resultArrayAfter = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            resultArrayAfter = resultArrayAfter.Replace("+", "%2B");

            return resultArrayAfter;
        }

        public static string AESDecrypt(string toDecrypt, string key)
        {
            // 将 %2B 替换为 +
            toDecrypt = toDecrypt.Replace("%2B", "+");
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string GetCardNameByNum(sbyte cardNum)
        {
            int baseNum = 0;
            if (cardNum < 0)
            {
                return "poker_88";
            }
            switch (cardNum / 13)
            {
                case 3:
                    baseNum = 1;
                    break;
                case 2:
                    baseNum = 2;
                    break;
                case 1:
                    baseNum = 3;
                    break;
                case 0:
                    baseNum = 4;
                    break;
                default:
                    break;
            }

            int tmp = 0;
            if (cardNum % 13 == 12)
            {
                tmp = 1;
            }
            else
            {
                tmp = cardNum % 13 + 2;
            }
            baseNum = baseNum + (tmp - 1) * 4;
            if (baseNum < 10)
            {
                return $"poker_0{baseNum}";
            }
            else
            {
                return $"poker_{baseNum}";
            }
        }
       

        /**
        *普通局赔率
        *Outs 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16
        *赔 30 16 10 8 6 5 4 3.5 3 2.5 2.2 2 1.8 1.6 1.4 1
        *
        *Omaha赔率
        *Outs 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16
        *赔 24 12 8 6 4.5 4 3.2 2.7 2.3 2 1.7 1.5 1.3 1.2 1.1 1
        */
        private static readonly float[] normalOuts = new[] { 0, 30, 16, 10, 8, 6, 5, 4, 3.5f, 3, 2.5f, 2.2f, 2, 1.8f, 1.6f, 1.4f, 1.3f };
        private static readonly float[] omahaOuts = new[] { 0, 24, 12, 8, 6, 4.5f, 4, 3.2f, 2.7f, 2.3f, 2, 1.7f, 1.5f, 1.3f, 1.2f, 1.1f, 1 };
        private static readonly float[] shortOuts = new[] { 0, 20, 9, 5.5f, 4, 3.2f, 2.5f, 2f, 1.6f, 1.4f, 1.3f, 1.2f, 1.1f, 1};

        public static float GetNormalOuts(int selectOuts)
        {
            if (selectOuts >= normalOuts.Length)
                return 0;
            return normalOuts[selectOuts];
        }

        public static float GetOmahaOuts(int selectOuts)
        {
            if (selectOuts >= omahaOuts.Length)
                return 0;
            return omahaOuts[selectOuts];
        }

        public static float GetShortOuts(int selectOuts)
        {
            if (selectOuts >= shortOuts.Length)
                return 0;
            return shortOuts[selectOuts];
        }

        /// <summary>
        /// 源变换本地坐标转化为目标变换本地坐标
        /// </summary>
        /// <param name="sourceLocalPos">源本地坐标</param>
        /// <param name="sourceTransform">源变换</param>
        /// <param name="targetTransform">目标变换</param>
        /// <returns>目标本地坐标</returns>
        public static Vector3 ChangeToLocalPos(Vector3 sourceLocalPos, Transform sourceTransform, Transform targetTransform)
        {
            return targetTransform.InverseTransformPoint(sourceTransform.TransformPoint(sourceLocalPos));
        }

        /// <summary>
        /// 源变换本地坐标转化为目标变换本地坐标
        /// </summary>
        /// <param name="sourceTransform">源变换</param>
        /// <param name="targetTransform">目标变换</param>
        /// <returns></returns>
        public static Vector3 ChangeToLocalPos(Transform sourceTransform, Transform targetTransform)
        {
            return targetTransform.parent.InverseTransformPoint(sourceTransform.parent.TransformPoint(sourceTransform.localPosition));
        }

        /// <summary>
        /// 获取筹码SpriteName
        /// </summary>
        /// <param name="chip"></param>
        /// <returns></returns>
        public static string GetChipSpriteName(int chip)
        {
            string mChipSpriteName = string.Empty;
            if (chip >= 1 && chip <= 19)
            {
                mChipSpriteName = "cm_huang";
            }
            else if (chip >= 20 && chip <= 49)
            {
                mChipSpriteName = "cm_orange";
            }
            else if (chip >= 50 && chip <= 99)
            {
                mChipSpriteName = "cm_green";
            }
            else if (chip >= 100 && chip <= 499)
            {
                mChipSpriteName = "cm_blue";
            }
            else if (chip >= 500 && chip <= 999)
            {
                mChipSpriteName = "cm_red";
            }
            else if (chip >= 1000 && chip <= 4999)
            {
                mChipSpriteName = "cm_gray";
            }
            else if (chip >= 5000 && chip <= 9999)
            {
                mChipSpriteName = "cm_purple";
            }
            else if (chip >= 10000 && chip <= 49999)
            {
                mChipSpriteName = "cm_brown";
            }
            else if (chip >= 50000)
            {
                mChipSpriteName = "cm_black";
            }
            else
            {
                mChipSpriteName = "cm_huang";
            }

            return mChipSpriteName;
        }

        /// <summary>
        /// 查看公共牌花费
        /// </summary>
        /// <param name="small"></param>
        /// <returns></returns>
        public static int GetSeeMoreCost(int small)
        {
            //1./50
            //2./20
            //3./10

            int result = (int)(small * 2 / 50);
            if (result == 0)
                return 1;
            return result;
        }

        #region 牌局内座位UI信息   
        // 0中下、1左下、2左中下、3左中、4左中上、5左上、6中上偏左、7中上、8中上偏右、9右上、10右中上、11右中、12右中下、13右下
        public static readonly Vector3[] SeatPosV3 = new Vector3[]
        {
                new Vector3(0, -932),
                new Vector3(-516, -427),
                new Vector3(-516, -250),
                new Vector3(-516, 0),
                new Vector3(-516, 340),
                new Vector3(-516, 427),
                new Vector3(-212, 856),
                new Vector3(0, 878),
                new Vector3(214, 856),
                new Vector3(512, 427),
                new Vector3(512, 340),
                new Vector3(512, 0),
                new Vector3(512, -250),
                new Vector3(512, -427),
        };

        // Dealer标识坐标 0左、1右
        public static readonly Vector3[] BankerLRV3 = new Vector3[]
        {
                new Vector3(-64, -64),
                new Vector3(64, -64),
        };

        // 牌背面坐标 0中下、1左下、2左中下、3左中、4左中上、5左上、6中上偏左、7中上、8中上偏右、9右上、10右中上、11右中、12右中下、13右下
        public static readonly Vector3[] CardBackLRV3 = new Vector3[]
        {
                new Vector3(136, 130),
                new Vector3(132, -30),
                new Vector3(132, -30),
                new Vector3(120, -72),
                new Vector3(120, -72),
                new Vector3(132, -64),
                new Vector3(110, -94),
                new Vector3(110, -94),
                new Vector3(-106, -96),
                new Vector3(-124, -68),
                new Vector3(-124, -76),
                new Vector3(-124, -76),
                new Vector3(-124, -32),
                new Vector3(-124, -32),
        };

        // 牌背面旋转 0中下、1左下、2左中下、3左中、4左中上、5左上、6中上偏左、7中上、8中上偏右、9右上、10右中上、11右中、12右中下、13右下
        public static readonly Vector3[] CardBackRotLRV3 = new Vector3[]
        {
                new Vector3(0, 0, 45),
                new Vector3(0, 0, -30),
                new Vector3(0, 0, -30),
                new Vector3(0, 0, -30),
                new Vector3(0, 0, -30),
                new Vector3(0, 0, -30),
                new Vector3(0, 0, -120),
                new Vector3(0, 0, -120),
                new Vector3(0, 0, 140),
                new Vector3(0, 0, 45),
                new Vector3(0, 0, 45),
                new Vector3(0, 0, 45),
                new Vector3(0, 0, 45),
                new Vector3(0, 0, 45),
        };

        // 牌坐标 0中下、1左下、2左中下、3左中、4左中上、5左上、6中上偏左、7中上、8中上偏右、9右上、10右中上、11右中、12右中下、13右下
        public static readonly Vector3[] CardsPosLRV3 = new Vector3[]
        {
                new Vector3(187, -20),
                new Vector3(155, -22),
                new Vector3(155, -22),
                new Vector3(155, 20),
                new Vector3(155, -77),
                new Vector3(155, -77),
                new Vector3(-155, 0),
                new Vector3(155, 0),
                new Vector3(155, 0),
                new Vector3(-155, -77),
                new Vector3(-155, -77),
                new Vector3(-155, 20),
                new Vector3(-155, -22),
                new Vector3(-155, -22),
        };

        // 本手已下注坐标 0中下、1左下、2左中下、3左中、4左中上、5左上、6中上偏左、7中上、8中上偏右、9右上、10右中上、11右中、12右中下、13右下
        public static readonly Vector3[] CurRoundHaveBetPosLRV3 = new Vector3[]
        {
                new Vector3(0, 182),
                new Vector3(124, 150),//70
                new Vector3(124, 70),
                new Vector3(124, -77),
                new Vector3(124, 28),
                new Vector3(124, 28),
                new Vector3(0, -167),
                new Vector3(0, -167),
                new Vector3(0, -167),
                new Vector3(-128, 28),
                new Vector3(-128, 28),
                new Vector3(-128, -77),
                new Vector3(-128, 70),
                new Vector3(-128, 150),
        };

        // 操作气泡坐标 0中下、1左下、2左中下、3左中、4左中上、5左上、6中上偏左、7中上、8中上偏右、9右上、10右中上、11右中、12右中下、13右下
        public static readonly Vector3[] BubblePosLRV3 = new Vector3[]
        {
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(-160, 50),
                new Vector3(160, 50),
                new Vector3(160, 50),
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(0, 158),
                new Vector3(0, 158),
        };

        public static readonly Dictionary<int, SeatUIInfo[]> SeatUIInfos = new Dictionary<int, SeatUIInfo[]>()
        {
            [2] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[7],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[7],
                                CardBackRot = CardBackRotLRV3[7],
                                CardsPos = CardsPosLRV3[7],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[7],
                                BubblePos = BubblePosLRV3[7],
                        },
                },
            [3] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[4],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[4],
                                CardBackRot = CardBackRotLRV3[4],
                                CardsPos = CardsPosLRV3[4],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[4],
                                BubblePos = BubblePosLRV3[4],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[10],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[10],
                                CardBackRot = CardBackRotLRV3[10],
                                CardsPos = CardsPosLRV3[10],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[10],
                                BubblePos = BubblePosLRV3[10],
                        },
                },
            [4] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[3],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[3],
                                CardBackRot = CardBackRotLRV3[3],
                                CardsPos = CardsPosLRV3[3],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[3],
                                BubblePos = BubblePosLRV3[3],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[7],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[7],
                                CardBackRot = CardBackRotLRV3[7],
                                CardsPos = CardsPosLRV3[7],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[7],
                                BubblePos = BubblePosLRV3[7],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[11],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[11],
                                CardBackRot = CardBackRotLRV3[11],
                                CardsPos = CardsPosLRV3[11],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[11],
                                BubblePos = BubblePosLRV3[11],
                        },
                },
            [5] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[3],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[3],
                                CardBackRot = CardBackRotLRV3[3],
                                CardsPos = CardsPosLRV3[3],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[3],
                                BubblePos = BubblePosLRV3[3],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[6],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[6],
                                CardBackRot = CardBackRotLRV3[6],
                                CardsPos = CardsPosLRV3[6],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[6],
                                BubblePos = BubblePosLRV3[6],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[8],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[8],
                                CardBackRot = CardBackRotLRV3[8],
                                CardsPos = CardsPosLRV3[8],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[8],
                                BubblePos = BubblePosLRV3[8],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[11],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[11],
                                CardBackRot = CardBackRotLRV3[11],
                                CardsPos = CardsPosLRV3[11],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[11],
                                BubblePos = BubblePosLRV3[11],
                        },
                },
            [6] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[2],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[2],
                                CardBackRot = CardBackRotLRV3[2],
                                CardsPos = CardsPosLRV3[2],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[2],
                                BubblePos = BubblePosLRV3[2],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[4],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[4],
                                CardBackRot = CardBackRotLRV3[4],
                                CardsPos = CardsPosLRV3[4],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[4],
                                BubblePos = BubblePosLRV3[4],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[7],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[7],
                                CardBackRot = CardBackRotLRV3[7],
                                CardsPos = CardsPosLRV3[7],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[7],
                                BubblePos = BubblePosLRV3[7],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[10],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[10],
                                CardBackRot = CardBackRotLRV3[10],
                                CardsPos = CardsPosLRV3[10],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[10],
                                BubblePos = BubblePosLRV3[10],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[12],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[12],
                                CardBackRot = CardBackRotLRV3[12],
                                CardsPos = CardsPosLRV3[12],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[12],
                                BubblePos = BubblePosLRV3[12],

                        },
                },
            [7] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[2],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[2],
                                CardBackRot = CardBackRotLRV3[2],
                                CardsPos = CardsPosLRV3[2],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[2],
                                BubblePos = BubblePosLRV3[2],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[4],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[4],
                                CardBackRot = CardBackRotLRV3[4],
                                CardsPos = CardsPosLRV3[4],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[4],
                                BubblePos = BubblePosLRV3[4],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[6],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[6],
                                CardBackRot = CardBackRotLRV3[6],
                                CardsPos = CardsPosLRV3[6],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[6],
                                BubblePos = BubblePosLRV3[6],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[8],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[8],
                                CardBackRot = CardBackRotLRV3[8],
                                CardsPos = CardsPosLRV3[8],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[8],
                                BubblePos = BubblePosLRV3[8],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[10],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[10],
                                CardBackRot = CardBackRotLRV3[10],
                                CardsPos = CardsPosLRV3[10],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[10],
                                BubblePos = BubblePosLRV3[10],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[12],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[12],
                                CardBackRot = CardBackRotLRV3[12],
                                CardsPos = CardsPosLRV3[12],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[12],
                                BubblePos = BubblePosLRV3[12],
                        },

                },
            [8] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[1],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[1],
                                CardBackRot = CardBackRotLRV3[1],
                                CardsPos = CardsPosLRV3[1],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[1],
                                BubblePos = BubblePosLRV3[1],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[3],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[3],
                                CardBackRot = CardBackRotLRV3[3],
                                CardsPos = CardsPosLRV3[3],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[3],
                                BubblePos = BubblePosLRV3[3],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[5],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[5],
                                CardBackRot = CardBackRotLRV3[5],
                                CardsPos = CardsPosLRV3[5],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[5],
                                BubblePos = BubblePosLRV3[5],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[7],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[7],
                                CardBackRot = CardBackRotLRV3[7],
                                CardsPos = CardsPosLRV3[7],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[7],
                                BubblePos = BubblePosLRV3[7],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[9],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[9],
                                CardBackRot = CardBackRotLRV3[9],
                                CardsPos = CardsPosLRV3[9],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[9],
                                BubblePos = BubblePosLRV3[9],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[11],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[11],
                                CardBackRot = CardBackRotLRV3[11],
                                CardsPos = CardsPosLRV3[11],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[11],
                                BubblePos = BubblePosLRV3[11],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[13],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[13],
                                CardBackRot = CardBackRotLRV3[13],
                                CardsPos = CardsPosLRV3[13],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[13],
                                BubblePos = BubblePosLRV3[13],
                        },
                },
            [9] = new SeatUIInfo[]
                {
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[0],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[0],
                                CardBackRot = CardBackRotLRV3[0],
                                CardsPos = CardsPosLRV3[0],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[0],
                                BubblePos = BubblePosLRV3[0],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[1],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[1],
                                CardBackRot = CardBackRotLRV3[1],
                                CardsPos = CardsPosLRV3[1],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[1],
                                BubblePos = BubblePosLRV3[1],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[3],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[3],
                                CardBackRot = CardBackRotLRV3[3],
                                CardsPos = CardsPosLRV3[3],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[3],
                                BubblePos = BubblePosLRV3[3],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[5],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[5],
                                CardBackRot = CardBackRotLRV3[5],
                                CardsPos = CardsPosLRV3[5],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[5],
                                BubblePos = BubblePosLRV3[5],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[6],
                                BankerPos = BankerLRV3[0],
                                CardBackPos = CardBackLRV3[6],
                                CardBackRot = CardBackRotLRV3[6],
                                CardsPos = CardsPosLRV3[6],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[6],
                                BubblePos = BubblePosLRV3[6],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[8],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[8],
                                CardBackRot = CardBackRotLRV3[8],
                                CardsPos = CardsPosLRV3[8],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[8],
                                BubblePos = BubblePosLRV3[8],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[9],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[9],
                                CardBackRot = CardBackRotLRV3[9],
                                CardsPos = CardsPosLRV3[9],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[9],
                                BubblePos = BubblePosLRV3[9],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[11],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[11],
                                CardBackRot = CardBackRotLRV3[11],
                                CardsPos = CardsPosLRV3[11],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[11],
                                BubblePos = BubblePosLRV3[11],
                        },
                        new SeatUIInfo()
                        {
                                Pos = SeatPosV3[13],
                                BankerPos = BankerLRV3[1],
                                CardBackPos = CardBackLRV3[13],
                                CardBackRot = CardBackRotLRV3[13],
                                CardsPos = CardsPosLRV3[13],
                                CurRoundHaveBetPos = CurRoundHaveBetPosLRV3[13],
                                BubblePos = BubblePosLRV3[13],
                        },
                },
        };
        #endregion

        #region 跑马灯人物扑倒位置

        // 0中下、1左下、2左中下、3左中、4左中上、5左上、6中上偏左、7中上、8中上偏右、9右上、10右中上、11右中、12右中下、13右下
        public static readonly Vector3[] MarquessFallDownPosV3 = new Vector3[]
        {
                new Vector3(142, -792),
                new Vector3(-350, -540),
                new Vector3(-425, -425),
                new Vector3(-480, -174),
                new Vector3(-516, 140),
                new Vector3(-516, 227),
                new Vector3(-313, 648),
                new Vector3(-152, 765),
                new Vector3(40, 800),
                new Vector3(390, 550),
                new Vector3(430, 490),
                new Vector3(490, 180),
                new Vector3(500, -80),
                new Vector3(512, -240),
        };

        #endregion

        /// <summary>
        /// 牌局分池位置
        /// </summary>
        public static readonly Vector3[] TexasPots = new Vector3[]
        {
                new Vector3(0, 0),
                new Vector3(-250, -146),
                new Vector3(-50, -146),
                new Vector3(150, -146),
                new Vector3(-250, -222),
                new Vector3(-50, -222),
                new Vector3(150, -222),
                new Vector3(-250, -298),
                new Vector3(-50, -298),
        };


        /// <summary>
        /// 魔法表情花费
        /// </summary>
        public static int AnimojiCost(string name)
        {
            return GameUtil.animojiCost[name];
        }

        private static readonly Dictionary<string, int> animojiCost = new Dictionary<string, int> {
            {"daodanboom",2},
            {"daoshui",2},
            {"jidan",2},
            {"zhuoji",2},
            {"shayu",2},
            {"pijiu",2},
            {"hongchun",2},
            {"zhadan",2},
            {"paizhao",2},
            {"dianzan",2},
            {"fanqie",2},
        };
    }
}
