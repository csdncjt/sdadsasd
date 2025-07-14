using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class ETModel_Hotfix_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.Hotfix);
            args = new Type[]{};
            method = type.GetMethod("GetHotfixTypes", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetHotfixTypes_0);

            field = type.GetField("OnGetNetLineSwith", flag);
            app.RegisterCLRFieldGetter(field, get_OnGetNetLineSwith_0);
            app.RegisterCLRFieldSetter(field, set_OnGetNetLineSwith_0);
            field = type.GetField("OnApplicationPauseTrue", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationPauseTrue_1);
            app.RegisterCLRFieldSetter(field, set_OnApplicationPauseTrue_1);
            field = type.GetField("OnApplicationPauseFalse", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationPauseFalse_2);
            app.RegisterCLRFieldSetter(field, set_OnApplicationPauseFalse_2);
            field = type.GetField("OnEmulatorInfo", flag);
            app.RegisterCLRFieldGetter(field, get_OnEmulatorInfo_3);
            app.RegisterCLRFieldSetter(field, set_OnEmulatorInfo_3);
            field = type.GetField("Update", flag);
            app.RegisterCLRFieldGetter(field, get_Update_4);
            app.RegisterCLRFieldSetter(field, set_Update_4);
            field = type.GetField("LateUpdate", flag);
            app.RegisterCLRFieldGetter(field, get_LateUpdate_5);
            app.RegisterCLRFieldSetter(field, set_LateUpdate_5);
            field = type.GetField("OnApplicationQuit", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationQuit_6);
            app.RegisterCLRFieldSetter(field, set_OnApplicationQuit_6);
            field = type.GetField("OnApplicationFocusTrue", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationFocusTrue_7);
            app.RegisterCLRFieldSetter(field, set_OnApplicationFocusTrue_7);
            field = type.GetField("OnApplicationFocusFalse", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationFocusFalse_8);
            app.RegisterCLRFieldSetter(field, set_OnApplicationFocusFalse_8);
            field = type.GetField("OnGetHeadImagePath", flag);
            app.RegisterCLRFieldGetter(field, get_OnGetHeadImagePath_9);
            app.RegisterCLRFieldSetter(field, set_OnGetHeadImagePath_9);
            field = type.GetField("OnGetGPS", flag);
            app.RegisterCLRFieldGetter(field, get_OnGetGPS_10);
            app.RegisterCLRFieldSetter(field, set_OnGetGPS_10);
            field = type.GetField("OnAwakeByURL", flag);
            app.RegisterCLRFieldGetter(field, get_OnAwakeByURL_11);
            app.RegisterCLRFieldSetter(field, set_OnAwakeByURL_11);
            field = type.GetField("OnOpearteMes", flag);
            app.RegisterCLRFieldGetter(field, get_OnOpearteMes_12);
            app.RegisterCLRFieldSetter(field, set_OnOpearteMes_12);
            field = type.GetField("OnServerMes", flag);
            app.RegisterCLRFieldGetter(field, get_OnServerMes_13);
            app.RegisterCLRFieldSetter(field, set_OnServerMes_13);
            field = type.GetField("OnGroupMes", flag);
            app.RegisterCLRFieldGetter(field, get_OnGroupMes_14);
            app.RegisterCLRFieldSetter(field, set_OnGroupMes_14);
            field = type.GetField("OnGetScreenShot", flag);
            app.RegisterCLRFieldGetter(field, get_OnGetScreenShot_15);
            app.RegisterCLRFieldSetter(field, set_OnGetScreenShot_15);
            field = type.GetField("OnInAppPurchaseCallBack", flag);
            app.RegisterCLRFieldGetter(field, get_OnInAppPurchaseCallBack_16);
            app.RegisterCLRFieldSetter(field, set_OnInAppPurchaseCallBack_16);


        }


        static StackObject* GetHotfixTypes_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ETModel.Hotfix instance_of_this_method = (ETModel.Hotfix)typeof(ETModel.Hotfix).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetHotfixTypes();

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_OnGetNetLineSwith_0(ref object o)
        {
            return ((ETModel.Hotfix)o).OnGetNetLineSwith;
        }
        static void set_OnGetNetLineSwith_0(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnGetNetLineSwith = (System.Action<System.String>)v;
        }
        static object get_OnApplicationPauseTrue_1(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationPauseTrue;
        }
        static void set_OnApplicationPauseTrue_1(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationPauseTrue = (System.Action)v;
        }
        static object get_OnApplicationPauseFalse_2(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationPauseFalse;
        }
        static void set_OnApplicationPauseFalse_2(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationPauseFalse = (System.Action)v;
        }
        static object get_OnEmulatorInfo_3(ref object o)
        {
            return ((ETModel.Hotfix)o).OnEmulatorInfo;
        }
        static void set_OnEmulatorInfo_3(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnEmulatorInfo = (System.Action<System.String>)v;
        }
        static object get_Update_4(ref object o)
        {
            return ((ETModel.Hotfix)o).Update;
        }
        static void set_Update_4(ref object o, object v)
        {
            ((ETModel.Hotfix)o).Update = (System.Action)v;
        }
        static object get_LateUpdate_5(ref object o)
        {
            return ((ETModel.Hotfix)o).LateUpdate;
        }
        static void set_LateUpdate_5(ref object o, object v)
        {
            ((ETModel.Hotfix)o).LateUpdate = (System.Action)v;
        }
        static object get_OnApplicationQuit_6(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationQuit;
        }
        static void set_OnApplicationQuit_6(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationQuit = (System.Action)v;
        }
        static object get_OnApplicationFocusTrue_7(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationFocusTrue;
        }
        static void set_OnApplicationFocusTrue_7(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationFocusTrue = (System.Action)v;
        }
        static object get_OnApplicationFocusFalse_8(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationFocusFalse;
        }
        static void set_OnApplicationFocusFalse_8(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationFocusFalse = (System.Action)v;
        }
        static object get_OnGetHeadImagePath_9(ref object o)
        {
            return ((ETModel.Hotfix)o).OnGetHeadImagePath;
        }
        static void set_OnGetHeadImagePath_9(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnGetHeadImagePath = (System.Action<System.String>)v;
        }
        static object get_OnGetGPS_10(ref object o)
        {
            return ((ETModel.Hotfix)o).OnGetGPS;
        }
        static void set_OnGetGPS_10(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnGetGPS = (System.Action<System.String>)v;
        }
        static object get_OnAwakeByURL_11(ref object o)
        {
            return ((ETModel.Hotfix)o).OnAwakeByURL;
        }
        static void set_OnAwakeByURL_11(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnAwakeByURL = (System.Action<System.String>)v;
        }
        static object get_OnOpearteMes_12(ref object o)
        {
            return ((ETModel.Hotfix)o).OnOpearteMes;
        }
        static void set_OnOpearteMes_12(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnOpearteMes = (System.Action<System.String>)v;
        }
        static object get_OnServerMes_13(ref object o)
        {
            return ((ETModel.Hotfix)o).OnServerMes;
        }
        static void set_OnServerMes_13(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnServerMes = (ETModel.Hotfix.IMDelegate)v;
        }
        static object get_OnGroupMes_14(ref object o)
        {
            return ((ETModel.Hotfix)o).OnGroupMes;
        }
        static void set_OnGroupMes_14(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnGroupMes = (System.Action<System.String>)v;
        }
        static object get_OnGetScreenShot_15(ref object o)
        {
            return ((ETModel.Hotfix)o).OnGetScreenShot;
        }
        static void set_OnGetScreenShot_15(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnGetScreenShot = (System.Action<UnityEngine.Texture2D>)v;
        }
        static object get_OnInAppPurchaseCallBack_16(ref object o)
        {
            return ((ETModel.Hotfix)o).OnInAppPurchaseCallBack;
        }
        static void set_OnInAppPurchaseCallBack_16(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnInAppPurchaseCallBack = (System.Action<System.String>)v;
        }


    }
}
