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
    unsafe class SuperScrollView_ListItem0_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(SuperScrollView.ListItem0);

            field = type.GetField("mRoot", flag);
            app.RegisterCLRFieldGetter(field, get_mRoot_0);
            app.RegisterCLRFieldSetter(field, set_mRoot_0);
            field = type.GetField("mText", flag);
            app.RegisterCLRFieldGetter(field, get_mText_1);
            app.RegisterCLRFieldSetter(field, set_mText_1);
            field = type.GetField("mArrow", flag);
            app.RegisterCLRFieldGetter(field, get_mArrow_2);
            app.RegisterCLRFieldSetter(field, set_mArrow_2);
            field = type.GetField("mWaitingIcon", flag);
            app.RegisterCLRFieldGetter(field, get_mWaitingIcon_3);
            app.RegisterCLRFieldSetter(field, set_mWaitingIcon_3);


        }



        static object get_mRoot_0(ref object o)
        {
            return ((SuperScrollView.ListItem0)o).mRoot;
        }
        static void set_mRoot_0(ref object o, object v)
        {
            ((SuperScrollView.ListItem0)o).mRoot = (UnityEngine.GameObject)v;
        }
        static object get_mText_1(ref object o)
        {
            return ((SuperScrollView.ListItem0)o).mText;
        }
        static void set_mText_1(ref object o, object v)
        {
            ((SuperScrollView.ListItem0)o).mText = (UnityEngine.UI.Text)v;
        }
        static object get_mArrow_2(ref object o)
        {
            return ((SuperScrollView.ListItem0)o).mArrow;
        }
        static void set_mArrow_2(ref object o, object v)
        {
            ((SuperScrollView.ListItem0)o).mArrow = (UnityEngine.GameObject)v;
        }
        static object get_mWaitingIcon_3(ref object o)
        {
            return ((SuperScrollView.ListItem0)o).mWaitingIcon;
        }
        static void set_mWaitingIcon_3(ref object o, object v)
        {
            ((SuperScrollView.ListItem0)o).mWaitingIcon = (UnityEngine.GameObject)v;
        }


    }
}
