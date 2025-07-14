using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//头像缓存管理，减少获取次数
namespace ETHotfix
{

    public enum eHeadType
    {
        NONE = -1,
        USER = 0,
        CLUB = 1,
    }

    public class Head{

        public string keyId;
        public eHeadType type;
        public string headId;
        public Texture2D t2d;

        public Head() {

            keyId = string.Empty;
            headId = string.Empty;
            type = eHeadType.NONE;
            t2d = null;
        }
    }

    public class HeadCache
    {

        public static List<Head> que_userhead = new List<Head>();
        public static List<Head> que_clubhead = new List<Head>();

        public static Head GetHead(eHeadType type , string keyId)
        {
            if (type == eHeadType.USER) {

                for (int i = 0; i < que_userhead.Count; ++i) {

                    if (que_userhead[i].keyId.Equals(keyId)) {

                        return que_userhead[i];
                    }
                }
                return GetNewHead(que_userhead , keyId , 100);

            } else if (type == eHeadType.CLUB) {

                for (int i = 0; i < que_clubhead.Count; ++i)
                {
                    if (que_clubhead[i].keyId.Equals(keyId))
                    {

                        return que_clubhead[i];
                    }
                }
                return GetNewHead(que_userhead, keyId, 20);
            }
            return null;
        }

        static Head GetNewHead(List<Head> list, string keyId , int max)
        {
            if (list.Count >= max)
            {
                list[0].t2d = null;
                list.RemoveAt(0);
            }
            Head h = new Head();
            h.keyId = keyId;
            list.Add(h);
            return h;
        }

        public static void Release(eHeadType type) {

            if (type == eHeadType.USER)
            {
                que_userhead.Clear();
            }
            else if (type == eHeadType.CLUB)
            {
                que_clubhead.Clear();
            }
        }
    }


}
