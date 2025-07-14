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
    unsafe class ETModel_NetLineSwitchComponent_Binding_LineInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.NetLineSwitchComponent.LineInfo);

            field = type.GetField("id", flag);
            app.RegisterCLRFieldGetter(field, get_id_0);
            app.RegisterCLRFieldSetter(field, set_id_0);
            field = type.GetField("http", flag);
            app.RegisterCLRFieldGetter(field, get_http_1);
            app.RegisterCLRFieldSetter(field, set_http_1);
            field = type.GetField("sck", flag);
            app.RegisterCLRFieldGetter(field, get_sck_2);
            app.RegisterCLRFieldSetter(field, set_sck_2);


        }



        static object get_id_0(ref object o)
        {
            return ((ETModel.NetLineSwitchComponent.LineInfo)o).id;
        }
        static void set_id_0(ref object o, object v)
        {
            ((ETModel.NetLineSwitchComponent.LineInfo)o).id = (System.Int32)v;
        }
        static object get_http_1(ref object o)
        {
            return ((ETModel.NetLineSwitchComponent.LineInfo)o).http;
        }
        static void set_http_1(ref object o, object v)
        {
            ((ETModel.NetLineSwitchComponent.LineInfo)o).http = (System.String)v;
        }
        static object get_sck_2(ref object o)
        {
            return ((ETModel.NetLineSwitchComponent.LineInfo)o).sck;
        }
        static void set_sck_2(ref object o, object v)
        {
            ((ETModel.NetLineSwitchComponent.LineInfo)o).sck = (System.String)v;
        }


    }
}
