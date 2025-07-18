//
//  XNUtilityHelper.h
//  XNChatCore
//
//  Created by Ntalker on 15/8/20.
//  Copyright (c) 2015年 Kevin. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface XNUtilityHelper : NSObject

//获取cid
+ (NSString *)cidFromLocalStore;
///1970据当前的时间(精确到毫秒)
+ (NSString *)getNowTimeInterval;
///1970据当前的时间(精确到毫秒)（校验之后的时间与服务器保持一致）
+ (NSString *)getNowTimeIntervalWithSetingId:(NSString *)setingId;
///获取消息ID当前时间的毫秒值
+ (NSString *)getMessageId;
//get请求时拼接完整的URL
+ (NSString *)URLStringByURLBody:(NSString *)businessName andParam:(NSMutableDictionary *)paramDic;

//模型转换成容器
+ (id)dictOrArrayWithModel:(id)dataModel;

//判断某个id下是否需要开启未读消息通知
+ (BOOL)needOpenUnreadmsgByUserid:(NSString *)uid;

//判断对应的siteID是否需要getflashserver请求
+ (BOOL)needGetflashserverBySited:(NSString *)sited;

//获取设备类型
+ (NSString *)deviceModel;

//长id变短id
+ (NSString *)shortUidFromUid:(NSString *)uid;

//
+ (NSString *)siteidFromUid:(NSString *)uid;

//
+ (NSString *)siteidfromSettingid:(NSString *)settingid;

+ (NSString *)md5:(NSString *)originStr;

+ (NSString *)stringFromGBData:(NSData *)data;

+ (NSString *)randomString;

+ (NSString *)gzipInflate:(NSData *)compressStr;

+ (BOOL)isKefuUserid:(NSString *)uid;

+ (BOOL)isVisitUserid:(NSString *)uid;

+ (NSString *)getFlashserverAddress;

+ (CGFloat)backConnectTime;

+ (NSString *) getFormatTimeString:(NSString *)timeStr;

+ (UIImage *)imageFromImage:(UIImage *)image inRect:(CGRect)rect ;

+ (void)compressVideo:(NSURL *)originVideoURL completeHandle:(void(^)(NSURL *videoURL))completeHandle;

+ (NSString *)getConfigFile:(NSString *)fileName;

+ (NSString *)stringByXNEncodeFromString:(NSString *)oldString;

+ (NSString *)stringByXNDecodeFromString:(NSString *)oldString;

/**
 获取设备唯一标示(生成之后不会改变，除非强制刷机)，替代IMEI码使用
 
 @return UUID
 */
+ (NSString *)getUUID;

@end
