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
    unsafe class JPushReceiver_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::JPushReceiver);

            field = type.GetField("jpushDelegate_mess", flag);
            app.RegisterCLRFieldGetter(field, get_jpushDelegate_mess_0);
            app.RegisterCLRFieldSetter(field, set_jpushDelegate_mess_0);
            field = type.GetField("jpushDelegate_noti", flag);
            app.RegisterCLRFieldGetter(field, get_jpushDelegate_noti_1);
            app.RegisterCLRFieldSetter(field, set_jpushDelegate_noti_1);
            field = type.GetField("jpushDelegate_opennoti", flag);
            app.RegisterCLRFieldGetter(field, get_jpushDelegate_opennoti_2);
            app.RegisterCLRFieldSetter(field, set_jpushDelegate_opennoti_2);
            field = type.GetField("jpushDelegate_tag", flag);
            app.RegisterCLRFieldGetter(field, get_jpushDelegate_tag_3);
            app.RegisterCLRFieldSetter(field, set_jpushDelegate_tag_3);
            field = type.GetField("jpushDelegate_atlas", flag);
            app.RegisterCLRFieldGetter(field, get_jpushDelegate_atlas_4);
            app.RegisterCLRFieldSetter(field, set_jpushDelegate_atlas_4);
            field = type.GetField("jpushDelegate_Reg", flag);
            app.RegisterCLRFieldGetter(field, get_jpushDelegate_Reg_5);
            app.RegisterCLRFieldSetter(field, set_jpushDelegate_Reg_5);


        }



        static object get_jpushDelegate_mess_0(ref object o)
        {
            return ((global::JPushReceiver)o).jpushDelegate_mess;
        }
        static void set_jpushDelegate_mess_0(ref object o, object v)
        {
            ((global::JPushReceiver)o).jpushDelegate_mess = (global::JPushReceiver.JPushDelegate)v;
        }
        static object get_jpushDelegate_noti_1(ref object o)
        {
            return ((global::JPushReceiver)o).jpushDelegate_noti;
        }
        static void set_jpushDelegate_noti_1(ref object o, object v)
        {
            ((global::JPushReceiver)o).jpushDelegate_noti = (global::JPushReceiver.JPushDelegate)v;
        }
        static object get_jpushDelegate_opennoti_2(ref object o)
        {
            return ((global::JPushReceiver)o).jpushDelegate_opennoti;
        }
        static void set_jpushDelegate_opennoti_2(ref object o, object v)
        {
            ((global::JPushReceiver)o).jpushDelegate_opennoti = (global::JPushReceiver.JPushDelegate)v;
        }
        static object get_jpushDelegate_tag_3(ref object o)
        {
            return ((global::JPushReceiver)o).jpushDelegate_tag;
        }
        static void set_jpushDelegate_tag_3(ref object o, object v)
        {
            ((global::JPushReceiver)o).jpushDelegate_tag = (global::JPushReceiver.JPushDelegate)v;
        }
        static object get_jpushDelegate_atlas_4(ref object o)
        {
            return ((global::JPushReceiver)o).jpushDelegate_atlas;
        }
        static void set_jpushDelegate_atlas_4(ref object o, object v)
        {
            ((global::JPushReceiver)o).jpushDelegate_atlas = (global::JPushReceiver.JPushDelegate)v;
        }
        static object get_jpushDelegate_Reg_5(ref object o)
        {
            return ((global::JPushReceiver)o).jpushDelegate_Reg;
        }
        static void set_jpushDelegate_Reg_5(ref object o, object v)
        {
            ((global::JPushReceiver)o).jpushDelegate_Reg = (global::JPushReceiver.JPushDelegate)v;
        }


    }
}
