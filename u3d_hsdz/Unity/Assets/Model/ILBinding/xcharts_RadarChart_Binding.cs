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
    unsafe class xcharts_RadarChart_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(xcharts.RadarChart);
            args = new Type[]{typeof(xcharts.RadarData[])};
            method = type.GetMethod("SetRadarData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetRadarData_0);
            args = new Type[]{typeof(xcharts.RadarData[]), typeof(System.Boolean)};
            method = type.GetMethod("SetRadarData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetRadarData_1);


        }


        static StackObject* SetRadarData_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            xcharts.RadarData[] @datas = (xcharts.RadarData[])typeof(xcharts.RadarData[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            xcharts.RadarChart instance_of_this_method = (xcharts.RadarChart)typeof(xcharts.RadarChart).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetRadarData(@datas);

            return __ret;
        }

        static StackObject* SetRadarData_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @pIsNameValue = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            xcharts.RadarData[] @datas = (xcharts.RadarData[])typeof(xcharts.RadarData[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            xcharts.RadarChart instance_of_this_method = (xcharts.RadarChart)typeof(xcharts.RadarChart).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetRadarData(@datas, @pIsNameValue);

            return __ret;
        }



    }
}
