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
    unsafe class NativeManager_Binding_SafeArea_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NativeManager.SafeArea);

            field = type.GetField("top", flag);
            app.RegisterCLRFieldGetter(field, get_top_0);
            app.RegisterCLRFieldSetter(field, set_top_0);
            field = type.GetField("width", flag);
            app.RegisterCLRFieldGetter(field, get_width_1);
            app.RegisterCLRFieldSetter(field, set_width_1);
            field = type.GetField("bottom", flag);
            app.RegisterCLRFieldGetter(field, get_bottom_2);
            app.RegisterCLRFieldSetter(field, set_bottom_2);


        }



        static object get_top_0(ref object o)
        {
            return ((global::NativeManager.SafeArea)o).top;
        }
        static void set_top_0(ref object o, object v)
        {
            ((global::NativeManager.SafeArea)o).top = (System.Single)v;
        }
        static object get_width_1(ref object o)
        {
            return ((global::NativeManager.SafeArea)o).width;
        }
        static void set_width_1(ref object o, object v)
        {
            ((global::NativeManager.SafeArea)o).width = (System.Single)v;
        }
        static object get_bottom_2(ref object o)
        {
            return ((global::NativeManager.SafeArea)o).bottom;
        }
        static void set_bottom_2(ref object o, object v)
        {
            ((global::NativeManager.SafeArea)o).bottom = (System.Single)v;
        }


    }
}
