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
    unsafe class JPush_JPushBinding_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(JPush.JPushBinding);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("Init", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Init_0);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("SetDebug", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetDebug_1);
            args = new Type[]{typeof(System.Int32), typeof(System.Collections.Generic.List<System.String>)};
            method = type.GetMethod("SetTags", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetTags_2);
            args = new Type[]{};
            method = type.GetMethod("GetRegistrationId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetRegistrationId_3);
            args = new Type[]{typeof(System.Int32), typeof(System.Collections.Generic.List<System.String>)};
            method = type.GetMethod("AddTags", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddTags_4);
            args = new Type[]{typeof(System.Int32), typeof(System.Collections.Generic.List<System.String>)};
            method = type.GetMethod("DeleteTags", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DeleteTags_5);
            args = new Type[]{typeof(System.Int32), typeof(System.String)};
            method = type.GetMethod("SetAlias", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetAlias_6);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("DeleteAlias", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, DeleteAlias_7);


        }


        static StackObject* Init_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @gameObject = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            JPush.JPushBinding.Init(@gameObject);

            return __ret;
        }

        static StackObject* SetDebug_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @enable = ptr_of_this_method->Value == 1;


            JPush.JPushBinding.SetDebug(@enable);

            return __ret;
        }

        static StackObject* SetTags_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<System.String> @tags = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @sequence = ptr_of_this_method->Value;


            JPush.JPushBinding.SetTags(@sequence, @tags);

            return __ret;
        }

        static StackObject* GetRegistrationId_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = JPush.JPushBinding.GetRegistrationId();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* AddTags_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<System.String> @tags = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @sequence = ptr_of_this_method->Value;


            JPush.JPushBinding.AddTags(@sequence, @tags);

            return __ret;
        }

        static StackObject* DeleteTags_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Collections.Generic.List<System.String> @tags = (System.Collections.Generic.List<System.String>)typeof(System.Collections.Generic.List<System.String>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @sequence = ptr_of_this_method->Value;


            JPush.JPushBinding.DeleteTags(@sequence, @tags);

            return __ret;
        }

        static StackObject* SetAlias_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @alias = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @sequence = ptr_of_this_method->Value;


            JPush.JPushBinding.SetAlias(@sequence, @alias);

            return __ret;
        }

        static StackObject* DeleteAlias_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @sequence = ptr_of_this_method->Value;


            JPush.JPushBinding.DeleteAlias(@sequence);

            return __ret;
        }



    }
}
