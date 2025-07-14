using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class PlayerAwakeSystem : AwakeSystem<Player>
	{
		public override void Awake(Player self)
		{
			self.Awake();
		}
	}

    public class Player: Entity
    {
        public sbyte seatID { get; set; } // 座位号
        public sbyte sex { get; set; } // 性别 0男，1女
        public string headPic { get; set; } // 头像id
        public string nick { get; set; } // 昵称
        public int userID { get; set; }
        public int chips { get; set; } // 该玩家牌桌上的筹码
        public string longitude { get; set; } // 定位lon
        public string latitude { get; set; } // 定位lat
        public string clientIP { get; set; } // 客户端IP，进房API有返回
        public int leavelChips { get; set; } // 玩家的USDT余额
        public int canPlayStatus { get; set; } // 玩家本手是否可以参与游戏的状态 1为可玩

        public int status { get; set; } // 玩家的当前状态数组:0:未操作过(显示名字) 1:下注  2:跟注  3:加注  4:全下 5:让牌  6:弃牌 7超时 8空闲等待下一局 9 等待入座 10 straddle 11 盖牌  15 表示玩家为留座状态 18 表示玩家为占座等待状态

        public int ante { get; set; } // 当前这一局各玩家下的赌注
        public int anteNumber { get; set; } // 当前操作下注筹码数
        public List<sbyte> cards { get; set; }
        public int leftSecs { get; set; } // 带入审核倒计时
        public int leftCreditValue { get; set; } // 用户信用额度
        public sbyte extraBlind { get; set; } // 各玩家是否补盲 0 1
        public int initialBets { get; set; } // 开始的时候玩家下注的筹码(SNG时会返回)
        public sbyte muckStatus { get; set; } // 盖牌的状态 0未盖牌 1为盖牌
        public sbyte cardType { get; set; } // 赢牌类型 1皇家同花 2同花顺 3四条 4葫芦 5同花 6顺子 7三条 8两对 9一对 10高牌
        public int trust { get; set; } // 托管 0否，1是
        public int playerStatus_insurance { get; set; } // 保险人状态（0 正购买 1 已购买 2 已放弃购买）
        public int timeLeft_insurance { get; set; } // 保险人剩余时间(单位：秒，对应所有可购买的人，状态不是正在购买的，值为0)
        public int timeLeft_dairu { get; set; } // 等待审核剩余时间(单位：秒)
        public int totalInsuredAmount { get; set; } // 保险人投保额
        public int autoInsuredAmount { get; set; } // 保险人背保额
        public int claimInsuredAmount { get; set; } // 保险赔付额

        public int recyclingChip { get; set; } // 收筹码数量
        public sbyte isWin { get; set; } // 收筹码玩家是否是赢家 0是 1否
        public int winChips { get; set; } // 赢家赢的筹码数
        public sbyte isMaxcard { get; set; } // 是否是最大手牌 0是 1不是 未使用
        public int jackPotUp { get; set; } // 向jackPot上交的金币数
        public int hitJackPotChips { get; set; } // 击中牌型获得的奖励金币数（大于0需要做玩家击中彩池的动画）

        public bool isFold { get; set; } // 是否已弃牌

        public virtual void Awake()
        {
            seatID = -1;

            if (null != cards)
                cards.Clear();
            else
                cards = new List<sbyte>();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            ClearData();

            base.Dispose();
        }

        public virtual bool isPlaying
        {
            get
            {
                return canPlayStatus == 1 && (status >= 0 && status < 7 || status == 10 || status == 11);
            }

        }

        public virtual void SetCards(List<sbyte> list)
        {
            if (null == cards)
                cards = new List<sbyte>();
            if (cards.Count > 0)
                cards.Clear();
            cards.AddRange(list);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public virtual void ClearData()
        {
            ClearGameData();
            sex = 0;
            headPic = string.Empty;
            nick = string.Empty;
            userID = -0;
            chips = 0;
        }

        /// <summary>
        /// 清空游戏数据
        /// </summary>
        public virtual void ClearGameData()
        {
            seatID = -1;
            longitude = string.Empty;
            latitude = string.Empty;
            clientIP = string.Empty;
            leavelChips = 0;
            canPlayStatus = 0;
            status = -1;
            ante = 0;
            anteNumber = 0;
            if (null != cards)
                cards.Clear();
            leftSecs = 0;
            leftCreditValue = 0;
            extraBlind = 0;
            initialBets = 0;
            muckStatus = 0;
            cardType = 0;
            trust = 0;
            playerStatus_insurance = 0;
            timeLeft_insurance = 0;
            totalInsuredAmount = 0;
            autoInsuredAmount = 0;
            claimInsuredAmount = 0;

            recyclingChip = 0;
            isWin = 0;
            winChips = 0;
            isMaxcard = 0;
            jackPotUp = 0;
            hitJackPotChips = 0;

            isFold = false;
        }
    }
}