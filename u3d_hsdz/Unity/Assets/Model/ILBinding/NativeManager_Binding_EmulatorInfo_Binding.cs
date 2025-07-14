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
    unsafe class NativeManager_Binding_EmulatorInfo_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NativeManager.EmulatorInfo);

            field = type.GetField("sensorNum", flag);
            app.RegisterCLRFieldGetter(field, get_sensorNum_0);
            app.RegisterCLRFieldSetter(field, set_sensorNum_0);
            field = type.GetField("baseBandVersion", flag);
            app.RegisterCLRFieldGetter(field, get_baseBandVersion_1);
            app.RegisterCLRFieldSetter(field, set_baseBandVersion_1);
            field = type.GetField("userAppNum", flag);
            app.RegisterCLRFieldGetter(field, get_userAppNum_2);
            app.RegisterCLRFieldSetter(field, set_userAppNum_2);


        }



        static object get_sensorNum_0(ref object o)
        {
            return ((global::NativeManager.EmulatorInfo)o).sensorNum;
        }
        static void set_sensorNum_0(ref object o, object v)
        {
            ((global::NativeManager.EmulatorInfo)o).sensorNum = (System.Int32)v;
        }
        static object get_baseBandVersion_1(ref object o)
        {
            return ((global::NativeManager.EmulatorInfo)o).baseBandVersion;
        }
        static void set_baseBandVersion_1(ref object o, object v)
        {
            ((global::NativeManager.EmulatorInfo)o).baseBandVersion = (System.String)v;
        }
        static object get_userAppNum_2(ref object o)
        {
            return ((global::NativeManager.EmulatorInfo)o).userAppNum;
        }
        static void set_userAppNum_2(ref object o, object v)
        {
            ((global::NativeManager.EmulatorInfo)o).userAppNum = (System.Int32)v;
        }


    }
}
