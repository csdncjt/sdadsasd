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
    unsafe class NativeManager_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::NativeManager);
            MethodInfo[] methods = type.GetMethods(flag).Where(t => !t.IsGenericMethod).ToArray();
            args = new Type[]{};
            method = type.GetMethod("GetGPSLocation", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetGPSLocation_0);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(System.Boolean)};
            if (genericMethods.TryGetValue("OnFuncCall", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.GetParameters().Length == 2)
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, OnFuncCall_1);

                        break;
                    }
                }
            }
            args = new Type[]{};
            method = type.GetMethod("GetDeviceSafeArea", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetDeviceSafeArea_2);
            args = new Type[]{};
            method = type.GetMethod("IntAwakeByURLObserver", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IntAwakeByURLObserver_3);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("ResolverForNativeMsg", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ResolverForNativeMsg_4);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("TakeHeadImage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, TakeHeadImage_5);
            args = new Type[]{typeof(System.String), typeof(System.Object[])};
            method = methods.Where(t => t.Name.Equals("OnFuncCall") && t.CheckMethodParams(args)).Single();
            app.RegisterCLRMethodRedirection(method, OnFuncCall_6);
            args = new Type[]{typeof(System.String), typeof(System.String), typeof(System.Int32), typeof(System.String)};
            method = type.GetMethod("IMLogin", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IMLogin_7);
            args = new Type[]{};
            method = type.GetMethod("IMLoginOut", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IMLoginOut_8);
            args = new Type[]{typeof(System.Int32)};
            if (genericMethods.TryGetValue("OnFuncCall", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.GetParameters().Length == 2)
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, OnFuncCall_9);

                        break;
                    }
                }
            }
            args = new Type[]{};
            method = type.GetMethod("GetImState", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetImState_10);
            args = new Type[]{};
            method = type.GetMethod("StartRecord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StartRecord_11);
            args = new Type[]{typeof(System.Int64)};
            if (genericMethods.TryGetValue("OnFuncCall", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.GetParameters().Length == 2)
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, OnFuncCall_12);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("StopRecord", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopRecord_13);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("PlayVoice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayVoice_14);
            args = new Type[]{};
            method = type.GetMethod("StopVoice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StopVoice_15);
            args = new Type[]{};
            method = type.GetMethod("IsPlayingVoice", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IsPlayingVoice_16);
            args = new Type[]{typeof(System.Single)};
            method = type.GetMethod("SetVolume", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetVolume_17);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("IMSendMessage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, IMSendMessage_18);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("ApplyJoinGroup", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ApplyJoinGroup_19);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("SetConversation", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetConversation_20);
            args = new Type[]{};
            method = type.GetMethod("ClearConversation", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ClearConversation_21);
            args = new Type[]{typeof(System.String), typeof(System.String)};
            method = type.GetMethod("KeFuInit", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, KeFuInit_22);
            args = new Type[]{typeof(System.String), typeof(System.String)};
            method = type.GetMethod("KeFuLogin", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, KeFuLogin_23);
            args = new Type[]{};
            method = type.GetMethod("KeFuLoginOut", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, KeFuLoginOut_24);
            args = new Type[]{typeof(System.String), typeof(System.String), typeof(System.String)};
            method = type.GetMethod("StartChat", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, StartChat_25);
            args = new Type[]{typeof(System.Byte[]), typeof(System.Int64)};
            method = type.GetMethod("SaveImageToNative", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SaveImageToNative_26);
            args = new Type[]{};
            method = type.GetMethod("CaptureScreenshot", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, CaptureScreenshot_27);
            args = new Type[]{typeof(System.String), typeof(System.String)};
            method = type.GetMethod("InAppPurchase", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InAppPurchase_28);
            args = new Type[]{typeof(System.Boolean)};
            method = type.GetMethod("UpdateShowStatusBar", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateShowStatusBar_29);

            field = type.GetField("Instance", flag);
            app.RegisterCLRFieldGetter(field, get_Instance_0);
            app.RegisterCLRFieldSetter(field, set_Instance_0);
            field = type.GetField("safeArea", flag);
            app.RegisterCLRFieldGetter(field, get_safeArea_1);
            app.RegisterCLRFieldSetter(field, set_safeArea_1);


        }


        static StackObject* GetGPSLocation_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::NativeManager.GetGPSLocation();

            return __ret;
        }

        static StackObject* OnFuncCall_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object[] @args = (System.Object[])typeof(System.Object[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @funcName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NativeManager.OnFuncCall<System.Boolean>(@funcName, @args);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* GetDeviceSafeArea_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::NativeManager.GetDeviceSafeArea();

            return __ret;
        }

        static StackObject* IntAwakeByURLObserver_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::NativeManager.IntAwakeByURLObserver();

            return __ret;
        }

        static StackObject* ResolverForNativeMsg_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @content = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.ResolverForNativeMsg(@content);

            return __ret;
        }

        static StackObject* TakeHeadImage_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @type = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.TakeHeadImage(@type);

            return __ret;
        }

        static StackObject* OnFuncCall_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object[] @args = (System.Object[])typeof(System.Object[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @funcName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.OnFuncCall(@funcName, @args);

            return __ret;
        }

        static StackObject* IMLogin_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @systemSender = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @appID = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @token = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @userID = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.IMLogin(@userID, @token, @appID, @systemSender);

            return __ret;
        }

        static StackObject* IMLoginOut_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::NativeManager.IMLoginOut();

            return __ret;
        }

        static StackObject* OnFuncCall_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object[] @args = (System.Object[])typeof(System.Object[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @funcName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NativeManager.OnFuncCall<System.Int32>(@funcName, @args);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetImState_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::NativeManager.GetImState();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* StartRecord_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::NativeManager.StartRecord();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* OnFuncCall_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Object[] @args = (System.Object[])typeof(System.Object[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @funcName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NativeManager.OnFuncCall<System.Int64>(@funcName, @args);

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* StopRecord_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @isSend = ptr_of_this_method->Value == 1;


            var result_of_this_method = global::NativeManager.StopRecord(@isSend);

            __ret->ObjectType = ObjectTypes.Long;
            *(long*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* PlayVoice_14(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @path = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.PlayVoice(@path);

            return __ret;
        }

        static StackObject* StopVoice_15(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::NativeManager.StopVoice();

            return __ret;
        }

        static StackObject* IsPlayingVoice_16(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            var result_of_this_method = global::NativeManager.IsPlayingVoice();

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method ? 1 : 0;
            return __ret + 1;
        }

        static StackObject* SetVolume_17(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @volume = *(float*)&ptr_of_this_method->Value;


            global::NativeManager.SetVolume(@volume);

            return __ret;
        }

        static StackObject* IMSendMessage_18(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @str = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.IMSendMessage(@str);

            return __ret;
        }

        static StackObject* ApplyJoinGroup_19(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @groupId = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.ApplyJoinGroup(@groupId);

            return __ret;
        }

        static StackObject* SetConversation_20(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @groupId = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.SetConversation(@groupId);

            return __ret;
        }

        static StackObject* ClearConversation_21(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::NativeManager.ClearConversation();

            return __ret;
        }

        static StackObject* KeFuInit_22(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @sdkkey = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @siteid = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.KeFuInit(@siteid, @sdkkey);

            return __ret;
        }

        static StackObject* KeFuLogin_23(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @userName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @userId = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NativeManager.KeFuLogin(@userId, @userName);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* KeFuLoginOut_24(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);


            global::NativeManager.KeFuLoginOut();

            return __ret;
        }

        static StackObject* StartChat_25(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @strbody = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @groupName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.String @settingid = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = global::NativeManager.StartChat(@settingid, @groupName, @strbody);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* SaveImageToNative_26(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int64 @length = *(long*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Byte[] @bytes = (System.Byte[])typeof(System.Byte[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.SaveImageToNative(@bytes, @length);

            return __ret;
        }

        static StackObject* CaptureScreenshot_27(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            global::NativeManager instance_of_this_method = (global::NativeManager)typeof(global::NativeManager).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.CaptureScreenshot();

            return __ret;
        }

        static StackObject* InAppPurchase_28(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @orderID = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @productID = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            global::NativeManager.InAppPurchase(@productID, @orderID);

            return __ret;
        }

        static StackObject* UpdateShowStatusBar_29(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @show = ptr_of_this_method->Value == 1;


            global::NativeManager.UpdateShowStatusBar(@show);

            return __ret;
        }


        static object get_Instance_0(ref object o)
        {
            return global::NativeManager.Instance;
        }
        static void set_Instance_0(ref object o, object v)
        {
            global::NativeManager.Instance = (global::NativeManager)v;
        }
        static object get_safeArea_1(ref object o)
        {
            return ((global::NativeManager)o).safeArea;
        }
        static void set_safeArea_1(ref object o, object v)
        {
            ((global::NativeManager)o).safeArea = (global::NativeManager.SafeArea)v;
        }


    }
}
