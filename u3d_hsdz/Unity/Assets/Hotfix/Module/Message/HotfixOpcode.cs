using ETModel;
namespace ETHotfix
{
    [Message(HotfixOpcode.REQ_LOGIN)]
    public partial class REQ_LOGIN : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_LOGIN_RESOURCES)]
    public partial class REQ_LOGIN_RESOURCES : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_ENTER_ROOM)]
    public partial class REQ_GAME_ENTER_ROOM : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_SEND_SEAT_ACTION)]
    public partial class REQ_GAME_SEND_SEAT_ACTION : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_SEND_ACTION)]
    public partial class REQ_GAME_SEND_ACTION : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_ACTION)]
    public partial class REQ_GAME_RECV_ACTION : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_LEAVE)]
    public partial class REQ_GAME_RECV_LEAVE : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_SEAT_DOWN)]
    public partial class REQ_GAME_RECV_SEAT_DOWN : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_CARDS)]
    public partial class REQ_GAME_RECV_CARDS : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_WINNER)]
    public partial class REQ_GAME_RECV_WINNER : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_READYTIME)]
    public partial class REQ_GAME_RECV_READYTIME : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_START_INFOR)]
    public partial class REQ_GAME_RECV_START_INFOR : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_ADD_CHIPS)]
    public partial class REQ_GAME_ADD_CHIPS : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_ENTER_ROOM)]
    public partial class REQ_ENTER_ROOM : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_START_GAME)]
    public partial class REQ_GAME_START_GAME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_ENDING)]
    public partial class REQ_GAME_ENDING : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_TAKE_CONTROL)]
    public partial class REQ_GAME_TAKE_CONTROL : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_CONTROL_RANGE)]
    public partial class REQ_GAME_CONTROL_RANGE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_PLAYER_CARDS)]
    public partial class REQ_GAME_PLAYER_CARDS : ICPResponse { }

    [Message(HotfixOpcode.REQ_SHOWDOWN)]
    public partial class REQ_SHOWDOWN : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_JACKPOT_SHOW_CARDS)]
    public partial class REQ_GAME_JACKPOT_SHOW_CARDS : ICPResponse { }

    [Message(HotfixOpcode.REQ_SYNC_SIGNAL_RESOURCE)]
    public partial class REQ_SYNC_SIGNAL_RESOURCE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_SYNC_SIGNAL_GAME)]
    public partial class REQ_SYNC_SIGNAL_GAME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_OFFLINE)]
    public partial class REQ_OFFLINE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME)]
    public partial class REQ_GAME_RECV_PRE_START_GAME : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_TRUST_ACTION)]
    public partial class REQ_GAME_TRUST_ACTION : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_FINISHGAME)]
    public partial class REQ_FINISHGAME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_CHECK_CAN_CICK)]
    public partial class REQ_GAME_CHECK_CAN_CICK : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_JACKPOT_CHANGE)]
    public partial class REQ_GAME_JACKPOT_CHANGE : ICPResponse { }

    [Message(HotfixOpcode.REQ_START_ROOM)]
    public partial class REQ_START_ROOM : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_REAL_TIME)]
    public partial class REQ_GAME_REAL_TIME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_ADD_TIME)]
    public partial class REQ_ADD_TIME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_PAUSE)]
    public partial class REQ_GAME_PAUSE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_PREV_GAME)]
    public partial class REQ_GAME_PREV_GAME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_COLLECT_CARD)]
    public partial class REQ_GAME_COLLECT_CARD : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_SHOW_SIDE_POTS)]
    public partial class REQ_SHOW_SIDE_POTS : ICPResponse { }

    [Message(HotfixOpcode.REQ_WAIT_BLIND)]
    public partial class REQ_WAIT_BLIND : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_INSURANCE_TRIGGED)]
    public partial class REQ_INSURANCE_TRIGGED : ICPResponse { }

    [Message(HotfixOpcode.REQ_BUY_INSURANCE)]
    public partial class REQ_BUY_INSURANCE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_CLAIM_INSURANCE)]
    public partial class REQ_CLAIM_INSURANCE : ICPResponse { }

    [Message(HotfixOpcode.REQ_INSURANCE_ADD_TIME)]
    public partial class REQ_INSURANCE_ADD_TIME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_INSURANCE_NOT_TRIGGED)]
    public partial class REQ_INSURANCE_NOT_TRIGGED : ICPResponse { }

    [Message(HotfixOpcode.REQ_SEE_MORE_PUBLIC_ACTION)]
    public partial class REQ_SEE_MORE_PUBLIC_ACTION : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_CURRENT_ROUND_FINISH)]
    public partial class REQ_CURRENT_ROUND_FINISH : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_KEEP_SEAT)]
    public partial class REQ_GAME_KEEP_SEAT : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_WAIT_DAIRU)]
    public partial class REQ_GAME_WAIT_DAIRU : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_ADD_ROOM_TIME)]
    public partial class REQ_ADD_ROOM_TIME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_STRADDLE)]
    public partial class REQ_GAME_RECV_PRE_START_GAME_STRADDLE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_PRE_START_GAME_MUCK)]
    public partial class REQ_GAME_RECV_PRE_START_GAME_MUCK : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_RECV_SPECTATORS_VOICE)]
    public partial class REQ_GAME_RECV_SPECTATORS_VOICE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_REMIND)]
    public partial class REQ_GAME_MTT_REMIND : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_Self_Ranking)]
    public partial class REQ_GAME_MTT_Self_Ranking : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_REAL_TIME)]
    public partial class REQ_GAME_MTT_REAL_TIME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_SYSTEM_NOTIFY)]
    public partial class REQ_GAME_MTT_SYSTEM_NOTIFY : ICPResponse { }

    [Message(HotfixOpcode.REQ_MTT_GAME_REBUY_REQUEST)]
    public partial class REQ_MTT_GAME_REBUY_REQUEST : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_FINAL_RANK)]
    public partial class REQ_GAME_MTT_FINAL_RANK : ICPResponse { }

    [Message(HotfixOpcode.REQ_MTT_GAME_REBUY_CANCEL)]
    public partial class REQ_MTT_GAME_REBUY_CANCEL : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_MTT_APPLY_JOIN)]
    public partial class REQ_MTT_APPLY_JOIN : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_MTT_REBUY_REQUEST_OUTSIDE)]
    public partial class REQ_MTT_REBUY_REQUEST_OUTSIDE : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_MTT_PLAYER_LIST)]
    public partial class REQ_MTT_PLAYER_LIST : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_ENTER_ROOM)]
    public partial class REQ_GAME_MTT_ENTER_ROOM : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_MTT_GAME_DISMISS)]
    public partial class REQ_MTT_GAME_DISMISS : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_RECV_START_INFOR)]
    public partial class REQ_GAME_MTT_RECV_START_INFOR : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_AUTO_OP)]
    public partial class REQ_GAME_MTT_AUTO_OP : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_MTT_START_GAME)]
    public partial class REQ_GAME_MTT_START_GAME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_MTT_ADD_TIME)]
    public partial class REQ_MTT_ADD_TIME : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_SYSTEM_BROADCAST)]
    public partial class REQ_SYSTEM_BROADCAST : ICPResponse { }

    [Message(HotfixOpcode.REQ_Guild_BROADCAST)]
    public partial class REQ_Guild_BROADCAST : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_JACKPOT_HIT_BROADCAST)]
    public partial class REQ_GAME_JACKPOT_HIT_BROADCAST : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_OMAHA_ENTER_ROOM)]
    public partial class REQ_GAME_OMAHA_ENTER_ROOM : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_OMAHA_RECV_WINNER)]
    public partial class REQ_GAME_OMAHA_RECV_WINNER : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_OMAHA_RECV_START_INFOR)]
    public partial class REQ_GAME_OMAHA_RECV_START_INFOR : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_OMAHA_PLAYER_CARDS)]
    public partial class REQ_GAME_OMAHA_PLAYER_CARDS : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_OMAHA_SHOWDOWN)]
    public partial class REQ_GAME_OMAHA_SHOWDOWN : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_OMAHA_SHOW_CARDS)]
    public partial class REQ_GAME_OMAHA_SHOW_CARDS : ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_ASK_TEST)]
    public partial class REQ_GAME_ASK_TEST : ICPRequest, ICPResponse { }

    [Message(HotfixOpcode.REQ_GAME_SEND_TEST)]
    public partial class REQ_GAME_SEND_TEST : ICPRequest, ICPResponse { }

}
namespace ETHotfix
{
    public static partial class HotfixOpcode
    {
        public const ushort REQ_LOGIN = 6;  // 账号登录
        public const ushort REQ_LOGIN_RESOURCES = 8;  // res登录
        public const ushort REQ_GAME_ENTER_ROOM = 12;  // 普通局进入房间
        public const ushort REQ_GAME_SEND_SEAT_ACTION = 18;  // 当前玩家站起或坐下或离开
        public const ushort REQ_GAME_SEND_ACTION = 19;  // 自己牌桌操作
        public const ushort REQ_GAME_RECV_ACTION = 20;  // 收到牌桌操作
        public const ushort REQ_GAME_RECV_LEAVE = 22;  // 别人离开座位
        public const ushort REQ_GAME_RECV_SEAT_DOWN = 23;  // 别人坐下
        public const ushort REQ_GAME_RECV_CARDS = 24;  // 收到公共牌
        public const ushort REQ_GAME_RECV_WINNER = 25;  // 收到胜利者
        public const ushort REQ_GAME_RECV_READYTIME = 26;  // 收到准备时间
        public const ushort REQ_GAME_RECV_START_INFOR = 27;  // 新一手开始（普通局、SNG）
        public const ushort REQ_GAME_ADD_CHIPS = 28;  // 带入
        public const ushort REQ_ENTER_ROOM = 30;  // 进入牌局等待页
        public const ushort REQ_GAME_START_GAME = 32;  // 房主点击开始游戏
        public const ushort REQ_GAME_ENDING = 33;  // 牌局结束
        public const ushort REQ_GAME_TAKE_CONTROL = 34;  // 房主设置控制带入开关
        public const ushort REQ_GAME_CONTROL_RANGE = 40;  // 房主设置带入倍数
        public const ushort REQ_GAME_PLAYER_CARDS = 42;  // Allin下发玩家手牌
        public const ushort REQ_SHOWDOWN = 43;  // 设置结束时亮的手牌
        public const ushort REQ_GAME_JACKPOT_SHOW_CARDS = 44;   // 玩家亮牌协议（此协议在比牌后，其他玩家结束点击亮牌击中jackpot才会触发）
        public const ushort REQ_SYNC_SIGNAL_RESOURCE = 52;  // Res心跳
        public const ushort REQ_SYNC_SIGNAL_GAME = 53;  // Game心跳
        public const ushort REQ_OFFLINE = 55;  // 退出登录
        public const ushort REQ_GAME_RECV_PRE_START_GAME = 66;  // 每手游戏开始前处理
        public const ushort REQ_GAME_TRUST_ACTION = 68;  // 托管
        public const ushort REQ_FINISHGAME = 69;  // 解散房间
        public const ushort REQ_GAME_CHECK_CAN_CICK = 72;  // 查询对某人是否有踢出权限
        public const ushort REQ_GAME_JACKPOT_CHANGE = 78;  // JackPot变化通知
        public const ushort REQ_START_ROOM = 80;  // 等待页开始、退出或解散
        public const ushort REQ_GAME_REAL_TIME = 82;  // 实时战况
        public const ushort REQ_ADD_TIME = 86;  // 操作加时
        public const ushort REQ_GAME_PAUSE = 87;  // 暂停游戏
        public const ushort REQ_GAME_PREV_GAME = 90;  // 上局回顾
        public const ushort REQ_GAME_COLLECT_CARD = 118;  // 收藏牌谱
        public const ushort REQ_SHOW_SIDE_POTS = 120;  // 显示分池筹码
        public const ushort REQ_WAIT_BLIND = 122;  // 过庄补盲
        public const ushort REQ_INSURANCE_TRIGGED = 140;  // 保险触发
        public const ushort REQ_BUY_INSURANCE = 141;  // 购买保险
        public const ushort REQ_CLAIM_INSURANCE = 142;  // 保险赔付消息
        public const ushort REQ_INSURANCE_ADD_TIME = 143;  // 保险加时
        public const ushort REQ_INSURANCE_NOT_TRIGGED = 144;  // 保险条件不足  (客户端端未解析内容，收到该协议则提示：“OUTS>16或OUTS=0，不能购买保险”)
        public const ushort REQ_SEE_MORE_PUBLIC_ACTION = 148;  // 查看未发公共牌  (查看成功，会在【24】收到公共牌协议下发查看的公共牌)
        public const ushort REQ_CURRENT_ROUND_FINISH = 151;  // 当前手结束  (收到该协议，清空当前手的界面和数据)
        public const ushort REQ_GAME_KEEP_SEAT = 152;  // 留座离桌
        public const ushort REQ_ADD_ROOM_TIME = 153;  // 房间加时  (房主操作了加时，房间内所有玩家都能收到通知)
        public const ushort REQ_GAME_RECV_PRE_START_GAME_STRADDLE = 154;  // 开启关闭强制Straddle  (房主操作了开启关闭Straddle，房间内所有玩家都能收到通知)
        public const ushort REQ_GAME_RECV_PRE_START_GAME_MUCK = 155;  // 开启关闭Muck  (房主操作了开启关闭Muck，房间内所有玩家都能收到通知)
        public const ushort REQ_GAME_WAIT_DAIRU = 156;  // 等待审核带入
        public const ushort REQ_GAME_RECV_SPECTATORS_VOICE = 160;  // 开启关闭观众语言  (房主操作了开启关闭Muck，房间内所有玩家都能收到通知)
        public const ushort REQ_GAME_MTT_REMIND = 166;  // MTT比赛开始消息  (收到该消息，会在全局顶部弹出比赛即将开始的通知:"锦标赛xxx即将开始，是否进入比赛？")
        public const ushort REQ_GAME_MTT_Self_Ranking = 167;  // 服务器主动下发的mtt排名
        public const ushort REQ_GAME_MTT_REAL_TIME = 168;  // MTT实时战况
        public const ushort REQ_GAME_MTT_SYSTEM_NOTIFY = 170;  // MTT牌局内各种系统消息
        public const ushort REQ_MTT_GAME_REBUY_REQUEST = 171;  // MTT重购申请
        public const ushort REQ_GAME_MTT_FINAL_RANK = 175;  // MTT结束游戏通知名次  (收到该消息，如果没有重购机会，则弹出游戏结束排名页。如果有重购机会，则弹出重购框)
        public const ushort REQ_MTT_GAME_REBUY_CANCEL = 176;  // MTT取消重购  (由175协议触发的重购提示框，点取消时会调这个协议)
        public const ushort REQ_MTT_APPLY_JOIN = 180;  // MTT报名
        public const ushort REQ_MTT_REBUY_REQUEST_OUTSIDE = 186;  // MTT牌局外重购申请
        public const ushort REQ_MTT_PLAYER_LIST = 188;  // MTT牌局外玩家列表
        public const ushort REQ_GAME_MTT_ENTER_ROOM = 212;  // MTT进入房间
        public const ushort REQ_MTT_GAME_DISMISS = 222;  // MTT解散
        public const ushort REQ_GAME_MTT_RECV_START_INFOR = 226;  // MTT每局开局消息  (该协议跟普通局的27协议返回是类似的，只是协议号不同，跟普通局做一样的处理就行。MTT的不同在于收到改协议要多做这几样东西，)
        public const ushort REQ_GAME_MTT_AUTO_OP = 228;  // MTT托管
        public const ushort REQ_GAME_MTT_START_GAME = 230;  // mtt比赛开始的消息
        public const ushort REQ_MTT_ADD_TIME = 236;  // MTT操作加时
        public const ushort REQ_SYSTEM_BROADCAST = 266;  // Res系统广播
        public const ushort REQ_Guild_BROADCAST = 268;  // 牌局广播  (牌局内收到广播，跟266协议一样的展示)
        public const ushort REQ_GAME_JACKPOT_HIT_BROADCAST = 269;  // JackPot击中广播
        public const ushort REQ_GAME_OMAHA_ENTER_ROOM = 412;  // 奥马哈进入房间  (如普通局不同的：158结束时亮牌为数组4张,178，179字段，后两张手牌，187血战模式开关)
        public const ushort REQ_GAME_OMAHA_RECV_WINNER = 420;  // 奥马哈收到胜利者  (跟普通局25协议一样的返回，多出了145，146字段，则后两张手牌)
        public const ushort REQ_GAME_OMAHA_RECV_START_INFOR = 426;  // 奥马哈新一手开始  (跟普通局不同：150，151则后两张手牌)
        public const ushort REQ_GAME_OMAHA_PLAYER_CARDS = 432;  // 奥马哈Allin下发玩家手牌  (跟普通局不同：133，134则后两张手牌)
        public const ushort REQ_GAME_OMAHA_SHOWDOWN = 433;  // 奥马哈设置结束时亮的手牌  (跟普通局不同：133改为数组)
        public const ushort REQ_GAME_OMAHA_SHOW_CARDS = 448;  // 奥马哈玩家亮牌协议

        public const ushort REQ_GAME_ASK_TEST = 29;  //测试查询
        public const ushort REQ_GAME_SEND_TEST = 35;  //测试发牌
    }
}