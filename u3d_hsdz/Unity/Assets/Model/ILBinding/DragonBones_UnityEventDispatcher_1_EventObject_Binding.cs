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
    unsafe class DragonBones_UnityEventDispatcher_1_EventObject_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(DragonBones.UnityEventDispatcher<DragonBones.EventObject>);
            args = new Type[]{typeof(System.String), typeof(DragonBones.ListenerDelegate<DragonBones.EventObject>)};
            method = type.GetMethod("AddEventListener", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, AddEventListener_0);


        }


        static StackObject* AddEventListener_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            DragonBones.ListenerDelegate<DragonBones.EventObject> @listener = (DragonBones.ListenerDelegate<DragonBones.EventObject>)typeof(DragonBones.ListenerDelegate<DragonBones.EventObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @type = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            DragonBones.UnityEventDispatcher<DragonBones.EventObject> instance_of_this_method = (DragonBones.UnityEventDispatcher<DragonBones.EventObject>)typeof(DragonBones.UnityEventDispatcher<DragonBones.EventObject>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.AddEventListener(@type, @listener);

            return __ret;
        }



    }
}
