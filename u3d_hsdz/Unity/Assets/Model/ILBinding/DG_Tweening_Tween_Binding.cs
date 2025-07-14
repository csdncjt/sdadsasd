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
    unsafe class DG_Tweening_Tween_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(DG.Tweening.Tween);

            field = type.GetField("onComplete", flag);
            app.RegisterCLRFieldGetter(field, get_onComplete_0);
            app.RegisterCLRFieldSetter(field, set_onComplete_0);


        }



        static object get_onComplete_0(ref object o)
        {
            return ((DG.Tweening.Tween)o).onComplete;
        }
        static void set_onComplete_0(ref object o, object v)
        {
            ((DG.Tweening.Tween)o).onComplete = (DG.Tweening.TweenCallback)v;
        }


    }
}
