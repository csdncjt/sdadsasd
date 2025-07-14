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
    unsafe class ETModel_InstallPacketConfig_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.InstallPacketConfig);

            field = type.GetField("IOSUrl", flag);
            app.RegisterCLRFieldGetter(field, get_IOSUrl_0);
            app.RegisterCLRFieldSetter(field, set_IOSUrl_0);
            field = type.GetField("IsAppStore", flag);
            app.RegisterCLRFieldGetter(field, get_IsAppStore_1);
            app.RegisterCLRFieldSetter(field, set_IsAppStore_1);


        }



        static object get_IOSUrl_0(ref object o)
        {
            return ((ETModel.InstallPacketConfig)o).IOSUrl;
        }
        static void set_IOSUrl_0(ref object o, object v)
        {
            ((ETModel.InstallPacketConfig)o).IOSUrl = (System.String)v;
        }
        static object get_IsAppStore_1(ref object o)
        {
            return ((ETModel.InstallPacketConfig)o).IsAppStore;
        }
        static void set_IsAppStore_1(ref object o, object v)
        {
            ((ETModel.InstallPacketConfig)o).IsAppStore = (System.Boolean)v;
        }


    }
}
