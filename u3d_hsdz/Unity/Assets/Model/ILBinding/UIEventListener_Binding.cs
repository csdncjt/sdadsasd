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
    unsafe class UIEventListener_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::UIEventListener);
            args = new Type[]{typeof(UnityEngine.GameObject)};
            method = type.GetMethod("Get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Get_0);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Int32)};
            method = type.GetMethod("Get", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Get_1);

            field = type.GetField("onClick", flag);
            app.RegisterCLRFieldGetter(field, get_onClick_0);
            app.RegisterCLRFieldSetter(field, set_onClick_0);
            field = type.GetField("onDown", flag);
            app.RegisterCLRFieldGetter(field, get_onDown_1);
            app.RegisterCLRFieldSetter(field, set_onDown_1);
            field = type.GetField("onUp", flag);
            app.RegisterCLRFieldGetter(field, get_onUp_2);
            app.RegisterCLRFieldSetter(field, set_onUp_2);
            field = type.GetField("onEnter", flag);
            app.RegisterCLRFieldGetter(field, get_onEnter_3);
            app.RegisterCLRFieldSetter(field, set_onEnter_3);
            field = type.GetField("onExit", flag);
            app.RegisterCLRFieldGetter(field, get_onExit_4);
            app.RegisterCLRFieldSetter(field, set_onExit_4);
            field = type.GetField("onIntClick", flag);
            app.RegisterCLRFieldGetter(field, get_onIntClick_5);
            app.RegisterCLRFieldSetter(field, set_onIntClick_5);


        }


        static StackObject* Get_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::UIEventListener.Get(@go);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Get_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @i = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.GameObject @go = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::UIEventListener.Get(@go, @i);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_onClick_0(ref object o)
        {
            return ((global::UIEventListener)o).onClick;
        }
        static void set_onClick_0(ref object o, object v)
        {
            ((global::UIEventListener)o).onClick = (global::UIEventListener.VoidDelegate)v;
        }
        static object get_onDown_1(ref object o)
        {
            return ((global::UIEventListener)o).onDown;
        }
        static void set_onDown_1(ref object o, object v)
        {
            ((global::UIEventListener)o).onDown = (global::UIEventListener.VoidDelegate)v;
        }
        static object get_onUp_2(ref object o)
        {
            return ((global::UIEventListener)o).onUp;
        }
        static void set_onUp_2(ref object o, object v)
        {
            ((global::UIEventListener)o).onUp = (global::UIEventListener.VoidDelegate)v;
        }
        static object get_onEnter_3(ref object o)
        {
            return ((global::UIEventListener)o).onEnter;
        }
        static void set_onEnter_3(ref object o, object v)
        {
            ((global::UIEventListener)o).onEnter = (global::UIEventListener.VoidDelegate)v;
        }
        static object get_onExit_4(ref object o)
        {
            return ((global::UIEventListener)o).onExit;
        }
        static void set_onExit_4(ref object o, object v)
        {
            ((global::UIEventListener)o).onExit = (global::UIEventListener.VoidDelegate)v;
        }
        static object get_onIntClick_5(ref object o)
        {
            return ((global::UIEventListener)o).onIntClick;
        }
        static void set_onIntClick_5(ref object o, object v)
        {
            ((global::UIEventListener)o).onIntClick = (global::UIEventListener.IntDelegate)v;
        }


    }
}
