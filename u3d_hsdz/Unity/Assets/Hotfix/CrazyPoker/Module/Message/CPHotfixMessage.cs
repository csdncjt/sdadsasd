using System;
using System.Collections.Generic;
using System.IO;
using ETModel;

namespace ETHotfix
{
    public partial class REQ_LOGIN : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte userType { get; set; }  // 登录方式，手机号登录12，token登录10
        public string userUUID { get; set; }  // 如果手机登录，则为手机号。token登录则为token
        public string macAddress { get; set; }  // mac地址
        public string imsi { get; set; }  // 设备唯一id
        public string HeighthAndWidth { get; set; }  // 设备分辨率，格式1080.0*720.0
        public string model { get; set; }  // 机型,如iPhone6.1
        public string system_version { get; set; }  // 系统版本号，如12.1.2
        public string sessionKey { get; set; }  // 如果手机登录，则为密码md5。token登录则为空
        public string nickname { get; set; }  // facebook登录时会用到，现在传空即可
        public string countryCode { get; set; }  // 空
        public string version { get; set; }  // 版本号
        public int channel { get; set; }  // 渠道号3

        // Response
        public sbyte status { get; set; }  // 状态，0成功 1用户不存在 2密码不对 3一般错误 4版本太低 5token不对 6其他错误 7账号被封禁 8手机异常，无法登录 9玩家非指定机器码登录。
        public sbyte user_type { get; set; }  // 登录方式，手机号登录12，token登录10
        public sbyte isfirstLogin { get; set; }  // 是否首次登录0、1
        public string IP { get; set; }  // 登录res服务器的IP或域名
        public string key { get; set; }  // 登录res服务器时要用到
        public int user_id { get; set; }  // 用户id
        public int port { get; set; }  // 登录res服务器的端口
        public int server_number { get; set; }  // 未使用，不知含义
        public string token { get; set; }  // Token,后面自动登录，和接口要用到
        public int isActivity { get; set; }  // 0 = 无资格 1 = 调用首次登陆API 2 = 已有首充资格 3 = 待领取
        public int kDouNum { get; set; }  // 待领取 状态时，对应的领取USDT数量


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(12);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, userType, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, userUUID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, macAddress, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, imsi, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, HeighthAndWidth, stream);
            bodysize += CPHotfixMessageHelper.WriteField(135, model, stream);
            bodysize += CPHotfixMessageHelper.WriteField(136, system_version, stream);
            bodysize += CPHotfixMessageHelper.WriteField(137, sessionKey, stream);
            bodysize += CPHotfixMessageHelper.WriteField(138, nickname, stream);
            bodysize += CPHotfixMessageHelper.WriteField(139, countryCode, stream);
            bodysize += CPHotfixMessageHelper.WriteField(140, version, stream);
            bodysize += CPHotfixMessageHelper.WriteField(141, channel, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.user_type = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.isfirstLogin = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.IP = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.key = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.user_id = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.port = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.server_number = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.token = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 136:
                        this.isActivity = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 137:
                        this.kDouNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_LOGIN_RESOURCES : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public string key { get; set; }  // 登录协议131字段返回的key

        // Response
        public sbyte status { get; set; }  // 状态，0成功 7账号被封禁  其他错误码：登录失败
        public sbyte sex { get; set; }  // 性别，0男，1女
        public sbyte msg_count { get; set; }  // 消息个数
        public sbyte level { get; set; }  // 用户等级，未使用
        public sbyte task { get; set; }  // 未使用
        public string unknow { get; set; }  // 未使用
        public string nick { get; set; }  // 用户昵称
        public string head_picName { get; set; }  // 头像id
        public List<sbyte> maxcards { get; set; }  // 未使用
        public int maxwin { get; set; }  // 未使用
        public int gold { get; set; }  // USDT余额
        public int idou { get; set; }  // 钻石余额
        public int lose_number { get; set; }  // 未使用
        public int win_number { get; set; }  // 未使用
        public int max_own { get; set; }  // 未使用
        public int online_count { get; set; }  // 未使用
        public int fileServerPort { get; set; }  // 未使用
        public string fileServerIP { get; set; }  // 未使用
        public int score { get; set; }  // 未使用
        public int match_score { get; set; }  // 未使用
        public List<sbyte> bounds { get; set; }  // 未使用
        public int gifCount { get; set; }  // 未使用
        public int poolCount { get; set; }  // 未使用
        public int gameCount { get; set; }  // 未使用
        public string birthday { get; set; }  // 未使用
        public sbyte isFirstLogin { get; set; }  // 未使用
        public sbyte vipLevel { get; set; }  // 会员等级,0普通玩家 1伯爵 2侯爵 3王公
        public string vipEndDate { get; set; }  // 会员到期日
        public sbyte playerflag { get; set; }


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(1);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, key, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.sex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.msg_count = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 63:
                        this.level = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 64:
                        this.task = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 65:
                        this.unknow = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 130:
                        this.nick = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.head_picName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.maxcards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 133:
                        this.maxwin = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.gold = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.idou = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 136:
                        this.lose_number = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 137:
                        this.win_number = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 138:
                        this.max_own = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.online_count = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 140:
                        this.fileServerPort = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.fileServerIP = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 142:
                        this.score = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 143:
                        this.match_score = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 144:
                        this.bounds = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 145:
                        this.gifCount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 146:
                        this.poolCount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.gameCount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 148:
                        this.birthday = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 149:
                        this.isFirstLogin = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 150:
                        this.vipLevel = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 151:
                        this.vipEndDate = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 152:
                        this.playerflag = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_ENTER_ROOM : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte type { get; set; }  // 1
        public sbyte connect { get; set; }  // 1
        public sbyte unknow { get; set; }  // 未知含义，传1
        public int roomPath { get; set; }  // 普通局:61 MTT:71 SNG:81 奥马哈:91 大菠萝:51
        public int roomID { get; set; }  // 房间号
        public int deskId { get; set; }  // 0

        // Response
        public sbyte status { get; set; }  // 状态，0成功 1进入失败 2房间已解散 3人数过多 4该桌已拆，请换桌观战（MTT才会有） 5已在该MTT比赛中（MTT才会有）
        public sbyte gamestatus { get; set; }  // 当前游戏状态，0:倒计时中 1:游戏中 -2:等待开局 -1:其他状态
        public sbyte countdown_times { get; set; }  // 未使用
        public sbyte bigIndex { get; set; }  // 大盲所在位置
        public sbyte smallIndex { get; set; }  // 小盲所在位置
        public sbyte bankerIndex { get; set; }  // 庄家所在位置
        public sbyte operationID { get; set; }  // 当前操作玩家所在位置
        public sbyte speed { get; set; }  // 未使用
        public sbyte mySeatID { get; set; }  // 我的座位号，未坐下的话是-1
        public sbyte maxCheckTimes { get; set; }  // 未使用
        public sbyte currentCheckTimes { get; set; }  // 未使用
        public List<sbyte> cards { get; set; }  // 已发出公共牌
        public List<int> playerIDArray { get; set; }  // 玩家userID的数组，空位为-1
        public List<int> statusArray { get; set; }  // 玩家的当前状态数组:0:未操作过(显示名字) 1:下注  2:跟注  3:加注  4:全下 5:让牌  6:弃牌 7超时 8空闲等待下一局 9 等待入座 10 straddle 11 盖牌  15 表示玩家为留座状态 18 表示玩家为占座等待状态
        public List<int> anteArray { get; set; }  // 当前这一局各玩家下的赌注
        public string nick { get; set; }  // 座位上所有玩家的昵称，以@%分割
        public List<int> chipsArray { get; set; }  // 玩家带入牌桌的筹码
        public string headPicString { get; set; }  // 座位上所有玩家的头像id，以@%分割
        public List<sbyte> firstCardarray { get; set; }  // 所有玩家第一张手牌
        public List<sbyte> SecondArray { get; set; }  // 所有玩家第二张手牌
        public int bigBlind { get; set; }  // 大盲
        public int smallBlind { get; set; }  // 小盲
        public int alreadAnte { get; set; }  // 底池数目
        public int maxAnte { get; set; }  // 未使用
        public int roomOwner { get; set; }  // 房主的userID
        public string gameBeginTime { get; set; }  // 未使用
        public int maxPlayTime { get; set; }  // 房间的时间总长度（分钟）
        public sbyte bControl { get; set; }  // 是否开启控制带入0、1
        public int userChip { get; set; }  // 用户金币余额，未使用
        public int nowChip { get; set; }  // 用户带入房间的筹码
        public int roomLeftTime { get; set; }  // 房间剩余时间（秒）
        public int pauseLeftTime { get; set; }  // 暂停剩余时间，大于0则为暂停状态（秒）
        public int lastMinRate { get; set; }  // 未使用
        public int lastMaxRate { get; set; }  // 未使用
        public int currentMinRate { get; set; }  // 当前最小带入倍数
        public int currentMaxRate { get; set; }  // 当前最大带入倍数
        public int leftOperateTime { get; set; }  // 当前操作玩家剩余时间（s）
        public List<sbyte> sexArray { get; set; }  // 所有玩家的性别，0男，1女
        public int showCardsId { get; set; }  // 结束时是否亮手牌 0不亮 1亮第一张 2亮第二张 3亮两张
        public int groupBet { get; set; }  // 前注
        public int leftSecs { get; set; }  // 申请带入等待剩余时间，大于0会在右上角显示倒计时
        public List<int> pots { get; set; }  // 各分池的筹码数
        public int minAnteNum { get; set; }  // 当前最小可加注额，操作按钮上的加注额要用到
        public int canRaise { get; set; }  // 是否可加注，与minAnteNum及剩余筹码联合判断是否显示加注按钮
        public int insurance { get; set; }  // 是否开启保险
        public List<int> canPlayStatus { get; set; }  // 各个玩家本手是否可以参与游戏的状态 1为可玩
        public List<int> seatStatus { get; set; }  //  各玩家座位状态 0: 刚坐下， >=1: 显示气泡
        public int waitBlind { get; set; }  //  1需要弹出选择补盲，0不需要弹出
        public int leftCreditValue { get; set; }  //  用户信用额度
        public int roomType { get; set; }  //  房间类型，1：普通房 2:战队有限时长局 3:战队无限时长局（现在没这种类型）
        public int needBring { get; set; }  //  需要带入的筹码数，只有roomType==2时有效 （未使用）
        public int isTrusted { get; set; }  //  是否在其他房间被托管
        public int callAmount { get; set; }  //  跟注额，操作按钮上使用
        public List<sbyte> publicCardsType { get; set; }  //  表示各公共牌类型 (0:正常发牌 1:玩家花钻石发牌)
        public int isKeptSeat { get; set; }  //  当前玩家是不是在其他房间有留座的状态 1 有 0 无
        public List<int> keptTimeArray { get; set; }  //  留座离桌玩家的倒计时，默认值-1
        public string seeCardPlayerName { get; set; }  // 花钻看牌玩家的的昵称
        public int isIpRestrictions { get; set; }  //  是否开启IP限制，1 开启 0关闭
        public int isGPSRestrictions { get; set; }  //  是否开启GPS限制，1 开启 0关闭
        public int tribeId { get; set; }  //  同步到同盟的id 未同步时为0
        public List<int> holdingSeatLeftTimeArray { get; set; }  //  占座等待状态：占座但未申请带入，返回-1，APP端显示“等待”；占座并申请带入中，返回等待剩余倒计时，如125
        public int bringIn { get; set; }  //  该玩家是否在本牌局中带入过0 1 用于判断是否是否显示菜单中的提前离桌
        public int preLeave { get; set; }  //  是否已提前离桌0 1
        public sbyte bSpectatorsVoice { get; set; }  //  是否开启了观众语音0 1
        public string ServerVersion { get; set; }  //  APP的最新版本，如果当前app版本较小，则在牌桌中间显示升级提示
        public sbyte canPreLeave { get; set; }  //  建房是房主设置的提前离桌功能是否开启 1 开启 0 不开启
        public int operationRound { get; set; }  //  当前操作轮数，用户操作时会把当前轮数发过去，后台会较验是否是本轮的操作
        public sbyte isOrientationTribe { get; set; }  //  是否定向同盟  0否  1是,定向同盟房间的延时，消耗钻石会不一样
        public int serviceFeePer { get; set; }  //  服务费百分比 如10%则为10
        public sbyte bCreditControl { get; set; }  //  是否开启社区分带入0 1
        public int opTime { get; set; } // 默认操作玩家时间（s）
        public int userDelayTimes { get; set; } // 用户延时操作次数
        public int potNumber { get; set; }  // pot值
        public int firstBet { get; set; }  // 第一个差额
        public int lastPlayerBet { get; set; }  // 上个玩家加注额
        public int round { get; set; }  // 是否第一轮0第一轮
        public int callnum { get; set; }  // call人数
        public int foldmum { get; set; }  // 弃牌人数
        public int roomMode { get; set; }  // 房间类型(0普通1积分房)
        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(6);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, type, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, connect, stream);
            bodysize += CPHotfixMessageHelper.WriteField(62, unknow, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, deskId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.gamestatus = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.countdown_times = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 65:
                        this.bigIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 66:
                        this.smallIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 67:
                        this.bankerIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 68:
                        this.operationID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 69:
                        this.speed = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 70:
                        this.mySeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 71:
                        this.maxCheckTimes = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 72:
                        this.currentCheckTimes = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 131:
                        this.cards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.playerIDArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.statusArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.anteArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.nick = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 136:
                        this.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 137:
                        this.headPicString = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 138:
                        this.firstCardarray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 139:
                        this.SecondArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 140:
                        this.bigBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.smallBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.alreadAnte = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 143:
                        this.maxAnte = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 144:
                        this.roomOwner = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 145:
                        this.gameBeginTime = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 146:
                        this.maxPlayTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.bControl = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 148:
                        this.userChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 149:
                        this.nowChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 150:
                        this.roomLeftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 151:
                        this.pauseLeftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 152:
                        this.lastMinRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 153:
                        this.lastMaxRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 154:
                        this.currentMinRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 155:
                        this.currentMaxRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 156:
                        this.leftOperateTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 157:
                        this.sexArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 158:
                        this.showCardsId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 159:
                        this.groupBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 160:
                        this.leftSecs = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 161:
                        this.pots = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 162:
                        this.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 163:
                        this.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 164:
                        this.insurance = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 165:
                        this.canPlayStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 166:
                        this.seatStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 167:
                        this.waitBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 168:
                        this.leftCreditValue = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 169:
                        this.roomType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 170:
                        this.needBring = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 171:
                        this.isTrusted = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 172:
                        this.callAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 173:
                        this.publicCardsType = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 174:
                        this.isKeptSeat = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 175:
                        this.keptTimeArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 176:
                        this.seeCardPlayerName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 177:
                        this.isIpRestrictions = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 178:
                        this.isGPSRestrictions = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 179:
                        this.tribeId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 180:
                        this.holdingSeatLeftTimeArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 181:
                        this.bringIn = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 182:
                        this.preLeave = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 183:
                        this.bSpectatorsVoice = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 184:
                        this.ServerVersion = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 185:
                        this.canPreLeave = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 186:
                        this.operationRound = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 187:
                        this.isOrientationTribe = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 188:
                        this.serviceFeePer = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 189:
                        this.bCreditControl = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 190:
                        this.opTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 191:
                        this.userDelayTimes = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 192:
                        this.potNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 193:
                        this.firstBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 194:
                        this.lastPlayerBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 195:
                        this.round = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 196:
                        this.callnum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 197:
                        this.foldmum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 198:
                        this.roomMode = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
    }
    }

    public partial class REQ_GAME_SEND_SEAT_ACTION : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte action { get; set; }  //  1 坐下 2 站起  3 离开房间 4强制站起玩家 5强制踢出玩家 6提前离桌 7不补分立刻结束房间（大菠萝）
        public sbyte seatID { get; set; }  // 座位号
        public string longitude { get; set; }  // 定位lon
        public string latitude { get; set; }  // 定位lat
        public string clientIP { get; set; }  // 客户端IP，进房API有返回
        public int roomPath { get; set; }
        public int roomID { get; set; }

        // Response
        public sbyte status { get; set; }  // 状态，0操作成功 1座位已被别的玩家坐下 2有相同IP玩家 3游戏即将结束 4信用额度不足 7有GPS位置相近玩家 9强制站起玩家失败/强制踢出玩家失败 10强制站起玩家成功/强制踢出玩家成功 11提前离桌成功 12坐下失败，已提前离桌 13被冻结
        public sbyte actionRec { get; set; }  //  1 坐下 2 站起  3 离开房间 4被强制站起 5被强制踢出 7被管理员踢出战队，强制站起
        public int tableChip { get; set; }  // 自己带入牌桌的筹码
        public int leavelChips { get; set; }  // 玩家的USDT余额
        public sbyte seatId { get; set; }  // 座位号
        public int waitBlind { get; set; }  // 1需要弹出补盲，0不需要弹


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(7);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, action, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, seatID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(62, longitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(63, latitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(64, clientIP, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomID, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.actionRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.tableChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.leavelChips = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.seatId = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 133:
                        this.waitBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_SEND_ACTION : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte action { get; set; }  //  下注putchip = 1,跟注call = 2,加注raise = 3,全下allin = 4,让牌check = 5,弃牌fold = 6,超时timeout = 7
        public int roomPath { get; set; }
        public int roomID { get; set; }
        public int anteNumber { get; set; }  // 下注筹码数
        public int operationRound { get; set; }  // 当前操作轮数

        // Response
        public sbyte status { get; set; }  // 状态，0操作成功 1操作失败，重新弹操作框 6操作轮数和服务器不对应，重连


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(5);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, action, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, anteNumber, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, operationRound, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_ACTION : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Response
        public sbyte seatID { get; set; }  // 操作人座位号
        public sbyte action { get; set; }  // 下注putchip = 1,跟注call = 2,加注raise = 3,全下allin = 4,让牌check = 5,弃牌fold = 6,超时timeout = 7
        public sbyte operationID { get; set; }  // 下一个操作人座位号
        public int anteNumber { get; set; }  // 下注额
        public int playerID { get; set; }  // 操作人userID
        public int minAnteNum { get; set; }  // 操作后最小加注额
        public int canRaise { get; set; }  // 下一操作人右按钮是否加注
        public int callAmount { get; set; }  // 下一操作人跟注额
        public int operationRound { get; set; }  // 当前轮数
        public int potNumber { get; set; }  // pot值
        public int firstBet { get; set; }  // 第一个差额
        public int lastPlayerBet { get; set; }  // 上个玩家加注额
        public int round { get; set; }  // 是否第一轮0第一轮

        public int callnum { get; set; }  // call人数
        public int foldmum { get; set; }  // 弃牌人数
        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 61:
                        this.seatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.action = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 64:
                        this.operationID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.anteNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.playerID = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.callAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.operationRound = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 136:
                        this.potNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 137:
                        this.firstBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 138:
                        this.lastPlayerBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.round = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 140:
                        this.callnum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.foldmum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_LEAVE : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte seatID { get; set; }  // 座位号
        public int userID { get; set; }



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.seatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.userID = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_SEAT_DOWN : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte seatID { get; set; }  // 座位号
        public sbyte sex { get; set; }  // 性别 0男，1女
        public string headPic { get; set; }  // 头像id
        public string nick { get; set; }  // 昵称
        public int userID { get; set; }
        public int chips { get; set; }  // 该玩家牌桌上的筹码



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 61:
                        this.seatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.sex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.headPic = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.nick = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.userID = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.chips = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_CARDS : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte seatID { get; set; }  // 收到公共牌后第一个操作人座位号
        public List<sbyte> systemIDArray { get; set; }  // 各公共牌
        public int minAnteNum { get; set; }  // 最小加注额
        public int canRaise { get; set; }  // 操作人右按钮是否加注
        public string seeMoreCardsPlayer { get; set; }  // 如果是花钻看牌，返回花钻看牌的玩家昵称
        public int cardType { get; set; }  // 牌型（现在都改成客户端自己算牌型了，这个没用）
        public int operationRound { get; set; }  // 当前轮数

        public int potNumber { get; set; }  // pot值

        public int round { get; set; }  // 是否第一轮0第一轮
        public int callnum { get; set; }  // call人数
        public int foldmum { get; set; }  // 弃牌人数

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.seatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.systemIDArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.seeMoreCardsPlayer = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 134:
                        this.cardType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.operationRound = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 136:
                        this.potNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 137:
                        this.round = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 138:
                        this.callnum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.foldmum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_WINNER : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public List<sbyte> seatIDArray { get; set; }  // 收筹码玩家座位号，有可能1个或多个
        public List<int> playerArray { get; set; }  // 收筹码玩家userID
        public List<int> chipsArray { get; set; }  // 收筹码数
        public List<sbyte> cardTypesArray { get; set; }  // 赢牌类型 1皇家同花 2同花顺 3四条 4葫芦 5同花 6顺子 7三条 8两对 9一对 10高牌
        public List<sbyte> isWinArray { get; set; }  // 收筹码玩家是否是赢家 0是 1否
        public List<int> winChipsArray { get; set; }  // 赢家赢的筹码数
        public List<sbyte> cardSort { get; set; }  // 哪几张牌赢的 系统牌0-4 底牌5,6,7,8对应于第1，2，3，4张手牌
        public List<int> allPlayerchip { get; set; }  // 每个玩家最后显示筹码
        public List<int> allplayerID { get; set; }  // 每个玩家ID
        public List<sbyte> firstCardArray { get; set; }  // 每个玩家第一张牌 有牌0-51 无牌-1
        public List<sbyte> secondCardArray { get; set; }  // 每个玩家第二张牌 有牌0-51 无牌-1
        public List<sbyte> isMaxcardArray { get; set; }  // 是否是最大手牌 0是 1不是 未使用
        public int selfChips { get; set; }  // 自己的筹码数
        public List<sbyte> muckStatus { get; set; }  // 盖牌的状态 0未盖牌 1为盖牌
        public List<int> jackPotUpArray { get; set; }  // 向jackPot上交的金币数（用来做玩家到彩池的筹码动画，但这个动画取消了）
        public List<int> hitJackPotChipsArray { get; set; }  // 击中牌型获得的奖励金币数（大于0需要做玩家击中彩池的动画）


        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 130:
                        this.seatIDArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.playerArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 132:
                        this.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.cardTypesArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 134:
                        this.isWinArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 135:
                        this.winChipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.cardSort = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 137:
                        this.allPlayerchip = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 138:
                        this.allplayerID = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 140:
                        this.firstCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 141:
                        this.secondCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 142:
                        this.isMaxcardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 143:
                        this.selfChips = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 144:
                        this.muckStatus = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 147:
                        this.jackPotUpArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 148:
                        this.hitJackPotChipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_READYTIME : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte readyTime { get; set; }  // 没有具体数值，收到该协议后SNG和MTT会开始升盲倒计时



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.readyTime = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_START_INFOR : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte bankerID { get; set; }  // 庄家座位号
        public sbyte bigSeatID { get; set; }  // 大盲座位号
        public sbyte smallSeatID { get; set; }  // 小盲盲座位号
        public sbyte operationID { get; set; }  // 首位操作人座位号
        public List<sbyte> firstCardArray { get; set; }  // 各玩家第一个手牌
        public List<sbyte> SecondCardArray { get; set; }  // 各玩家第二个手牌
        public int smallChip { get; set; }  // 小盲
        public int bigChip { get; set; }  // 大盲
        public List<int> chipsArray { get; set; }  // 各玩家手上的筹码
        public List<int> timesArray { get; set; }  // 各玩家倒计时,未使用
        public List<int> unknow1 { get; set; }  // 未定义,未使用
        public int unknow2 { get; set; }  // 未定义,未使用
        public int unknow3 { get; set; }  // 未定义,未使用
        public int unknow4 { get; set; }  // 未定义,未使用
        public int minAnteNum { get; set; }  // 当前最小可加注额，操作按钮上的加注额要用到
        public int canRaise { get; set; }  // 操作人右按钮是否加注
        public List<int> canPlayStatus { get; set; }  // 是否打牌，0为不可以，1为可以
        public List<int> seatStatus { get; set; }  // 0为刚坐下，>=1显示气泡,未使用
        public int handNum { get; set; }  // 第几手，未使用
        public List<sbyte> extraBlind { get; set; }  // 各玩家是否补盲 0 1
        public int leftCreditValue { get; set; }  // 剩余信用额度
        public List<int> straddlePop { get; set; }  // straddle筹码
        public int isCurStraddle { get; set; }  // 0straddle成功， 1straddle失败，2不处理 （应该是以前的手动straddle模式用到，现在都是自动的）
        public int callAmount { get; set; }  // 跟注额，操作按钮上使用
        public List<int> initialBets { get; set; }  // 开始的时候玩家下注的筹码(SNG时会返回)
        public int operationRound { get; set; }  // 当前操作轮数
        public int potNumber { get; set; }  // pot值
        public int firstBet { get; set; }  // 第一个差额
        public int lastPlayerBet { get; set; }  // 上个玩家加注额
        public int round { get; set; }  // 是否第一轮0第一轮
        public int callnum { get; set; }  // call人数
        public int foldmum { get; set; }  // 弃牌人数



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.bankerID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.bigSeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.smallSeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 63:
                        this.operationID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.firstCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.SecondCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.smallChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.bigChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.timesArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.unknow1 = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 137:
                        this.unknow2 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 138:
                        this.unknow3 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.unknow4 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 140:
                        this.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.canPlayStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 143:
                        this.seatStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 144:
                        this.handNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 145:
                        this.extraBlind = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 146:
                        this.leftCreditValue = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.straddlePop = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 148:
                        this.isCurStraddle = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 149:
                        this.callAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 150:
                        this.initialBets = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 151:
                        this.operationRound = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 152:
                        this.potNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 153:
                        this.firstBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 154:
                        this.lastPlayerBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 155:
                        this.round = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 156:
                        this.callnum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 157:
                        this.foldmum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_ADD_CHIPS : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte seatID { get; set; }  // 座位号
        public sbyte unknow { get; set; }  // 未知含义，固定传1
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string clientIP { get; set; }  // 客户端IP，进房API返回
        public int anteNumber { get; set; }  // 带入筹码数
        public int roomPath { get; set; }
        public int roomId { get; set; }
        public int anteType { get; set; }  // 0:普通筹码，1：上次离开房间时的剩余筹码。 现在客户端都传0
        public int clubID { get; set; }  // 选择带入的俱乐部ID
        public int managerID { get; set; }  // 俱乐部管理员ID
        public string uuid { get; set; }  // 设备唯一识别码
        public string mac { get; set; }  // 设备Mac地址，拿不到的话传空
        public int simulator { get; set; }  // 是否模拟器 0 1
        public string locationName { get; set; }  // 定位到的地址名称

        // Response
        public sbyte status { get; set; }  // 0 带入成功 2 申请发送成功，等待房主确认 4 房主拒绝添加筹码的请求  6 有相同IP的玩家已坐下  7 信用额度不足 8 信用额度正在结算 9 游戏即将结束 10 有附近GPS玩家已坐下 11 占座玩家申请带入，头像下方进行倒计时 12 战队信用不足 13 钻石不足 14 带入战队的同盟发生了变更 15 黑名单玩家，无法带入 16 玩家所在俱乐部被联盟踢出
        public sbyte seatIDRec { get; set; }  // 座位号
        public int Chips { get; set; }  // 带入后玩家房间筹码
        public int playerID { get; set; }  // 用户id
        public int allChips { get; set; }  // 用户账户余额（USDT）
        public int leftSecs { get; set; }  // 带入审核倒计时
        public int leftCreditValue { get; set; }  // 用户信用额度
        public int clubInfo { get; set; }  // 错误12时要用到的文案，格式为：“同盟名称,社区名称”，用来组装该文案：xx社区在xx同盟积分不足，已拒绝带入请求


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(15);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, seatID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, unknow, stream);
            bodysize += CPHotfixMessageHelper.WriteField(62, longitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(63, latitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(64, clientIP, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, anteNumber, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, anteType, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, clubID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(135, managerID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(136, uuid, stream);
            bodysize += CPHotfixMessageHelper.WriteField(137, mac, stream);
            bodysize += CPHotfixMessageHelper.WriteField(138, simulator, stream);
            bodysize += CPHotfixMessageHelper.WriteField(139, locationName, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 61:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.seatIDRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.Chips = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.playerID = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.allChips = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.leftSecs = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.leftCreditValue = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.clubInfo = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_ENTER_ROOM : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte roomPath { get; set; }
        public sbyte roomStatus { get; set; }  // 无用，传0即可
        public int roomId { get; set; }  // 房间号

        // Response
        public sbyte status { get; set; }  // 0
        public List<int> playerIdArray { get; set; }  // 等待中的玩家id
        public string nickString { get; set; }  // 等待中的玩家昵称,以@%分割
        public string headPicString { get; set; }  // 等待中的玩家头像id,以@%分割
        public List<sbyte> playerSexArray { get; set; }  // 等待中的玩家性别


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, roomStatus, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 132:
                        this.playerIdArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.nickString = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 137:
                        this.headPicString = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 138:
                        this.playerSexArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_START_GAME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomPath { get; set; }
        public int roomId { get; set; }  // 房间号

        // Response
        public sbyte status { get; set; }  // 0 成功


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(2);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_ENDING : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0
        public string nickNameStr { get; set; }  // 昵称 土豪/MVP/大鱼 以@%分割
        public string userPortrait { get; set; }  // 头像 土豪/MVP/大鱼 以@%分割
        public List<int> userIdArray { get; set; }  // 房间所有用户ID
        public List<int> plArray { get; set; }  // 房间所有用户战绩
        public List<int> handArray { get; set; }  // 房间所有用户手数
        public List<int> bringArray { get; set; }  // 房间所有用户带入
        public int allBring { get; set; }  // 房间所有用户带入总和
        public string userNickname { get; set; }  // 房间所有用户昵称 以@%分割
        public int maxPot { get; set; }  // 该局最大池底
        public int insurance { get; set; }  // 该局是否开启保险 0 1
        public int insuranceSum { get; set; }  // 该局保险总和
        public int finalFund { get; set; }  // 总基金 未使用
        public int playTime { get; set; }  // 游戏时长(s)
        public string jackPotUserNickname { get; set; }  // 所有击中JackPot用户昵称 以@%分割
        public List<int> jackPotUserIdArray { get; set; }  // 所有击中JackPot用户ID
        public List<int> jackPotCardTypeArray { get; set; }  // 所有JackPot用户击中牌型
        public List<int> jackPotRewardArray { get; set; }  // 所有JackPot用户奖励
        public int vpOn { get; set; }  // 是否打开入池率统计
        public List<int> vpArray { get; set; }  // 房间所有用户的vp值

        public int fee { get; set; }  //服务费



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.nickNameStr = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.userPortrait = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.userIdArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.plArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.handArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.bringArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.allBring = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 137:
                        this.userNickname = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 138:
                        this.maxPot = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.insurance = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 140:
                        this.insuranceSum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.finalFund = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.playTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 143:
                        this.jackPotUserNickname = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 144:
                        this.jackPotUserIdArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 145:
                        this.jackPotCardTypeArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 146:
                        this.jackPotRewardArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 147:
                        this.vpOn = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 148:
                        this.vpArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 149:
                        this.fee = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_TAKE_CONTROL : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte bControl { get; set; }  //  0：关闭 1：开启
        public int roomPath { get; set; }
        public int roomId { get; set; }  // 房间号

        // Response
        public sbyte status { get; set; }  // 0 成功
        public sbyte bControlRec { get; set; }  // 设置后状态 0：关闭 1：开启
        public int roomPathRec { get; set; }
        public int roomIdRec { get; set; }  // 房间号


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, bControl, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.bControlRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 131:
                        this.roomPathRec = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.roomIdRec = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_CONTROL_RANGE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomId { get; set; }  // 房间号
        public int roomPath { get; set; }
        public int minTimes { get; set; }  // 最小带入倍数
        public int maxTimes { get; set; }  // 最大带入倍数

        // Response
        public sbyte status { get; set; }  // 0 成功
        public int minTime { get; set; }  // 最小带入倍数
        public int maxTime { get; set; }  // 最大带入倍数


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(4);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, minTimes, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, maxTimes, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.minTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.maxTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_PLAYER_CARDS : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public List<sbyte> playerSeat { get; set; }  // 所有Allin玩家座位号
        public List<sbyte> firstCards { get; set; }  // 玩家第一张手牌
        public List<sbyte> secondCards { get; set; }  // 玩家第二张手牌



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 130:
                        this.playerSeat = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.firstCards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.secondCards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_SHOWDOWN : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int pokernum { get; set; }  // 0不亮 1亮第一张 2亮第二张 3亮两张

        // Response
        public sbyte status { get; set; }  // 0 成功
        public sbyte pokerstates { get; set; }  // 0不亮 1亮第一张 2亮第二张 3亮两张


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(131, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, pokernum, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.pokerstates = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_JACKPOT_SHOW_CARDS: ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Response
        public sbyte seatId { get; set; }  // 座位下标
        public sbyte firstCard { get; set; }  // 第一张手牌
        public sbyte secondCard { get; set; }  // 第二张手牌

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.seatId = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.firstCard = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.secondCard = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_SYNC_SIGNAL_RESOURCE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        // 无参数

        // Response
        public sbyte status { get; set; }  // 0 成功
        public int onlineCount { get; set; }  // 未使用


        public ushort SerializeTo(MemoryStream stream)
        {
            return 0;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.onlineCount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_SYNC_SIGNAL_GAME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        // 无参数

        // Response
        public sbyte status { get; set; }  // 0 成功 1 错误，需要重连


        public ushort SerializeTo(MemoryStream stream)
        {
            return 0;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_OFFLINE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        // 无参数

        // Response
        public sbyte status { get; set; }  // 0 成功


        public ushort SerializeTo(MemoryStream stream)
        {
            return 0;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_PRE_START_GAME : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte dealerIndex { get; set; }  // 庄家座位号
        public int isStraddle { get; set; }  // 是否开启straddle
        public int isCurStraddle { get; set; }  // 当前玩家是否需要操作straddle（现在都是自动straddle，所以忽略这个）
        public List<int> canPlayStatus { get; set; }  // 各个玩家本手是否可以参与游戏的状态 1为可玩
        public List<int> seatStatus { get; set; }  // 各玩家座位状态 0: 刚坐下， >=1: 显示气泡
        public List<int> playerChips { get; set; }  // 未使用
        public int straddleCost { get; set; }  // 未使用
        public int straddleStatus { get; set; }  // 未使用



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.dealerIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.isStraddle = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.isCurStraddle = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.canPlayStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.seatStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.playerChips = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.straddleCost = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 136:
                        this.straddleStatus = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_TRUST_ACTION : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public sbyte option { get; set; }  // 操作 0取消托管 1托管 2进入房间时请求所有玩家的托管状态 3托管时间到 4其它房间状态查询
        public sbyte initiative { get; set; }  // 主动点击托管或取消托管时传1，其他传0

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public sbyte optionRec { get; set; }  // 0取消托管 1托管 2进入房间时请求所有玩家的托管状态 3托管时间到 4其它房间状态查询 5服务器主动下发其他玩家的托管状态
        public int trustTime { get; set; }  // 倒计时时间，单位S
        public int firstSit { get; set; }  // 是否第一次坐下，0不是，1是
        public List<int> trustStatus { get; set; }  // 托管状态数组 0没托管 1托管
        public int isTrusted { get; set; }  // 是否有在其他房间被托管
        public string roomName { get; set; }  // 在其他房间托管中的房间名
        public int underTrust { get; set; }  // 是否处于主动托管


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(4);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, option, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, initiative, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.optionRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.trustTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.firstSit = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.trustStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.isTrusted = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.roomName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 135:
                        this.underTrust = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_FINISHGAME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }

        // Response
        public sbyte status { get; set; }  // 0成功 1失败


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(2);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_CHECK_CAN_CICK : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int userID { get; set; }  // 被查询人的userID

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public sbyte canKickOut { get; set; }  // 0:没有权限，1：有踢出站起权限
        public int userIDRec { get; set; }  // 被查询人的userID


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, userID, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.canKickOut = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 131:
                        this.userIDRec = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_JACKPOT_CHANGE : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public string jackPotNum { get; set; }  // 总JackPot值
        public string subJackPotNum { get; set; }  // 当前房间子JackPot值



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.jackPotNum = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.subJackPotNum = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_START_ROOM : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte roomPath { get; set; }
        public sbyte roomStatus { get; set; }  // 0 如果是房主代表解散房间，如果是普通玩家代表退出等待页 3 开始
        public int roomId { get; set; }

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public sbyte roomStatusRec { get; set; }  // 0 如果是房主代表解散房间，如果是普通玩家代表退出等待页 3开始


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, roomStatus, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.roomStatusRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_REAL_TIME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public List<int> userIdArray { get; set; }  // 用户ID
        public string headStr { get; set; }  // 用户头像,@%分割
        public string nickNameStr { get; set; }  // 用户昵称,@%分割
        public List<int> plArray { get; set; }  // 净胜筹码
        public List<int> bringArray { get; set; }  // 玩家总带入
        public List<int> watcherIdArray { get; set; }  // 观众用户ID
        public string watcherHeadStr { get; set; }  // 观众头像,@%分割
        public string watcherNickname { get; set; }  // 观众昵称,@%分割
        public List<sbyte> playingPersonArray { get; set; }  // 用户状态 0在玩 1不在 2提前离桌
        public List<sbyte> watcherSexArray { get; set; }  // 观众性别 0男 1女
        public int insuranceSum { get; set; }  // 保险额
        public int commission { get; set; }  // 基金额，未使用
        public int hideInsurance { get; set; }  // 隐藏保险条目（0 不隐藏  1 隐藏）
        public List<int> handsArray { get; set; }  // 手数
        public List<sbyte> watcherLeaveRoomArray { get; set; }  // 观众是否离开房间   1已离开  0在房
        public int roomLeftTime { get; set; }  // 牌局剩余时间（秒）


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(2);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.userIdArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 131:
                        this.headStr = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.nickNameStr = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 133:
                        this.plArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.bringArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.watcherIdArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.watcherHeadStr = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 137:
                        this.watcherNickname = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 138:
                        this.playingPersonArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 139:
                        this.watcherSexArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 140:
                        this.insuranceSum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.commission = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.hideInsurance = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 143:
                        this.handsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 144:
                        this.watcherLeaveRoomArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 145:
                        this.roomLeftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_ADD_TIME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int time { get; set; }  // 加的时间，现在是写死20秒
        public int diamonds { get; set; }  // 花费的钻石，公式：10 * pow(2, count + 1);count为第几次点加时
        public int seatId { get; set; }  // 座位号
        public int coins { get; set; }  // 花费的USDT，0

        // Response
        public sbyte status { get; set; }  // 0 成功 1 失败 2钻石不足
        public int playerId { get; set; }  // 用户ID，检查一下是不是自己的，不是的话就是别人在加时
        public int addTime { get; set; }  // 加的时间
        public int coastJewel { get; set; }  // 花费的钻石
        public int coastCoins { get; set; }  // 花费的USDT


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(6);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, time, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, diamonds, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, seatId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(135, coins, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.playerId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.addTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.coastJewel = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.coastCoins = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_PAUSE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte bPause { get; set; }  // 0：继续 1：暂停
        public int roomID { get; set; }
        public int roomPath { get; set; }

        // Response
        public sbyte isPause { get; set; }  // 0 继续 1 暂停 2 操作失败
        public int remainingTime { get; set; }  // 暂停剩余时间  秒


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, bPause, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.isPause = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.remainingTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_PREV_GAME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int handId { get; set; }  // 第几手，传-1表示最近一手

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public sbyte isCollected { get; set; }  // 0：未收藏 1：已收藏
        public string paipu { get; set; } //牌谱
        public string paipuinfo { get; set; } //牌谱明细
        public string showdown { get; set; } //牌谱结算明细
        public int totalHandsNumber { get; set; }  // 总手数
        public int currentHandId { get; set; }  // 当前手数
        public string blind { get; set; } //盲注

        public int playermum { get; set; } //最大人数
        public List<int> playerIds { get; set; }  // 玩家id
        public string userNames { get; set; }  // 玩家昵称，@%分割
        public string headPics { get; set; }  // 玩家头像，@%分割
        public List<sbyte> firstCard { get; set; }  // 第一张手牌
        public List<sbyte> secondCard { get; set; }  // 第二张手牌
        public List<sbyte> publicCard { get; set; }  // 公共牌
        public List<sbyte> maxCardIndex { get; set; }  // 最大牌型的组成的牌的Index，比如某玩家的是0，1，3，5，6代表第12467张牌要高亮
        public List<sbyte> maxCardType { get; set; }  // 最大牌型，-1:无牌型 0:弃牌 1:皇家同花顺  2:同花顺 3:4条 4:葫芦 5:同花 6:顺子 7:三张 8:两对 9:一对 10:高牌
        public List<int> winAntes { get; set; }  // 输赢筹码数
        public int insuranceSum { get; set; }  // 保险池
        public List<int> insuranceGains { get; set; }  // 每个用户保险额
        public List<sbyte> operationTypes { get; set; }  // 每轮的操作(4 * n个数，依次为第1个人的第一手操作，第1个人的第二手操作……)，每个数：-1表示前一轮已经弃牌或者ALL IN，其余情况最高1位表示本轮是否弃牌（1表示弃牌，0表示进行到下一轮），剩余7位（0表示直接弃牌，1表示看牌，2表示跟注，3表示加注，4表示ALL IN
        public List<int> operationAntes { get; set; }  // 每轮操作的筹码数
        public List<sbyte> playerTypes { get; set; }  // 用户类型，0普通人，1庄家，2小盲，3即是小盲又是庄家，4大盲
        public string playbackLink { get; set; }  // 当前局的回放链接
        public int commission { get; set; }  // 基金额，未使用
        public List<sbyte> thirdCard { get; set; }  // 第3张手牌(奥马哈)
        public List<sbyte> fourthCard { get; set; }  // 第4张手牌(奥马哈)
        public int jackPotSum { get; set; }  // JackPot总额


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, handId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.isCollected = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 131:
                        this.paipu = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.paipuinfo = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 133:
                        this.roomID = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.roomPath = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.blind = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 136:
                        this.playermum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 137:
                        this.showdown = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 130:
                        this.playerIds = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    //case 131:
                    //    this.userNames = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                    //    break;
                    //case 132:
                    //    this.headPics = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                    //    break;
                    //case 133:
                    //    this.firstCard = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 134:
                    //    this.secondCard = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 135:
                    //    this.publicCard = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 136:
                    //    this.maxCardIndex = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 137:
                    //    this.maxCardType = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 138:
                    //    this.winAntes = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                    //    break;
                    case 139:
                        this.insuranceSum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    //case 140:
                    //    this.insuranceGains = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                    //    break;
                    //case 141:
                    //    this.operationTypes = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 142:
                    //    this.operationAntes = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                    //    break;
                    //case 143:
                    //    this.playerTypes = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    case 144:
                        this.totalHandsNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 145:
                        this.currentHandId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    //case 146:
                    //    this.playbackLink = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                    //    break;
                    //case 147:
                    //    this.commission = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                    //    break;
                    //case 148:
                    //    this.thirdCard = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 149:
                    //    this.fourthCard = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                    //    break;
                    //case 150:
                    //    this.jackPotSum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                    //    break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_COLLECT_CARD : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte collect { get; set; }  // 0 取消收藏 1收藏
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int handId { get; set; }  // 需收藏第几手的牌

        // Response
        public sbyte status { get; set; }  // 0成功 1超过上限 2其他错误
        public sbyte collectRec { get; set; }  // 收藏还是取消收藏 0 取消收藏 1收藏
        public int handIdRec { get; set; }  // 第几手


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(4);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(61, collect, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, handId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 61:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.collectRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.handIdRec = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_SHOW_SIDE_POTS : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public List<int> pots { get; set; }  // 各分池筹码数



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 130:
                        this.pots = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_WAIT_BLIND : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public sbyte option { get; set; }  // 0过庄，1补盲 （其实只有选择补盲，过庄应该是自动的，后台会下发）

        // Response
        public sbyte status { get; set; }  // 0成功
        public sbyte optionRec { get; set; }  // 0过庄，1补盲


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, option, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.optionRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_INSURANCE_TRIGGED : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte count { get; set; }  // 当前触发保险次数，如果<=1，则先显示“保险模式”动画，再弹出保险框
        public List<int> players { get; set; }  // 参与保险人列表
        public List<int> playerStatus { get; set; }  // 各保险人状态（0 正购买 1 已购买 2 已放弃购买）
        public List<int> playerInsuredAmount { get; set; }  // 各保险人投保额
        public List<int> timeLeft { get; set; }  // 各保险人剩余时间(单位：秒，对应所有可购买的人，状态不是正在购买的，值为0)
        public List<sbyte> subPots { get; set; }  // 当前人可购分池下标列表
        public List<int> outs { get; set; }  // OUTS列表[两个分池OUTS用-1隔开]（注意连续的-1和结尾或起始为-1情况）
        public string odds { get; set; }  // 赔率数组（字符串，","隔开）
        public List<int> leastAmount { get; set; }  // 最低投保额列表
        public List<int> mostAmount { get; set; }  // 最高投保额列表
        public List<int> secureAmount { get; set; }  // 分池保本额度数组
        public List<sbyte> potAllowOutSelections { get; set; }  // 是否允许选择部分保险, 0表示不允许选择，其他值表示允许选择
        public List<int> potTotalCosts { get; set; }  // 包含上轮未中保险的已投入游戏币金额
        public string userNames { get; set; }  // @% 分隔的所有用户名 @%@% 分隔不同池（不包含自己）
        public List<int> outsPerUser { get; set; }  // 各对应用户对于玩家的OUTS（不包含自己，与用户数一致）
        public List<int> myChipsInPool { get; set; }  // 自己在各个池中有多少钱
        public List<int> playerCards { get; set; }  // 各玩家手牌（不包含自己，为用户数的2倍或4倍），依次表示第一个玩家第一张牌、第二张牌……第二个玩家第一张牌……
        public int existEqualOuts { get; set; }  // 0 转牌  1 河牌存在平分Outs  2 河牌不存在平分Outs
        public int insuranceDelayTimes { get; set; }  // 投保人购买保险延时次数



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.count = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.players = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 131:
                        this.playerStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 132:
                        this.playerInsuredAmount = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.timeLeft = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.subPots = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 135:
                        this.outs = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.odds = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 137:
                        this.leastAmount = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 138:
                        this.mostAmount = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 139:
                        this.secureAmount = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 140:
                        this.potAllowOutSelections = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 141:
                        this.potTotalCosts = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 142:
                        this.userNames = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 143:
                        this.outsPerUser = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 144:
                        this.myChipsInPool = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 145:
                        this.playerCards = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 146:
                        this.existEqualOuts = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.insuranceDelayTimes = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_BUY_INSURANCE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int seatId { get; set; }  // 座位号
        public int buy { get; set; }  // 是否购买保险，0不买 1买 只有buy为1时才需要传下面的134-136字段
        public List<sbyte> pots { get; set; }  // 购买的分池下标
        public List<int> potInsureAmount { get; set; }  // 各分池购买的保险额
        public List<int> insuredCards { get; set; }  // 内容为每张牌的花色值，-1隔开每个池的购买，必须与上面的134字段中的投保池索引对应，起始和结尾不要-1

        // Response
        public sbyte status { get; set; }  // 0成功 1、失败 、2强制背保XXX（132字段的值） 3、转牌保险投保额不得超过0.45*底池，系统自动购买最大值XXX（132字段的值） 4 河牌保险投保额不得超过0.5*底池，系统自动购买最大值XXX 5 房间未处于保险激活状态 不允许购买
        public sbyte seatIdRec { get; set; }  // 座位号
        public int userId { get; set; }  // 用户id
        public int totalInsuredAmount { get; set; }  // 总购买保险额
        public int autoInsuredAmount { get; set; }  // 自动购买的保险额


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(7);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, seatId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, buy, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, pots, stream);
            bodysize += CPHotfixMessageHelper.WriteField(135, potInsureAmount, stream);
            bodysize += CPHotfixMessageHelper.WriteField(136, insuredCards, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.seatIdRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.userId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.totalInsuredAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.autoInsuredAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_CLAIM_INSURANCE : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public List<int> seatIds { get; set; }  // 赔付的座位号
        public List<int> userIds { get; set; }  // 赔付的用户ID
        public List<int> claimAmounts { get; set; }  // 赔付金额



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 130:
                        this.seatIds = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 131:
                        this.userIds = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 132:
                        this.claimAmounts = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_INSURANCE_ADD_TIME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int time { get; set; }  // 加的时间，现在是写死20秒
        public int costDiamonds { get; set; }  // 花费的钻石，公式：10 * pow(2, count + 1);count为第几次点加时
        public int seatId { get; set; }  // 座位号
        public int coins { get; set; }  // 花费的USDT，0

        // Response
        public sbyte status { get; set; }  // 0 成功 1 失败 2钻石不足
        public int playerId { get; set; }  // 用户ID，检查一下是不是自己的，不是的话就是别人在加时
        public int addTime { get; set; }  // 加的时间
        public int coastJewel { get; set; }  // 花费的钻石
        public int coastCoins { get; set; }  // 花费的USDT


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(6);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, time, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, costDiamonds, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, seatId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(135, coins, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.playerId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.addTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.coastJewel = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.coastCoins = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_INSURANCE_NOT_TRIGGED : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        // 客户端端未解析内容



        public object DeserializeFrom(MemoryStream stream)
        {
            return this;
        }
    }

    public partial class REQ_SEE_MORE_PUBLIC_ACTION : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte recvCardsNum { get; set; }  // 当前已经能看到的公共牌数量
        public int roomPath { get; set; }
        public int roomID { get; set; }

        // Response
        public sbyte status { get; set; }  // 0 成功 1 失败 2操作超时
        public int chips { get; set; }  // 用户的最新USDT数


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, recvCardsNum, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomID, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.chips = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_CURRENT_ROUND_FINISH : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_WAIT_DAIRU : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public int userId { get; set; }  // 用户id
        public int seatId { get; set; }  // 座位号
        public int leftTime { get; set; }  // 留座剩余时间（s）


        public ushort SerializeTo(MemoryStream stream)
        {
            return 0;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.userId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.seatId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.leftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_KEEP_SEAT : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte option { get; set; }  // 1 玩家主动点留座离桌（该功能已删除，不能主动留座离桌） 2 查询是否在其他房间有留座（如果进房或托管协议返回该用户在其他房间有留座，坐下前需要先调这个协议请求留座的情况）
        public int roomPath { get; set; }
        public int roomID { get; set; }

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public int userId { get; set; }  // 用户id
        public sbyte optionRec { get; set; }  // 1 玩家主动点留座离桌操作，已废弃 2 查询是否在其他房间有留座，弹提示“您在"xxx"牌局中处于留座状态，无法加入本局游戏” 3 服务器主动下发留座玩家倒计时（如果是自己，则检查是否筹码不足，弹出带入框）
        public int seatId { get; set; }  // 座位号
        public int leftTime { get; set; }  // 留座剩余时间（s）
        public string roomName { get; set; }  // 托管中的房间名


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, option, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomID, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.userId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.optionRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 132:
                        this.seatId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.leftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.roomName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_ADD_ROOM_TIME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int minutes { get; set; }  // 加时多少分钟

        // Response
        public sbyte status { get; set; }  // 0 成功 1 失败
        public int minutesRec { get; set; }  // 加的时间
        public int jewels { get; set; }  // 花费的钻石


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(131, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, minutes, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.minutesRec = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.jewels = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_PRE_START_GAME_STRADDLE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int bStraddle { get; set; }  // 0 关闭 1 开启

        // Response
        public sbyte status { get; set; }  // 0 成功
        public sbyte straddle { get; set; }  // 0 关闭 1 开启


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(131, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, bStraddle, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.straddle = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_PRE_START_GAME_MUCK : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int bMuck { get; set; }  // 0 关闭 1 开启

        // Response
        public sbyte status { get; set; }  // 0 成功
        public sbyte bMuckRec { get; set; }  // 0 关闭 1 开启


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(131, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, bMuck, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.bMuckRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_RECV_SPECTATORS_VOICE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int bSpectatorsVoice { get; set; }  // 0 关闭 1 开启

        // Response
        public sbyte status { get; set; }  // 0 成功
        public sbyte spectatorsVoice { get; set; }  // 0 关闭 1 开启


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(131, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, bSpectatorsVoice, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.spectatorsVoice = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_REMIND : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0 成功
        public int mttId { get; set; }  // 比赛的房间id
        public int mttType { get; set; }  // 6/9人局
        public string mttName { get; set; }  // 比赛名称
        public string ip { get; set; }  // 房间服务器ip
        public int port { get; set; }  // 房间服务器port
        public int enterType { get; set; }  // 0 是开赛通知 1是延时报名进入房间的通知



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.mttId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.mttType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.mttName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 133:
                        this.ip = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 134:
                        this.port = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.enterType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_Self_Ranking : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0 成功
        public int totalRank { get; set; }  // 总排名
        public int currentRank { get; set; }  // 参赛者的排名



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.totalRank = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.currentRank = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_REAL_TIME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int currentPage { get; set; }  // 当前页数

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public int currentRank { get; set; }  // 参赛者自己的排名
        public List<int> allRank { get; set; }  // 所有参赛者的排名
        public List<int> allUserId { get; set; }  // 所有用户的列表
        public List<int> allCoin { get; set; }  // 所有用户USDT
        public int allLoser { get; set; }  // 已经淘汰的用户数
        public string allNickName { get; set; }  // 所有用户的昵称,@%分割
        public int playerNum { get; set; }  // 总人数
        public int updateTime { get; set; }  // 升盲时间
        public int currentBlindLevel { get; set; }  // 当前盲注级别
        public int currentHand { get; set; }  // 当前盲注手数
        public int playedTime { get; set; }  // 牌局进行时间
        public int blindScale { get; set; }  // 盲注表倍数
        public int rebuyTime { get; set; }  // 剩余重购次数
        public string allHuntGold { get; set; }  // 所有用户猎人赏金，用@%分割
        public int rebuyPlayer { get; set; }  // 重购人数
        public int currentPageRec { get; set; }  // 当前页码
        public int totalPage { get; set; }  // 总页数
        public int totalScore { get; set; }  // 总USDT
        public List<int> allDesk { get; set; }  // 所有用户牌桌号
        public int totalReward { get; set; }  // 总奖池
        public int raiseBlindTime { get; set; }  // 升盲剩余时间
        public int hunterReward { get; set; }  // 猎人奖励
        public int myScore { get; set; }  // USDT
        public int totalRebuyTime { get; set; }  // 总重购次数
        public sbyte bInTheGame { get; set; }  // 0否 1是  自己是否参赛
        public int hunterKingId { get; set; }  // 猎人王用户id


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, currentPage, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.currentRank = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.allRank = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 132:
                        this.allUserId = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.allCoin = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.allLoser = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.allNickName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 136:
                        this.playerNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 137:
                        this.updateTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.currentBlindLevel = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 140:
                        this.currentHand = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.playedTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.blindScale = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 143:
                        this.rebuyTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 144:
                        this.allHuntGold = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 145:
                        this.rebuyPlayer = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 146:
                        this.currentPageRec = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.totalPage = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 148:
                        this.totalScore = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 149:
                        this.allDesk = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 150:
                        this.totalReward = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 151:
                        this.raiseBlindTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 152:
                        this.hunterReward = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 153:
                        this.myScore = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 154:
                        this.totalRebuyTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 155:
                        this.bInTheGame = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 156:
                        this.hunterKingId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_SYSTEM_NOTIFY : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0
        public int type { get; set; }  //                 消息类型1、拆分桌；2、升盲；3、奖池变动；4、实时战况；5、人员加入；6、人员离开       7、进入奖励圈；8、进入决赛桌     9、观众被分桌   10、进入前圈+1
        public string content { get; set; }  // 消息内容，对应不同Type,1、空；2、盲注级别；3、奖池总值；4、分钟和人数；5、昵称；6、昵称            7、空；8、空                  9、空         10、空



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.type = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.content = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_MTT_GAME_REBUY_REQUEST : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte operate { get; set; }  // 固定传1，未知含义
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public string longitude { get; set; }  // 坐标long
        public string latitude { get; set; }  // 坐标lat
        public string uuid { get; set; }  // 设备唯一识别码

        // Response
        public sbyte status { get; set; }  // 0 重购成功 1 服务器异常 2 前一个重购申请中 3 USDT不足 4 钻石不足 5 申请成功，正在审核 6 报名券不足 7 重购关闭 8 重购请求被拒绝 9 战队在公会信用不足 10 拆分桌中，无法重购
        public int rebuyTime { get; set; }  // 剩余重购次数
        public string clubInfo { get; set; }  // 错误9时要用到的文案，格式为：“同盟名称,社区名称”，用来组装该文案：xx社区在xx同盟积分不足，已拒绝带入请求


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(6);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, operate, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, longitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, latitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, uuid, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.rebuyTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 135:
                        this.clubInfo = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_FINAL_RANK : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0 游戏结束，没有重购机会了，直接显示结束名次页 2 弹出重购提示框
        public int ranking { get; set; }  // 排名
        public string giftReward { get; set; }  // 奖励礼品，如“100元京东卡”
        public string chipsReward { get; set; }  // USDT奖励
        public string hunterReward { get; set; }  // 猎人奖励



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.ranking = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.giftReward = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.chipsReward = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 133:
                        this.hunterReward = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_MTT_GAME_REBUY_CANCEL : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }

        // Response
        // 客户端未接受响应，需要问一下后台同事有没有响应


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(2);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            return this;
        }
    }

    public partial class REQ_MTT_APPLY_JOIN : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte operate { get; set; }  // 固定传1，未知含义
        public string longitude { get; set; }  // 坐标long
        public string latitude { get; set; }  // 坐标lat
        public string clientIP { get; set; }  // 客户端IP地址
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int clubID { get; set; }  // 选择带入的俱乐部
        public string uuid { get; set; }  // 设备唯一识别码

        // Response
        public sbyte status { get; set; }  // 0:请求成功，判断result 1:MTT未开放报名 2: MTT不存在,3: 赛事还未到报名时间，暂时不可报名,4: 报名人数已经达到上限，请选择其他赛事,5: 延时报名条件不足,6: 等待开赛(1-5min)
        public sbyte result { get; set; }  // 1: 报名券不足 2: 游戏币不足 3: 报名被拒绝2次, 4: 开赛前1分钟，没开延时报名的比赛，报名已经截止, 5: 报名人数达到上限,无法报名 6: 当前已经开赛且延时报名开关关闭或不满足延时报名的条件 7: 等待开赛 8: 立即进入 9: 开了控制带入，等待审核 10:开了延时报名,开赛前1min，人数小于开赛人数下限，不能报名 11:其他异常导致报名失败； 12:GPS 限制，没法报名； 13:IP 限制，没法报名； 14:战队在公会信用不足
        public string ipString { get; set; }  // 游戏服务ip
        public string port { get; set; }  // 游戏服务端口
        public string clubInfo { get; set; }  // 错误14时要用到的文案，格式为：“同盟名称,社区名称”，用来组装该文案：xx社区在xx同盟积分不足，已拒绝带入请求


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(8);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(61, operate, stream);
            bodysize += CPHotfixMessageHelper.WriteField(62, longitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(63, latitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(64, clientIP, stream);
            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, clubID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, uuid, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.result = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.ipString = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.port = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 135:
                        this.clubInfo = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_MTT_REBUY_REQUEST_OUTSIDE : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public string longitude { get; set; }  // 坐标long
        public string latitude { get; set; }  // 坐标lat
        public string uuid { get; set; }  // 设备唯一识别码

        // Response
        public sbyte status { get; set; }  // 0 重购成功 1 服务器异常 2 前一个重购申请中 3 USDT不足 4 钻石不足 5 申请成功，正在审核 6 报名券不足 7 重购关闭 8 重购请求被拒绝 9 战队在公会信用不足 10 拆分桌中，无法重购
        public string clubInfo { get; set; }  // 错误9时要用到的文案，格式为：“同盟名称,社区名称”，用来组装该文案：xx社区在xx同盟积分不足，已拒绝带入请求


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(5);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, longitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, latitude, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, uuid, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 135:
                        this.clubInfo = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_MTT_PLAYER_LIST : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int page { get; set; }
        public string searchWork { get; set; }  // 搜索关键词
        public int clubID { get; set; }  // 俱乐部ID

        // Response
        public sbyte status { get; set; }  // 0
        public string jsonInfo { get; set; }  // Json字符串

        public ResponseData response { get; set; }
        public sealed class ResponseData
        {
            public int participants { get; set; }  // 已报名人数
            public string totalHunterNum { get; set; }  // 总猎人数
            public int totalHunterMoney { get; set; }  // 总猎人金额
            public List<PlayerListElement> playerList { get; set; }  // 玩家列表
        }

        public sealed class PlayerListElement
        {
            public string rank { get; set; }  // 排名
            public string nickName { get; set; }  // 昵称
            public int rebuyTimes { get; set; }  // 重购次数
            public string hunterMoney { get; set; }  // 猎人金
            public string scoreBoard { get; set; }  // 记分牌
            public string userId { get; set; }
            public int roomNum { get; set; }
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }

        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(4);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, page, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, searchWork, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, clubID, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.jsonInfo = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        this.response = REQ_MTT_PLAYER_LIST.Response(this.jsonInfo);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_ENTER_ROOM : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte type { get; set; }  // 1
        public sbyte connect { get; set; }  // 1
        public sbyte unknow { get; set; }  // 未知含义，传1
        public int roomPath { get; set; }  // 普通局:61 MTT:71 SNG:81 奥马哈:91 大菠萝:51
        public int roomID { get; set; }  // 房间号
        public int deskId { get; set; }  // MTT分桌号

        // Response
        public REQ_GAME_ENTER_ROOM baseMsg = new REQ_GAME_ENTER_ROOM();
        public int currentRank { get; set; }  // 用户当前排名
        public List<int> loserRanks { get; set; }  // 淘汰的人的排名，未使用
        public int mttUpBlindLeftTime { get; set; }  // MTT升盲倒计时
        public int resideMinTime { get; set; }  // 倒计时时间（还剩1min内），已开始为-1
        public int blindType { get; set; }  // 盲注表0为A表(普通)，1为B表（快速）
        public int rebuyLevel { get; set; }  // 重购的级别, 0为关
        public int rebuyCount { get; set; }  // 最大可重购次数, 0为关
        public int isAppend { get; set; }  // 增购开关0为关，1为开
        public int raiseBlindTime { get; set; }  // 升盲的时间，单位分钟
        public int matchCost { get; set; }  // 参赛的费用
        public int rebuyResideCount { get; set; }  // 重购剩余的次数
        public int subRoomId { get; set; }  // MTT分房间号，已弃用
        public int rebuyScore { get; set; }  // 重购所得计分牌
        public int rebuyLeftTime { get; set; }  // 重购所剩时间
        public string rebuyCost { get; set; }  // 重购所需要的门票或筹码
        public int blindScale { get; set; }  // 盲注表的倍数（1 - 7）
        public sbyte huntMode { get; set; }  // 1为猎人赛 0不是
        public sbyte inRewardCircle { get; set; }  // 是否进入奖励圈+1
        public int deskIdRec { get; set; }  // MTT分桌号


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(6);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, type, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, connect, stream);
            bodysize += CPHotfixMessageHelper.WriteField(62, unknow, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, deskId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.baseMsg.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.baseMsg.gamestatus = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.baseMsg.countdown_times = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 65:
                        this.baseMsg.bigIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 66:
                        this.baseMsg.smallIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 67:
                        this.baseMsg.bankerIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 68:
                        this.baseMsg.operationID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 69:
                        this.baseMsg.speed = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 70:
                        this.baseMsg.mySeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 71:
                        this.baseMsg.maxCheckTimes = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 72:
                        this.baseMsg.currentCheckTimes = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 131:
                        this.baseMsg.cards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.baseMsg.playerIDArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.baseMsg.statusArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.baseMsg.anteArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.baseMsg.nick = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 136:
                        this.baseMsg.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 137:
                        this.baseMsg.headPicString = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 138:
                        this.baseMsg.firstCardarray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 139:
                        this.baseMsg.SecondArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 140:
                        this.baseMsg.bigBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.baseMsg.smallBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.baseMsg.alreadAnte = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 143:
                        this.baseMsg.maxAnte = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 144:
                        this.baseMsg.roomOwner = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 146:
                        this.baseMsg.bControl = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 147:
                        this.baseMsg.userChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 148:
                        this.baseMsg.nowChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 149:
                        this.currentRank = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 151:
                        this.loserRanks = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 152:
                        this.baseMsg.leftOperateTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 153:
                        this.baseMsg.sexArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 154:
                        this.baseMsg.showCardsId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 155:
                        this.baseMsg.groupBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 156:
                        this.baseMsg.pots = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 157:
                        this.baseMsg.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 158:
                        this.baseMsg.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 159:
                        this.baseMsg.leftCreditValue = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 160:
                        this.baseMsg.isTrusted = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 161:
                        this.baseMsg.callAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 162:
                        this.mttUpBlindLeftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 163:
                        this.baseMsg.isIpRestrictions = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 164:
                        this.baseMsg.isGPSRestrictions = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 165:
                        this.resideMinTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 166:
                        this.blindType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 167:
                        this.rebuyLevel = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 168:
                        this.rebuyCount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 169:
                        this.isAppend = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 170:
                        this.raiseBlindTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 171:
                        this.matchCost = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 172:
                        this.rebuyResideCount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 173:
                        this.subRoomId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 174:
                        this.rebuyScore = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 175:
                        this.rebuyLeftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 176:
                        this.rebuyCost = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 177:
                        this.blindScale = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 178:
                        this.huntMode = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 179:
                        this.inRewardCircle = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 180:
                        this.deskIdRec = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 181:
                        this.baseMsg.ServerVersion = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 182:
                        this.baseMsg.opTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 183:
                        this.baseMsg.userDelayTimes = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_MTT_GAME_DISMISS : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0
        public int MTTID { get; set; }  // 比赛ID
        public string mttName { get; set; }  // 比赛名称
        public string msg { get; set; }  // 取消信息



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.MTTID = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.mttName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 132:
                        this.msg = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_RECV_START_INFOR : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public REQ_GAME_RECV_START_INFOR baseMsg = new REQ_GAME_RECV_START_INFOR();
        public int isRebuy { get; set; }  // 是否能重购0，1
        public int isAppend { get; set; }  // 是否能增购0，1

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.baseMsg.bankerID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.baseMsg.bigSeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.baseMsg.smallSeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 63:
                        this.baseMsg.operationID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.baseMsg.firstCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.baseMsg.SecondCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.baseMsg.smallChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.baseMsg.bigChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.baseMsg.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.baseMsg.timesArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.baseMsg.unknow1 = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 137:
                        this.baseMsg.unknow2 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 138:
                        this.baseMsg.unknow3 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.baseMsg.unknow4 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 140:
                        this.isRebuy = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.isAppend = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.baseMsg.canPlayStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 143:
                        this.baseMsg.seatStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 144:
                        this.baseMsg.handNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 145:
                        this.baseMsg.extraBlind = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 146:
                        this.baseMsg.leftCreditValue = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.baseMsg.straddlePop = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 148:
                        this.baseMsg.isCurStraddle = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 149:
                        this.baseMsg.callAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 150:
                        this.baseMsg.initialBets = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 151:
                        this.baseMsg.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 152:
                        this.baseMsg.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_AUTO_OP : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public sbyte option { get; set; }  // 操作 0取消托管 1托管 2进入房间时请求所有玩家的托管状态 3托管时间到 4其它房间状态查询
        public sbyte initiative { get; set; }  // 主动点击托管或取消托管时传1，其他传0

        // Response
        public sbyte status { get; set; }  // 0成功 1失败
        public sbyte optionRec { get; set; }  // 0取消托管 1托管 2进入房间时请求所有玩家的托管状态 3托管时间到 4其它房间状态查询 5服务器主动下发其他玩家的托管状态
        public List<int> trustStatus { get; set; }  // 托管状态数组 0没托管 1托管 包括所有玩家


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(4);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, option, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, initiative, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.optionRec = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.trustStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_MTT_START_GAME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomPath { get; set; }
        public int roomId { get; set; }  // 房间号

        // Response
        public REQ_GAME_START_GAME baseMsg = new REQ_GAME_START_GAME();


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(2);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.baseMsg.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_MTT_ADD_TIME : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public int time { get; set; }  // 加的时间，现在是写死20秒
        public int diamonds { get; set; }  // 花费的钻石，公式：10 * pow(2, count + 1);count为第几次点加时
        public int seatId { get; set; }  // 座位号
        public int coins { get; set; }  // 花费的USDT，0

        // Response
        public REQ_ADD_TIME baseMsg = new REQ_ADD_TIME();
        public int delayTimes { get; set; }  // 本手牌已加时次数，最大可加时次数为10，超过了不可再点加时


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(6);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(130, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, time, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, diamonds, stream);
            bodysize += CPHotfixMessageHelper.WriteField(134, seatId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(135, coins, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.baseMsg.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.baseMsg.playerId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.baseMsg.addTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.baseMsg.coastJewel = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.baseMsg.coastCoins = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.delayTimes = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_SYSTEM_BROADCAST : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0
        public string content { get; set; }  // 广播内容
        public int playTime { get; set; }  // 播放次数



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.content = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.playTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_Guild_BROADCAST : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0
        public int roomID { get; set; }  // 播放广播的房间号
        public int playTime { get; set; }  // 播放次数
        public string content { get; set; }  // 广播内容
        
        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.roomID = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 131:
                        this.playTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.content = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_JACKPOT_HIT_BROADCAST : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public sbyte status { get; set; }  // 0
        public string userName { get; set; }  // 击中JackPot的用户名
        public int cardType { get; set; }  // 牌型代号，APP内通用，1皇家同花顺 2同花顺 3四条 4葫芦 5同花 6顺子 7三条 8两对 9一对 10高牌
        public int reward { get; set; }  // 奖励
        public int roomid { get; set; }  // 房间号
        public sbyte seatIndex { get; set; }  // 座位号



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.userName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 131:
                        this.cardType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 132:
                        this.reward = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.roomid = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.seatIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_OMAHA_ENTER_ROOM : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public sbyte type { get; set; }  // 1
        public sbyte connect { get; set; }  // 1
        public sbyte unknow { get; set; }  // 未知含义，传1
        public int roomPath { get; set; }  // 普通局:61 MTT:71 SNG:81 奥马哈:91 大菠萝:51
        public int roomID { get; set; }  // 房间号
        public int deskId { get; set; }  // 0

        // Response
        public REQ_GAME_ENTER_ROOM baseMsg = new REQ_GAME_ENTER_ROOM();
        public List<sbyte> showCardsId { get; set; }  // 结束时是否亮手牌 0 不亮 1亮 对应四张牌
        public List<sbyte> thirdCardArray { get; set; }  // 所有玩家第3张手牌
        public List<sbyte> fourthCardArray { get; set; }  // 所有玩家第4张手牌
        public int cardType { get; set; }  //  牌型（已弃用，改完客户端计算）
        public int omahaBloody { get; set; }  //  奥马哈血战模式0 1

        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(6);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(60, type, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, connect, stream);
            bodysize += CPHotfixMessageHelper.WriteField(62, unknow, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, deskId, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.baseMsg.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.baseMsg.gamestatus = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.baseMsg.countdown_times = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 65:
                        this.baseMsg.bigIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 66:
                        this.baseMsg.smallIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 67:
                        this.baseMsg.bankerIndex = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 68:
                        this.baseMsg.operationID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 69:
                        this.baseMsg.speed = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 70:
                        this.baseMsg.mySeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 71:
                        this.baseMsg.maxCheckTimes = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 72:
                        this.baseMsg.currentCheckTimes = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 131:
                        this.baseMsg.cards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.baseMsg.playerIDArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.baseMsg.statusArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 134:
                        this.baseMsg.anteArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.baseMsg.nick = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 136:
                        this.baseMsg.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 137:
                        this.baseMsg.headPicString = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 138:
                        this.baseMsg.firstCardarray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 139:
                        this.baseMsg.SecondArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 140:
                        this.baseMsg.bigBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.baseMsg.smallBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.baseMsg.alreadAnte = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 143:
                        this.baseMsg.maxAnte = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 144:
                        this.baseMsg.roomOwner = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 145:
                        this.baseMsg.gameBeginTime = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 146:
                        this.baseMsg.maxPlayTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.baseMsg.bControl = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 148:
                        this.baseMsg.userChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 149:
                        this.baseMsg.nowChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 150:
                        this.baseMsg.roomLeftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 151:
                        this.baseMsg.pauseLeftTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 152:
                        this.baseMsg.lastMinRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 153:
                        this.baseMsg.lastMaxRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 154:
                        this.baseMsg.currentMinRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 155:
                        this.baseMsg.currentMaxRate = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 156:
                        this.baseMsg.leftOperateTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 157:
                        this.baseMsg.sexArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 158:
                        this.showCardsId = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 159:
                        this.baseMsg.groupBet = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 160:
                        this.baseMsg.leftSecs = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 161:
                        this.baseMsg.pots = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 162:
                        this.baseMsg.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 163:
                        this.baseMsg.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 164:
                        this.baseMsg.insurance = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 165:
                        this.baseMsg.canPlayStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 166:
                        this.baseMsg.seatStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 167:
                        this.baseMsg.waitBlind = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 168:
                        this.baseMsg.leftCreditValue = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 169:
                        this.baseMsg.roomType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 170:
                        this.baseMsg.needBring = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 171:
                        this.baseMsg.isTrusted = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 172:
                        this.baseMsg.callAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 173:
                        this.baseMsg.publicCardsType = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 174:
                        this.baseMsg.isKeptSeat = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 175:
                        this.baseMsg.keptTimeArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 176:
                        this.baseMsg.seeCardPlayerName = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 177:
                        this.baseMsg.isIpRestrictions = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 178:
                        this.thirdCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 179:
                        this.fourthCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 180:
                        this.cardType = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 181:
                        this.baseMsg.isGPSRestrictions = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 182:
                        this.baseMsg.tribeId = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 183:
                        this.baseMsg.holdingSeatLeftTimeArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 184:
                        this.baseMsg.bringIn = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 185:
                        this.baseMsg.preLeave = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 186:
                        this.baseMsg.bSpectatorsVoice = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 187:
                        this.omahaBloody = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 188:
                        this.baseMsg.ServerVersion = CPHotfixMessageHelper.ReadFieldString(stream, ref offset);
                        break;
                    case 189:
                        this.baseMsg.canPreLeave = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 190:
                        this.baseMsg.operationRound = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 191:
                        this.baseMsg.isOrientationTribe = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 192:
                        this.baseMsg.serviceFeePer = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 193:
                        this.baseMsg.bCreditControl = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 194:
                        this.baseMsg.opTime = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 195:
                        this.baseMsg.userDelayTimes = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 196:
                        this.baseMsg.potNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 198:
                        this.baseMsg.roomMode = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_OMAHA_RECV_WINNER : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public REQ_GAME_RECV_WINNER baseMsg = new REQ_GAME_RECV_WINNER();
        public List<sbyte> thirdCardArray { get; set; }  // 每个玩家第三张牌，只有奥马哈有该字段
        public List<sbyte> fourthCardArray { get; set; }  // 每个玩家第四张牌，只有奥马哈有该字段



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 130:
                        this.baseMsg.seatIDArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.baseMsg.playerArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 132:
                        this.baseMsg.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 133:
                        this.baseMsg.cardTypesArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 134:
                        this.baseMsg.isWinArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 135:
                        this.baseMsg.winChipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.baseMsg.cardSort = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 137:
                        this.baseMsg.allPlayerchip = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 138:
                        this.baseMsg.allplayerID = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 140:
                        this.baseMsg.firstCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 141:
                        this.baseMsg.secondCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 142:
                        this.baseMsg.isMaxcardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 143:
                        this.baseMsg.selfChips = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 144:
                        this.baseMsg.muckStatus = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 145:
                        this.thirdCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 146:
                        this.fourthCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 147:
                        this.baseMsg.jackPotUpArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 148:
                        this.baseMsg.hitJackPotChipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_OMAHA_RECV_START_INFOR : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public REQ_GAME_RECV_START_INFOR baseMsg = new REQ_GAME_RECV_START_INFOR();
        public List<sbyte> thirdCardArray { get; set; }  // 各玩家第3个手牌
        public List<sbyte> fourthCardArray { get; set; }  // 各玩家第4个手牌



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.baseMsg.bankerID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.baseMsg.bigSeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.baseMsg.smallSeatID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 63:
                        this.baseMsg.operationID = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 130:
                        this.baseMsg.firstCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.baseMsg.SecondCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.baseMsg.smallChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 133:
                        this.baseMsg.bigChip = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 134:
                        this.baseMsg.chipsArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 135:
                        this.baseMsg.timesArray = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 136:
                        this.baseMsg.unknow1 = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 137:
                        this.baseMsg.unknow2 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 138:
                        this.baseMsg.unknow3 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 139:
                        this.baseMsg.unknow4 = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 140:
                        this.baseMsg.minAnteNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 141:
                        this.baseMsg.canRaise = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 142:
                        this.baseMsg.canPlayStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 143:
                        this.baseMsg.seatStatus = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 144:
                        this.baseMsg.handNum = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 145:
                        this.baseMsg.extraBlind = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 146:
                        this.baseMsg.leftCreditValue = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 147:
                        this.baseMsg.straddlePop = CPHotfixMessageHelper.ReadFieldInt32s(stream, ref offset);
                        break;
                    case 148:
                        this.baseMsg.isCurStraddle = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 149:
                        this.baseMsg.callAmount = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 150:
                        this.thirdCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 151:
                        this.fourthCardArray = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 152:
                        this.baseMsg.operationRound = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    case 153:
                        this.baseMsg.potNumber = CPHotfixMessageHelper.ReadFieldInt32(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_OMAHA_PLAYER_CARDS : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }


        // Response
        public List<sbyte> playerSeat { get; set; }  // 所有Allin玩家座位号
        public List<sbyte> firstCards { get; set; }  // 玩家第一张手牌
        public List<sbyte> secondCards { get; set; }  // 玩家第二张手牌
        public List<sbyte> thirdCards { get; set; }  // 玩家第3张手牌
        public List<sbyte> fourthCards { get; set; }  // 玩家第4张手牌



        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 130:
                        this.playerSeat = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 131:
                        this.firstCards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 132:
                        this.secondCards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 133:
                        this.thirdCards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    case 134:
                        this.fourthCards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_OMAHA_SHOWDOWN : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Request
        public int roomID { get; set; }
        public int roomPath { get; set; }
        public List<sbyte> showCards { get; set; }  // 第一到第四张牌，0不亮牌，1亮牌

        // Response
        public sbyte status { get; set; }  // 0 成功
        public List<sbyte> showCardsRec { get; set; }  // 第一到第四张牌，0不亮牌，1亮牌


        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;

            bodysize += CPHotfixMessageHelper.WriteField(131, roomID, stream);
            bodysize += CPHotfixMessageHelper.WriteField(132, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(133, showCards, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.showCardsRec = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_OMAHA_SHOW_CARDS : ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        // Response
        public sbyte seatId { get; set; }  // 座位下标
        public List<sbyte> cards { get; set; }  // 手牌数组[4] 0-3下标分别对应四张手牌 -1则为空

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 60:
                        this.seatId = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 61:
                        this.cards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

    public partial class REQ_GAME_SEND_TEST : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        public int roomId { get; set; }  // 房间号
        public int roomPath { get; set; }
        public List<sbyte> card { get; set; }

        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(3);   // 业务数据个数
            int bodysize = 1;
            bodysize += CPHotfixMessageHelper.WriteField(130, roomId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            bodysize += CPHotfixMessageHelper.WriteField(61, card, stream);
            return (ushort)bodysize;
        }

        public sbyte status { get; set; }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 61:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                }
                count++;
            }
            return this;
        }

    }

    public partial class REQ_GAME_ASK_TEST : ICPRequest, ICPResponse
    {
        public int RpcId { get; set; }
        public int Error { get; set; }

        public int roomId { get; set; }  // 房间号
        public int roomPath { get; set; }

        // Response
        public sbyte status { get; set; }
        public sbyte cardnum { get; set; }  // 几张牌
        public List<sbyte> cards { get; set; }  // 牌
        public ushort SerializeTo(MemoryStream stream)
        {
            stream.WriteByte(2);   // 业务数据个数
            int bodysize = 1;
            bodysize += CPHotfixMessageHelper.WriteField(130, roomId, stream);
            bodysize += CPHotfixMessageHelper.WriteField(131, roomPath, stream);
            return (ushort)bodysize;
        }

        public object DeserializeFrom(MemoryStream stream)
        {
            RpcId = BitConverter.ToInt32(stream.GetBuffer(), 2);
            int offset = 6;

            byte paramCount = stream.GetBuffer()[offset];
            offset += 1;

            int count = 0;
            byte filedId = 0;
            short length = 0;
            while (count < paramCount)
            {
                filedId = stream.GetBuffer()[offset];
                offset += 1;
                switch (filedId)
                {
                    case 61:
                        this.status = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 62:
                        this.cardnum = CPHotfixMessageHelper.ReadFieldByte(stream, ref offset);
                        break;
                    case 63:
                        this.cards = CPHotfixMessageHelper.ReadFieldBytes(stream, ref offset);
                        break;
                    default:
                        break;
                }
                count++;
            }
            return this;
        }
    }

}