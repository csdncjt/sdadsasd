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
    unsafe class AudioManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(global::AudioManager);
            args = new Type[]{};
            method = type.GetMethod("StopBGM", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopBGM_0);
            args = new Type[]{};
            method = type.GetMethod("StopAllSFX", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopAllSFX_1);
            args = new Type[]{typeof(UnityEngine.AudioClip), typeof(global::MusicTransition), typeof(System.Single), typeof(System.Single)};
            method = type.GetMethod("PlayBGM", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayBGM_2);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_IsSoundOn", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_IsSoundOn_3);
            args = new Type[]{};
            method = type.GetMethod("SaveSFXPreferences", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SaveSFXPreferences_4);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("set_IsMusicOn", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_IsMusicOn_5);
            args = new Type[]{};
            method = type.GetMethod("SaveBGMPreferences", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SaveBGMPreferences_6);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("set_SoundVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_SoundVolume_7);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("set_MusicVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, set_MusicVolume_8);
            args = new Type[]{typeof(UnityEngine.AudioClip), typeof(System.Action)};
            method = type.GetMethod("PlayOneShot", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayOneShot_9);
            args = new Type[]{typeof(UnityEngine.AudioClip), typeof(System.Int32), typeof(System.Boolean), typeof(System.Action)};
            method = type.GetMethod("RepeatSFX", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RepeatSFX_10);
            args = new Type[]{};
            method = type.GetMethod("get_Instance", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_Instance_11);
            args = new Type[]{};
            method = type.GetMethod("get_SoundVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_SoundVolume_12);


        }


        static StackObject* StopBGM_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StopBGM();

            return __ret;
        }

        static StackObject* StopAllSFX_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.StopAllSFX();

            return __ret;
        }

        static StackObject* PlayBGM_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @volume = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Single @transition_duration = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::MusicTransition @transition = (global::MusicTransition)typeof(global::MusicTransition).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.AudioClip @clip = (UnityEngine.AudioClip)typeof(UnityEngine.AudioClip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.PlayBGM(@clip, @transition, @transition_duration, @volume);

            return __ret;
        }

        static StackObject* set_IsSoundOn_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.IsSoundOn = value;

            return __ret;
        }

        static StackObject* SaveSFXPreferences_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SaveSFXPreferences();

            return __ret;
        }

        static StackObject* set_IsMusicOn_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @value = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.IsMusicOn = value;

            return __ret;
        }

        static StackObject* SaveBGMPreferences_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SaveBGMPreferences();

            return __ret;
        }

        static StackObject* set_SoundVolume_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SoundVolume = value;

            return __ret;
        }

        static StackObject* set_MusicVolume_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @value = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.MusicVolume = value;

            return __ret;
        }

        static StackObject* PlayOneShot_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            UnityEngine.AudioClip @clip = (UnityEngine.AudioClip)typeof(UnityEngine.AudioClip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.PlayOneShot(@clip, @callback);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* RepeatSFX_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Action @callback = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @singleton = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @repeat = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            UnityEngine.AudioClip @clip = (UnityEngine.AudioClip)typeof(UnityEngine.AudioClip).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.RepeatSFX(@clip, @repeat, @singleton, @callback);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_Instance_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::AudioManager.Instance;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_SoundVolume_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::AudioManager instance_of_this_method = (global::AudioManager)typeof(global::AudioManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SoundVolume;

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }



    }
}
