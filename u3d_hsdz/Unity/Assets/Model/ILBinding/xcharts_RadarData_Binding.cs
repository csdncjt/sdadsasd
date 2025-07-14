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
    unsafe class xcharts_RadarData_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(xcharts.RadarData);

            field = type.GetField("name", flag);
            app.RegisterCLRFieldGetter(field, get_name_0);
            app.RegisterCLRFieldSetter(field, set_name_0);
            field = type.GetField("value", flag);
            app.RegisterCLRFieldGetter(field, get_value_1);
            app.RegisterCLRFieldSetter(field, set_value_1);

            app.RegisterCLRCreateArrayInstance(type, s => new xcharts.RadarData[s]);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_name_0(ref object o)
        {
            return ((xcharts.RadarData)o).name;
        }
        static void set_name_0(ref object o, object v)
        {
            ((xcharts.RadarData)o).name = (System.String)v;
        }
        static object get_value_1(ref object o)
        {
            return ((xcharts.RadarData)o).value;
        }
        static void set_value_1(ref object o, object v)
        {
            ((xcharts.RadarData)o).value = (System.Single)v;
        }

        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new xcharts.RadarData();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
