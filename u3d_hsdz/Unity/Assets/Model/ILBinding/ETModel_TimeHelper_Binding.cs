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
    unsafe class ETModel_TimeHelper_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(ETModel.TimeHelper);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ShowRemainingSemicolonPure", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowRemainingSemicolonPure_0);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("ShowRemainingSemicolon", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowRemainingSemicolon_1);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("GetDateTimer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetDateTimer_2);
            args = new Type[]{};
            method = type.GetMethod("GetTimestamp", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetTimestamp_3);
            args = new Type[]{};
            method = type.GetMethod("ClientNow", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClientNow_4);
            args = new Type[]{};
            method = type.GetMethod("GetCodeTime", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetCodeTime_5);
            args = new Type[]{typeof(System.Int64), typeof(System.Boolean)};
            method = type.GetMethod("ShowRemainingTimer", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowRemainingTimer_6);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("TimerDateStr", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TimerDateStr_7);
            args = new Type[]{typeof(System.Int64)};
            method = type.GetMethod("TimerDateMinStr", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TimerDateMinStr_8);


        }


        static StackObject* ShowRemainingSemicolonPure_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @pNum = ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeHelper.ShowRemainingSemicolonPure(@pNum);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ShowRemainingSemicolon_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @pNum = ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeHelper.ShowRemainingSemicolon(@pNum);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetDateTimer_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @pNum = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeHelper.GetDateTimer(@pNum);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* GetTimestamp_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ETModel.TimeHelper.GetTimestamp();

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ClientNow_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ETModel.TimeHelper.ClientNow();

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetCodeTime_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = ETModel.TimeHelper.GetCodeTime();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ShowRemainingTimer_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @pSec = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int64 @pNum = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeHelper.ShowRemainingTimer(@pNum, @pSec);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* TimerDateStr_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @pNum = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeHelper.TimerDateStr(@pNum);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* TimerDateMinStr_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @pNum = *(long*)&ptr_of_this_method->Value;


            var result_of_this_method = ETModel.TimeHelper.TimerDateMinStr(@pNum);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }



    }
}
