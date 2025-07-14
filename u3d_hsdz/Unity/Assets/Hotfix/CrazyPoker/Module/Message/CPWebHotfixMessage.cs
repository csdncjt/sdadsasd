using System.Collections.Generic;

namespace ETHotfix
{
    public class RequestDataBase
    {
        public string version;
        public int channel_id;
        public int cli_type;

        public RequestDataBase()
        {
            version = GlobalData.WebVersion;
            channel_id = GlobalData.WebChannelId;
            cli_type = GlobalData.Instance.WebCliType;
        }
    }

    public partial class WEB_upgrade_v2_systemConfig  // 升级接口
    {
        public const string API = @"/api/upgrade/v2/systemConfig";

        public sealed class RequestData : RequestDataBase
        {
            public int device_type { get; set; }  // 设备类型 0 - Android；1 - iOS
            public string lan_type { get; set; }  // 0中文 1英文
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int showServerEntry { get; set; }  // 是否显示线路切换入口，0 1
            public string theNewVersion { get; set; }  // 最新版本号，如3.9.3
            public string message { get; set; }  // 升级内容
            public int rechargeMode { get; set; }  // 充值渠道开关 0内购 1支付宝 2内购+支付宝
            public int fullUpgradeType { get; set; }  // 升级类型，0无需更新 1软更 2硬更
            public int showGetTicketEntry { get; set; }  // 是否显示官方MTT门票入口，0 1
            public string update_url { get; set; }  // 升级链接
            public int upgradeType { get; set; }  // 升级类型，0无需更新 1软更 2硬更
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_user_register_code  // 获取验证码
    {
        public const string API = @"/api/user/register-code";

        public sealed class RequestData : RequestDataBase
        {
            public string phone { get; set; }  // 手机号码
            public string type { get; set; }  // 0注册   1找回密码
            public string country { get; set; }  // 国家代号，如cn
            public string country_code { get; set; }  // 国家区号，如86
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0：发送成功 1 发送验证码失败 2 手机已注册 3 手机号不合法 38 超过验证码每日最大次数
            public string msg { get; set; }  // 错误信息
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_user_register  // 注册账户
    {
        public const string API = @"/api/user/register";

        public sealed class RequestData : RequestDataBase
        {
            public string phone { get; set; }  // 手机号码
            public string code { get; set; }  // 验证码
            public string passwd { get; set; }  // 密码md5
            public string country { get; set; }  // 国家代号，如cn
            public string country_code { get; set; }  // 国家区号，如86
            public string m { get; set; }  // 加密参数,则 phone-code-passwd-a~A}vb_5nr>%y!ER md5后的第21,13,6,17,23,29个字符（从0位开始取）
            public string imei { get; set; }  // 设备唯一识别码
            public int isSimulator { get; set; }  // 是否模拟器 1 0
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0：注册成功 2 手机已注册 3 验证码错误 4 对不起，请重启APP，再进行注册！5 对不起，您的设备注册已达上限！6 对不起，当前设备无法注册，请用手机注册! 16 验证码过期
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int userid { get; set; }  // 用户ID
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_user_modify_pw  // 修改密码/找回密码
    {
        public const string API = @"/api/user/modify-pw";

        public sealed class RequestData : RequestDataBase
        {
            public string phone { get; set; }  // 手机号码
            public string code { get; set; }  // 验证码
            public string passwd { get; set; }  // 密码md5
            public string country { get; set; }  // 国家代号，如cn
            public string country_code { get; set; }  // 国家区号，如86
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0：密码修改成功 2 手机未注册 3 验证码错误 16 验证码过期
            public string msg { get; set; }  // 错误信息
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_room_create  // 普通局组局
    {
        public const string API = @"/api/room/create";

        public sealed class RequestData : RequestDataBase
        {
            public int in_chip { get; set; }  // 最小带入USDT，如200
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int ip { get; set; }  // 是否开启ip限制，0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public string source { get; set; }  // 野局无改参数，俱乐部局传：club
            public int credit_control { get; set; }  // 是否开启俱乐部分带入 0 1
            public int max_play_time { get; set; }  // 牌局时长（分钟），如30
            public int advance_charge { get; set; }  // 建局服务费，现在不需要了，0
            public int sb_chip { get; set; }  // 小盲
            public int qianzhu { get; set; }  // 前注
            public int min_rate { get; set; }  // 最少带入倍数
            public string name { get; set; }  // 牌局名称
            public int vp { get; set; }  // 是否开启当桌入池率，0 1
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int source_id { get; set; }  // 俱乐部局的话，就是俱乐部ID，如101256
            public int control { get; set; }  // 是否开启控制带入，0
            public int commission { get; set; }  // 基金，未使用，传0
            public int tribe_id { get; set; }  // 同步到的同盟id，如100148
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int ahead_leave { get; set; }  // 是否开启提前离桌，0 1
            public int max_rate { get; set; }  // 最大带入倍数，如4
            public int charge { get; set; }  // 记录费，现在不需要，传0
            public int player_count { get; set; }  // 牌桌人数，如9
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int gps { get; set; }  // 是否开启gps限制，0 1
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 状态：0成功 1失败 2网络错误 3该用户已经创建了在玩房间，并且还未结束 4房间不存在 5USDT不足 6钻石不足 8权限不够 9无限局数已经超过上限 10已经存在相同大小盲的无限局 11只有管理员和创建者可以建无限局 12超过VIP等级可创建房间 13MTT比赛时间需要5分钟后 14当前盲注的战队JackPot没开启 15当前盲注的公会JackPot没开启 16该同盟关闭了同步
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int qianzhu { get; set; }  // 前注
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT 
            public string room_create_time { get; set; }  // 建房时间戳，如1548322786000
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int small_blind { get; set; }  // 小盲，如1
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int room_port { get; set; }  // 房间服务器端口，如8051
            public string room_name { get; set; }  // 牌局名称
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int room_master { get; set; }  // 是否房主，0 1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int commission { get; set; }  // 基金，未使用
            public int control { get; set; }  // 是否开启控制带入，0
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如4
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int game_time { get; set; }  // 牌局时长（分），如30
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public int jackPot_fund { get; set; }  // jackPot基金，如60824
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int jackPot_id { get; set; }  // JackPot奖池的id，如190
            public int player_count { get; set; }  // 牌桌人数，如9
            public int room_id { get; set; }  // 房间号，如961672
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int room_path { get; set; }  // roomPath，如61
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int charge { get; set; }  // 记录费
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_omaha_create  // 奥马哈组局
    {
        public const string API = @"/api/omaha/create";

        public sealed class RequestData : RequestDataBase
        {
            public int player_count { get; set; }  // 牌桌人数，如9
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int ip { get; set; }  // 是否开启ip限制，0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public string source { get; set; }  // 野局无该参数，俱乐部局传：club
            public int credit_control { get; set; }  // 是否开启俱乐部分带入 0 1
            public int max_play_time { get; set; }  // 牌局时长（分钟），如30
            public int advance_charge { get; set; }  // 建局服务费，现在不需要了，0
            public int in_chip { get; set; }  // 最小带入USDT，如200
            public int sb_chip { get; set; }  // 小盲
            public int qianzhu { get; set; }  // 前注
            public int min_rate { get; set; }  // 最少带入倍数
            public string name { get; set; }  // 牌局名称
            public int vp { get; set; }  // 是否开启当桌入池率，0 1
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int source_id { get; set; }  // 俱乐部局的话，就是俱乐部ID，如101256
            public int control { get; set; }  // 是否开启控制带入，0
            public int commission { get; set; }  // 基金，未使用，传0
            public int tribe_id { get; set; }  // 同步到的同盟id，如100148
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int ahead_leave { get; set; }  // 是否开启提前离桌，0 1
            public int max_rate { get; set; }  // 最大带入倍数，如4
            public int mode { get; set; }  // 是否开启血战模式，0 1
            public int charge { get; set; }  // 记录费，现在不需要，传0
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int gps { get; set; }  // 是否开启gps限制，0 1
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public int player_count { get; set; }  // 牌桌人数，如9
            public int room_id { get; set; }  // 房间号，如961672
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int charge { get; set; }  // 记录费
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int room_master { get; set; }  // 是否房主0 1
            public int mode { get; set; }  // 是否开启血战模式，0 1
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int qianzhu { get; set; }  // 前注
            public int jackPot_fund { get; set; }  // jackPot基金，如60824
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int room_port { get; set; }  // 房间服务器端口，如8052
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public string room_create_time { get; set; }  // 建房时间戳，如1548385475000
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如4
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int room_path { get; set; }  // roomPath，如91
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public int commission { get; set; }  // 基金，未使用
            public string room_name { get; set; }  // 牌局名称
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int small_blind { get; set; }  // 小盲，如1
            public int jackPot_id { get; set; }  // JackPot奖池的id，如190
            public int control { get; set; }  // 是否开启控制带入，0
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int game_time { get; set; }  // 牌局时长（分），如30
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_sng_create  // SNG组局
    {
        public const string API = @"/api/sng/create";

        public sealed class RequestData : RequestDataBase
        {
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int up_blind_time { get; set; }  // 升盲时间（分钟）
            public int control { get; set; }  // 是否开启控制带入，0
            public int tribe_id { get; set; }  // 同步到的同盟id，如100148
            public int gps { get; set; }  // 是否开启gps限制，0 1
            public int init_chip { get; set; }  // 起始积分，如3000
            public int type { get; set; }  // 0快速局 1标准局
            public string source { get; set; }  // 野局无该参数，俱乐部局传：club
            public int credit_control { get; set; }  // 是否开启俱乐部分带入 0 1
            public int ip { get; set; }  // 是否开启ip限制，0 1
            public int source_id { get; set; }  // 俱乐部局的话，就是俱乐部ID，如101256
            public int player_count { get; set; }  // 牌桌人数，如9
            public string name { get; set; }  // 牌局名称
            public int in_chip { get; set; }  // 买入设置（报名费），如500
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public int room_path { get; set; }  // roomPath，如81
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT
            public int room_id { get; set; }  // 房间号，如878529
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int room_port { get; set; }  // 房间服务器端口，如8054
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_create  // MTT组局
    {
        public const string API = @"/api/mtt/create";

        public sealed class RequestData : RequestDataBase
        {
            public int ip { get; set; }  // 是否开启ip限制，0 1
            public int allowDelay { get; set; }  // 是否允许延迟报名，0 1
            public int allowRebuy { get; set; }  // 是否允许重购，0 1
            public string source { get; set; }  // 野局无该参数，俱乐部局传：club
            public int maxLevel { get; set; }  // 截止重购级别
            public int maxDelayLevel { get; set; }  // 最大延迟报名级别
            public int updateCycle { get; set; }  // 升盲时间（分钟）
            public int mttType { get; set; }  // 6:6人桌，9:9人桌
            public int huntsman_match { get; set; }  // 是否猎人赛，0 1
            public int allowAppend { get; set; }  // 是否允许增购，0 1 未用
            public string name { get; set; }  // 比赛名次
            public int initialScore { get; set; }  // 起始USDT，如2500
            public int isControl { get; set; }  // 是否开启控制带入，0
            public int blindType { get; set; }  // 标准局0 快速局1
            public int source_id { get; set; }  // 俱乐部局的话，就是俱乐部ID，如101256
            public int huntsman_percent { get; set; }  // 猎人赛奖金池比例，如40代表40%
            public int initialChip { get; set; }  // 起始USDT，如2500
            public int lowerLimit { get; set; }  // 参数人数下限，如5
            public int tribe_id { get; set; }  // 100148
            public int rebuyTimes { get; set; }  // 重购次数
            public int upperLimit { get; set; }  // 参赛人数上限，如50
            public int registationFee { get; set; }  // 报名费
            public int startTime { get; set; }  // 比赛开始时间
            public int sb_chip { get; set; }  // 起始小盲
            public int gps { get; set; }  // 是否开启GPS限制,0 1
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int room_port { get; set; }  // 房间服务器端口，如8055
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT
            public int room_path { get; set; }  // roomPath，如71
            public int room_id { get; set; }  // 房间号，如120827
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_bigpineapple_create  // 大菠萝组局
    {
        public const string API = @"/api/bigpineapple/create";

        public sealed class RequestData : RequestDataBase
        {
            public int player_count { get; set; }  // 牌桌人数，如3
            public int in_chip { get; set; }  // 最小带入USDT，如100
            public int bringin_limit_on { get; set; }  // 限制补充 0关闭 1开启
            public int charge { get; set; }  // 记录费，现在不需要，传0
            public int source_id { get; set; }  // 俱乐部局的话，就是俱乐部ID，如101256
            public int min_rate { get; set; }  // 最少带入倍数
            public int mode { get; set; }  // 1普通模式 2血战模式 3血进血出
            public int gps { get; set; }  // 是否开启gps限制，0 1
            public int op_time { get; set; }  // 操作时间s，如20
            public int advance_charge { get; set; }  // 建局服务费，现在不需要了，0
            public int game_min_chip { get; set; }  // 最低入局分，如20
            public int sb_chip { get; set; }  // 分值
            public int deal_mode { get; set; }  // 发牌模式 0 同时   1 顺序发牌
            public string source { get; set; }  // 野局无改参数，俱乐部局传：club
            public string name { get; set; }  // 牌局名称
            public int tillOver_on { get; set; }  // 是否开启一战到底，0 1
            public int blind { get; set; }  // 分值
            public int tribe_id { get; set; }  // 同步到的同盟id，如100148
            public int max_rate { get; set; }  // 最大带入倍数，如8
            public int jokers_on { get; set; }  // 是否开启赖子模式，0 1
            public int bringin_limit_chip { get; set; }  // 最低入局分，如20
            public int control { get; set; }  // 是否开启控制带入，0
            public int ip { get; set; }  // 是否开启ip限制，0 1
            public int max_play_time { get; set; }  // 牌局时长（分钟），如120
            public int credit_control { get; set; }  // 是否开启俱乐部分带入 0 1
            public int competition_on { get; set; }  // 竞技模式 0关闭 1开启
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public int charge { get; set; }  // 记录费
            public int bringin_limit_chip { get; set; }  // 最低入局分，如20
            public int competition_on { get; set; }  // 竞技模式 0关闭 1开启
            public int game_min_chip { get; set; }  // 最低入局分，如20
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT
            public int deal_mode { get; set; }  // 发牌模式 0 同时   1 顺序发牌
            public string room_create_time { get; set; }  // 建房时间戳，如1548385475000
            public int bringin_limit_on { get; set; }  // 限制补充 0关闭 1开启
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int room_port { get; set; }  // 房间服务器端口，如8053
            public string room_name { get; set; }  // 牌局名称
            public int room_master { get; set; }  // 是否房主0 1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int op_time { get; set; }  // 操作时间s，如20
            public int control { get; set; }  // 是否开启控制带入，0
            public int blind { get; set; }  // 分值
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如8
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public int game_time { get; set; }  // 牌局时长（分），如120
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int jokers_on { get; set; }  // 是否开启赖子模式，0 1
            public int tillOver_on { get; set; }  // 是否开启一战到底，0 1
            public int player_count { get; set; }  // 牌桌人数，如3
            public int room_id { get; set; }  // 房间号，如608122
            public int mode { get; set; }  // 1普通模式 2血战模式 3血进血出
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int room_path { get; set; }  // roomPath，如51
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_aof_create  // 必下场组局
    {
        public const string API = @"/api/aof/create";

        public sealed class RequestData : RequestDataBase
        {
            public int in_chip { get; set; }  // 最小带入USDT，如200
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int ip { get; set; }  // 是否开启ip限制，0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public string source { get; set; }  // 野局无改参数，俱乐部局传：club
            public int credit_control { get; set; }  // 是否开启俱乐部分带入 0 1
            public int max_play_time { get; set; }  // 牌局时长（分钟），如30
            public int advance_charge { get; set; }  // 建局服务费，现在不需要了，0
            public int sb_chip { get; set; }  // 小盲
            public int qianzhu { get; set; }  // 前注
            public int min_rate { get; set; }  // 最少带入倍数
            public string name { get; set; }  // 牌局名称
            public int vp { get; set; }  // 是否开启当桌入池率，0 1
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int source_id { get; set; }  // 俱乐部局的话，就是俱乐部ID，如101256
            public int control { get; set; }  // 是否开启控制带入，0
            public int commission { get; set; }  // 基金，未使用，传0
            public int tribe_id { get; set; }  // 同步到的同盟id，如100148
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int ahead_leave { get; set; }  // 是否开启提前离桌，0 1
            public int max_rate { get; set; }  // 最大带入倍数，如4
            public int charge { get; set; }  // 记录费，现在不需要，传0
            public int player_count { get; set; }  // 牌桌人数，如9
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int gps { get; set; }  // 是否开启gps限制，0 1
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 同普通局
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int qianzhu { get; set; }  // 前注
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT 
            public string room_create_time { get; set; }  // 建房时间戳，如1548322786000
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int small_blind { get; set; }  // 小盲，如1
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int room_port { get; set; }  // 房间服务器端口，如8051
            public string room_name { get; set; }  // 牌局名称
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int room_master { get; set; }  // 是否房主，0 1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int commission { get; set; }  // 基金，未使用
            public int control { get; set; }  // 是否开启控制带入，0
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如4
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int game_time { get; set; }  // 牌局时长（分），如30
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public int jackPot_fund { get; set; }  // jackPot基金，如60824
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int jackPot_id { get; set; }  // JackPot奖池的id，如190
            public int player_count { get; set; }  // 牌桌人数，如9
            public int room_id { get; set; }  // 房间号，如961672
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int room_path { get; set; }  // roomPath，如41
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int charge { get; set; }  // 记录费
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_room_blindData
    {
        public const string API = @"/api/room/pumplist";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public List<GameBlindElement> data { get; set; }  // Data
            public int status { get; set; }  // 0 成功 其他失败
        }

        public sealed class GameBlindElement
        {
            public string blindnote { get; set; }  // 小盲|大盲
            public double pump { get; set; }  // 收取服务费比例
            public int pattern { get; set; }  // 小中大（0，1，2）
            public int maxbet { get; set; }  
            public int type { get; set; }  // 游戏类型 奥马哈1，德州2，AOF奥马哈3，AOF德州4

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_room_room_list  // 获取首页房间列表
    {
        public const string API = @"/api/room/room-list";

        public sealed class RequestData : RequestDataBase
        {
            public string time { get; set; }  // 加载更多时，传最后底下一条房间数据的建房时间戳；刷新时，传0
            public string m { get; set; }  // 加密参数,则 userID-time-direction-a~A}vb_5nr>%y!ER md5后的第21,13,6,17,23,29个字符（从0位开始取）
            public string direction { get; set; }  // 0加载更多 1刷新
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0 成功 其他失败
        }

        public sealed class Data
        {
            public List<RoomsElement> matchList { get; set; }  // 我的比赛，包含元素:RoomsElement
            public List<RoomsElement> myRooms { get; set; }  // 参与的房间，包含元素:RoomsElement
            public int recordCount { get; set; }  // 数据条数
            public List<RoomsElement> relatedRooms { get; set; }  // 其他房间，包含元素:RoomsElement
            public int hasMore { get; set; }  // 是否还有更多数据，0 1
        }

        public sealed class RoomsElement
        {
            public int player_count { get; set; }  // 牌桌人数，如9
            public int game_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int room_mode { get; set; }  // 是否开启奥马哈血战模式，0 1
            public string id { get; set; }  // 房间号，如918606
            public int official_club { get; set; }  // 是否官方俱乐部，0 1
            public int creator_id { get; set; }  // 房主id，如172479
            public int min_rate { get; set; }  // 最小带入倍数
            public string room_master { get; set; }  // 房主昵称
            public int mode { get; set; }  // 大菠萝模式，1普通模式 2血战模式 3血进血出
            public int pause { get; set; }  // 是否暂停，0 1
            public int registerFee { get; set; }  // mtt报名费
            public int qianzhu { get; set; }  // 前注
            public int source_type { get; set; }  // 牌局类型 0快速 2战队 3公会
            public int reside_time { get; set; }  // 牌局剩余时间
            public int deal_mode { get; set; }  // 大菠萝发牌模式，0同步发牌  1顺序发牌
            public int create_time { get; set; }  // 创建时间戳，如1548405167
            public int room_people { get; set; }  // 房间当前人数
            public int jackPot_on { get; set; }  // 是否开启JackPot
            public int insurance { get; set; }  // 是否开启保险，0 1
            public string club_name { get; set; }  // 俱乐部名称
            public int big_blind { get; set; }  // 大盲
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT 
            public string onwer_head { get; set; }  // 头像ID
            public string room_name { get; set; }  // 房间名称
            public int max_rate { get; set; }  // 最多带入倍数
            public int is_start { get; set; }  // 是否开局，在牌局内是否点了开始
            public int jokers_on { get; set; }  // 大菠萝是否开启赖子模式，0 1
            public int raiseBlindTime { get; set; }  // 升盲时间
            public int small_blind { get; set; }  // 小盲
            public int bring_chip { get; set; }  // 最小带入游戏币
            public int is_hunter { get; set; }  // mtt是否猎人模式
            public int control { get; set; }  // 是否开启控制带入，0
            public int match_type { get; set; }  // sng牌局类型0快速局 1标准局
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_game_banner  // 小游戏页面Banner
    {
        public const string API = @"/api/banner/addUserUid";

        public sealed class RequestData
        {
 
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public string msg { get; set; }
            public Data data { get; set; }
        }

        public sealed class Data
        {
            public string userUid { get; set; }
            public string gameIp { get; set; }

        }
        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }
        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_common_banner  // 游戏页面Banner
    {
        public const string API = @"/api/banner/list";

        public sealed class RequestData
        {
            public string lang { get; set; }//语言(默认简体中文)，zh_CN：简体中文，zh_HK：繁体中文，en_US：英文
            public int type { get; set; }//类型(默认大厅), 0：大厅Banner，1：发现页Banner，2：发现页主图，3：游戏公平，4：游戏商城主图
            public int product { get; set; }//上架产品，0：疯狂扑克
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public string msg { get; set; }
            public List<DataElement> data { get; set; }  // 包含元素:DataElement
        }

        public sealed class DataElement
        {
            public string bannerImg { get; set; }  // 点击后跳转到的网页地址
            public string bannerUrl { get; set; }  // banner图片的完整地址
            public string desc { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_room_enter  // 普通局进房间
    {
        public const string API = @"/api/room/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 61
            public int room_id { get; set; }  // 房间id，如863804
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 状态：0成功 1失败 2该用户已经创建了在玩房间，并且还未结束 3房间不存在 5USDT不足 6钻石不足 7MTT玩家被淘汰且桌子已经被合并 8已被管理员请出该牌局，不能再次加入
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int qianzhu { get; set; }  // 前注
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT 
            public string room_create_time { get; set; }  // 建房时间戳，如1548322786000
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int small_blind { get; set; }  // 小盲，如1
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int room_port { get; set; }  // 房间服务器端口，如8051
            public string room_name { get; set; }  // 牌局名称
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int room_master { get; set; }  // 是否房主，0 1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int commission { get; set; }  // 基金，未使用
            public int control { get; set; }  // 是否开启控制带入，0
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如4
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int game_time { get; set; }  // 牌局时长（分），如30
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public int jackPot_fund { get; set; }  // jackPot基金，如60824
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int jackPot_id { get; set; }  // JackPot奖池的id，如190
            public int player_count { get; set; }  // 牌桌人数，如9
            public int room_id { get; set; }  // 房间号，如961672
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int room_path { get; set; }  // roomPath，如61
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int charge { get; set; }  // 记录费
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_omaha_enter  // 奥马哈进房间
    {
        public const string API = @"/api/omaha/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 91
            public int room_id { get; set; }  // 房间号，如851131
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public int player_count { get; set; }  // 牌桌人数，如9
            public int room_id { get; set; }  // 房间号，如961672
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int charge { get; set; }  // 记录费
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int room_master { get; set; }  // 是否房主0 1
            public int mode { get; set; }  // 是否开启血战模式，0 1
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int qianzhu { get; set; }  // 前注
            public int jackPot_fund { get; set; }  // jackPot基金，如60824
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int room_port { get; set; }  // 房间服务器端口，如8052
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public string room_create_time { get; set; }  // 建房时间戳，如1548385475000
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如4
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int room_path { get; set; }  // roomPath，如91
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public int commission { get; set; }  // 基金，未使用
            public string room_name { get; set; }  // 牌局名称
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int small_blind { get; set; }  // 小盲，如1
            public int jackPot_id { get; set; }  // JackPot奖池的id，如190
            public int control { get; set; }  // 是否开启控制带入，0
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int game_time { get; set; }  // 牌局时长（分），如30
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_sng_enter  // SNG进房间
    {
        public const string API = @"/api/sng/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 81
            public int room_id { get; set; }  // 房间号，如465625
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int room_port { get; set; }  // 房间服务器端口，如8054
            public string name { get; set; }  // 牌局名称
            public int apply_player { get; set; }  // 报名人数
            public int init_chip { get; set; }  // 起始积分，如3000
            public int type { get; set; }  // 0快速局 1标准局
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int control { get; set; }  // 是否开启控制带入，0
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int tribe_id { get; set; }  // 同步到的同盟id，如100148
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public int apply_status { get; set; }  // 报名状态，0未报名 1等待审核 2已报名 3被拒绝
            public int player_count { get; set; }  // 牌桌人数，如6
            public int room_id { get; set; }  // 房间号，如465625
            public int room_path { get; set; }  // 81
            public int in_chip { get; set; }  // 买入设置（报名费），如200
            public int can_remove { get; set; }  // 是否有移除玩家权限，0 1
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_enter  // MTT进房间
    {
        public const string API = @"/api/mtt/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 71
            public int room_id { get; set; }  // 比赛id，如337195
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public string room_path { get; set; }  // 71
            public string mtt_type { get; set; }  // 6:6人桌，9：9人桌
            public string room_id { get; set; }  // 比赛id，如337195
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public string room_master_id { get; set; }  // 房主用户ID，如172378
            public int room_port { get; set; }  // 房间服务器端口，如8055
            public int mtt_status { get; set; }  // mtt游戏状态，未使用
            public string mtt_name { get; set; }  // 比赛名称
            public string room_master { get; set; }  // 是否房主0 1
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_countdown  // MTT倒计时
    {
        public const string API = @"/api/mtt/countdown";

        public sealed class RequestData : RequestDataBase
        {
            public int game_path { get; set; }  // 71
            public int game_id { get; set; }  // 比赛id，如337195
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int data { get; set; }  // 倒计时，毫秒
            public int status { get; set; }  // 0 成功
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_bigpineapple_enter  // 大菠萝进房间
    {
        public const string API = @"/api/bigpineapple/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 51
            public int room_id { get; set; }  // 房间id，如210049
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public int charge { get; set; }  // 记录费
            public int bringin_limit_chip { get; set; }  // 最低入局分，如20
            public int competition_on { get; set; }  // 竞技模式 0关闭 1开启
            public int game_min_chip { get; set; }  // 最低入局分，如20
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT
            public int deal_mode { get; set; }  // 发牌模式 0 同时   1 顺序发牌
            public string room_create_time { get; set; }  // 建房时间戳，如1548385475000
            public int bringin_limit_on { get; set; }  // 限制补充 0关闭 1开启
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int room_port { get; set; }  // 房间服务器端口，如8053
            public string room_name { get; set; }  // 牌局名称
            public int room_master { get; set; }  // 是否房主0 1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int op_time { get; set; }  // 操作时间s，如20
            public int control { get; set; }  // 是否开启控制带入，0
            public int blind { get; set; }  // 分值
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如8
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public int game_time { get; set; }  // 牌局时长（分），如120
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int jokers_on { get; set; }  // 是否开启赖子模式，0 1
            public int tillOver_on { get; set; }  // 是否开启一战到底，0 1
            public int player_count { get; set; }  // 牌桌人数，如3
            public int room_id { get; set; }  // 房间号，如608122
            public int mode { get; set; }  // 1普通模式 2血战模式 3血进血出
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int room_path { get; set; }  // roomPath，如51
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_aof_enter  // 推推乐进房间
    {
        public const string API = @"/api/aof/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 41
            public int room_id { get; set; }  // 房间ID，如372019
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 同普通局
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int muck_switch { get; set; }  // 是否开启Muck,0 1
            public int straddle { get; set; }  // 是否开启Straddle 0 1
            public int shortest_time { get; set; }  // 最短上桌时间（分钟）,如30
            public int qianzhu { get; set; }  // 前注
            public int carry_small { get; set; }  // 最小带入USDT，如200
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT 
            public string room_create_time { get; set; }  // 建房时间戳，如1548322786000
            public int room_status { get; set; }  // 房间状态，0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            public int min_take_in_rate { get; set; }  // 最少带入倍数，如1
            public int small_blind { get; set; }  // 小盲，如1
            public int ip_restrict { get; set; }  // 是否开启ip限制，0 1
            public int room_port { get; set; }  // 房间服务器端口，如8051
            public string room_name { get; set; }  // 牌局名称
            public int insurance { get; set; }  // 是否开启保险，0 1
            public int room_master { get; set; }  // 是否房主，0 1
            public int gps_restrict { get; set; }  // 是否开启gps限制，0 1
            public int commission { get; set; }  // 基金，未使用
            public int control { get; set; }  // 是否开启控制带入，0
            public int max_take_in_rate { get; set; }  // 最大带入倍数，如4
            public int carry_big { get; set; }  // 带入上限，如2147483647
            public string room_ip { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int game_time { get; set; }  // 牌局时长（分），如30
            public int room_master_id { get; set; }  // 房主用户ID，如172378
            public int jackPot_fund { get; set; }  // jackPot基金，如60824
            public int jackPot_on { get; set; }  // 是否开启JackPot，0 1
            public int jackPot_id { get; set; }  // JackPot奖池的id，如190
            public int player_count { get; set; }  // 牌桌人数，如9
            public int room_id { get; set; }  // 房间号，如961672
            public int adv_setting { get; set; }  // 是否允许高级设置，固定为1
            public int room_path { get; set; }  // roomPath，如41
            public string client_ip { get; set; }  // 当前客户端的ip地址，在牌局内坐下带入时要用到
            public int charge { get; set; }  // 记录费
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_customer_detail  // 获取用户信息
    {
        public const string API = @"/api/customer/detail";

        public sealed class RequestData : RequestDataBase
        {
            public int user_id { get; set; }  // 要查询的用户id
            public int operator_id { get; set; }  // 请求接口的用户id
            public int room_type { get; set; }  // 房间内用时，普通房间为0，omaha为1
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public string randomNum { get; set; }  // 显式用户id，如1715234978
            public int remarkColor { get; set; }  // 用户标志颜色，0无 1颜色1 2颜色2 3颜色3
            public int win_rate { get; set; }  // 胜率
            public int sex { get; set; }  // 性别，0男 1女
            public int chip { get; set; }  // USDT余额，如394558
            public int idou { get; set; }  // 钻石余额，如990188
            public int lose_number { get; set; }  // 输了多少手
            public int level_index { get; set; }  // 未知
            public string remarkName { get; set; }  // 备注名
            public string alias { get; set; }  // 打法标志
            public int spectrum_count { get; set; }  // 已收藏的牌谱数
            public int honorNum { get; set; }  // 荣耀分
            public int win_number { get; set; }  // 赢了多少手
            public int vip_level { get; set; }  // 玩家的VIP类型 0无VIP 1侯爵 2伯爵 3王公
            public int everyday_chip { get; set; }  // 是否已经领取每天奖励，0 1
            public string head_pic_name { get; set; }  // 头像ID
            public int pool_rate { get; set; }  // 入池率
            public int modify_nick_num { get; set; }  // 修改昵称次数
            public string person_sign { get; set; }  // 个性签名
            public string vip_end_date { get; set; }  // VIP结束时间
            public string nick { get; set; }  // 昵称
            public int coin { get; set; }  // 未使用
            public int game_cnt { get; set; }  // 未使用
            public int vip { get; set; }  // 未使用
            public int pool_cnt { get; set; }  // 总手数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_customer_update  // 用户信息修改
    {
        public const string API = @"/api/customer/update";

        public sealed class RequestData : RequestDataBase
        {
            public string nick_name { get; set; }  // 新昵称
            public string head { get; set; }  // 新头像id
            public int sex { get; set; }  // 新性别，0男 1女
            public string sign { get; set; }  // 新签名
            public int user_id { get; set; }  // 用户ID
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0 成功 1失败 2网络错误 3已有相同昵称 4钻石余额不足 5不合法的昵称
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_upload_head  // 头像上传
    {
        public const string API = @"/api/upload/head";

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0 成功
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public string pic_name { get; set; }  // 头像id
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_pay_product_list  // 商城列表
    {
        public const string API = @"/api/pay/product/list";

        public sealed class RequestData : RequestDataBase
        {
            public int ios_pay { get; set; }  // 1为内购
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0 成功
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public string banner { get; set; }  // 顶部Banner图链接
            public List<ListElement> list { get; set; }
        }

        public sealed class ListElement
        {
            public string price_show { get; set; }  // ¥20
            public string title { get; set; }  // x 200
            public string icon { get; set; }  // icon链接
            public int price { get; set; }  // 20
            public int state { get; set; }  // 0不可用 1可用
            public string pdt_id { get; set; }  // xxxxx_6
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_pay_recharge_apply  // 创建订单
    {
        public const string API = @"/api/pay/recharge/apply";

        public sealed class RequestData : RequestDataBase
        {
            public int pay_type { get; set; }  // 0 = 系统决定支付类型  1 = 支付宝 2 = 内购
            public string pdt_id { get; set; }  // xxxxx_8
            public int lang { get; set; }
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int pay_type { get; set; }  // 0 = 系统决定支付类型  1 = 支付宝 2 = 内购
            public string order_id { get; set; }  // 订单号
            public int pay_mode { get; set; }  // 1支付宝sdk 2跳转网页
            public string pay_info { get; set; }  // 发起订单的信息或链接
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_pay_recharge_check  // 较验订单
    {
        public const string API = @"/api/pay/recharge/check";

        public sealed class RequestData : RequestDataBase
        {
            public string order_id { get; set; }  // xxxxx_8
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int pay_status { get; set; }  // 支付状态，0=待支付 ， 1=支付成功 ， 2=支付失败
            public int chip { get; set; }  // 当前用户USDT余额
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_pay_iospay_verify  // 校验内购订单
    {
        public const string API = @"/api/pay/iospay/verify";

        public sealed class RequestData : RequestDataBase
        {
            public string order_id { get; set; }  // xxxxx_8
            public int pay_status { get; set; }  // 1
            public string trade_no { get; set; }
            public string receipt { get; set; }
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public string pdt_id { get; set; }
            public string order_id { get; set; }
            public string trade_no { get; set; }
            public int idous { get; set; }
            public int chips { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_vip_config  // 三种贵族特权详情
    {
        public const string API = @"/api/vip/config";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public List<DataElement> data { get; set; }  // 包含元素:DataElement
        }

        public sealed class DataElement
        {
            public string corp { get; set; }  // 创建俱乐部数
            public string cardspectrum { get; set; }  // 保存牌谱上限
            public int view_days { get; set; }  // 数据查看天数 -1为生涯
            public string coin { get; set; }  // 赠送USDT（暂无）
            public string corp_guild { get; set; }  // 同盟俱乐部数（暂无）
            public string remark { get; set; }  // 备注上限（暂无）-1为无限
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_user_vip_info  // 我的贵族特权
    {
        public const string API = @"/api/user/vip/info";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int tribe_club_num { get; set; }  // 已用公会战队数
            public int club_new_num { get; set; }  // 已用创建俱乐部数
            public string vip_end_date { get; set; }  // vip到期时间，如1568390400
            public int vip_card_type { get; set; }  // 未使用
            public int collect_spectrum { get; set; }  // 已用保存牌谱数
            public int vip_level { get; set; }  // VIP类型 0无VIP 1侯爵 2伯爵 3王公
            public string vip_start_date { get; set; }  // vip开始时间，如1534348800
            public int remain_days { get; set; }  // VIP剩余天数
            public int remark_num { get; set; }  // 已用备注数
            public int remain_fee { get; set; }  // VIP剩余价值
            public int tribe_created_num { get; set; }  // 已用创建公会数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_user_vip_setting  // 开通/续费/过期后降级/升级VIP
    {
        public const string API = @"/api/user/vip/setting";

        public sealed class RequestData : RequestDataBase
        {
            public int card_num { get; set; }  // 购买个数
            public int card_type { get; set; }  // 购买类型 1月卡 2年卡
            public int vip_level { get; set; }  // 购买VIP类型 1侯爵 2伯爵 3王公
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int idou { get; set; }  // 购买后钻石余额
            public int remain_fee { get; set; }  // VIP剩余价值
            public int vip_level { get; set; }  // 当前VIP类型 0无 1侯爵 2伯爵 3王公
            public string vip_end_date { get; set; }  // vip到期时间，如1599926400
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_user_info_break  // 破隐
    {
        public const string API = @"/api/user/info/break";

        public sealed class RequestData : RequestDataBase
        {
            public int break_user_id { get; set; }  // 被破隐人id
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0 成功 3钻石余额不足
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_report_spectrum_collect_list  // 获取收藏牌谱列表
    {
        public const string API = @"/api/report/spectrum_collect_list";

        public sealed class RequestData : RequestDataBase
        {
            public int page_no { get; set; }  // 第几页 1开始
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public List<HandsElement> hands { get; set; }  // 包含元素:HandsElement
            public int count { get; set; }  // 数量
            public int page { get; set; }  // 页数
        }

        public sealed class HandsElement
        {
            public string handName { get; set; }  // 手牌名字
            public string id { get; set; }  // 手牌的id
            public int bigBlind { get; set; }  // 大盲注
            public int smallBlind { get; set; }  // 小盲注
            public string timeStamp { get; set; }  // 时间，如1525765314000
            public string win { get; set; }  // 输赢的筹码，包含正负号，如-2，+10
            public string roomType { get; set; }  // 牌局类型 0:普通 1:奥马哈，只有牌局牌谱列表接口有该字段
            public int index { get; set; }  // 编号
            public int collect { get; set; }  // 是否已收藏，只有牌谱收藏列表接口有该字段
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_report_collect_spectrum  // 设置牌谱是否收藏
    {
        public const string API = @"/api/report/collect_spectrum";

        public sealed class RequestData : RequestDataBase
        {
            public string hand_id { get; set; }  // 手牌的id，如5af154d570401f4ad1c7c320
            public int is_collect { get; set; }  // 是否需要收藏，0取消收藏 1收藏
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0 成功 1失败 2网络错误 4 超过收藏上限
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int spectrum_count { get; set; }  // 共收藏的牌谱数14
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_data_person_profit  // 数据统计（注意，按以下牌局分类返回不同的数据【德州，奥马哈】【大菠萝】【SNG，MTT】）
    {
        public const string API = @"/api/data/person_profit";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 普通局:61 MTT:71 SNG:81 奥马哈:91 大菠萝:51
            public int time_type { get; set; }  // 1 全部 2 近一个月 3 近7天
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int room_path { get; set; }  // 普通局:61 MTT:71 SNG:81 奥马哈:91 大菠萝:51
            public int time_type { get; set; }  // 1 全部 2 近一个月 3 近7天
            public int show_advance { get; set; }  // 是否显示高阶数据 0 1
            public int Allin_Wins { get; set; }  // 全下胜率//==========从这开始为【德州，奥马哈】特有返回数据==========
            public int aveageEarn { get; set; }  // 场均战绩
            public int totalEarn { get; set; }  // 战绩总积分
            public int totalGameCnt { get; set; }  // 总局数
            public int aveageBringin { get; set; }  // 场均带入
            public int totalHand { get; set; }  // 总手数
            public int VPIP { get; set; }  // 入池率，如18代表18%
            public int PRF { get; set; }  // 翻牌前加注，如1代表1%
            public int CBet { get; set; }  // 持续下注，如20代表20%
            public int Wins { get; set; }  // 入池胜率，如10代表10%
            public int AF { get; set; }  // 激进率，如17代表17%
            public int threeBet { get; set; }  // 翻前再加注，如20代表20%
            public int WTSD { get; set; }  // 摊牌率，如79代表79%
            public int fantasy_average { get; set; }  // 进范平均分//==========从这开始为【大菠萝】特有返回数据==========
            public int hand_average { get; set; }  // 手牌平均分
            public int pap_Wins { get; set; }  // 胜率，如30代表30%
            public int fantasy { get; set; }  // 进范率，如5代表5%
            public int play_times { get; set; }  // 参赛次数//==========从这开始为【SNG，MTT】特有返回数据==========
            public int win_times { get; set; }  // 获奖次数
            public int first_times { get; set; }  // 第一名次数
            public int second_times { get; set; }  // 第二名次数
            public int third_times { get; set; }  // 第三名次数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_report_list  // 请求战绩时间线列表
    {
        public const string API = @"/api/report/list";

        public sealed class RequestData : RequestDataBase
        {
            public string lang { get; set; }  // 语言，中文zh 英文en
            public string time { get; set; }  // 最后一条记录的时间，如果为刷新，传0
            public int direction { get; set; }  // 1下拉刷新 0加载更多
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public List<int> total_hands { get; set; }  // 总手数，包含元素:int
            public List<string> club_name { get; set; }  // 俱乐部名称，包含元素:string
            public List<int> total_bring { get; set; }  // 总带入，包含元素:int
            public List<int> roomModeArr { get; set; }  // 奥马哈是否血战模式，0否 1是，包含元素:int
            public List<int> dealModeArr { get; set; }  // 大菠萝发牌模式，0同步发牌 1顺序发牌，包含元素:int
            public List<int> room_type { get; set; }  // 房间类型：0普通局，1战队有限时长局，2战队无限时长局，包含元素:int
            public List<string> room_name { get; set; }  // 房间名称，包含元素:string
            public List<int> insurance { get; set; }  // 是否开启保险，0 1，包含元素:int
            public List<string> head_pic { get; set; }  // 牌局头像id，包含元素:string
            public List<string> blind { get; set; }  // 大小盲，包含元素:string
            public List<string> nick { get; set; }  // 房主名称，包含元素:string
            public List<int> game_time { get; set; }  // 牌局时间(m)，包含元素:int
            public List<int> jackPotArr { get; set; }  // 是否开启JackPot 0 否 1 是，包含元素:int
            public List<string> start_time { get; set; }  // 开始时间，包含元素:string
            public int direction { get; set; }  // 1下拉刷新 0加载更多
            public List<string> display_id { get; set; }  // 显示房间ID,包含元素:string
            public List<string> room_id { get; set; }  // 真正的房间ID，用于点详情时请求参数，包含元素:string
            public List<int> person_record { get; set; }  // 个人战绩,包含元素:int
            public int total_record { get; set; }  // 总记录条数，如200
            public List<string> room_dec { get; set; }  // 房间描述，包含元素:string
            public List<int> qianzhu { get; set; }  // 前注，包含元素:int
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_report_detail  // 请求战绩详情
    {
        public const string API = @"/api/report/detail";

        public sealed class RequestData : RequestDataBase
        {
            public string room_id { get; set; }  // 真正的房间ID，如5c4abadc70401f2379945c70
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public List<int> top_remark { get; set; }  // 【MVP，大鱼，土豪】的标注颜色，包含元素:int
            public int room_id { get; set; }  // 房间号，如767295
            public List<string> jack_nick { get; set; }  // JackPot获奖用户昵称，包含元素:string
            public List<int> total_bring { get; set; }  // 总带入，包含元素:int
            public int vp { get; set; }  // 是否开启当桌入池率，0 1
            public List<int> all_insurance { get; set; }  // 所有玩家的保险盈利，包含元素:int
            public List<int> jack_reward { get; set; }  // JackPot获奖用户奖励，包含元素:int
            public int insurance_pool { get; set; }  // 保险池
            public List<int> total_remark { get; set; }  // 所有玩家的标注颜色，包含元素:int
            public List<string> top_head_pic { get; set; }  // 【MVP，大鱼，土豪】的头像id，包含元素:string
            public List<int> vp_array { get; set; }  // 所有玩家的当桌入池率，包含元素:int
            public int competeMode { get; set; }  // 大菠萝是否开启竞技模式，0 1
            public int fund { get; set; }  // 未使用
            public List<string> insure_total { get; set; }  // 未使用，包含元素:string
            public List<int> total_record { get; set; }  // 带入，包含元素:int
            public List<string> club_pl { get; set; }  // 俱乐部战绩，包含元素:string
            public int insurance { get; set; }  // 是否开启保险，0 1
            public List<string> club_name { get; set; }  // 所有玩家俱乐部名称，包含元素:string
            public int total_count { get; set; }  // 总带入
            public List<string> insure_earn { get; set; }  // 未使用，包含元素:string
            public int commission { get; set; }  // 基金，未使用
            public List<string> jack_card { get; set; }  // JackPot获奖牌型，包含元素:string
            public List<int> total_hand { get; set; }  // 手数，包含元素:int
            public List<string> play_earn { get; set; }  // 未使用，包含元素:string
            public int jackpot { get; set; }  // 是否开启JackPot，0 1
            public List<int> jack_userid { get; set; }  // JackPot获奖玩家id，包含元素:int
            public List<int> user_id { get; set; }  // 所有玩家id，包含元素:int
            public List<int> total_reward { get; set; }  // 所有玩家积分，包含元素:int
            public int hand { get; set; }  // 总手数
            public List<string> club_name_no_repeat { get; set; }  // 俱乐部名称（俱乐部战绩），包含元素:string
            public List<string> all_head_pic { get; set; }  // 所有玩家头像id，包含元素:string
            public List<string> top_nick { get; set; }  // 【MVP，大鱼，土豪】的昵称，包含元素:string
            public List<string> all_nick { get; set; }  // 所有玩家昵称，包含元素:string
            public int max_pot { get; set; }  // 最大底池
            public int manager { get; set; }  // 是否管理员，0 1 管理员需要展示战队战绩和保险详情
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_report_sng_list  // 请求比赛战绩列表
    {
        public const string API = @"/api/report/sng-list";

        public sealed class RequestData : RequestDataBase
        {
            public string lang { get; set; }  // 语言，中文zh 英文en
            public string time { get; set; }  // 最后一条记录的时间，如果为刷新，传0
            public int direction { get; set; }  // 1下拉刷新 0加载更多
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public List<SngListElement> sngList { get; set; }  // 包含元素:SngListElement
            public int direction { get; set; }  // 1下拉刷新 0加载更多
        }

        public sealed class SngListElement
        {
            public int rank { get; set; }  // 本人排名
            public int totalBonus { get; set; }  // MTT总奖池
            public int initial_coin { get; set; }  // 初始带入筹码
            public string creator { get; set; }  // 房主昵称
            public int owner_id { get; set; }  // 房主ID
            public string club_name { get; set; }  // 俱乐部名称
            public int user_id { get; set; }  // 用户id
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT 
            public int sng_id { get; set; }  // 比赛id
            public string sng_name { get; set; }  // 比赛名称
            public int hand { get; set; }  // 总手数
            public int type { get; set; }  // SNG赛事类型(0快速局 1标准局)
            public string id { get; set; }  // 真正房间id，5c4ab87970401f593356075d
            public int player { get; set; }  // 总人数
            public int game_time { get; set; }  // 总耗时
            public int start_time { get; set; }  // 开始时间，如1548400768
            public int end_time { get; set; }  // 结束时间，如1548400828
            public int voucher { get; set; }  // MTT门票
            public int room_path { get; set; }  // 81
            public int registationFee { get; set; }  // 报名费
            public string voucher_name { get; set; }  // 门票字符串
            public int bonus_num { get; set; }  // 总赢家数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_report_sng_detail  // 请求比赛战绩详情
    {
        public const string API = @"/api/report/sng-detail";

        public sealed class RequestData : RequestDataBase
        {
            public int sng_id { get; set; }  // 比赛id，如356058
            public int room_path { get; set; }  // MTT:71 SNG:81
            public int page { get; set; }  // 1
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public string creator { get; set; }  // 房主昵称
            public int voucher { get; set; }  // MTT门票
            public int isManager { get; set; }  // 是否管理员
            public string id { get; set; }  // 真正房间id，如5c4ab8f870401f59f14938c2
            public int initial_coin { get; set; }  // 初始带入筹码
            public int end_time { get; set; }  // 结束时间
            public int player { get; set; }  // 总人数
            public int rank { get; set; }  // 本人排名
            public int registationFee { get; set; }  // MTT报名费
            public int type { get; set; }  // SNG赛事类型(0快速局 1标准局)
            public int sng_id { get; set; }  // 比赛id，如356058
            public List<string> top_head_pic { get; set; }  // 前几名(最多3)玩家头像id，包含元素:string
            public List<string> all_club_id { get; set; }  // 所有玩家的俱乐部id，包含元素:string
            public string sng_name { get; set; }  // 比赛名称
            public List<int> top_userid { get; set; }  // 前几名(最多3)玩家id，包含元素:int
            public List<string> all_club { get; set; }  // 所有玩家的俱乐部名称，包含元素:string
            public List<string> all_reward { get; set; }  // 所以玩家的奖励积分，包含元素:string
            public int room_path { get; set; }  // MTT:71 SNG:81
            public int room_type { get; set; }  // 房间类型，0普通德州局 1俱乐部德州局 3普通SNG 4俱乐部SNG 5普通奥马哈 6俱乐部奥马哈 7普通大菠萝 8俱乐部大菠萝 9普通必下场 10俱乐部必下场 11普通MTT 12俱乐部MTT 
            public List<int> all_hunter { get; set; }  // 所有玩家的猎人赏金，包含元素:int
            public int totalBonus { get; set; }  // MTT总奖池
            public int bonus { get; set; }  // 我获得的筹码
            public List<string> all_portrait { get; set; }  // 所有玩家头像列表，包含元素:string
            public string voucher_name { get; set; }  // 门票字符串
            public List<int> all_rebuytime { get; set; }  // 所有玩家的重购次数，包含元素:int
            public List<string> no_repeat_club_id { get; set; }  // 俱乐部id(俱乐部战绩),包含元素:string
            public int start_time { get; set; }  // 开始时间,如1548401290
            public int user_id { get; set; }  // 我的用户id
            public int hand { get; set; }  // 总手数
            public int bonus_num { get; set; }  // 总赢家数
            public int owner_id { get; set; }  // 房主ID
            public List<string> top_nick { get; set; }  // 前几名(最多3)玩家昵称，包含元素:string
            public List<int> all_userid { get; set; }  // 所有玩家的用户id，包含元素:int
            public string rewardName { get; set; }  // 我获得的奖品
            public List<string> all_nick { get; set; }  // 所有玩家的昵称，包含元素:string
            public List<string> no_repeat_club { get; set; }  // 俱乐部昵称(俱乐部战绩)，包含元素:string
            public int game_time { get; set; }  // 总耗时
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_report_sng_club_detail  // 比赛详情中的俱乐部战绩
    {
        public const string API = @"/api/report/sng-club-detail";

        public sealed class RequestData : RequestDataBase
        {
            public int sng_id { get; set; }  // 比赛ID，如356058
            public int room_path { get; set; }  // MTT:71 SNG:81
            public int club_id { get; set; }  // 俱乐部id，如101256
            public int page { get; set; }  // 页数，1开始
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public List<int> all_rebuytime { get; set; }  // 所有玩家的重购次数，包含元素:int
            public int room_path { get; set; }  // MTT:71 SNG:81
            public List<int> all_hunter { get; set; }  // 所有玩家的猎人赏金，包含元素:int
            public List<string> all_reward { get; set; }  // 所以玩家的奖励积分，包含元素:string
            public List<int> all_userid { get; set; }  // 所有玩家的用户id，包含元素:int
            public List<string> all_nick { get; set; }  // 所有玩家的昵称，包含元素:string
            public List<int> all_rank { get; set; }  // 所有玩家的排名，包含元素:int
            public List<string> all_portrait { get; set; }  // 所有玩家头像列表，包含元素:string
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_bag_view  // 获取背包列表
    {
        public const string API = @"/api/mtt/bag-view";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public List<DataElement> data { get; set; }  // 包含元素:DataElement
        }

        public sealed class DataElement
        {
            public int gameId { get; set; }  // 门票跳转的比赛id
            public int propStatus { get; set; }  // 物品状态，0未使用 1使用中 2已使用
            public int id { get; set; }  // 物品id
            public string propDesc { get; set; }  // 物品描述，如：获得时间：2017-06-19\n获得赛事：可用于报名官方 MTT 赛事
            public string propIcon { get; set; }  // 物品图片，完整链接
            public int propValue { get; set; }  // 物品价值，如100
            public int isVirtual { get; set; }  // 是否虚拟物品，未使用，0否 1是
            public int gameType { get; set; }  // 未使用
            public string propName { get; set; }  // 物品名称，如：100元参赛券
            public int propType { get; set; }  // 物品类型，1 mtt门票 2 物品
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_add_consignee  // 编辑收货地址
    {
        public const string API = @"/api/mtt/add_consignee";

        public sealed class RequestData : RequestDataBase
        {
            public string mobile { get; set; }  // 电话
            public string wechat { get; set; }  // 微信号
            public string address { get; set; }  // 地址
            public string name { get; set; }  // 姓名
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_check_consignee  // 检查收货地址
    {
        public const string API = @"/api/mtt/check_consignee";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 1为已有地址记录 0为未有记录
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public string address { get; set; }  // 地址
            public int id { get; set; }  // 记录id
            public string mobile { get; set; }  // 电话
            public string wechat { get; set; }  // 微信号
            public int userId { get; set; }  // 用户id
            public string userName { get; set; }  // 姓名
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_apply_prize  // 背包使用
    {
        public const string API = @"/api/mtt/apply-prize";

        public sealed class RequestData : RequestDataBase
        {
            public int id { get; set; }  // 物品id
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0成功 1未填写地址 2没有该物品 3该奖品已经申请过领取 4该奖品已经领取
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_user_report  // 举报
    {
        public const string API = @"/api/user/report";

        public sealed class RequestData : RequestDataBase
        {
            public string report_content { get; set; }  // 举报内容
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_list  // 俱乐部列表
    {
        public const string API = @"/api/club/list";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public List<ListElement> list { get; set; }  // 包含元素:ListElement
            public int idou { get; set; }  // 用户钻石余额，如978676
        }

        public sealed class ListElement
        {
            public int onlineCount { get; set; }  // 俱乐部成员人数
            public string id { get; set; }  // 俱乐部id，如club_101667
            public int totalCount { get; set; }  // 俱乐部最大可用容纳成员数
            public string showId { get; set; }  // 俱乐部显示id，如club_522329
            public int time { get; set; }  // 创建时间，如1537962431
            public int type { get; set; }  // 我的身份，1=创建者、2=成员. 3=非成员 4-管理员
            public string header { get; set; }  // 俱乐部头像id，没有设置为-1
            public string areaID { get; set; }  // 地区id，如211，客户端在本地查对应的地区名
                                                //public int switch { get; set; }  // 仅允许房主组局 0-否，1-是
            public int official_club { get; set; }  // 是否官方俱乐部，如0 1
            public string name { get; set; }  // 俱乐部名称
            public int status { get; set; }  // 该俱乐部当前的牌局数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_new  // 创建俱乐部
    {
        public const string API = @"/api/club/new";

        public sealed class RequestData : RequestDataBase
        {
            public string club_name { get; set; }  // 俱乐部名称
            public string area_id { get; set; }  // 地区ID，如9991514
            public string desc { get; set; }  // 简介
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0 成功 1失败 2 创建失败，您已不能创建更多社区 5创建失败，已存在的社区名 88 你建立的社区已达上限xx个，请提升会员等级以获得更多社区权限。
        }

        public sealed class Data
        {
            public string showId { get; set; }  // 显示id，如club_680665
            public string clubId { get; set; }  // 俱乐部id，如club_101767
            public int idou { get; set; }  // 剩余钻石，如978176
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_search_area  // 搜索俱乐部
    {
        public const string API = @"/api/club/search-area";

        public sealed class RequestData : RequestDataBase
        {
            public string area_id { get; set; }  // 9991514
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // 获取数据成功
            public List<DataElement> data { get; set; }  // 包含元素:DataElement
            public int status { get; set; }  // 0
        }

        public sealed class DataElement
        {
            public string showId { get; set; }  // 显示id，如club_100287
            public string areaID { get; set; }  // 地区id，如9991514，客户端在本地查对应的地区名
            public int onlineCount { get; set; }  // 俱乐部成员人数
            public string header { get; set; }  // 俱乐部头像id，没有设置为-1
            public string id { get; set; }  // 俱乐部id，如club_100287
            public int totalCount { get; set; }  // 俱乐部最大可用容纳成员数
            public string status { get; set; }  // 该俱乐部当前的牌局数
            public int type { get; set; }  // 我的身份，1=创建者、2=成员. 3=非成员 4-管理员
            public string name { get; set; }  // 俱乐部名称
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_search  // 通过ID搜索俱乐部
    {
        public const string API = @"/api/club/search";

        public sealed class RequestData : RequestDataBase
        {
            public string key { get; set; }  // 搜索关键词，当前只支持id，如788138
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // 获取数据成功
            public List<DataElement> data { get; set; }  // 包含元素:DataElement
            public int status { get; set; }  // 0
        }

        public sealed class DataElement
        {
            public string showId { get; set; }  // 显示id，如club_100287
            public string areaID { get; set; }  // 地区id，如9991514，客户端在本地查对应的地区名
            public int onlineCount { get; set; }  // 俱乐部成员人数
            public string header { get; set; }  // 俱乐部头像id，没有设置为-1
            public string id { get; set; }  // 俱乐部id，如club_100287
            public int totalCount { get; set; }  // 俱乐部最大可用容纳成员数
            public string status { get; set; }  // 该俱乐部当前的牌局数
            public int type { get; set; }  // 我的身份，1=创建者、2=成员. 3=非成员 4-管理员
            public string name { get; set; }  // 俱乐部名称
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_apply_join  // 申请加入俱乐部
    {
        public const string API = @"/api/club/apply-join";

        public sealed class RequestData : RequestDataBase
        {
            public string club_id { get; set; }  // 申请加入的俱乐部，如club_101096
            public string remark { get; set; }  // 验证信息，可不填
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 已发送申请 2/6 俱乐部不存在 3俱乐部已满员 4你已经是社区成员
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_view  // 俱乐部详情
    {
        public const string API = @"/api/club/view";

        public sealed class RequestData : RequestDataBase
        {
            public string club_id { get; set; }  // 俱乐部id，如club_101256
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public int myCreditValue { get; set; }  // 我的信用额度，如143
            public int totalCount { get; set; }  // 俱乐部最大可用容纳成员数
            public string showId { get; set; }  // 俱乐部显示id，如club_788138
            public List<UserListElement> userList { get; set; }  // 预览用户列表，未使用，包含元素:UserListElement
            public string creator { get; set; }  // 创建者昵称
            public int pushStatus { get; set; }  // 推送通知 0-关，1-开
            public int jackPotID { get; set; }  // 俱乐部JackPot的id
            public int idou { get; set; }  // 当前用户钻石余额，如978176
            public int tribeCount { get; set; }  // 同盟个数
            public int creditStatus { get; set; }  // 信用额度开关，现在都为1
            public string name { get; set; }  // 俱乐部名称
            public int type { get; set; }  // 我的身份，1=创建者、2=成员. 3=非成员 4-管理员
            public int creditValue { get; set; }  // 俱乐部默认的信用额度，如1000
            public int onlineCount { get; set; }  // 俱乐部成员人数
            public int tribe_jackPotID { get; set; }  // 同盟JackPot的id
            public string areaID { get; set; }  // 地区代号，如9991514
                                                //public int switch { get; set; }  // 仅允许房主组局 0-否，1-是
            public int official_club { get; set; }  // 是否官方俱乐部，0 1
            public string header { get; set; }  // 俱乐部头像id
            public int creatorID { get; set; }  // 创建者用户id
            public int ownTribe { get; set; }  // 是否有创建同盟，0无 1有
            public string desc { get; set; }  // 俱乐部简介
            public string clubID { get; set; }  // 俱乐部id，如club_101256
            public int clearStatus { get; set; }  // 信用分清算状态，0没有进行中的信用清算 1正在进行信用清算
            public int fund { get; set; }  // 基金余额
        }

        public sealed class UserListElement
        {
            public string nick { get; set; }  // 成员昵称
            public string uid { get; set; }  // 成员用户id
            public int type { get; set; }  // 成员的身份，1=创建者、2=成员. 3=非成员 4-管理员
            public string avatar { get; set; }  // 成员头像id
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_update  // 修改俱乐部信息
    {
        public const string API = @"/api/club/update";

        public sealed class RequestData : RequestDataBase
        {
            public string club_id { get; set; }  // 俱乐部id，如club_101767
            public string value { get; set; }  // 修改后的值，头像名称简介传修改后的字符串，开关传0，1
            public string key { get; set; }  // 修改内容，头像：header， 名称：name， 简介：desc， 仅允许房主组局:switch， 推送通知:pushStatus
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_update_limit  // 升级俱乐部人数
    {
        public const string API = @"/api/club/update-limit";

        public sealed class RequestData : RequestDataBase
        {
            public string club_id { get; set; }  // 俱乐部id，如club_101767
            public int idou { get; set; }  // 需要花费的钻石
            public int num { get; set; }  // 增加的人数（非增加后的人数）
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public int idou { get; set; }  // 升级后的个人钻石余额
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_tribe_list  // 俱乐部的同盟列表
    {
        public const string API = @"/api/club/tribe_list";

        public sealed class RequestData : RequestDataBase
        {
            public int club_id { get; set; }  // 俱乐部id，如101256
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // 获取成功
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public List<ListElement> list { get; set; }  // 包含元素:ListElement
        }

        public sealed class ListElement
        {
            public int id { get; set; }  // 同盟id，如100148
            public int showId { get; set; }  // 同盟显示id，如5702183
            public string name { get; set; }  // 同盟名称
            public int type { get; set; }  // 同盟类型 1创建的同盟， 2加入的同盟。 去除结盟功能后不再使用的：（3互相结盟的同盟 4向我结盟的同盟 5我发起结盟的同盟）
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_new  // 创建同盟接口
    {
        public const string API = @"/api/tribe/new";

        public sealed class RequestData : RequestDataBase
        {
            public int club_id { get; set; }  // 俱乐部id，如101257
            public string tribe_name { get; set; }  // 创建的同盟名称
            public int creator { get; set; }  // 创建者id，如172378
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0 创建成功 3 同盟名称已存在 4已有创建好的同盟，不可再创建 6当前社区加入的同盟数量已达到上限
        }

        public sealed class Data
        {
            public int showId { get; set; }  // 同盟显示id，如4263926
            public int tribeId { get; set; }  // 同盟id，如100426
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_apply_join  // 申请加入同盟
    {
        public const string API = @"/api/tribe/apply-join";

        public sealed class RequestData : RequestDataBase
        {
            public int tribe_id { get; set; }  // 要加入的同盟显示id，如4263926
            public int club_id { get; set; }  // 俱乐部id，如101256
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 申请成功 3 查找不到该同盟 4 同盟已满员，不能纳入新的俱乐部 5 该同盟已解散 6 当前俱乐部加入的同盟数量已达到上限 7 你已经加入了该同盟 8 您所申请的同盟已和您所在的同盟结盟，不可以加入该同盟
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_club_list  // 同盟详情
    {
        public const string API = @"/api/tribe/club_list";

        public sealed class RequestData : RequestDataBase
        {
            public int tribe_id { get; set; }  // 同盟id，如100148
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0 请求成功
        }

        public sealed class Data
        {
            public int status { get; set; }  // 是否开启同盟申请，0开着公会申请 1已经关闭了
            public string tribe_creator { get; set; }  // 同盟创建者用户id
            public string tribe_creator_name { get; set; }  // 同盟创建者昵称
            public int club_room_sync { get; set; }  // 同盟创房同步开关，0 关 1开
            public List<ListElement> list { get; set; }  // 俱乐部列表，包含元素:ListElement
        }

        public sealed class ListElement
        {
            public int showId { get; set; }  // 俱乐部显示id，如788138
            public string areaID { get; set; }  // 地区代号，如9991514
            public string creator_id { get; set; }  // 俱乐部创建者id，如172378
            public string header { get; set; }  // 俱乐部头像id
            public int id { get; set; }  // 俱乐部id，如101256
            public string creatorName { get; set; }  // 俱乐部创建者昵称
            public int totalCount { get; set; }  // 俱乐部最大可用容纳成员数
            public int clubMembers { get; set; }  // 俱乐部成员数
            public int type { get; set; }  // 我在俱乐部的身份，1=创建者、2=成员. 3=非成员 4-管理员
            public string name { get; set; }  // 俱乐部名称
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_remove  // 把俱乐部从同盟中踢出
    {
        public const string API = @"/api/tribe/remove";

        public sealed class RequestData : RequestDataBase
        {
            public int tribe_id { get; set; }  // 同盟ID，如100189
            public int club_id { get; set; }  // 被踢出的俱乐部ID，如101667
            public int creator { get; set; }  // 同盟的创建者id，如172378
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 踢出成功 3 本俱乐部已加入所申请结盟的同盟
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_leave  // 俱乐部主动退出同盟
    {
        public const string API = @"/api/tribe/leave";

        public sealed class RequestData : RequestDataBase
        {
            public int tribe_id { get; set; }  // 同盟ID，如100189
            public int club_id { get; set; }  // 退出同盟的俱乐部ID，如101423
            public int creator { get; set; }  // 同盟的创建者id，如172378
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 退出成功
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_dismiss  // 解散同盟
    {
        public const string API = @"/api/tribe/dismiss";

        public sealed class RequestData : RequestDataBase
        {
            public int tribe_id { get; set; }  // 要解散的同盟id，如100425
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 解散成功 3 请清退所有同盟成员后再执行解散操作 4已结盟，必须解散结盟后才能解散您的同盟。
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_apply_switch  // 打开、关闭同盟申请
    {
        public const string API = @"/api/tribe/apply-switch";

        public sealed class RequestData : RequestDataBase
        {
            public int tribe_id { get; set; }  // 同盟id，如100189
            public int type { get; set; }  // 0 打开公会申请 1关闭公会申请
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 修改成功
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_tribe_sync_switch  // 打开、关闭同盟创房同步
    {
        public const string API = @"/api/tribe/sync-switch";

        public sealed class RequestData : RequestDataBase
        {
            public int syncStatus { get; set; }  // 0关 1开
            public int tribeId { get; set; }  // 同盟id，如100189
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 修改成功
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_fund_detail  // 俱乐部基金详情
    {
        public const string API = @"/api/club/fund-detail";

        public sealed class RequestData : RequestDataBase
        {
            public string time { get; set; }  // 第一页传0，加载更多时，传最后一条记录的time时间戳
            public string club_id { get; set; }  // 俱乐部id，如club_101256
            public string direction { get; set; }  // 1下拉刷新 0加载更多
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public int idou { get; set; }  // 个人钻石余额，如977576
            public int fund { get; set; }  // 基金余额
            public List<ListElement> list { get; set; }  // 包含元素:ListElement
            public int hasMore { get; set; }  // 是否还有更多数据，0 1
            public int direction { get; set; }  // 1下拉刷新 0加载更多
        }

        public sealed class ListElement
        {
            public int amount { get; set; }  // 基金变动额，如10000，根据类型来添加加减号
            public int time { get; set; }  // 记录生成时间，如1523516922
            public string userName { get; set; }  // 被操作的成员昵称
                                                  //public string operator { get; set; }  // 操作人昵称
            public int user_id { get; set; }  // 被操作成员的用户id，如182970
            public int type { get; set; }  // 记录类型（现在有使用的类型为3 4 6 7），0 已拒绝俱乐部基金申请 1 待审核俱乐部基金申请 2 已同意俱乐部基金申请 3 管理员主动发放俱乐部基金 4 充值记录 5 牌局抽水 6 注入俱乐部彩池 7 注入同盟彩池 8 俱乐部商城购买 9 牌局服务费收益
            public int operator_id { get; set; }  // 操作人用户id
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_recharge  // 俱乐部基金充值
    {
        public const string API = @"/api/club/recharge";

        public sealed class RequestData : RequestDataBase
        {
            public string club_id { get; set; }  // 俱乐部id，如club_101256
            public int fund { get; set; }  // 充值的基金，如36800
            public int idou { get; set; }  // 花费的钻石，如3280
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 充值成功
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_give_fund  // 俱乐部基金发放
    {
        public const string API = @"/api/club/give-fund";

        public sealed class RequestData : RequestDataBase
        {
            public int amount { get; set; }  // 发放基金额，如100
            public string user_id { get; set; }  // 要发放成员的用户id
            public string operator_id { get; set; }  // 操作人id，如172378
            public string club_id { get; set; }  // 俱乐部id，如club_101256
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 发放成功 3 您的基金已被冻结，如有疑问请联系官方客服。
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_mlist  // 获取俱乐部成员列表
    {
        public const string API = @"/api/club/mlist";

        public sealed class RequestData : RequestDataBase
        {
            public string uid { get; set; }  // 请求接口的用户id，如172378
            public string gid { get; set; }  // 俱乐部id，如club_101256
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public List<DataElement> data { get; set; }  // 包含元素:DataElement
            public int status { get; set; }  // 0
        }

        public sealed class DataElement
        {
            public string phone { get; set; }  // 手机号，现在都会返回0
            public string nick { get; set; }  // 昵称
            public int uid { get; set; }  // 成员userID，如172479
            public string avatar { get; set; }  // 成员头像id
            public int status { get; set; }  // 如本人关系，未使用
            public string signature { get; set; }  // 个性签名
            public int coin { get; set; }  // 该成员USDT余额
            public int vip_level { get; set; }  // 该成员VIP类型 0无 1侯爵 2伯爵 3王公
            public int type { get; set; }  // 该成员的身份，1=创建者、2=成员. 3=非成员 4-管理员
            public int sex { get; set; }  // 0男 1女
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    public partial class WEB_club_mod_trans
    {
        public const string API = @"/api/club/mod_trans";

        public sealed class RequestData : RequestDataBase
        {
            public int transType { get; set; }
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public List<DataElement> data { get; set; }  // 包含元素:DataElement
            public int status { get; set; }  // 0
        }

        public sealed class DataElement
        {

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    public partial class WEB_friend_e_detail  // 联系人详情
    {
        public const string API = @"/api/friend/e_detail";

        public sealed class RequestData : RequestDataBase
        {
            public int operator_id { get; set; }  // 请求接口的用户id，如172378
            public int uid { get; set; }  // 要查看的联系人id，如174585
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public int coin { get; set; }  // USDT余额，如1602472
            public int uid { get; set; }  // 用户id，如174585
            public int sex { get; set; }  // 0男 1女
            public int hand_cnt { get; set; }  // 参与手数
            public int remarkColor { get; set; }  // 用户标志颜色，0无 1颜色1 2颜色2 3颜色3
            public string org_nick { get; set; }  // 昵称
            public string nick { get; set; }  // 打法标志
            public int pool_rate { get; set; }  // 入池率
            public string avatar { get; set; }  // 头像id
            public string signature { get; set; }  // 个性签名
            public int win_rate { get; set; }  // 入池胜率
            public int series_cnt { get; set; }  // 参与局数
            public string remarkName { get; set; }  // 备注名
            public string randomNum { get; set; }  // 显示id，如2147483647
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_club_modify_remarks  // 修改联系人备注
    {
        public const string API = @"/api/club/modify-remarks";

        public sealed class RequestData : RequestDataBase
        {
            public string user_id { get; set; }  // 被修改人的id
            public string operator_id { get; set; }  // 操作人id
            public string remarks { get; set; }  // 打法标志
            public string club_id { get; set; }  // 俱乐部id，如club_101256，可不传
            public string remarkName { get; set; }  // 备注名
            public int remarkColor { get; set; }  // 标注颜色，0无 1颜色1 2颜色2 3颜色3
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_view  // MTT赛事详情
    {
        public const string API = @"/api/mtt/view";

        public sealed class RequestData : RequestDataBase
        {
            public int game_path { get; set; }  // 71
            public int game_id { get; set; }  // 比赛id，如228630
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }  // success
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public int advancedEntry { get; set; }  // 未使用
            public string serviceFee { get; set; }  // 服务费，如10
            public int isManager { get; set; }  // 是否管理员，0 1
            public int rebuyFees { get; set; }  // 重购费用
            public int maxDelayLevel { get; set; }  // 最大可延迟报名级别
            public string lowerLimit { get; set; }  // 最低报名人数
            public int entryFees { get; set; }  // 未使用
            public string startTime { get; set; }  // 比赛开始时间，如1548666161
            public string clubID { get; set; }  // 创建比赛的俱乐部id，如101256
            public string updateCycle { get; set; }  // 升盲时间（分钟），如2
            public string start { get; set; }  // 比赛是否已开始，0 1
            public string ipRestrictionOn { get; set; }  // 是否开启ip限制，0 1
            public int ante { get; set; }  // 当前前注
            public int registerFee { get; set; }  // 报名费
            public int appendTimes { get; set; }  // 未使用
            public int blindScale { get; set; }  // 盲注表倍数
            public string allowAppend { get; set; }  // 是否允许增购，0 1，未使用
            public string showTime { get; set; }  // 未使用
            public string client_ip { get; set; }  // 客户端的ip地址，如113.111.4.137
            public string upperLimit { get; set; }  // 报名人数上限
            public string activePlayers { get; set; }  // 未使用
            public int isCreator { get; set; }  // 是否创建者，0 1
            public int averageScore { get; set; }  // 计分牌平均值，如8000
            public string isControl { get; set; }  // 是否开启控制带入，0
            public string name { get; set; }  // 比赛名称
            public int participants { get; set; }  // 当前报名人数，如2
            public string maxLevel { get; set; }  // 最大可重购级别，如12
            public string initialPool { get; set; }  // 未使用
            public string gpsRestrictionOn { get; set; }  // 是否开启gps限制，0 1
            public string tribeId { get; set; }  // 同步到的同盟id，如100148
            public string initialChip { get; set; }  // 起始USDT
            public int totalChips { get; set; }  // 总奖池，如100
            public string totalBonus { get; set; }  // 未使用
            public int alreadyRebuy { get; set; }  // 本玩家已重购次数
            public int rebuyTimes { get; set; }  // 可重购次数上限
            public string leftPlayer { get; set; }  // 剩余玩家数
            public string leftBlindTime { get; set; }  // 涨盲剩余时间（s）
            public string allowRebuy { get; set; }  // 是否允许重购，0 1
            public int blindLevel { get; set; }  // 当前盲注级别
            public int rebuyStatus { get; set; }  // 0下方按钮显示“重购”  1其他状态
            public int runningTime { get; set; }  // 未使用
            public int allowDelay { get; set; }  // 是否允许延迟报名，0 1
            public string gameID { get; set; }  // 比赛id，如228630
            public int smallBlind { get; set; }  // 未使用
            public int gameStatus { get; set; }  // 0:可报名;1:等待开赛;2:延期报名;3:进行中;4:立即进入;5:报名截止;6:等待审批;7:已淘汰，且不能重购。
            public string entryTime { get; set; }  // 未使用
            public int appendFees { get; set; }  // 未使用
            public string gameType { get; set; }  // 比赛类型：1 欢乐赛，2 特别赛
            public string initialScore { get; set; }  // 起始计分牌，如8000
            public string blindType { get; set; }  // 盲注表类型 0普通 1快速
            public int isHunter { get; set; }  // 是否猎人赛，0 1
            public string mttType { get; set; }  // 6:6人桌，9:9人桌
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_reward_info  // MTT赛事奖励列表
    {
        public const string API = @"/api/mtt/reward-info";

        public sealed class RequestData : RequestDataBase
        {
            public int game_path { get; set; }  // 71
            public int game_id { get; set; }  // 比赛id，如228630
            public string system_lan { get; set; }  // 语言，zh en
            public int page { get; set; }  // 当前页数，1开始
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public int nextRewardNum { get; set; }  // 下一个奖励圈名次
            public int bonusType { get; set; }  // 未使用
            public int initialPool { get; set; }  // 未使用
            public int rewardNumber { get; set; }  // 奖励圈，如1
            public int gameStatus { get; set; }  // 未使用
            public string nextReward { get; set; }  // 下一个奖励圈奖励，如：100王者币
            public List<ContentsElement> contents { get; set; }  // 包含元素:ContentsElement
            public int participants { get; set; }  // 参赛人数
            public int totalBonus { get; set; }  // 总奖池，如100
        }

        public sealed class ContentsElement
        {
            public string bonus { get; set; }  // 奖励，如：100王者币
            public int percent { get; set; }  // 奖励比例，未显示
            public int rank { get; set; }  // 名次
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_desk_info  // MTT牌桌列表
    {
        public const string API = @"/api/mtt/desk-info";

        public sealed class RequestData : RequestDataBase
        {
            public int game_path { get; set; }  // 71
            public int game_id { get; set; }  // 比赛id，如994474
            public int page { get; set; }  // 页数，1开始
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 0
        }

        public sealed class Data
        {
            public List<DeskArrElement> deskArr { get; set; }  // 包含元素:DeskArrElement
            public int totalDeskNum { get; set; }  // 总牌桌数
        }

        public sealed class DeskArrElement
        {
            public int deskNum { get; set; }  // 桌号
            public int playerCount { get; set; }  // 人数
            public int score { get; set; }  // USDT
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_mtt_remove  // 解散MTT
    {
        public const string API = @"/api/mtt/remove";

        public sealed class RequestData : RequestDataBase
        {
            public int game_path { get; set; }  // 71
            public int game_id { get; set; }  // 比赛id，如272445
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public int status { get; set; }  // 0 解散成功 其他：解散失败，请稍后再试
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_my_popularize_info  // 获取我的推广员信息
    {
        public const string API = @"/api/popularize/my_popularize_info";

        public sealed class RequestData : RequestDataBase
        {
            public int user_id { get; set; }  // 玩家id
        }

        public sealed class ResponseData
        {
            public int status { get; set; }         //          0成功 其他失败
            public string msg { get; set; }   //String                提示信息
            public int user_id { get; set; }       //          Int 玩家id
            public string nike_name { get; set; }  //String     //           昵称
            public string head { get; set; }            //      String 头像url
            public int popularize_id { get; set; }  // Int                   推广员id
            public string invite_code { get; set; }   //          String 邀请码
            public string qr_code { get; set; }  // String     //           二维码
            public int level_two_password_status { get; set; }   //           Int                0未设置二级密码 1已设置
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_room_jackpot_view  // 牌局内JackPot标签信息
    {
        public const string API = @"/api/room/jackpot/view";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 91
            public int room_id { get; set; }  // 580645
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int jackPot_fund { get; set; }  // 34849
            public float fourOfAKindRatio { get; set; }  // 0
            public string blind { get; set; }  // 5/10
            public float straightFlushRatio { get; set; }  // 2
            public float royalFlushRatio { get; set; }  // 4  
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB_room_jackpot_record  // 牌局内JackPot奖励
    {
        public const string API = @"/api/room/jackpot/record";

        public sealed class RequestData : RequestDataBase
        {
            public int room_path { get; set; }  // 91
            public int room_id { get; set; }  // 580645
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public List<RecordArrElement> recordArr { get; set; }  // 包含元素:RecordArrElement
            public TopJackPot topJackPot { get; set; }  // TopJackPot
        }

        public sealed class RecordArrElement
        {
            public int reward { get; set; }  // 172
            public int cardType { get; set; }  // 3
            public string nickName { get; set; }  // 物易国
            public string blind { get; set; }  // 1/2
            public int userId { get; set; }  // 331191
            public int time { get; set; }  // 1553854240
        }

        public sealed class TopJackPot
        {
            public int reward { get; set; }  // 8467
            public int cardType { get; set; }  // 1
            public string nickName { get; set; }  // 中国中铁
            public string blind { get; set; }  // 10/20
            public int userId { get; set; }  // 303217
            public int time { get; set; }  // 1552649313
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 查看俱乐部详情 - 【俱乐部Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1205 = 俱乐部不存在
    ///  </summary>
    public partial class WEB2_club_view
    {
        public const string API = @"/api/club/view";

        public sealed class RequestData : RequestDataBase
        {
            public string clubId { get; set; }  // 俱乐部id<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int clubId { get; set; }  // 用于api调用使用
            public int areaId { get; set; }  // 俱乐部所属地区，创建时选择，用于展示
            public int clubMembers { get; set; }  // 目前成员人数
            public int randomId { get; set; }  // 随机id，用于展示
            public string clubName { get; set; }  // 俱乐部名称
            public int membersLimit { get; set; }  // 俱乐部最大人数限制
            public string clubHeader { get; set; }  // 俱乐部头像地址
            public string description { get; set; }  // 俱乐部简介，创建时
            public int userType { get; set; }  // 在此俱乐部的身份<br/>1 = 创建者<br/>2 = 成员<br/>3 = 非成员
            public List<string> userHeads { get; set; }  // 成员头像地址
            public List<RecordArrElement> contactList { get; set; }
            public int transType { get; set; }//0-俱乐部充值(默认),1-平台充值
            public int clubStatus { get; set; }//俱乐部的状态 0-正常,1-是关闭
            public int roomCount { get; set; }//牌局数量
        }

        public sealed class RecordArrElement
        {
            public int type { get; set; }//联系方式1 = 微信   2 = telegram    3 = skype       4 = sugram
            public string content { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_room_enter
    {
        public const string API = @"/api/room/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1
            public int rmid { get; set; }  // 牌局ID,必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int qzhu { get; set; }  // 前注,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启
            public int jpfnd { get; set; }  // jp彩池资金
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int jpid { get; set; }  // jp彩池ID
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 进入牌局 - 【普通局-组局&进房API】
    /// 
    /// 玩家进入牌局前，通过此接口获取牌局的基础数据及所在游戏服务的访问地址。
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2001 : 牌局不存在或已解散 
    ///   * 2004 : 账号已被冻结
    ///   * 2005 : 玩家未加入俱乐部
    ///   * 2006 : 玩家所属俱乐部已被关闭
    ///   * 2007 : 玩家所属俱乐部未绑定联盟
    ///   * 2008 : 玩家所属俱乐部转移中或已被踢出
    ///  </summary>
    public partial class WEB2_room_dz_enter
    {
        public const string API = @"/api/room/dz/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int rmid { get; set; }  // 牌局ID,必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int qzhu { get; set; }  // 前注,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启
            public int jpfnd { get; set; }  // jp彩池资金
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int jpid { get; set; }  // jp彩池ID
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
           
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_room_dp_enter
    {
        public const string API = @"/api/room/shortCard/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int rmid { get; set; }  // 牌局ID,必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int qzhu { get; set; }  // 前注,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启
            public int jpfnd { get; set; }  // jp彩池资金
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int jpid { get; set; }  // jp彩池ID
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 创建牌局 - 【奥马哈局-组局&进房API】
    /// 
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2002  : 权限不足!
    ///  </summary>
    public partial class WEB2_room_omaha_create
    {
        public const string API = @"/api/room/omaha/create";

        public sealed class RequestData : RequestDataBase
        {
            public int clubRoomType { get; set; }//0默认1积分房
            public int ahdlon { get; set; }  // 提前离桌开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public int slmz { get; set; }  // 小盲注,必填,必须>=1
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int mode { get; set; }  // 血战模式:0 = 普通玩法 ,1 = 血战模式<br/>不需此条件，字段不出现或传null值
            public int vpon { get; set; }  // 当桌入池开关:0 = 关闭 ,1 = 开启</br>不需此条件，字段不出现或传null值 <br/>
            public int ipon { get; set; }  // IP控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int gpson { get; set; }  // GPS控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int delay { get; set; } //延迟看牌
            public int qzhu { get; set; }  // 前注,必填,>=0
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 
            public int opts { get; set; }  // 操作思考时间,必填,>=0,单位：秒
            public int incp { get; set; }  // 带入USDT,必填,>=200
            public int gmnt { get; set; }  // 牌局时长,必填,>=30,单位：分钟
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启<br/>+不需此条件，字段不出现或传null值 <br/>
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public string name { get; set; }  // 牌局名称,必填,最大长度：50字符
            public int rpu { get; set; }  // 牌桌人数,必填,取值范围：[2,9]
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int autopu { get; set; } //满人自动开局 2 - 9
            public int autoPlanOn { get; set; } //是否开启开房计划 0 1
            public int emptySeatConfig { get; set; } //自动开桌的空位配置 2 - 9
            public string startTime { get; set; } //每天计划开始时间，格式00:00
            public string endTime { get; set; } //每天计划结束时间，格式00:00
            public int autoDismiss { get; set; } //空桌自动解散 0 1
            public int roomsLimit { get; set; } //满多少桌后停止开桌
            public int requestToBringIn { get; set; } //请求带入
            public int fee { get; set; }//房间服务费
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int qzhu { get; set; }  // 前注,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启
            public int jpfnd { get; set; }  // jp彩池资金
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int jpid { get; set; }  // jp彩池ID
            public int mode { get; set; }  // 血战模式:0 = 普通玩法 ,1 = 血战模式
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 联盟详情 - 【联盟Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1303 = 联盟权限不足
    ///  </summary>
    public partial class WEB2_tribe_detail
    {
        public const string API = @"/api/tribe/detail";

        public sealed class RequestData : RequestDataBase
        {
            public int tribeId { get; set; }  // 联盟id<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int profit { get; set; }  // 联盟占成比例，用户客户端展示使用
            public int randomId { get; set; }  // 联盟随机id，用户客户端展示使用
            public int tribeId { get; set; }  // 联盟隐性id，用于api调用使用
            public string creatorName { get; set; }  // 联盟主名称，用户客户端展示使用
            public int clubCount { get; set; }  // 联盟目前总俱乐部数，用户客户端展示使用
            public string tribeName { get; set; }  // 联盟名称，用户客户端展示使用
            public int clubLimit { get; set; }  // 联盟俱乐部上限，用户客户端展示使用
            public int mainClubId { get; set; }  // 联盟主机俱乐部id，用于api调用
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 俱乐部所在的联盟 - 【联盟Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 权限不足
    /// 1205 = 俱乐部主体不存在
    /// 1302 = 没有已加入的联盟
    ///  </summary>
    public partial class WEB2_tribe_mine
    {
        public const string API = @"/api/tribe/mine";

        public sealed class RequestData : RequestDataBase
        {
            public int clubId { get; set; }  // 俱乐部id<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int canCreateTribe { get; set; }
            public int tribeId { get; set; }  // 联盟隐性id，用于api调用使用
            public int randomId { get; set; }  // 联盟隐性id，用于客户端展示使用
            public string tribeName { get; set; }  // 联盟名称
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 加入俱乐部 - 【俱乐部Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1202 = 俱乐部申请待处理
    /// 1203 = 已加入俱乐部
    /// 1205 = 目标俱乐部不存在
    /// 1206 = 俱乐部人数上限
    ///  </summary>
    public partial class WEB2_club_join
    {
        public const string API = @"/api/club/join";

        public sealed class RequestData : RequestDataBase
        {
            public string clubId { get; set; }  // 俱乐部id<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 搜索俱乐部 - 【俱乐部Api】
    /// 
    /// 原先的地区查找和关键字查找合并为一个接口
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1207 = 俱乐部查找无结果(此情况不解析data字段数据)
    ///  </summary>
    public partial class WEB2_club_search
    {
        public const string API = @"/api/club/search";

        public sealed class RequestData : RequestDataBase
        {
            public string key { get; set; }  // 关键字查找则为关键字，地区查找则是地区id<br/>必填
            public int type { get; set; }  // 搜索方式<br/>必填<br/>1 = id搜索<br/>2 = 地区搜索
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int clubId { get; set; }  // 用于请求接口使用
            public int currPe { get; set; }  // 当前俱乐部人数
            public string areaId { get; set; }  // 俱乐部归属地区id
            public string clubName { get; set; }  // 俱乐部名称唯一，并且不为空
            public int randomId { get; set; }  // 展示给用户看，不作为接口调用参数使用
            public long createTime { get; set; }  // 毫秒时间戳 东八区
            public int isOfficial { get; set; }  // 是否官方俱乐部<br/>0 = 不是<br/>1 = 是
            public int total { get; set; }  // 俱乐部总上限人数
            public int userType { get; set; }  // 当前请求接口玩家对应俱乐部的身份<br/>1 = 创建者<br/>2 = 普通成员<br/>3 = 非成员
            public string logo { get; set; }  // 俱乐部头像地址，不为空，默认值：-1
            public int roomCount { get; set; }  // 当前俱乐部进行中房间数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 联盟成员俱乐部列表 - 【联盟Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1303 = 联盟权限不足
    ///  </summary>
    public partial class WEB2_tribe_member
    {
        public const string API = @"/api/tribe/member";

        public sealed class RequestData : RequestDataBase
        {
            public int tribeId { get; set; }  // 联盟id<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int id { get; set; }  // 俱乐部隐性id，用于api调用使用
            public string clubHeader { get; set; }  // 俱乐部头像地址，用于客户端展示使用
            public string clubAreaId { get; set; }  // 俱乐部地区id，用于客户端展示使用
            public int randomId { get; set; }  // 俱乐部随机id，用于客户端展示使用
            public string clubName { get; set; }  // 俱乐部名称，用于展示使用
            public string clubPleNum { get; set; }  // 俱乐部当前人数/上限，用于客户端展示使用
            public int status { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 成为推广员 - 【分享赚USDT模块API】
    /// 
    /// 状态码
    ///  0 = 成功
    /// 999 = 失败，异常情况等
    /// 1601 = 请先加入俱乐部，才可以成为推广员  2101
    ///  </summary>
    public partial class WEB2_promotion_to_be_promoter
    {
        public const string API = @"/api/promotion/to_be_promoter";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string inventCode { get; set; }  // 推广员邀请码
            public string url { get; set; }  // 
            public int userId { get; set; }  // 推广员id
            public int clubId { get; set; }  // 俱乐部id
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 牌局列表 - 【游戏大厅-牌局列表API】
    /// 
    /// 根据查询条件，翻页查询各个类型的当前有效的牌局。
    /// 排序规则：按牌局创建时间倒序，每页20条
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///  </summary>
    public partial class WEB2_room_list
    {
        public const string API = @"/api/room/list";

        public sealed class RequestData : RequestDataBase
        {
            public int act { get; set; }  // 翻页动作:1 = 上拉，滚下一页<br/>0 = 下拉，刷新，即：重新拉第一页数据，默认<br/>
            public int rtype { get; set; }  // 牌局类型,不需此条件，传值：-1 <br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int mz { get; set; }  // 盲注级别,不需此条件，传值：-1 <br/>5 = 5/10以下<br/>10 = 10/20 ~ 50/100<br/>100 = 100/200以上
            public int seat { get; set; }  // 空位,不需此条件，传值：-1 <br/>0 = 无 ,1 = 有
            public int insurance { get; set; }  // 保险,不需此条件，传值：-1 <br/>0 = 无 ,1 = 有
            public int lrt { get; set; }  // 上一页最后记录的时间戳:方向=1时有效，当前最后一条的牌桌的创建时间，大于0值有效，默认是：0
            public int rpu { get; set; }  // 牌桌人数,不需此条件，传值：-1 <br/>1 = 2~4 <br/>2 = 5~7 <br/>3 = 8 ~ 9
            public int qzhu { get; set; }  // 前注,不需此条件，传值：-1 <br/>0 = 无 ,1 = 有
            public List<int> mzlist {get;set;} //盲注
            public int listSize { get; set; }//默认0
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int state { get; set; }  // 游戏状态:5 = 游戏中 , 4 = 进行中 , 3 = 等待中
            public int slmz { get; set; }  // 盲注级别,最小盲注值，字段不存在或值>0
            public int qianzhu { get; set; }  // 前注
            public int zhuatou { get; set; }  // 是否抓头
            public int htron { get; set; }  // 游戏小标签-MTT猎人赛模式:0 = 关闭 , 1 = 开启
            public int stdn { get; set; }  // 坐下人数,默认值：0
            public int rmsec { get; set; }  // 牌桌剩余时间,单位：秒
            public int omamod { get; set; }  // 游戏小标签-omaha模式:0 = 普通模式 , 1 = 血战模式
            public int dlmod { get; set; }  // 游戏小标签-大菠萝发牌模式:0 = 同步发牌 , 1 = 顺序发牌
            public int rmid { get; set; }  // 牌局ID
            public string logo { get; set; }  // 牌局LOGO的http访问URL，空值无效
            public int jkon { get; set; }  // 游戏小标签-大菠萝癞子模式:0 = 关闭 , 1 = 开启
            public int isron { get; set; }  // 游戏小标签-保险模式:0 = 关闭 , 1 = 开启
            public int jpon { get; set; }  // 游戏小标签-jackpot模式:0 = 关闭 , 1 = 开启
            public int rtype { get; set; }  // 牌局类型:61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int lcrdt { get; set; }  // 创建时间:方向=1时有效，当前最后一条的牌桌的创建时间，大于0值有效
            public int bpmod { get; set; }  // 游戏小标签-大菠萝模式:1 = 普通模式 , 2 = 血战模式 , 3 = 血进血出
            public int rpu { get; set; }  // 牌桌人数,默认值：0
            public string name { get; set; }  // 
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 进入牌局 - 【奥马哈局-组局&进房API】
    /// 
    /// 玩家进入牌局前，通过此接口获取牌局的基础数据及所在游戏服务的访问地址。
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2001 : 牌局不存在或已解散 
    ///   * 2004 : 账号已被冻结
    ///   * 2005 : 玩家未加入俱乐部
    ///   * 2006 : 玩家所属俱乐部已被关闭
    ///   * 2007 : 玩家所属俱乐部未绑定联盟
    ///   * 2008 : 玩家所属俱乐部转移中或已被踢出
    ///  </summary>
    public partial class WEB2_room_omaha_enter
    {
        public const string API = @"/api/room/omaha/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int rmid { get; set; }  // 牌局ID,必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int qzhu { get; set; }  // 前注,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启
            public int jpfnd { get; set; }  // jp彩池资金
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int jpid { get; set; }  // jp彩池ID
            public int mode { get; set; }  // 血战模式:0 = 普通玩法 ,1 = 血战模式
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 创建联盟 - 【联盟Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 权限不足
    /// 1205 = 俱乐部主体不存在
    /// 1300 = 已有联盟
    /// 1301 = 联盟重名
    /// 1305 = 联盟创建中
    ///  </summary>
    public partial class WEB2_tribe_create
    {
        public const string API = @"/api/tribe/create";

        public sealed class RequestData : RequestDataBase
        {
            public int clubId { get; set; }  // 此俱乐部将成为联盟主机，拥有联盟最高权限<br/>必填
            public string phone { get; set; }  // 联系人手机号码<br/>非必填
            public string wechat { get; set; }  // 联系人手机号码<br/>非必填
            public string telegram { get; set; }  // telegram联系<br/>非必填
            public string message { get; set; }  // 留言<br/>非必填
            public string tribeName { get; set; }  // 联盟名称，用于展示<br/>必填
            public string email { get; set; }  // 电子邮箱地址<br/>非必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 判断手机号是否已经注册 - 【用户模块】
    /// 
    /// 判断手机号是否已经注册:ture-已经注册,false-未注册
    ///  </summary>
    public partial class WEB2_user_exist_phone
    {
        public const string API = @"/api/user/exist_phone";

        public sealed class RequestData : RequestDataBase
        {
            public string phone { get; set; }  // 手机号码必须加上地区号并用下划线隔开如:86-12345676789
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public bool data { get; set; }  // 业务数据，成功情况才有数据
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 俱乐部基金详情 - 【俱乐部基金Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 无操作俱乐部权限
    /// 1205 = 俱乐部不存在
    ///  </summary>
    public partial class WEB2_club_fund_detail
    {
        public const string API = @"/api/club/fund/detail";

        public sealed class RequestData : RequestDataBase
        {
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public long fund { get; set; }  // 俱乐部剩余基金总余额
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public long fund { get; set; }  // 俱乐部剩余基金总余额
            public long userChip { get; set; }
            public long betChip { get; set; }  // 俱乐部剩余对赌基金总余额
            public int isOpen { get; set; }
            public int isBurst { get; set; }

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 获取用户公开信息信息 - 【用户模块】
    /// 
    /// 获取用户公开信息
    ///  </summary>
    public partial class WEB2_user_public_info
    {
        public const string API = @"/api/user/public_info";

        public sealed class RequestData : RequestDataBase
        {
            public string randomId { get; set; }  // 用户randomId,请优先根据此字段进行查询,传空串也视为不传此字段
            public int userId { get; set; }  // 用户userId,以后逐渐废弃,传小于等于0则视为传该字段
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string personSign { get; set; }  // 个人签名
            public int lose { get; set; }  // 输了多少手
            public int poolCnt { get; set; }  // 总入池率
            public string randomNum { get; set; }  // 用户id
            public int win { get; set; }  // 赢了多少手
            public string head { get; set; }  // 头像
            public int vip { get; set; }  // 是否为VIP:0-否,1-是
            public long vipEndDate { get; set; }  // VIP结束时间,没有开通VIP返回null
            public int sex { get; set; }  // 用户性别:0-男,1-女
            public string nickName { get; set; }  // 用户昵称
            public int remarkColor { get; set; }  // 用户标志颜色，0无 1颜色1 2颜色2 3颜色3
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 用户注册接口 - 【用户模块】
    /// 
    /// 用户注册接口
    ///  </summary>
    public partial class WEB2_user_register
    {
        public const string API = @"/api/user/register";

        public sealed class RequestData : RequestDataBase
        {
            public string phone { get; set; }  // 手机号码必须加上地区号并用中分割线隔开如:86-12345676789
            public string password { get; set; }  // 密码
            public int channelId { get; set; }  // 渠道id
            public string area { get; set; }  // 国家代号  todo暂不传
            public string imei { get; set; }  // 机器码
            public string code { get; set; }  // 验证码
            public int isSimulator { get; set; }  // 是否为模拟器
            public string invitationCode { get; set; }//邀请码
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int userId { get; set; }  // 用户id
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 我的VIP特权 - 【用户VIP模块】
    /// 
    /// 我的VIP特权
    ///  </summary>
    public partial class WEB2_user_vip_my
    {
        public const string API = @"/api/user_vip/my";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int collectSpectrum { get; set; }  // 已用保存牌谱数
            public int vipCardType { get; set; }  // 卡类型:
            public int vipLevel { get; set; }  // VIP类型 0-VIP 1-黄金 2-铂金 3-钻石
            public int remarkNum { get; set; }  // 已用备注数
            public long vipEndDate { get; set; }  // vip到期时间,精确到毫秒
            public long vipStartDate { get; set; }  // vip开始时间戳,精确到毫秒
            public int remainFee { get; set; }  // VIP剩余价值
            public int remainDays { get; set; }  // VIP剩余天数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 创建牌局 - 【普通局-组局&进房API】
    /// 
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2002  : 权限不足!
    ///  </summary>
    public partial class WEB2_room_dz_create
    {
        public const string API = @"/api/room/dz/create";

        public sealed class RequestData : RequestDataBase
        {
            public int clubRoomType { get; set; }//0默认1积分房
            public int ahdlon { get; set; }  // 提前离桌开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public int slmz { get; set; }  // 小盲注,必填,必须>=1
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int vpon { get; set; }  // 当桌入池开关:0 = 关闭 ,1 = 开启</br>不需此条件，字段不出现或传null值 <br/>
            public int ipon { get; set; }  // IP控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int gpson { get; set; }  // GPS控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int delay { get; set; } //延迟看牌
            public int qzhu { get; set; }  // 前注,必填,>=0
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 
            public int opts { get; set; }  // 操作思考时间,必填,>=0,单位：秒
            public int incp { get; set; }  // 带入USDT,必填,>=200
            public int gmnt { get; set; }  // 牌局时长,必填,>=30,单位：分钟
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启<br/>+不需此条件，字段不出现或传null值 <br/>
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public string name { get; set; }  // 牌局名称,必填,最大长度：50字符
            public int rpu { get; set; }  // 牌桌人数,必填,取值范围：[2,9]
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int autopu { get; set; } //满人自动开局 2 - 9
            public int autoPlanOn { get; set; } //是否开启开房计划 0 1
            public int emptySeatConfig { get; set; } //自动开桌的空位配置 2 - 9
            public string startTime { get; set; } //每天计划开始时间，格式00:00
            public string endTime { get; set; } //每天计划结束时间，格式00:00
            public int autoDismiss { get; set; } //空桌自动解散 0 1
            public int roomsLimit { get; set; } //满多少桌后停止开桌
            public int requestToBringIn { get; set; } //请求带入
            public int fee { get; set; }//房间服务费
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int qzhu { get; set; }  // 前注,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启
            public int jpfnd { get; set; }  // jp彩池资金
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int jpid { get; set; }  // jp彩池ID
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    //创建短牌
    public partial class WEB2_room_dp_create
    {
        public const string API = @"/api/room/shortCard/create";

        public sealed class RequestData : RequestDataBase
        {
            public int clubRoomType { get; set; }//0默认1积分房
            public int ahdlon { get; set; }  // 提前离桌开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public int slmz { get; set; }  // 小盲注,必填,必须>=1
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int vpon { get; set; }  // 当桌入池开关:0 = 关闭 ,1 = 开启</br>不需此条件，字段不出现或传null值 <br/>
            public int ipon { get; set; }  // IP控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int gpson { get; set; }  // GPS控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int delay { get; set; } //延迟看牌
            public int qzhu { get; set; }  // 前注,必填,>=0
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 
            public int opts { get; set; }  // 操作思考时间,必填,>=0,单位：秒
            public int incp { get; set; }  // 带入USDT,必填,>=200
            public int gmnt { get; set; }  // 牌局时长,必填,>=30,单位：分钟
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启<br/>+不需此条件，字段不出现或传null值 <br/>
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值 <br/>
            public string name { get; set; }  // 牌局名称,必填,最大长度：50字符
            public int rpu { get; set; }  // 牌桌人数,必填,取值范围：[2,9]
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int autopu { get; set; } //满人自动开局 2 - 9
            public int autoPlanOn { get; set; } //是否开启开房计划 0 1
            public int emptySeatConfig { get; set; } //自动开桌的空位配置 2 - 9
            public string startTime { get; set; } //每天计划开始时间，格式00:00
            public string endTime { get; set; } //每天计划结束时间，格式00:00
            public int autoDismiss { get; set; } //空桌自动解散 0 1
            public int roomsLimit { get; set; } //满多少桌后停止开桌
            public int requestToBringIn { get; set; } //请求带入
            public int fee { get; set; }//房间服务费
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int sdlon { get; set; }  // straddle开关:0 = 关闭 ,1 = 开启
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int qzhu { get; set; }  // 前注,必填
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int jpon { get; set; }  // jackpot开关:0 = 关闭 ,1 = 开启
            public int jpfnd { get; set; }  // jp彩池资金
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int isron { get; set; }  // 保险开关:0 = 关闭 ,1 = 开启
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int gmxt { get; set; }  // 最短上桌时间,单位：分钟
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int jpid { get; set; }  // jp彩池ID
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 俱乐部成员列表 - 【俱乐部Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 没有俱乐部操作权限
    /// 1205 = 目标俱乐部不存在
    ///  </summary>
    public partial class WEB2_club_mlist
    {
        public const string API = @"/api/club/mlist";

        public sealed class RequestData : RequestDataBase
        {
            public int clubId { get; set; }  // 俱乐部id<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string userHead { get; set; }  // 俱乐部成员头像地址
            public int sex { get; set; }  // 俱乐部成员性别
            public string nickName { get; set; }  // 俱乐部成员昵称
            public string uid { get; set; }  // 俱乐部用户随机id，唯一值
            public int vipLevel { get; set; }  // 俱乐部成员vip等级
            public int userType { get; set; }  // 成员在此俱乐部的身份<br/>1 = 创建者<br/>2 = 成员<br/>3 = 非成员
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 通过手机号找回密码 - 【用户模块】
    /// 
    /// 通过手机号找回密码
    ///  </summary>
    public partial class WEB2_user_mod_pwd
    {
        public const string API = @"/api/user/mod/pwd";

        public sealed class RequestData : RequestDataBase
        {
            public string phone { get; set; }  // 手机号码必须加上地区号并用下划线隔开如:86-12345676789
            public string code { get; set; }  // 验证码
            public string password { get; set; }  // 密码
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int userId { get; set; }  // 用户id
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 手数列表 - 【牌谱】
    /// 
    /// 手数列表
    ///  </summary>
    public partial class WEB2_brand_spe_hand_list
    {
        public const string API = @"/api/brand_spe/hand_list";

        public sealed class RequestData : RequestDataBase
        {
            public string roomId { get; set; }
            public int pageNo { get; set; }
            public int sortType { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public List<HandsElement> hands { get; set; }  // 手牌列表
            public long count { get; set; }  // 总数
            public int page { get; set; }  // 页码
        }

        public sealed class HandsElement
        {
            public string handName { get; set; }  // 手牌名称
            public string id { get; set; }  // 手牌id
            public int bigBlind { get; set; }  // 大盲注
            public int smallBlind { get; set; }  // 小盲注
            public long timeStamp { get; set; }  // 时间
            public int win { get; set; }  // 输赢筹码
            public int roomType { get; set; }  // 牌局类型:
            public bool collect { get; set; }  // 是否收藏:false-没有,true-收藏
            public int index { get; set; }  // 编号
            public int qianzhu { get; set; }  // 前注
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 分享赚USDT功能是否开启 - 【分享赚USDT模块API】
    /// 
    /// 状态码
    /// 0 = 成功，如果需要数据可以从data字段解析。data为true，则功能开启，false则功能隐藏。
    /// 999 = 失败，异常情况等
    ///  </summary>
    public partial class WEB2_query_function_open
    {
        public const string API = @"/api/function/query_function_open";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; } //true开启了
        }

        public sealed class DataElement
        {
            public string function { get; set; }//            功能名称。promotion：分享赚USDT，wallet:钱包， mall：商城， backup_download_url: 备份下载链接
            public int functionCode { get; set; }//功能代号。1：分享赚USDT，2:钱包， 3：商城， 4: 备份下载链接   5:游戏公平链接  6常见问题页面链接  7转USDT开关
            public bool open { get; set; }
            public string value { get; set; }//功能设置值，功能需要取值，从此字段获取，如备份下载链接。
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    ///    用户是否推广员 状态码
    ///0 = 成功，如果需要数据可以从data字段解析。data为true，则功能开启，false则功能隐藏。
    ///999 = 失败，异常情况等
    ///  </summary>
    public partial class WEB2_promotion_is_promoter
    {
        public const string API = @"/api/promotion/is_promoter";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public bool data { get; set; } //true开启了
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }



    /// <summary>
    /// 查询我的推广员列表，数量和今日新增 - 【分享赚USDT模块API】
    /// 
    /// 状态码
    ///  0 = 成功
    /// 999 = 失败，异常情况等 2109 = 需要成为推广员才能查看
    ///  </summary>
    public partial class WEB2_promotion_query_my_promoters
    {
        public const string API = @"/api/promotion/query_my_promoters";

        public sealed class RequestData : RequestDataBase
        {
            public string search { get; set; }  // 推广员ID或昵称搜索<br/>非必填
            public int size { get; set; }//分页每页数量            非必填
            public int start { get; set; }//分页，第几页，以0开始            非必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int myPromotersNumber { get; set; }  // 我的推广员人数
            public List<PromotersListElement> promotersList { get; set; }
            public int todayNumber { get; set; }  // 今日新增 暂无用
            public int myGroupNumber { get; set; }//我的下级人数（直属玩家+直属推广员）暂无用

            public int todayPromotorNumber { get; set; }//今日直属推广员新增
            public int todayPlayerNumber { get; set; }//  今日直属玩家新增
        }

        public sealed class PromotersListElement
        {
            public int rank { get; set; }//排名
            public int weekAchievement { get; set; }  // 本周业绩
            public int promoterId { get; set; }  // 推广员id--暂无用
            public string userId { get; set; }  // 用户id  
            public int groupMembers { get; set; }  // 团队人数
            public string nickname { get; set; }  // 昵称

            public int promotionType { set; get; }//身份类型。0-玩家，1-推广员
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 获取用户地址 - 【背包模块】
    /// 
    /// 获取用户地址
    ///  </summary>
    public partial class WEB2_backpack_consignee_info
    {
        public const string API = @"/api/backpack/consignee/info";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string address { get; set; }  // 收件人详细地址
            public int id { get; set; }  // 用户地址id
            public string mobile { get; set; }  // 收件人手机号码
            public string wechat { get; set; }  // 收件人微信
            public int userId { get; set; }  // 用户id
            public string userName { get; set; }  // 收件人名称
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 发送短信验证码 - 【发送短信接口API】
    /// 
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 1000  ：短信发送失败！
    ///  </summary>
    public partial class WEB2_sms_sendCode
    {
        public const string API = @"/api/sms/sendCode";

        public sealed class RequestData : RequestDataBase
        {
            public string countryCode { get; set; }  // 手机号码的国家码
            public int sendCodeType { get; set; }  // 验证码类型1 = 注册    2 = 找回密码   3 = 设置二级密码 4 = 找回二级密码     5 = 提取
            public string phoneNum { get; set; }  // 手机号码
            public int lang { get; set; }//语言标识:0-简体中文(默认),1-英语,2-繁体中文
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// breakUser - 【用户VIP模块】
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_user_vip_break
    {
        public const string API = @"/api/user_vip/break";

        public sealed class RequestData : RequestDataBase
        {
            public string breakUserRId { get; set; }  // 被破隐人randomId
            public int breakUserId { get; set; }  // 被破隐人id,暂时用userId,如果前端能获取用户的randomNum就改为randomNum
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 申请加入联盟 - 【联盟Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 俱乐部权限不足
    /// 1300 = 已有联盟
    /// 1305 = 联盟创建待处理
    ///  </summary>
    public partial class WEB2_tribe_aJoin
    {
        public const string API = @"/api/tribe/aJoin";

        public sealed class RequestData : RequestDataBase
        {
            public int tribeId { get; set; }  // 联盟id<br/>必填，不能为空
            public int clubId { get; set; }  // 俱乐部id<br/>必填，不能为空
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 设置牌谱收藏 - 【牌谱】
    /// 
    /// 设置牌谱收藏
    ///  </summary>
    public partial class WEB2_brand_spe_collect
    {
        public const string API = @"/api/brand_spe/collect";

        public sealed class RequestData : RequestDataBase
        {
            public bool collect { get; set; }  // 是否需要收藏，false取消收藏 true收藏
            public string handId { get; set; }  // 手牌的id
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int spectrumCnt { get; set; }  // 收藏牌谱数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 开通/续费/过期后降级/升级VIP - 【用户VIP模块】
    /// 
    /// 我的VIP特权
    ///  </summary>
    public partial class WEB2_user_vip_setting
    {
        public const string API = @"/api/user_vip/setting";

        public sealed class RequestData : RequestDataBase
        {
            public int cardNum { get; set; }  // 购买个数
            public int vipLevel { get; set; }  // VIP类型 0-VIP 1-黄金 2-铂金 3-钻石
            public int cardType { get; set; }  // 购买类型:1-月卡 2-年卡
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int remainFee { get; set; }  // VIP剩余价值
            public int vipLevel { get; set; }  // VIP类型 0-VIP 1-黄金 2-铂金 3-钻石
            public long vipEndDate { get; set; }  // vip到期时间,精确到毫秒
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 俱乐部基金下发 - 【俱乐部基金Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 无操作俱乐部权限
    /// 1205 = 俱乐部不存在
    /// 1208 = 基金被冻结
    /// 1209 = 基金不足
    ///  </summary>
    public partial class WEB2_club_fund_give
    {
        public const string API = @"/api/club/fund/give";

        public sealed class RequestData : RequestDataBase
        {
            public string memberId { get; set; }  // 操作对象的随机id<br/>必填
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public int amount { get; set; }  // 基金发放数额<br/>必填
            public int type { get; set; } //1-可提USDT(默认) 2-不可提USDT
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 推荐俱乐部列表 - 【俱乐部Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    ///  </summary>
    public partial class WEB2_club_hotClubs
    {
        public const string API = @"/api/club/hotClubs";

        public sealed class RequestData : RequestDataBase
        {
            public int reqType { get; set; }  // 查询类型<br/>必填<br/>1 = 简单查找(5个)<br/>2 = 详细列表查找(30个)
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public DataElement data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int joinStatus { get; set; }  // 是否显示加入按钮<br/>0 = 不显示<br/>1 = 显示加入按钮
            public List<ListElement> list { get; set; }
        }

        public sealed class ListElement
        {
            public int clubId { get; set; }  // 俱乐部隐性id
            public int randomId { get; set; }  // 俱乐部随机id
            public string clubName { get; set; }  // 俱乐部名称
            public string clubHeader { get; set; }  // 俱乐部头像文件地址

            public int memberLimit { get; set; }
            public int memberNum { get; set; }  // 俱乐部当前人数
            public string clubAreaId { get; set; }  // 俱乐部地区id
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 俱乐部基金历史记录 - 【俱乐部基金Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 无操作俱乐部权限
    /// 1205 = 俱乐部不存在
    ///  </summary>
    public partial class WEB2_club_fund_history
    {
        public const string API = @"/api/club/fund/history";

        public sealed class RequestData : RequestDataBase
        {
            public int direction { get; set; }  // 加载方向<br/>必填<br/>0 = lt<br/>1 = gt
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public long time { get; set; }  // 上次刷新时间<br/>第一次传0，其他时候传上次刷新的时间戳<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int direction { get; set; }  // 此次查询的加载方向
            public List<ListElement> list { get; set; }  // 历史记录列表
        }

        public sealed class ListElement
        {
            public int amount { get; set; }  // 操作导致的基金变动数额
            public string userId { get; set; }  // 被操作玩家的用户随机id
            public string userName { get; set; }  // 被操作玩家昵称
            public long time { get; set; }  // 该操作时间，毫秒
            public string operatorName { get; set; }  // 操作人昵称，一般是管理员
            public int type { get; set; }  // 记录类型<br/>1 = 发放基金<br/>2 = 充值基金<br/>3 = 牌局收益
            public string operatorId { get; set; }  // 操作管理员的随机id

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_club_fund_betHistory
    {
        public const string API = @"/api/club/fund/betHistory";

        public sealed class RequestData : RequestDataBase
        {
            public int direction { get; set; }  // 加载方向<br/>必填<br/>0 = lt<br/>1 = gt
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public long time { get; set; }  // 上次刷新时间<br/>第一次传0，其他时候传上次刷新的时间戳<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int direction { get; set; }  // 此次查询的加载方向
            public List<ListElement> list { get; set; }  // 历史记录列表
        }

        public sealed class ListElement
        {
            public int amount { get; set; }  // 操作导致的基金变动数额
            public string userId { get; set; }  // 被操作玩家的用户随机id
            public string userName { get; set; }  // 被操作玩家昵称
            public long time { get; set; }  // 该操作时间，毫秒
            public string operatorName { get; set; }  // 操作人昵称，一般是管理员
            public int type { get; set; }  // type  0：转入   1：转出   2：俱乐部对赌  3:联盟对赌
            public string operatorId { get; set; }  // 操作管理员的随机id

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 搜索联盟 - 【联盟Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 俱乐部权限不足
    ///  </summary>
    public partial class WEB2_tribe_search
    {
        public const string API = @"/api/tribe/search";

        public sealed class RequestData : RequestDataBase
        {
            public string key { get; set; }  // 关键字<br/>必填，不能为空
            public int clubId { get; set; }  // 俱乐部id<br/>必填，不能为空
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int tribeId { get; set; }  // 联盟隐性id，用于api调用使用
            public int randomId { get; set; }  // 联盟随机id，用于客户端展示使用
            public string tribeName { get; set; }  // 联盟名称，用于客户端展示使用
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 批量获取玩家的与其它玩家的关系信息表 - 【sns模块】
    /// 
    /// 批量获取玩家的与其它玩家的关系信息表
    ///  </summary>
    public partial class WEB2_sns_batch_relations
    {
        public const string API = @"/api/sns/batch_relations";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string randomNum { get; set; }  // 用户随机id
            public string remarkName { get; set; }  // 备注名称
            public int remarkColor { get; set; }  // 备注颜色:-1无颜色
            public int userId { get; set; }  // 用户id,调整后会考虑废弃
            public string remark { get; set; }  // 打法标志
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 俱乐部列表 - 【俱乐部Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    ///  </summary>
    public partial class WEB2_club_list
    {
        public const string API = @"/api/club/list";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public DataElement data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int canCreate { get; set; }
            public int canCreateClub { get; set; }
            public List<ListElement> list;
        }

        public sealed class ListElement
        {

            public int clubId { get; set; }  // 用于请求接口使用
            public int currPe { get; set; }  // 当前俱乐部人数
            public string areaId { get; set; }  // 俱乐部归属地区id
            public string clubName { get; set; }  // 俱乐部名称唯一，并且不为空
            public int randomId { get; set; }  // 展示给用户看，不作为接口调用参数使用
            public long createTime { get; set; }  // 毫秒时间戳 东八区
            public int isOfficial { get; set; }  // 是否官方俱乐部<br/>0 = 不是<br/>1 = 是
            public int total { get; set; }  // 俱乐部总上限人数
            public int userType { get; set; }  // 当前请求接口玩家对应俱乐部的身份<br/>1 = 创建者<br/>2 = 普通成员<br/>3 = 非成员
            public string logo { get; set; }  // 俱乐部头像地址，不为空，默认值：-1
            public int roomCount { get; set; }  // 当前俱乐部进行中房间数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 查询他人统计数据 - 【数据统计模块】
    /// 
    /// 查询他人统计数据
    ///  </summary>
    public partial class WEB2_data_stat_person
    {
        public const string API = @"/api/data_stat/person";

        public sealed class RequestData : RequestDataBase
        {
            public string breakRandomId { get; set; }  // 被查看的用户randomId,请优先根据此字段进行查询,传空串也视为不传此参数
            public int breakUserId { get; set; }  // 被查看的用户ID,以后逐渐废弃,传小于等于0也视为不传该参数
            public int roomPath { get; set; }  // 牌局:61-普通局, 71-MTT, 81-SNG 91-奥马哈 51-大菠萝
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int breakPrice { get; set; }  // 破隐需要的USDT数
            public int imageType { get; set; }  // 影像数据级别 1=生涯 ， 2=30天 ， 3=7天
            public int totalHand { get; set; }  // omaha,普通局特有参数.总手数
            public int CBet { get; set; }  // omaha,普通局特有参数.4Flop持续下注率 如20%返回20即可
            public int AF { get; set; }  // omaha,普通局特有参数.激进程度
            public int fantasy { get; set; }  // 大菠萝特有参数.进范率.如20%返回20即可
            public int handAverage { get; set; }  // 大菠萝特有参数.手牌平均分
            public int thirdTimes { get; set; }  // sng mtt特有参数.第三名次数
            public int totalGameCnt { get; set; }  // omaha,普通局特有参数.总局数
            public int totalEarn { get; set; }  // 总战绩
            public int VPIP { get; set; }  // omaha,普通局特有参数.入池率 如20%返回20即可
            public int breakStatus { get; set; }  // 能否有权限破隐 1401-无权破隐 1402-已破隐（只有已破隐), 1403-可破隐(未破隐)
            public int threeBet { get; set; }  // omaha,普通局特有参数.翻牌前再加注率 如20%返回20即可
            public int roomPath { get; set; }  // 牌局:61-普通局, 71-MTT, 81-SNG 91-奥马哈 51-大菠萝
            public int PRF { get; set; }  // omaha,普通局特有参数.翻牌前加注率 如20%返回20即可
            public int WTSD { get; set; }  // omaha,普通局特有参数.摊牌胜率.如20%返回20即可
            public int Allin_Wins { get; set; }  // omaha,普通局特有参数.全下胜率.如20%返回20即可
            public int papWins { get; set; }  // 大菠萝特有参数.胜率.如20%返回20即可
            public int fantasyAverage { get; set; }  // 大菠萝特有参数.进范平均分
            public int winTimes { get; set; }  // sng mtt特有参数.获奖次数
            public int firstTimes { get; set; }  // sng mtt特有参数.第一名次数
            public int secondTimes { get; set; }  // sng mtt特有参数.第二名次数
            public int playTimes { get; set; }  // sng mtt特有参数.参赛次数
            public int Wins { get; set; }  // omaha,普通局特有参数.入池胜率 如20%返回20即可
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 战绩详情 - 【战绩列表】
    /// 
    /// 战绩详情
    ///  </summary>
    public partial class WEB2_record_detail
    {
        public const string API = @"/api/record/detail";

        public sealed class RequestData : RequestDataBase
        {
            public string roomId { get; set; }  // 房间ID
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string roomName { get; set; }  // 房间名称
            public int totalHand { get; set; }  // 总手数
            public int totalBring { get; set; }  // 总带入
            public List<HonorsElement> honors { get; set; }  // 荣耀榜 MVP之类
            public int totalPl { get; set; }  // 赢利总和
            public bool jpOp { get; set; }  // 是否有彩池: false-没有,true-有
            public int totalLose { get; set; }  // 亏损总和
            public long endTime { get; set; }  // 结束时间
            public int insurancePool { get; set; }  // 保险池
            public int roomId { get; set; }  // 房间id
            public List<InsurancesElement> insurances { get; set; }  // 保险
            public string blind { get; set; }
            public bool insuranceOp { get; set; }  // 是否开启保险: false-没有,true-有
            public string jpPokerType { get; set; }  // jp 击中牌型
            public bool vpOp { get; set; }  // 是否有入池率
            public List<UserRecordsElement> userRecords { get; set; }  // 玩家数据
            public int playTime { get; set; }  // 游戏时间
            public bool manager { get; set; }  // 是否为管理员: 0-否,1-是
            public List<ClubGameRecordElement> clubGameRecord { get; set; }  // 俱乐部数据
            public List<JpRecordsElement> jpRecords { get; set; }  // JP战绩
            public int roomPath { get; set; }//牌局类型:61-德州,62-德州必下场,63-德州AOF,91-奥马哈,92-奥马哈必下场,93-奥马哈-AOF

            public int qianzhu { get; set; }//前注

            public int roomChargeTotal { get; set; }//服务费
        }

        public sealed class HonorsElement
        {
            public User user { get; set; }  // 用户
            public int labelType { get; set; }  // 标签:1-MVP，2-大鱼，3-土豪
        }

        public sealed class User
        {
            public string head { get; set; }  // 用户头像
            public int userId { get; set; }  // 用户ID
            public string nickName { get; set; }  // 用户名称
        }

        public sealed class InsurancesElement
        {
            public string nickName { get; set; }  // 用户名称
            public int insurance { get; set; }  // 保险盈亏
            public int userId { get; set; }  // 用户ID
            public string head { get; set; }  // 用户头像

            public int clubId { get; set; }
            public string clubName { get; set; }
        }

        public sealed class UserRecordsElement
        {
            public int userId { get; set; }  // 用户ID
            public int bring { get; set; }  // 带入
            public int pl { get; set; }  // 战绩
            public int hand { get; set; }  // 手数
            public string head { get; set; }  // 用户头像
            public string nickName { get; set; }  // 用户名称
            public int vp { get; set; }  // 入池率
        }

        public sealed class ClubGameRecordElement
        {
            public int totalPl { get; set; }  // 总盈利
            public int clubId { get; set; }  // 俱乐部id
            public List<int> userIds { get; set; }  // 该俱乐部下的用户id
            public string clubName { get; set; }  // 俱乐部名称
        }

        public sealed class JpRecordsElement
        {
            public User user { get; set; }  // 击中用户基本信息
            public int jpPokerType { get; set; }  // 击中牌型:1皇家同花顺 2同花顺 3四条
            public int jpReward { get; set; }  // 击中奖金
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// MTT战绩详情 - 【战绩列表】
    /// 
    /// 战绩详情
    ///  </summary>
    public partial class WEB2_record_mtt_detail
    {
        public const string API = @"/api/record/mtt_detail";

        public sealed class RequestData : RequestDataBase
        {
            public int gameID { get; set; }  // 游戏ID
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int gameID { get; set; }  // 游戏ID
            public string mttName { get; set; }  // 游戏名字
            public int roomPath { get; set; }
            public long endTime { get; set; }  // 结束时间
            public int playTime { get; set; }  // 游戏时间
            public int totalHand { get; set; }  // 总手数
            public int totalBonus { get; set; }  
            public double myBonus { get; set; }  
            public int rank { get; set; }  
            public string creator { get; set; }  
            public int pariticipants { get; set; }  
            public List<HonorsElement> honors { get; set; }  // 荣耀榜 MVP之类
            public List<ClubGameRecordElement> clubGameRecord { get; set; }  // 俱乐部数据
            public List<AllUsersRecords> allUsersRecords { get; set; }
        }

        public sealed class HonorsElement
        {
            public int userId { get; set; }  // 用户ID
            public string head { get; set; }  // 用户头像
            public string nickName { get; set; }  
            public string clubId { get; set; }  
            public string clubName { get; set; }  
            public int rank { get; set; }  
            public double bonus { get; set; }  
        }

        public sealed class UserGameRecord
        {
            public int userId { get; set; }  // 用户ID
            public string head { get; set; }  // 用户头像
            public string nickName { get; set; }  // 用户名称
            public string clubId { get; set; } 
            public string clubName { get; set; } 
            public int rank { get; set; } 
            public double bonus { get; set; } 
        }

        public sealed class ClubGameRecordElement
        {
            public string clubId { get; set; }  
            public string clubName { get; set; }  
            public double totalPl { get; set; } 
            public List<UserGameRecord> userGameRecords { get; set; }  // 该俱乐部下的用户id
        }

        public sealed class AllUsersRecords
        {
            public int userId { get; set; }  // 用户ID
            public string head { get; set; }  // 用户头像
            public string nickName { get; set; }  // 用户名称
            public string clubId { get; set; }
            public string clubName { get; set; }
            public int rank { get; set; }
            public double bonus { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// banner列表 - 【Banner模块】
    /// 
    /// banner列表
    ///  </summary>
    public partial class WEB2_banner_list
    {
        public const string API = @"/api/banner/list";

        public sealed class RequestData : RequestDataBase
        {
            public string lang { get; set; }//语言(默认简体中文)，zh_CN：简体中文，zh_TW：繁体中文，en_US：英文
            public int type { get; set; }//类型(默认大厅), 0：大厅Banner，1：发现页Banner，2：发现页主图，3：游戏公平，4：游戏商城主图
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string bannerImg { get; set; }  // bannerImg 图像url
            public string bannerUrl { get; set; }  // bannerUrl 点击url
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 进入牌局 - 【大菠萝局-组局&进房API】
    /// 
    /// 玩家进入牌局前，通过此接口获取牌局的基础数据及所在游戏服务的访问地址。
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2001 : 牌局不存在或已解散 
    ///   * 2004 : 账号已被冻结
    ///   * 2005 : 玩家未加入俱乐部
    ///   * 2006 : 玩家所属俱乐部已被关闭
    ///   * 2007 : 玩家所属俱乐部未绑定联盟
    ///   * 2008 : 玩家所属俱乐部转移中或已被踢出
    ///  </summary>
    public partial class WEB2_room_bp_enter
    {
        public const string API = @"/api/room/bp/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int rtype { get; set; }  // 牌局类型,必填<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int rmid { get; set; }  // 牌局ID,必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int inltcp { get; set; }  // 补充带入阈值,USDT小于此值，才允许补充带入
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int inlton { get; set; }  // 限制补充模式:0 = 关闭 ,1 = 开启
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int cptton { get; set; }  // 竞技模式:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int gmcp { get; set; }  // 最低入局分
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int jkon { get; set; }  // 赖子模式：0 = 关闭 ,1 = 开启
            public int dlmod { get; set; }  // 发牌模式：0 =  同步 ,1 = 顺序
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int tloon { get; set; }  // 一战到底模式：0 = 关闭 ,1 = 开启
            public int mode { get; set; }  // 游戏模式：1=普通模式 , 2=血战模式 , 3=血进血出
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 修改用户信息 - 【用户模块】
    /// 
    /// 修改用户信息
    ///  </summary>
    public partial class WEB2_user_mod_user_info
    {
        public const string API = @"/api/user/mod/user_info";

        public sealed class RequestData : RequestDataBase
        {
            public int sex { get; set; }  // 性别:0-男,1-女
            public string nickName { get; set; }  // 用户昵称
            public string sign { get; set; }  // 个性签名
            public string head { get; set; }  // 用户头像
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int presentation { get; set; }//赠送USDT数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 查询我的今日业绩、我的昨日返还、本月业绩、本月返还（截止至昨日返还） - 【分享赚USDT模块API】
    /// 
    /// 状态码
    ///  0 = 成功
    /// 999 = 失败，异常情况等
    ///  </summary>
    public partial class WEB2_promotion_query_achievements
    {
        public const string API = @"/api/promotion/query_achievements";

        public sealed class RequestData : RequestDataBase
        {
            public int size { get; set; }
            public int start { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public List<MonthAchievementsElement> monthAchievements { get; set; }
            public int userId { get; set; }  // 推广员id
            public List<MonthBeansElement> monthBeans { get; set; }
            public int todayAchievement { get; set; }  // 今日业绩
            public int totalMonthAchievement { get; set; }        //今日业绩

            public int yesterdayBeans { get; set; }  // 昨日返还
            public int totalMonthBeans { get; set; }//本月返还
        }

        public sealed class MonthAchievementsElement
        {
            public long date { get; set; }
            public int dailyData { get; set; }
        }

        public sealed class MonthBeansElement
        {
            public long date { get; set; }
            public int dailyData { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 创建俱乐部 - 【俱乐部Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1200 = 关键参数错误
    /// 1202 = 俱乐部申请待处理
    /// 1203 = 已加入俱乐部
    /// 1204 = 俱乐部重名
    ///  </summary>
    public partial class WEB2_club_create
    {
        public const string API = @"/api/club/create";

        public sealed class RequestData : RequestDataBase
        {
            public string areaId { get; set; }  // 俱乐部地区id<br/>非必填
            public string phone { get; set; }  // 联系人手机号码<br/>非必填
            public string wechat { get; set; }  // 联系人手机号码<br/>非必填
            public string clubName { get; set; }  // 俱乐部名称<br/>必填
            public string telegram { get; set; }  // telegram联系<br/>非必填
            public string clubHead { get; set; }  // 俱乐部头像文件地址<br/>目前版本可以传 -1的字符串
            public string email { get; set; }  // 电子邮箱地址<br/>非必填
            public string message { get; set; }  // 留言<br/>非必填
            public string desc { get; set; }  // 俱乐部简介<br/>非必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 修改信息 - 【俱乐部Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1201 = 没有俱乐部操作权限
    /// 1204 = 俱乐部重名
    /// 1205 = 目标俱乐部不存在
    ///  </summary>
    public partial class WEB2_club_modify
    {
        public const string API = @"/api/club/modify";

        public sealed class RequestData : RequestDataBase
        {
            public string content { get; set; }  // 修改内容<br/>必填，如果是修改名称，则此字段代表名称，如果是修改简介，则代表简介，如果是修改俱乐部头像，则是头像地址，最长60字符，并且不能为空
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public int type { get; set; }  // 请求方式<br/>必填<br/>1 = 修改俱乐部名称<br/>2 = 修改俱乐部简介<br/>3 = 修改俱乐部头像地址
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 背包物品使用 - 【背包模块】
    /// 
    /// 背包物品使用
    ///  </summary>
    public partial class WEB2_backpack_goods_apply_prize
    {
        public const string API = @"/api/backpack/goods/apply_prize";

        public sealed class RequestData : RequestDataBase
        {
            public int id { get; set; }  // 物品id
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string userName { get; set; }
            public int id { get; set; }
            public string mobile { get; set; }
            public string wechat { get; set; }
            public int userId { get; set; }
            public string address { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 进入牌局 - 【SNG局-组局&进房API】
    /// 
    /// 玩家进入牌局前，通过此接口获取牌局的基础数据及所在游戏服务的访问地址。
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2001 : 牌局不存在或已解散 
    ///   * 2004 : 账号已被冻结
    ///   * 2005 : 玩家未加入俱乐部
    ///   * 2006 : 玩家所属俱乐部已被关闭
    ///   * 2007 : 玩家所属俱乐部未绑定联盟
    ///   * 2008 : 玩家所属俱乐部转移中或已被踢出
    ///  </summary>
    public partial class WEB2_room_sng_enter
    {
        public const string API = @"/api/room/sng/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int rtype { get; set; }  // 牌局类型,必填<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int rmid { get; set; }  // 牌局ID,必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int rmid { get; set; }  // 牌局ID
            public int onrid { get; set; }  // 房主用户ID，必填
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public string cliip { get; set; }  // 客户端真实IP
            public string name { get; set; }  // 牌局名称
            public string gmprt { get; set; }  // 牌局服务port，必填
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 获取用户私人信息 - 【用户模块】
    /// 
    /// 获取用户私人信息
    ///  </summary>
    public partial class WEB2_user_self_info
    {
        public const string API = @"/api/user/self_info";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string personSign { get; set; }  // 个人签名
            public int chip { get; set; }  // USDT
            public int modifyNickNum { get; set; }  // 修改用户名称次数
            public int modifyHeadTime { get; set; }  // 修改用户[头像]次数
            public string randomNum { get; set; }  // 用户id
            public string head { get; set; }  // 头像
            public int vip { get; set; }  // 是否为VIP:0-否,1-是
            public long vipEndDate { get; set; }  // VIP结束时间,没有开通VIP返回null
            public int sex { get; set; }  // 用户性别:0-男,1-女
            public string nickName { get; set; }  // 用户昵称
            public int remarkColor { get; set; }  // 用户标志颜色，0无 1颜色1 2颜色2 3颜色3

            public int payPwdStatus { get; set; }//支付密码状态(0-未设置,1-已设置)
            public WalletElement wallet { get; set; }
        }

        public sealed class WalletElement
        {
            public int chip { get; set; }//可提USDT
            public int notExtractChip { get; set; }//不可提USDT
            public int status { get; set; }//钱包状态:(1-正常,2-停用)
            public long plCount { get; set; }//战绩流水总和  单位USDT
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 创建牌局 - 【大菠萝局-组局&进房API】
    /// 
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2002  : 权限不足!
    ///  </summary>
    public partial class WEB2_room_bp_create
    {
        public const string API = @"/api/room/bp/create";

        public sealed class RequestData : RequestDataBase
        {
            public int cptton { get; set; }  // 竞技模式:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int slmz { get; set; }  // 小盲注,必填,必须>=1
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int dlmod { get; set; }  // 发牌模式：0 =  同步 ,1 = 顺序<br/>不需此条件，字段不出现或传null值
            public int gmcp { get; set; }  // 最低入局分<br/>不需此条件，字段不出现或传null值
            public int tloon { get; set; }  // 一战到底模式：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int ipon { get; set; }  // IP控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int gpson { get; set; }  // GPS控制：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int inlton { get; set; }  // 限制补充模式:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int inltcp { get; set; }  // 补充带入阈值,USDT小于此值，才允许补充带入<br/>不需此条件，字段不出现或传null值
            public int mode { get; set; }  // 游戏模式：1=普通模式 , 2=血战模式 , 3=血进血出<br/>不需此条件，字段不出现或传null值
            public int opts { get; set; }  // 操作思考时间,必填,>=0,单位：秒
            public int incp { get; set; }  // 带入USDT,必填,>=200
            public int gmnt { get; set; }  // 牌局时长,必填,>=30,单位：分钟
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public int jkon { get; set; }  // 赖子模式：0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public string name { get; set; }  // 牌局名称,必填,最大长度：50字符
            public int rpu { get; set; }  // 牌桌人数,必填,取值范围：[2,9]
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string gmprt { get; set; }  // 牌局服务port，必填
            public int gmnt { get; set; }  // 牌局时长,单位：分钟
            public int inmxr { get; set; }  // 带入最大倍数,必填
            public int inltcp { get; set; }  // 补充带入阈值,USDT小于此值，才允许补充带入
            public int incp { get; set; }  // 最小带入USDT,必填
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public string cliip { get; set; }  // 客户端真实IP
            public int inlton { get; set; }  // 限制补充模式:0 = 关闭 ,1 = 开启
            public int onrid { get; set; }  // 房主用户ID，必填
            public int rmid { get; set; }  // 牌局ID
            public int cptton { get; set; }  // 竞技模式:0 = 关闭 ,1 = 开启<br/>不需此条件，字段不出现或传null值
            public int gmcp { get; set; }  // 最低入局分
            public int slmz { get; set; }  // 小盲注
            public string name { get; set; }  // 牌局名称
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int ipon { get; set; }  // IP限制:0 = 无 ,1 = 有
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public int jkon { get; set; }  // 赖子模式：0 = 关闭 ,1 = 开启
            public int dlmod { get; set; }  // 发牌模式：0 =  同步 ,1 = 顺序
            public int rpu { get; set; }  // 牌桌人数,取值范围：[2,9]
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int inmnr { get; set; }  // 带入最小倍数,必填
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int gpson { get; set; }  // GPS控制:0 = 无 ,1 = 有
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int tloon { get; set; }  // 一战到底模式：0 = 关闭 ,1 = 开启
            public int mode { get; set; }  // 游戏模式：1=普通模式 , 2=血战模式 , 3=血进血出
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 获取用户背包信息 - 【背包模块】
    /// 
    /// 获取用户背包信息
    ///  </summary>
    public partial class WEB2_backpack_info
    {
        public const string API = @"/api/backpack/info";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public List<GameBagGoodsElement> gameBagGoods { get; set; }  // 背包物品项列表
        }

        public sealed class GameBagGoodsElement
        {
            public int gameId { get; set; }  // 门票跳转的比赛id
            public int propStatus { get; set; }  // 物品状态，0未使用 1使用中 2已使用
            public int id { get; set; }  // 物品id
            public string propDesc { get; set; }  // 物品描述，如：获得时间：2017-06-19<br/>获得赛事：可用于报名官方 MTT 赛事
            public string propIcon { get; set; }  // 物品图片完整链接
            public int propValue { get; set; }  // 物品价值，如100
            public int isVirtual { get; set; }  // 是否虚拟物品，未使用，0否 1是
            public int gameType { get; set; }  // 比赛类型
            public string propName { get; set; }  // 物品名称，如：100元参赛券
            public int propType { get; set; }  // 物品类型，1 mtt门票 2 物品
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 修改用户对其它玩家备注信息 - 【sns模块】
    /// 
    /// 修改用户对其它玩家备注信息
    ///  </summary>
    public partial class WEB2_sns_mod_remark
    {
        public const string API = @"/api/sns/mod_remark";

        public sealed class RequestData : RequestDataBase
        {
            public int remarkColor { get; set; }  // 标注颜色,传-1无颜色
            public string remarkName { get; set; }  // 备注名
            public string otherRandomId { get; set; }  // 其它用户id.与前端协调考虑改成randomId
            public string remark { get; set; }  // 打法标志
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 带邀请码注册 - 【分享赚USDT模块API】
    /// 
    /// 状态码
    ///  0 = 成功
    /// 999 = 失败，异常情况等
    /// 1601 = 请先加入俱乐部，才可以成为推广员
    ///  </summary>
    public partial class WEB2_promotion_resgister
    {
        public const string API = @"/api/promotion/resgister";

        public sealed class RequestData : RequestDataBase
        {
            public string inventCode { get; set; }  // 邀请码<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 创建牌局 - 【SNG局-组局&进房API】
    /// 
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 101  ：参数必须指定！
    ///   * 102  ：参数长度超出允许范围！
    ///   * 103  ：参数值未满足最小值约束！
    ///   * 104  ：参数值无效！
    ///   * 199  ：未知参数错误！
    ///   * 2002  : 权限不足!
    ///  </summary>
    public partial class WEB2_room_sng_create
    {
        public const string API = @"/api/room/sng/create";

        public sealed class RequestData : RequestDataBase
        {
            public int incp { get; set; }  // 带入USDT【买入设置】,必填,>=200
            public int muckon { get; set; }  // muck开关,不需此条件，字段不出现或传null值 <br/>0 = 关闭 ,1 = 开启
            public int initcp { get; set; }  // 起始积分,必填,取值范围：[3000|5000]
            public int upmzt { get; set; }  // 涨盲时间,必填,单位：分钟，取值范围：[3|5]
            public int mhtp { get; set; }  // 比赛类型:0 = 快速局 ,1 = 标准局
            public int rpu { get; set; }  // 牌桌人数,必填,取值范围：[2|6|9]
            public int gpson { get; set; }  // GPS控制,不需此条件，字段不出现或传null值 <br/>0 = 无 ,1 = 有
            public string name { get; set; }  // 牌局名称,必填
            public int ipon { get; set; }  // IP控制,不需此条件，字段不出现或传null值 <br/>0 = 无 ,1 = 有
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int state { get; set; }  // 房间状态<br/>0 = 结束<br/>3 = 新建<br/>4 = 已开始
            public string gmip { get; set; }  // 牌局服务IP，必填
            public int ctdts { get; set; }  // 牌局创建时间,单位：秒
            public int rmid { get; set; }  // 牌局ID
            public int onrid { get; set; }  // 房主用户ID，必填
            public int opts { get; set; }  // 操作思考时间,单位：秒
            public int rtype { get; set; }  // 牌局类型<br/>61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int isonr { get; set; }  // 是否房主：0=否，1=是
            public int muckon { get; set; }  // muck开关:0 = 关闭 ,1 = 开启
            public string cliip { get; set; }  // 客户端真实IP
            public string name { get; set; }  // 牌局名称
            public string gmprt { get; set; }  // 牌局服务port，必填
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 战绩列表 - 【战绩列表】
    /// 
    /// 战绩列表
    ///  </summary>
    public partial class WEB2_record_my
    {
        public const string API = @"/api/record/my";

        public sealed class RequestData : RequestDataBase
        {
            public long nextTime { get; set; }  // 下次查询时间,首次查询,设置当前时间
            public int searchType { get; set; }  // 牌局类型  搜索牌局类型:1-德州,2-Omaha,3-AOF,4-必下场
            public int clubId { get; set; }
            
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public long nextTime { get; set; }  // 下一个时间起点
            public List<ListElement> list { get; set; }  // 详细列表项
        }

        public sealed class ListElement
        {
            public string roomId { get; set; }  // 房间id
            public string roomName { get; set; }  // 房间名称
            public bool insuranceOp { get; set; }  // 保险开启标志:0-没开启,1-开启
            public bool jackPotOp { get; set; }  // jp开启标志:0-没开启,1-开启
            public long endTime { get; set; }  // 结束时间
            public string blind { get; set; }  // 大小盲
            public long startTime { get; set; }  // 牌局开始时间
            public int gameTime { get; set; }  // 个人游戏时间
            public int personRecord { get; set; }  // 收益
            public int roomPath { get; set; }//61-德州,62-德州必下场,63-德州AOF,91-奥马哈,92-奥马哈必下场,93-奥马哈-AOF
            public int qianzhu { set; get; }//前注
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// MTT战绩列表 - 【战绩列表】
    /// 
    /// MTT战绩列表
    ///  </summary>
    public partial class WEB2_record_mtt_list
    {
        public const string API = @"/api/record/mtt_list";

        public sealed class RequestData : RequestDataBase
        {
            public long nextTime { get; set; }  // 下次查询时间,首次查询,设置当前时间
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public long nextTime { get; set; }  // 下一个时间起点
            public List<ListElement> list { get; set; }  // 详细列表项
        }

        public sealed class ListElement
        {
            public int gameId { get; set; }  // 比赛id
            public string mttName { get; set; }  // 比赛名称
            public long startTime { get; set; }  // 牌局开始时间
            public long endTime { get; set; }  // 结束时间
            public int pariticipants { get; set; }  // 参与人数
            public int rank { get; set; }   // 排名
            public int registationFee { get; set; }  // 报名费
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 判断昵称是否存在 - 【用户模块】
    /// 
    /// 判断昵称是否存在:ture-已经存在,false-未存在
    ///  </summary>
    public partial class WEB2_user_exist_nick
    {
        public const string API = @"/api/user/exist_nick";

        public sealed class RequestData : RequestDataBase
        {
            public string nickName { get; set; }  // 手机号码必须加上地区号并用下划线隔开如:86-12345676789
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public bool data { get; set; }  // 业务数据，成功情况才有数据
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 三种VIP特权详情 - 【用户VIP模块】
    /// 
    /// 三种VIP特权详情
    ///  </summary>
    public partial class WEB2_user_vip_config
    {
        public const string API = @"/api/user_vip/config";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string cardSpectrum { get; set; }  // 保存牌谱上限
            public string remark { get; set; }  // 备注上限（暂无）-1为无限
            public int viewDays { get; set; }  // 数据查看范围:<0 标识生涯，0=无法查看 ， 其他标识对应天数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 俱乐部基金充值 - 【俱乐部基金Api】
    /// 
    /// 状态码
    ///  0 = 成功，如果需要数据可以从data字段解析
    /// 999 = 失败，异常情况等
    /// 1200 = 俱乐部关键参数错误
    /// 1201 = 无操作俱乐部权限
    /// 1205 = 俱乐部不存在
    ///  </summary>
    public partial class WEB2_club_fund_recharge
    {
        public const string API = @"/api/club/fund/recharge";

        public sealed class RequestData : RequestDataBase
        {
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public int fund { get; set; }  // 充值基金数额<br/>必填
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }
    public partial class WEB2_club_fund_trade
    {
        public const string API = @"/api/club/fund/trade";

        public sealed class RequestData : RequestDataBase
        {
            public int clubId { get; set; }  // 俱乐部id<br/>必填
            public int fund { get; set; }  // 充值基金数额<br/>必填
            public int fundType { get; set; }//0：转入  1：转出

        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public long fund { get; set; }
            public long betChip { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    /// <summary>
    /// 查询个人统计数据 - 【数据统计模块】
    /// 
    /// 查询个人统计数据
    ///  </summary>
    public partial class WEB2_data_stat_person_profit
    {
        public const string API = @"/api/data_stat/person_profit";

        public sealed class RequestData : RequestDataBase
        {
            public int timeType { get; set; }  // 查询时间范围:1-全部, 2-30天, 3-7天
            public int roomPath { get; set; }  // 牌局:61-普通局, 71-MTT, 81-SNG 91-奥马哈 51-大菠萝
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int totalHand { get; set; }  // omaha,普通局特有参数.总手数
            public int showAdvance { get; set; }  // 是否显示高级数据:0-否,1-是
            public int CBet { get; set; }  // omaha,普通局特有参数.4Flop持续下注率 如20%返回20即可
            public int AF { get; set; }  // omaha,普通局特有参数.激进程度
            public int fantasy { get; set; }  // 大菠萝特有参数.进范率.如20%返回20即可
            public int handAverage { get; set; }  // 大菠萝特有参数.手牌平均分
            public int thirdTimes { get; set; }  // sng mtt特有参数.第三名次数
            public int aveageBringIn { get; set; }  // 场均带入
            public int totalGameCnt { get; set; }  // omaha,普通局特有参数.总局数
            public int aveageEarn { get; set; }  // 场均战绩
            public int totalEarn { get; set; }  // 总战绩
            public int VPIP { get; set; }  // omaha,普通局特有参数.入池率 如20%返回20即可
            public int timeType { get; set; }  // 查询时间范围:1-全部, 2-30天, 3-7天
            public int threeBet { get; set; }  // omaha,普通局特有参数.翻牌前再加注率 如20%返回20即可
            public int roomPath { get; set; }  // 牌局:61-普通局, 71-MTT, 81-SNG 91-奥马哈 51-大菠萝
            public int PRF { get; set; }  // omaha,普通局特有参数.翻牌前加注率 如20%返回20即可
            public int WTSD { get; set; }  // omaha,普通局特有参数.摊牌胜率.如20%返回20即可
            public int Allin_Wins { get; set; }  // omaha,普通局特有参数.全下胜率.如20%返回20即可
            public int papWins { get; set; }  // 大菠萝特有参数.胜率.如20%返回20即可
            public int fantasyAverage { get; set; }  // 大菠萝特有参数.进范平均分
            public int winTimes { get; set; }  // sng mtt特有参数.获奖次数
            public int firstTimes { get; set; }  // sng mtt特有参数.第一名次数
            public int secondTimes { get; set; }  // sng mtt特有参数.第二名次数
            public int playTimes { get; set; }  // sng mtt特有参数.参赛次数
            public int Wins { get; set; }  // omaha,普通局特有参数.入池胜率 如20%返回20即可
            public int clubRoomPlTotal { get; set; }//房间积分
            
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 保存收获地址 - 【背包模块】
    /// 
    /// 保存收获地址
    ///  </summary>
    public partial class WEB2_backpack_consignee_save
    {
        public const string API = @"/api/backpack/consignee/save";

        public sealed class RequestData : RequestDataBase
        {
            public string address { get; set; }  // 收件人详细地址
            public string mobile { get; set; }  // 收件人手机号码
            public string wechat { get; set; }  // 收件人微信
            public string userName { get; set; }  // 收件人名称
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string userName { get; set; }
            public int id { get; set; }
            public string mobile { get; set; }
            public string wechat { get; set; }
            public int userId { get; set; }
            public string address { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 我的牌局列表 - 【游戏大厅-牌局列表API】
    /// 
    /// 返回所有玩家带入过的牌局。
    ///  </summary>
    public partial class WEB2_room_clublist
    {
        public const string API = @"/api/club/room/list";

        public sealed class RequestData : RequestDataBase
        {

        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string name { get; set; }
            public int state { get; set; }  // 游戏状态:5 = 游戏中 , 4 = 进行中 , 3 = 等待中
            public int slmz { get; set; }  // 盲注级别,最小盲注值，字段不存在或值>0
            public int zhuatou { get; set; }  // 是否抓头
            public int htron { get; set; }  // 游戏小标签-MTT猎人赛模式:0 = 关闭 , 1 = 开启
            public int stdn { get; set; }  // 坐下人数,默认值：0
            public int rmsec { get; set; }  // 牌桌剩余时间,单位：秒
            public int omamod { get; set; }  // 游戏小标签-omaha模式:0 = 普通模式 , 1 = 血战模式
            public int dlmod { get; set; }  // 游戏小标签-大菠萝发牌模式:0 = 同步发牌 , 1 = 顺序发牌
            public int rmid { get; set; }  // 牌局ID
            public string logo { get; set; }  // 牌局LOGO的http访问URL，空值无效
            public int jkon { get; set; }  // 游戏小标签-大菠萝癞子模式:0 = 关闭 , 1 = 开启
            public int isron { get; set; }  // 游戏小标签-保险模式:0 = 关闭 , 1 = 开启
            public int jpon { get; set; }  // 游戏小标签-jackpot模式:0 = 关闭 , 1 = 开启
            public int rtype { get; set; }  // 牌局类型:61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int lcrdt { get; set; }  // 创建时间:方向=1时有效，当前最后一条的牌桌的创建时间，大于0值有效
            public int bpmod { get; set; }  // 游戏小标签-大菠萝模式:1 = 普通模式 , 2 = 血战模式 , 3 = 血进血出
            public int rpu { get; set; }  // 牌桌人数,默认值：0
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int qianzhu { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 我的牌局列表 - 【游戏大厅-牌局列表API】
    /// 
    /// 返回所有玩家带入过的牌局。
    ///  </summary>
    public partial class WEB2_room_mylist
    {
        public const string API = @"/api/room/mylist";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string name { get; set; }
            public int state { get; set; }  // 游戏状态:5 = 游戏中 , 4 = 进行中 , 3 = 等待中
            public int slmz { get; set; }  // 盲注级别,最小盲注值，字段不存在或值>0
            public int zhuatou { get; set; }  // 是否抓头
            public int htron { get; set; }  // 游戏小标签-MTT猎人赛模式:0 = 关闭 , 1 = 开启
            public int stdn { get; set; }  // 坐下人数,默认值：0
            public int rmsec { get; set; }  // 牌桌剩余时间,单位：秒
            public int omamod { get; set; }  // 游戏小标签-omaha模式:0 = 普通模式 , 1 = 血战模式
            public int dlmod { get; set; }  // 游戏小标签-大菠萝发牌模式:0 = 同步发牌 , 1 = 顺序发牌
            public int rmid { get; set; }  // 牌局ID
            public string logo { get; set; }  // 牌局LOGO的http访问URL，空值无效
            public int jkon { get; set; }  // 游戏小标签-大菠萝癞子模式:0 = 关闭 , 1 = 开启
            public int isron { get; set; }  // 游戏小标签-保险模式:0 = 关闭 , 1 = 开启
            public int jpon { get; set; }  // 游戏小标签-jackpot模式:0 = 关闭 , 1 = 开启
            public int rtype { get; set; }  // 牌局类型:61 = 德州<br/>91 = 奥马哈<br/>51 = 大菠萝<br/>41 = 必下场<br/>31 = AOF<br/>81 = SNG<br/>71 = MTT
            public int lcrdt { get; set; }  // 创建时间:方向=1时有效，当前最后一条的牌桌的创建时间，大于0值有效
            public int bpmod { get; set; }  // 游戏小标签-大菠萝模式:1 = 普通模式 , 2 = 血战模式 , 3 = 血进血出
            public int rpu { get; set; }  // 牌桌人数,默认值：0
            public int rpath { get; set; }  // 牌局类型,不需此条件，传值：-1  61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF 51 = 大菠萝 81 = SNG 71 = MTT
            public int qianzhu { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 获取用户牌谱列表 - 【牌谱】
    /// 
    /// 获取用户牌谱列表
    ///  </summary>
    public partial class WEB2_brand_spe_info
    {
        public const string API = @"/api/brand_spe/info";

        public sealed class RequestData : RequestDataBase
        {
            public int pageNo { get; set; }  // 第几页 1开始
            public int sortType { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public List<HandsElement> hands { get; set; }  // 手牌列表
            public long count { get; set; }  // 总数
            public int page { get; set; }  // 页码
        }

        public sealed class HandsElement
        {
            public string handName { get; set; }  // 手牌名称
            public string id { get; set; }  // 手牌id
            public int bigBlind { get; set; }  // 大盲注
            public int smallBlind { get; set; }  // 小盲注
            public long timeStamp { get; set; }  // 时间
            public int win { get; set; }  // 输赢筹码
            public int roomType { get; set; }  // 牌局类型:
            public bool collect { get; set; }  // 是否收藏:false-没有,true-收藏
            public int index { get; set; }  // 编号
            public int qianzhu { get; set; }  // 前注
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// jackpot 标签信息 - 【jack-pot-record-controller】
    /// 
    /// jackpot 标签信息
    ///  </summary>
    public partial class WEB2_jackpot_view
    {
        public const string API = @"/api/jackpot/view";

        public sealed class RequestData : RequestDataBase
        {
            public int roomId { get; set; }
            public int roomPath { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public long jackPotFund { get; set; }  // jackpot 彩池
            public double fourofakindRatio { get; set; }  // 四条击中比例
            public string blind { get; set; }  // 大小盲
            public double straightflushRatio { get; set; }  // 同花顺击中比例
            public double royalflushRatio { get; set; }  // 皇家同花顺击中比例
            public List<SubpoolListElement> subpoolList { get; set; }  // 分池列表
        }

        public sealed class SubpoolListElement
        {
            public long fund { get; set; }  // jackpot 彩池分池
            public int blindCode { get; set; }  // 
            public string blindName { get; set; }  // 大小盲
            public int isCurrent { get; set; }  // 是否当前盲注 0 1
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// jackpot 奖励 - 【jack-pot-record-controller】
    /// 
    /// jackpot 奖励
    ///  </summary>
    public partial class WEB2_jackpot_record
    {
        public const string API = @"/api/jackpot/record";

        public sealed class RequestData : RequestDataBase
        {
            public int roomId { get; set; }
            public int roomPath { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public List<RecordListElement> recordList { get; set; }  // 包含元素:RecordListElement
            public TopReward topReward { get; set; }  // 
        }

        public sealed class RecordListElement
        {
            public int rewardChip { get; set; }  // 172
            public int pocerType { get; set; }  // 3
            public string nickName { get; set; }  // 物易国
            public string blindName { get; set; }  // 1/2
            public int userId { get; set; }  // 331191
            public long createTime { get; set; }  // 1553854240
        }

        public sealed class TopReward
        {
            public int rewardChip { get; set; }  // 8467
            public int pocerType { get; set; }  // 1
            public string nickName { get; set; }  // 中国中铁
            public string blindName { get; set; }  // 10/20
            public int userId { get; set; }  // 303217
            public long createTime { get; set; }  // 1552649313
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// getUnreadMsgNums - 【消息Api】列表
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_message_nums_detail
    {
        public const string API = @"/api/message/nums-detail";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string content { get; set; }//消息主要参数 {0}
            public int messageType { get; set; }//消息种类1 = 俱乐部消息  2=联盟消息 3=系统消息 4=钱包消息 5=分享赚USDT消息 6=背包消息
            public int num { get; set; }//未读消息数量
            public string remark { get; set; }//消息附加参数   {1}
            public long time { get; set; }//消息生成毫秒时间戳
            public int type { get; set; }// 消息类型  对应dic
            public string title { get; set; }//{2}
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// getUnreadMsgNums - 【消息Api】
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_message_nums
    {
        public const string API = @"/api/message/nums";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public DataElement data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int unreadClubNum { get; set; }
            public int unreadTribeNum { get; set; }
            public int unreadSystemNum { get; set; }
            public int unreadMoneyNum { get; set; }
            public int unreadShareKdouNum { get; set; }
            public int unreadBagNum { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    /// <summary>
    /// updatePushInfo - 【推送API】 h
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_push_uinfo
    {
        public const string API = @"/api/push/uinfo";

        public sealed class RequestData : RequestDataBase
        {
            public int deviceType { get; set; }
            public int lanType { get; set; }
            public string regId { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// genToken - 【IM API】
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_im_gentoken
    {
        public const string API = @"/api/im/gentoken";

        public sealed class RequestData : RequestDataBase
        {
            public int userId { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string token { get; set; }  // 登录im凭据
            public string gid { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// genToken - 【IM API】
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_im_upim_info
    {
        public const string API = @"/api/im/upim_info";

        public sealed class RequestData : RequestDataBase
        {
            public int userId { get; set; }
            public string groupId { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// getUnreadMsgNums - 【消息Api】
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_message_clear
    {
        public const string API = @"/api/message/clear";

        public sealed class RequestData : RequestDataBase
        {
            public int clearType { get; set; }  // 清理消息类型<br/>1 = 俱乐部消息<br/>2 = 联盟消息<br/>3 = 系统消息
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// getMsgRecord - 【消息Api】
    /// 
    /// (null)
    ///  </summary>
    public partial class WEB2_message_list
    {
        public const string API = @"/api/message/list";

        public sealed class RequestData : RequestDataBase
        {
            public long time { get; set; }
            public int direction { get; set; }
            public int messageType { get; set; }  // 请求历史消息类型<br/>1 = 俱乐部消息<br/>2 = 联盟消息<br/>3 = 系统消息
            public int lanType { get; set; }  // 0中文  1英文 2繁体
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public DataElement data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public int direction { get; set; }
            public List<ListElement> list;
        }
        public sealed class ListElement
        {
            public string header { get; set; }  // 消息对应发送者头像地址，空字符创或者-1则为默认头像
            public string content { get; set; }  // 消息体，一般为消息核心参数，例如发放500USDT，此处数据则为500   {0}
            public string remark { get; set; }  // 消息备注，如果content字段不够用，则可能从此字段取数据展示        {1}
            public int msgStatus { get; set; }  // 消息处理状态<br/>0 = 成功<br/>1 = 失败
            public string title { get; set; }  // 消息标题，为空则不显示标题                                                                   {2}
            public string msgId { get; set; }  // 消息id，无特殊情况为唯一
            public int type { get; set; }  // 消息类型
            public long time { get; set; }  // 消息生成时间
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_message_handle
    {
        public const string API = @"/api/message/handle";

        public sealed class RequestData : RequestDataBase
        {
            public string messageId { get; set; }
            public int handleType { get; set; }//1同意2拒绝
        }

        public sealed class ResponseData
        {

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    public partial class WEB2_club_modify_contact
    {
        public const string API = @"/api/club/modify-contact";
        public sealed class RequestData : RequestDataBase
        {
            public int userId { get; set; }
            public int clubId { get; set; }
            public int firstType { get; set; }
            public string firstContact { get; set; }
            public int secondType { get; set; }
            public string secondContact { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 举报 
    ///  </summary>
    public partial class WEB2_Report_User
    {
        public const string API = @"/api/report/user";

        public sealed class RequestData : RequestDataBase
        {
            public string reportContent { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户            
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    /// <summary>
    /// 切换语言  zsx
    ///  </summary>
    public partial class WEB2_UpdatePushInfo
    {
        public const string API = @"/api/push/uinfo";

        public sealed class RequestData : RequestDataBase
        {
            public int lanType { get; set; }//0 = 中文    1 = 英文    2 = 繁体中文          
            public int deviceType { get; set; }//0 = android    1 = ios
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户            
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 平台充值记录 - 【钱包模块】
    /// 
    /// 平台充值记录
    ///  </summary>
    public partial class WEB2_wallet_recharge_log
    {
        public const string API = @"/api/wallet/recharge/log";

        public sealed class RequestData : RequestDataBase
        {
            public long nextTime { get; set; }  // 下一个时间戳  若无,传当前时间
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public long nextTime { get; set; }  // 下一个id
            public List<ListElement> list { get; set; }  // 数据视图
        }

        public sealed class ListElement
        {
            public string orderNo { get; set; }  // 充值订单号
            public int chipNum { get; set; }  // 充值数量
            public long createTime { get; set; }  // 充值时间戳    3
            public int status { get; set; }  // 充值状态(1-充值中,2-充值成功,3-充值失败)
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 转USDT记录 - 【钱包模块】
    /// 
    /// 转USDT记录
    ///  </summary>
    public partial class WEB2_wallet_flow_list
    {
        public const string API = @"/api/wallet/flow/list";

        public sealed class RequestData : RequestDataBase
        {
            public int nextId { get; set; }  // 下一个id
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int nextId { get; set; }  // 下一个id
            public List<ListElement> list { get; set; }  // 数据视图
        }

        public sealed class ListElement
        {
            public string otherName { get; set; }  // 交易对手昵称
            public int chip { get; set; }  // 转USDT数量(转出为负,转入为正)  2
            public long createdTime { get; set; }  // 转USDT时间戳    3
            public string flowChipNo { get; set; }  // 转USDT订单号
            public string otherRId { get; set; }  // 交易对手随机id
            public int status { get; set; }//0-成功
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 平台提取 - 【钱包模块】
    /// 
    /// 平台提取
    ///  </summary>
    public partial class WEB2_wallet_withdraw
    {
        public const string API = @"/api/wallet/withdraw";

        public sealed class RequestData : RequestDataBase
        {
            public string code { get; set; }  // 验证码
            public string phone { get; set; }  // 手机号
            public int chip { get; set; }  // USDT数
            public int transType { get; set; }  // 交易方式:1-银行卡
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public bool data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }
    //获取Gas值


    /// <summary>
    /// 平台充值 - 【钱包模块】
    /// 
    /// 平台充值
    ///  </summary>
    public partial class WEB2_wallet_recharge
    {
        public const string API = @"/api/wallet/recharge";

        public sealed class RequestData : RequestDataBase
        {
            public int amount { get; set; }//充值金额
            public int transType { get; set; }//交易方式(1-银行卡)
            public int appType { get; set; }//app类型: 1=android 2=ios
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string webPayUrl { get; set; }
            public int amount { get; set; }// cms余额不足, 返回最多可充余额
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_wallet_getGas
    {
        public const string API = @"/api/wallet/getGas";

        public sealed class RequestData : RequestDataBase
        {
            public int type { get; set; }  // 账户类型:1-银行卡,2-支付宝3-usdt
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public double rate { get; set; }//汇率
            public int gas { get; set; }// gas
            public int isboss { get; set; }// 是不是俱乐部主
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }

    }

    /// <summary>
    /// 保存用户收款账户信息 - 【钱包模块】
    /// 
    /// 保存用户收款账户信息
    ///  </summary>
    public partial class WEB2_wallet_account_payee_save
    {
        public const string API = @"/api/wallet/account_payee/save";

        public sealed class RequestData : RequestDataBase
        {
            public string bankAccName { get; set; }  // 银行卡 用户名
            public string bankAccNo { get; set; }  // 银行卡卡号  661414145141
            public string bankName { get; set; }  // 银行名称  --工商银行
            public string bankOfDeposit { get; set; }  // 开户行
            public string bankLocProvince { get; set; }  // 开户行所在省 
            public string bankLocCity { get; set; }  // 开户行所在市
            public string bankCode { get; set; }  // 银行代号

            public string alipayRealName { get; set; }  // 账户类型:1-银行卡,2-支付宝           
            public int type { get; set; }  // 账户类型:1-银行卡,2-支付宝3-usdt
            public string alipayAcc { get; set; }  // 支付宝账户
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 平台提取记录 - 【钱包模块】
    /// 
    /// 平台提取记录
    ///  </summary>
    public partial class WEB2_wallet_withdraw_log
    {
        public const string API = @"/api/wallet/withdraw/log";

        public sealed class RequestData : RequestDataBase
        {
            public int nextId { get; set; }  // 下一个id
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int nextId { get; set; }  // 下一个id
            public List<ListElement> list { get; set; }  // 数据视图
        }

        public sealed class ListElement
        {
            public int status { get; set; }  // 提取状态
            public int chip { get; set; }  // 提取数量
            public string withdrawNo { get; set; }  // 提取单号
            public long createdTime { get; set; }  // 提取时间戳
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 返还记录 - 【钱包模块】
    /// 
    /// 返还记录
    ///  </summary>
    public partial class WEB2_wallet_feedback_list
    {
        public const string API = @"/api/wallet/feedback/list";

        public sealed class RequestData : RequestDataBase
        {
            public int nextId { get; set; }  // 下一个id
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int nextId { get; set; }  // 下一个id
            public List<ListElement> list { get; set; }  // 数据视图
        }

        public sealed class ListElement
        {
            public int id { get; set; }  // 返还记录id
            public int chip { get; set; }  // 返还USDT数
            public int status { get; set; }  // 返还状态.0-返还失败，1-返还成功
            public long grantTime { get; set; }  // 提取时间戳
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 返还记录 - 【钱包模块】
    /// 
    /// 返还记录
    ///  </summary>
    public partial class WEB2_wallet_other_income_list
    {
        public const string API = @"/api/wallet/other_income/list";

        public sealed class RequestData : RequestDataBase
        {
            public int pageSize { get; set; }  // 分页大小
            public List<idTable> idTables { get; set; }
        }
        public sealed class idTable
        {
            public string id { get; set; }
            public int table { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<OtherIncomeBo> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class OtherIncomeBo
        {
            public string id { get; set; }  // 记录id, 用于分页
            public int table { get; set; }//表的类型, 用于分页
            public int type { get; set; }//收入类型: 1-分享赚USDT, 2-系统赠送, 3-JP击中
            public int chip { get; set; }  // USDT数
            public int status { get; set; }  // 返还状态.0-返还失败
            public long grantTime { get; set; }  // 提取时间戳
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 转USDT - 【钱包模块】
    /// 
    /// 转USDT
    ///  </summary>
    public partial class WEB2_wallet_flow
    {
        public const string API = @"/api/wallet/flow";

        public sealed class RequestData : RequestDataBase
        {
            public string randomId { get; set; }  // 转入豆随机id
            public int chip { get; set; }  // USDT数
            public string payPassword { get; set; }  // 支付密码
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 获取用户收款账户信息 - 【钱包模块】
    /// 
    /// 获取用户收款账户信息
    ///  </summary>
    public partial class WEB2_wallet_account_payee_info
    {
        public const string API = @"/api/wallet/account_payee/info";

        public sealed class RequestData : RequestDataBase
        {
            public int type { get; set; }//请求数据类型。1银行卡 3Usdt地址
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string bankAccName { get; set; }  // 银行卡账户名称
            public string bankName { get; set; }  // 银行名称
            public string bankLocProvince { get; set; }  // 开户行所在省
            public string bankCode { get; set; }  // 银行代号
            public string alipayRealName { get; set; }  // 账户类型:1-银行卡,2-支付宝
            public string bankAccNo { get; set; }  // 银行卡卡号
            public string alipayAcc { get; set; }  // 支付宝账户
            public string bankOfDeposit { get; set; }  // 开户行
            public string bankLocCity { get; set; }  // 开户行所在市
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    /// <summary>
    /// 设置支付密码 - 【钱包模块】
    /// 
    /// 设置支付密码
    ///  </summary>
    public partial class WEB2_wallet_PayPWDSave
    {
        public const string API = @"/api/user/pay_pwd/save";

        public sealed class RequestData : RequestDataBase
        {
            public string code { get; set; }
            public string payPwd { get; set; }
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public bool data { get; set; }  // 业务数据，成功情况才有数据
        }



        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 发送表情后扣USDT
    /// 
    /// 
    ///  </summary>
    public partial class WEB2_expression_use
    {
        public const string API = @"/api/expression/use";

        public sealed class RequestData : RequestDataBase
        {
            public string emojiKey { get; set; }  // 表情Key
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int chip { get; set; }  // 余额
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 配置信息获取接口，暂时用于检测维护状态
    ///  </summary>
    public partial class WEB2_system_config_query
    {
        public const string API = @"/api/system/config/query";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    ///    获取当前用户使用的渠道充值提取配置信息
    ///  </summary>
    public partial class WEB2_recharge_config
    {
        public const string API = @"/api/wallet/recharge/config";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int minRecharge { get; set; }//最小的充值金额
            public int maxRecharge { get; set; }//最大的充值金额
            public int minWithdrawChip { get; set; }//最低的提取数量(USDT)
            public int maxWithdrawChip { get; set; }//最大的提取数量(USDT)
            public List<ProductsData> products { get; set; }//产品配置列表
        }

        public sealed class ProductsData
        {
            public int amount { get; set; }//金额
            public int num { get; set; }//USDT数目
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    /// <summary>
    ///   摇摇乐 参加活动
    ///  </summary>
    public partial class WEB2_activity_kdouApply
    {
        public const string API = @"/api/activity/kdou/apply";

        public sealed class RequestData : RequestDataBase
        {
            public string i { get; set; }//机器码            
            public string g { get; set; }//能否拿到gps权限           0 = 不能拿到gps           1 = 能拿到gps权限
            public string m { get; set; }//手机型号
            public string s { get; set; }//是否模拟器  0不是  1是
        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }
        public sealed class Data
        {
        }
        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    ///   摇摇乐 获取奖励
    ///  </summary>
    public partial class WEB2_activity_kdouGain
    {
        public const string API = @"/api/activity/kdou/gain";

        public sealed class RequestData : RequestDataBase
        {
        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }
        public sealed class Data
        {
        }
        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    ///   参加首充活动
    ///  </summary>
    public partial class WEB2_activity_recharge_apply
    {
        public const string API = @"/api/activity/recharge/apply";

        public sealed class RequestData : RequestDataBase
        {
            public string i { get; set; }//机器码            
            public string g { get; set; }//能否拿到gps权限           0 = 不能拿到gps           1 = 能拿到gps权限
            public string m { get; set; }//手机型号
            public string s { get; set; }//是否模拟器  0不是  1是
        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }
        public sealed class Data
        {
        }
        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    ///   首充活动获取奖励
    ///  </summary>
    public partial class WEB2_activity_recharge_gain
    {
        public const string API = @"/api/activity/recharge/gain";

        public sealed class RequestData : RequestDataBase
        {
        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }
        public sealed class Data
        {
        }
        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_activity_recharge_query
    {
        public const string API = @"/api/activity/recharge/query";

        public sealed class RequestData : RequestDataBase
        {
        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }
        public sealed class Data
        {
            public int num { get; set; }//最小的充值金额
        }
        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    //投诉请求
    public partial class WEB2_complaintRequest
    {
        public const string API = @"/api/complaint/request";

        public sealed class RequestData : RequestDataBase
        {
            public int roomid { get; set; }
            public int type { get; set; }//投诉类型。1-伙牌，2-使用外挂，3-其他            必填
            public string content { get; set; }
            public List<int> userIds { get; set; }
            public int bbChip { get; set; }//盲注级别（BB）必填
        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    //获取投诉手续费
    public partial class WEB2_queryComplaintFee
    {
        public const string API = @"/api/complaint/query_complaint_fee";

        public sealed class RequestData : RequestDataBase
        {
            public int bbChip { get; set; }//盲注级别必填

        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    //任务  领取奖励
    public partial class WEB2_user_task_accepting_an_award
    {
        public const string API = @"/api/user_task/accepting_an_award";

        public sealed class RequestData : RequestDataBase
        {
            public int taskId { get; set; }//任务id

        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }


    public partial class WEB2_user_task_list
    {
        public const string API = @"/api/user_task/list";

        public sealed class RequestData : RequestDataBase
        {
            public int deviceType { get; set; } //设备类型
        }
        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<TaskItem> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class TaskItem
        {
            public List<TaskElement> userTaskItems { get; set; }
            public int status { get; set; }//状态:1-已领取,2-待完成,3-已完成待领取
            public int taskId { get; set; }//任务id
            public int chip { get; set; }//奖励USDT数
            public string taskCode { get; set; }//任务编码(多个任务id可能有同个任务编码,但是任务id都是唯一的,不会重复
            public string checksum { get; set; }//任务校验码,用于app外执行的任务,例如唤起浏览器保存网址,应在网页端保存网址触发请求完成任务接口时,把此作为其中的参数传给后台校验
        }

        public sealed class TaskElement
        {
            public int id { get; set; }//子任务项id
            public int action { get; set; }//任务项行动标识:与前端协调,定义不同的行动标识,未完成任务根据不同的标识做出不同的前往行为 1-修改名称,2-修改头像,3-保存官网
            public int finishTimes { get; set; }//完成次数
            public int reqTimes { get; set; }//要求完成次数
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    //任务  APP内完成任务接口
    public partial class WEB2_user_task_finish_task_app
    {
        public const string API = @"/api/user_task/finish_task/app";

        public sealed class RequestData : RequestDataBase
        {
            public int action { get; set; }//动作:3-保存官网
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    //跑马灯奖池
    public partial class WEB2_raffle_query
    {
        public const string API = @"/api/raffle/query";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    // 查询跑马灯开关状态 0关闭 1开启
    public partial class WEB2_raffle_status_query
    {
        public const string API = @"/api/raffle/status/query";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public int data { get; set; }  // 业务数据，成功情况才有数据 0关闭 1开启
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_GetBeanList
    {
        public const string API = @"/api/wallet/bank/list";

        public sealed class RequestData : RequestDataBase
        {
            public int type { get; set; }//类型1银行列表3数字货币列表
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<listData> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class listData
        {
            public string bankCode { get; set; }
            public string bankName { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    public partial class WEB2_raffle_user_prize_query
    {
        public const string API = @"/api/raffle/user/prize/query";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<RewardData> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class RewardData
        {
            public string nickName { get; set; }
            public string manzhu { get; set; }
            public int kdou { get; set; }
            public string date { get; set; }
            public string headImg { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt比赛盲注信息
    /// </summary>
    // * 0 : 操作成功
    // * 101 ：参数必须指定！
    // * 102 ：参数长度超出允许范围！
    // * 103 ：参数值未满足最小值约束！
    // * 104 ：参数值无效！
    // * 199 ：未知参数错误！
    // * 2001 : 牌局不存在或已解散
    // * 2004 : 账号已被冻结
    // * 2005 : 玩家未加入俱乐部
    // * 2006 : 玩家所属俱乐部已被关闭
    // * 2007 : 玩家所属俱乐部未绑定联盟
    // * 2008 : 玩家所属俱乐部转移中或已被踢出
    // * 2010 : Mtt比赛不存在
    public partial class WEB2_room_mtt_blind_info
    {
        public const string API = @"/api/room/mtt/blind-info";

        public sealed class RequestData : RequestDataBase
        {
            public int page { get; set; }  // 页码
            public int rmid { get; set; }  
            public int rpath { get; set; }  // 71
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public List<BlindListElement> blindList { get; set; }  // mtt列表
            public bool canAppend { get; set; }  
            public bool canRebuy { get; set; }  
            public string clientIp { get; set; }  
            public int currentBlindLevel { get; set; }  
            public int gameStatus { get; set; }  
            public int matchId { get; set; }  
            public string matchName { get; set; }  
            public int maxLevel { get; set; }  
            public string mttIp { get; set; }  
            public string mttPort { get; set; }  
        }

        public sealed class BlindListElement
        {
            public int ante { get; set; }  
            public int level { get; set; }  
            public int sb { get; set; }  
            public long time { get; set; }  
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt比赛倒计时
    /// </summary>
    // * 0 : 操作成功
    // * 101 ：参数必须指定！
    // * 102 ：参数长度超出允许范围！
    // * 103 ：参数值未满足最小值约束！
    // * 104 ：参数值无效！
    // * 199 ：未知参数错误！
    // * 2001 : 牌局不存在或已解散
    // * 2004 : 账号已被冻结
    // * 2005 : 玩家未加入俱乐部
    // * 2006 : 玩家所属俱乐部已被关闭
    // * 2007 : 玩家所属俱乐部未绑定联盟
    // * 2008 : 玩家所属俱乐部转移中或已被踢出
    // * 2010 : Mtt比赛不存在
    public partial class WEB2_room_mtt_countdown
    {
        public const string API = @"/api/room/mtt/countdown";

        public sealed class RequestData : RequestDataBase
        {
            public int rmid { get; set; }  
            public int rpath { get; set; }  // 71
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int countDownTime { get; set; }  // 秒
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt比赛牌桌信息
    /// </summary>
    // * 0 : 操作成功
    // * 101 ：参数必须指定！
    // * 102 ：参数长度超出允许范围！
    // * 103 ：参数值未满足最小值约束！
    // * 104 ：参数值无效！
    // * 199 ：未知参数错误！
    // * 2001 : 牌局不存在或已解散
    // * 2004 : 账号已被冻结
    // * 2005 : 玩家未加入俱乐部
    // * 2006 : 玩家所属俱乐部已被关闭
    // * 2007 : 玩家所属俱乐部未绑定联盟
    // * 2008 : 玩家所属俱乐部转移中或已被踢出
    // * 2010 : Mtt比赛不存在
    public partial class WEB2_room_mtt_desk_info
    {
        public const string API = @"/api/room/mtt/desk-info";

        public sealed class RequestData : RequestDataBase
        {
            public int page { get; set; }
            public int rmid { get; set; }
            public int rpath { get; set; }  // 71
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public int totalDeskNum { get; set; }  //总牌桌数
            public string clientIp { get; set; }  //客户端ip
            public List<DeskListElement> deskList { get; set; }  
        }

        public sealed class DeskListElement
        {
            public int deskNum { get; set; }  //桌号
            public int playerCount { get; set; }  //已坐人数
            public int minScore { get; set; }  //最小记分牌
            public int maxScore { get; set; }  //最大记分牌
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt进入牌局
    /// status 业务操作状态码
    //* 0 : 操作成功
    //* 101 ：参数必须指定！
    //* 102 ：参数长度超出允许范围！
    //* 103 ：参数值未满足最小值约束！
    //* 104 ：参数值无效！
    //* 199 ：未知参数错误！
    //* 2001 : 牌局不存在或已解散
    //* 2004 : 账号已被冻结
    //* 2005 : 玩家未加入俱乐部
    //* 2006 : 玩家所属俱乐部已被关闭
    //* 2007 : 玩家所属俱乐部未绑定联盟
    //* 2008 : 玩家所属俱乐部转移中或已被踢出
    ///  </summary>
    public partial class WEB2_room_mtt_enter  // MTT进房间
    {
        public const string API = @"/api/room/mtt/enter";

        public sealed class RequestData : RequestDataBase
        {
            public int rpath { get; set; }  // 71
            public int rmid { get; set; }  // 比赛id，如337195
        }

        public sealed class ResponseData
        {
            public string msg { get; set; }
            public Data data { get; set; }  // Data
            public int status { get; set; }  // 同普通局
        }

        public sealed class Data
        {
            public int roomPath { get; set; }  // 71
            public int mttType { get; set; }  // 6:6人桌，9：9人桌
            public int matchId { get; set; }  // 比赛id，如337195
            public string matchIp { get; set; }  // 房间服务器的域名或ip，如tsk.wdjjsc.biz
            public int matchPort { get; set; }  // 房间服务器端口，如8055
            public int gameStatus { get; set; }  // mtt游戏状态，未使用
            public string matchName { get; set; }  // 比赛名称
            public string clientIp { get; set; }  // 客户端Ip
            public List<int> blindNote { get; set; }  // 大小盲列表
            public List<int> ante { get; set; }  // 前注列表
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt比赛玩家列表
    /// </summary>
    // * 0 : 操作成功
    // * 101 ：参数必须指定！
    // * 102 ：参数长度超出允许范围！
    // * 103 ：参数值未满足最小值约束！
    // * 104 ：参数值无效！
    // * 199 ：未知参数错误！
    // * 2001 : 牌局不存在或已解散
    // * 2004 : 账号已被冻结
    // * 2005 : 玩家未加入俱乐部
    // * 2006 : 玩家所属俱乐部已被关闭
    // * 2007 : 玩家所属俱乐部未绑定联盟
    // * 2008 : 玩家所属俱乐部转移中或已被踢出
    // * 2010 : Mtt比赛不存在
    public partial class WEB2_room_mtt_player_list
    {
        public const string API = @"/api/room/mtt/player-list";

        public sealed class RequestData : RequestDataBase
        {
            public int page { get; set; }  
            public int rmid { get; set; }  // 比赛id，如337195
            public int rpath { get; set; }  // 71
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  
            public string msg { get; set; }
            public Data data { get; set; }  
            
        }

        public sealed class Data
        {
            public List<PlayerListElement> playerList { get; set; }
            public int totalPlayerNum { get; set; }  
        }

        public sealed class PlayerListElement
        {
            public string head { get; set; } 
            public string nickName { get; set; }
            public int score { get; set; }
            public int userId { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt比赛奖励详情
    /// </summary>
    // * 0 : 操作成功
    // * 101 ：参数必须指定！
    // * 102 ：参数长度超出允许范围！
    // * 103 ：参数值未满足最小值约束！
    // * 104 ：参数值无效！
    // * 199 ：未知参数错误！
    // * 2001 : 牌局不存在或已解散
    // * 2004 : 账号已被冻结
    // * 2005 : 玩家未加入俱乐部
    // * 2006 : 玩家所属俱乐部已被关闭
    // * 2007 : 玩家所属俱乐部未绑定联盟
    // * 2008 : 玩家所属俱乐部转移中或已被踢出
    // * 2010 : Mtt比赛不存在
    public partial class WEB2_room_mtt_reward_info
    {
        public const string API = @"/api/room/mtt/reward-info";

        public sealed class RequestData : RequestDataBase
        {
            public int page { get; set; }
            public int rmid { get; set; }  // 比赛id，如337195
            public int rpath { get; set; }  // 71
        }

        public sealed class ResponseData
        {
            public int status { get; set; }
            public string msg { get; set; }
            public Data data { get; set; }
        }

        public sealed class Data
        {
            public int bonusType { get; set; }
            public string clientIp { get; set; }
            public List<CurrMttRewardListElement> currMttRewardList { get; set; }
            public int gameStatus { get; set; }
            public int initialPool { get; set; }
            public string mttIp { get; set; }
            public string mttPort { get; set; }
            public string nextReward { get; set; }
            public int nextRewardNum { get; set; }
            public int rewardNumber { get; set; }
            public int totalChips { get; set; }
            public int totalPlayerNum { get; set; }
        }

        public sealed class CurrMttRewardListElement
        {
            public string bonus { get; set; }
            public double percent { get; set; }
            public int rank { get; set; }
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt比赛详情
    /// </summary>
    // * 0 : 操作成功
    // * 101 ：参数必须指定！
    // * 102 ：参数长度超出允许范围！
    // * 103 ：参数值未满足最小值约束！
    // * 104 ：参数值无效！
    // * 199 ：未知参数错误！
    // * 2001 : 牌局不存在或已解散
    // * 2004 : 账号已被冻结
    // * 2005 : 玩家未加入俱乐部
    // * 2006 : 玩家所属俱乐部已被关闭
    // * 2007 : 玩家所属俱乐部未绑定联盟
    // * 2008 : 玩家所属俱乐部转移中或已被踢出
    // * 2010 : Mtt比赛不存在
    public partial class WEB2_room_mtt_view 
    {
        public const string API = @"/api/room/mtt/view";

        public sealed class RequestData : RequestDataBase
        {
            public int rmid { get; set; }  // 比赛id，如228630
            public int rpath { get; set; }  // 71
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 0
            public string msg { get; set; }  // success
            public Data data { get; set; }  // Data
        }

        public sealed class Data
        {
            public int activePlayers { get; set; }  // 剩余玩家数，未使用
            public int advancedEntry { get; set; }  // 未使用
            public int alreadyRebuy { get; set; }  // 本玩家已重购次数
            public double averageScore { get; set; }  // 计分牌平均值，如8000
            public double blindScale { get; set; }  // 盲注表倍数
            public int blindType { get; set; }  // 盲注表类型 0普通 1快速
            public bool canAppend { get; set; }  // 是否允许增购，未使用
            public bool canDelay { get; set; }  // 是否允许延迟报名
            public bool canPlayerRebuy { get; set; }  // 下方按钮显示“重购”
            public bool canRebuy { get; set; }  // 比赛是否允许重购，0 1
            public string clientIp { get; set; }  // 客户端的ip地址，如113.111.4.137
            public int currAnte { get; set; }  // 当前前注
            public int currBlindLevel { get; set; }  // 当前盲注级别
            public int currentSb { get; set; }  // 当前小盲,未使用
            public int entryTime { get; set; }  // 开放报名时间,未使用
            public int gameStatus { get; set; }  // 0:可报名;1:等待开赛;2:延期报名;3:进行中;4:立即进入;5:报名截止;6:等待审批;7:已淘汰，且不能重购。
            public int gameType { get; set; }  // 比赛类型：1 欢乐赛，2 特别赛
            public bool hasGpsLimit { get; set; }  // 是否开启gps限制
            public bool hasIpLimit { get; set; }  // 是否开启ip限制
            public int initialChip { get; set; }  // 起始USDT
            public string initialPool { get; set; }  // 固定奖池,未使用
            public int initialScore { get; set; }  // 起始计分牌，如8000
            public bool isHunter { get; set; }  // 是否猎人赛
            public bool isStart { get; set; }  // 比赛是否已开始
            public int leftBlindTime { get; set; }  // 涨盲剩余时间（s）
            public int leftPlayer { get; set; }  // 剩余玩家数
            public int lowerLimit { get; set; }  // 最低报名人数
            public int matchId { get; set; }  // 比赛id，如228630
            public int maxDelayLevel { get; set; }  // 最大可延迟报名级别
            public int maxRebuyLevel { get; set; }  // 最大可重购级别，如12
            public string mttIp { get; set; }
            public string mttPort { get; set; }
            public int mttType { get; set; }  // 6:6人桌，9:9人桌
            public string name { get; set; }  // 比赛名称
            public int participants { get; set; }  // 当前报名人数，如2
            public int rebuyTimes { get; set; }  // 可重购次数上限
            public int registerFee { get; set; }  // 报名费
            public int runningTime { get; set; }  // 比赛已进行时间
            public int serviceFee { get; set; }  // 服务费，如10
            public int showTime { get; set; }  // 未使用
            public int startTime { get; set; }  // 比赛开始时间，如1548666161
            public string totalBonus { get; set; }  // 总奖池筹码数,未使用
            public int totalChips { get; set; }  // 总奖池，如100
            public int totalEntryFees { get; set; }  // 未使用
            public int totalRebuyFees { get; set; }  // 重购费用
            public int updateCycle { get; set; }  // 升盲时间（分钟），如2
            public int upperLimit { get; set; }  // 报名人数上限
            public List<int> blindNote { get; set; }  // 大小盲列表
            public List<int> ante { get; set; }  // 前注列表

        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// mtt比赛列表
    ///  </summary>
    public partial class WEB2_room_mtt_list
    {
        public const string API = @"/api/room/mtt/list";

        public sealed class RequestData : RequestDataBase
        {
            public int matchType { get; set; }  // 比赛类型 1 = 积分赛 2 = 实物赛
            public int pageNum { get; set; }  // 页码
            public int pageSize { get; set; }  // 页大小
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
            public string clientIp { get; set; }  // 客户端IP
            public List<RoomListElement> roomList { get; set; }  // mtt列表
        }

        public sealed class RoomListElement
        {
            public int currEntryNum { get; set; }  // 当前报名人数
            public int entryFee { get; set; }  // 报名费
            public int gameStatus { get; set; }  // 游戏状态 0 = 可报名 1 = 等待开赛 2 = 延迟报名 3 = 进行中 4 = 立即进入 5 = 报名截止 6 = 等待审批 7 = 重购条件不足
            public string logoUrl { get; set; }  // logo地址
            public int matchId { get; set; }  // 比赛id
            public string matchName { get; set; }  // 比赛名称
            public long startTime { get; set; }  // 开赛时间
            public int upperLimit { get; set; }  // 参赛上限
            public int voucher { get; set; }  // 报名券id
            public bool hasGpsLimit { get; set; }  // GPS是否开启
            public bool canPlayerRebuy { get; set; }  // 是否显示重购按钮
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    /// <summary>
    /// 查询自动开房列表
    /// 
    /// status 业务操作状态码
    ///   * 0    : 操作成功
    ///   * 2002 : 权限不足!
    ///  </summary>
    public partial class WEB2_roomplan_list
    {
        public const string API = @"/api/roomplan/list";

        public sealed class RequestData : RequestDataBase
        {
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public List<DataElement> data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class DataElement
        {
            public string creatorName { get; set; }  // 房主姓名
            public string endTime { get; set; }  // 结束时间
            public int id { get; set; }  // 计划id
            public string name { get; set; }  // 计划名称
            public int playerCount { get; set; }  // 游戏人数
            public int roomPath { get; set; }  // 游戏类型 61 = 德州 62 = 德州-必下场 63 = 德州-AOF 91 = 奥马哈 92 = 奥马哈-必下场 93 = 奥马哈-AOF
            public int sbChip { get; set; }  // 小盲
            public int qianzhu { get; set; }  // 前注
            public string startTime { get; set; }  // 开始时间
            public int status { get; set; }  // 计划状态。0-不开启，1-开启，2-结束
            public int omamod { get; set; }  // 游戏小标签-omaha模式:0 = 普通模式 , 1 = 血战模式
            public int isron { get; set; }  // 游戏小标签-保险模式:0 = 关闭 , 1 = 开启
            public int jpon { get; set; }  // 游戏小标签-jackpot模式:0 = 关闭 , 1 = 开启
            public int self { get; set; }  // 是否自己创建的计划 0 1
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

    //启动/暂停/结束计划
    public partial class WEB2_roomplan_operate
    {
        public const string API = @"/api/roomplan/operate";

        public sealed class RequestData : RequestDataBase
        {
            public int id { get; set; }//计划id
            public int status { get; set; }//计划状态。0-不开启，1-开启，2-结束
        }

        public sealed class ResponseData
        {
            public int status { get; set; }  // 执行状态码：成功(0)
            public string msg { get; set; }  // 错误描述,只做错误描述，不建议直接用来提示终端用户
            public Data data { get; set; }  // 业务数据，成功情况才有数据
        }

        public sealed class Data
        {
        }

        public static string Request(RequestData data)
        {
            return JsonHelper.ToJson(data);
        }

        public static ResponseData Response(string json)
        {
            return JsonHelper.FromJson<ResponseData>(json);
        }
    }

}