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
    unsafe class ETModel_NetLineSwitchComponent_Binding_NetLineSwitchData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.NetLineSwitchComponent.NetLineSwitchData);

            field = type.GetField("selector", flag);
            app.RegisterCLRFieldGetter(field, get_selector_0);
            app.RegisterCLRFieldSetter(field, set_selector_0);
            field = type.GetField("list", flag);
            app.RegisterCLRFieldGetter(field, get_list_1);
            app.RegisterCLRFieldSetter(field, set_list_1);


        }



        static object get_selector_0(ref object o)
        {
            return ((ETModel.NetLineSwitchComponent.NetLineSwitchData)o).selector;
        }
        static void set_selector_0(ref object o, object v)
        {
            ((ETModel.NetLineSwitchComponent.NetLineSwitchData)o).selector = (System.Int32)v;
        }
        static object get_list_1(ref object o)
        {
            return ((ETModel.NetLineSwitchComponent.NetLineSwitchData)o).list;
        }
        static void set_list_1(ref object o, object v)
        {
            ((ETModel.NetLineSwitchComponent.NetLineSwitchData)o).list = (System.Collections.Generic.List<ETModel.NetLineSwitchComponent.LineInfo>)v;
        }


    }
}
