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
    unsafe class SuperScrollView_LoopListView2_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(SuperScrollView.LoopListView2);
            args = new Type[]{typeof(System.String)};
            method = type.GetMethod("NewListViewItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, NewListViewItem_0);
            args = new Type[]{};
            method = type.GetMethod("get_ShownItemCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ShownItemCount_1);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetShownItemByItemIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetShownItemByItemIndex_2);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("OnItemSizeChanged", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, OnItemSizeChanged_3);
            args = new Type[]{};
            method = type.GetMethod("get_ScrollRect", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ScrollRect_4);
            args = new Type[]{typeof(System.Int32), typeof(System.Func<SuperScrollView.LoopListView2, System.Int32, SuperScrollView.LoopListViewItem2>), typeof(SuperScrollView.LoopListViewInitParam)};
            method = type.GetMethod("InitListView", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, InitListView_5);
            args = new Type[]{typeof(System.Int32), typeof(System.Boolean)};
            method = type.GetMethod("SetListItemCount", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetListItemCount_6);
            args = new Type[]{};
            method = type.GetMethod("RefreshAllShownItem", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, RefreshAllShownItem_7);
            args = new Type[]{};
            method = type.GetMethod("UpdateAllShownItemSnapData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, UpdateAllShownItemSnapData_8);
            args = new Type[]{};
            method = type.GetMethod("get_CurSnapNearestItemIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_CurSnapNearestItemIndex_9);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("GetShownItemByIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetShownItemByIndex_10);
            args = new Type[]{typeof(System.Int32)};
            method = type.GetMethod("SetSnapTargetItemIndex", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetSnapTargetItemIndex_11);
            args = new Type[]{typeof(SuperScrollView.LoopListViewItem2), typeof(SuperScrollView.ItemCornerEnum)};
            method = type.GetMethod("GetItemCornerPosInViewPort", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetItemCornerPosInViewPort_12);
            args = new Type[]{};
            method = type.GetMethod("get_ViewPortSize", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_ViewPortSize_13);

            field = type.GetField("mOnEndDragAction", flag);
            app.RegisterCLRFieldGetter(field, get_mOnEndDragAction_0);
            app.RegisterCLRFieldSetter(field, set_mOnEndDragAction_0);
            field = type.GetField("mOnDownMoreDragAction", flag);
            app.RegisterCLRFieldGetter(field, get_mOnDownMoreDragAction_1);
            app.RegisterCLRFieldSetter(field, set_mOnDownMoreDragAction_1);
            field = type.GetField("mOnUpRefreshDragAction", flag);
            app.RegisterCLRFieldGetter(field, get_mOnUpRefreshDragAction_2);
            app.RegisterCLRFieldSetter(field, set_mOnUpRefreshDragAction_2);
            field = type.GetField("mOnBeginDragAction", flag);
            app.RegisterCLRFieldGetter(field, get_mOnBeginDragAction_3);
            app.RegisterCLRFieldSetter(field, set_mOnBeginDragAction_3);
            field = type.GetField("mOnDragingAction", flag);
            app.RegisterCLRFieldGetter(field, get_mOnDragingAction_4);
            app.RegisterCLRFieldSetter(field, set_mOnDragingAction_4);


        }


        static StackObject* NewListViewItem_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.String @itemPrefabName = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.NewListViewItem(@itemPrefabName);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_ShownItemCount_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ShownItemCount;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetShownItemByItemIndex_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @itemIndex = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetShownItemByItemIndex(@itemIndex);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* OnItemSizeChanged_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @itemIndex = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.OnItemSizeChanged(@itemIndex);

            return __ret;
        }

        static StackObject* get_ScrollRect_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ScrollRect;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* InitListView_5(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.LoopListViewInitParam @initParam = (SuperScrollView.LoopListViewInitParam)typeof(SuperScrollView.LoopListViewInitParam).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Func<SuperScrollView.LoopListView2, System.Int32, SuperScrollView.LoopListViewItem2> @onGetItemByIndex = (System.Func<SuperScrollView.LoopListView2, System.Int32, SuperScrollView.LoopListViewItem2>)typeof(System.Func<SuperScrollView.LoopListView2, System.Int32, SuperScrollView.LoopListViewItem2>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            System.Int32 @itemTotalCount = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.InitListView(@itemTotalCount, @onGetItemByIndex, @initParam);

            return __ret;
        }

        static StackObject* SetListItemCount_6(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @resetPos = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @itemCount = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetListItemCount(@itemCount, @resetPos);

            return __ret;
        }

        static StackObject* RefreshAllShownItem_7(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.RefreshAllShownItem();

            return __ret;
        }

        static StackObject* UpdateAllShownItemSnapData_8(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.UpdateAllShownItemSnapData();

            return __ret;
        }

        static StackObject* get_CurSnapNearestItemIndex_9(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.CurSnapNearestItemIndex;

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* GetShownItemByIndex_10(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @index = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetShownItemByIndex(@index);

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* SetSnapTargetItemIndex_11(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Int32 @itemIndex = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetSnapTargetItemIndex(@itemIndex);

            return __ret;
        }

        static StackObject* GetItemCornerPosInViewPort_12(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.ItemCornerEnum @corner = (SuperScrollView.ItemCornerEnum)typeof(SuperScrollView.ItemCornerEnum).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            SuperScrollView.LoopListViewItem2 @item = (SuperScrollView.LoopListViewItem2)typeof(SuperScrollView.LoopListViewItem2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetItemCornerPosInViewPort(@item, @corner);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* get_ViewPortSize_13(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            SuperScrollView.LoopListView2 instance_of_this_method = (SuperScrollView.LoopListView2)typeof(SuperScrollView.LoopListView2).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.ViewPortSize;

            __ret->ObjectType = ObjectTypes.Float;
            *(float*)&__ret->Value = result_of_this_method;
            return __ret + 1;
        }


        static object get_mOnEndDragAction_0(ref object o)
        {
            return ((SuperScrollView.LoopListView2)o).mOnEndDragAction;
        }
        static void set_mOnEndDragAction_0(ref object o, object v)
        {
            ((SuperScrollView.LoopListView2)o).mOnEndDragAction = (System.Action)v;
        }
        static object get_mOnDownMoreDragAction_1(ref object o)
        {
            return ((SuperScrollView.LoopListView2)o).mOnDownMoreDragAction;
        }
        static void set_mOnDownMoreDragAction_1(ref object o, object v)
        {
            ((SuperScrollView.LoopListView2)o).mOnDownMoreDragAction = (System.Action)v;
        }
        static object get_mOnUpRefreshDragAction_2(ref object o)
        {
            return ((SuperScrollView.LoopListView2)o).mOnUpRefreshDragAction;
        }
        static void set_mOnUpRefreshDragAction_2(ref object o, object v)
        {
            ((SuperScrollView.LoopListView2)o).mOnUpRefreshDragAction = (System.Action)v;
        }
        static object get_mOnBeginDragAction_3(ref object o)
        {
            return ((SuperScrollView.LoopListView2)o).mOnBeginDragAction;
        }
        static void set_mOnBeginDragAction_3(ref object o, object v)
        {
            ((SuperScrollView.LoopListView2)o).mOnBeginDragAction = (System.Action)v;
        }
        static object get_mOnDragingAction_4(ref object o)
        {
            return ((SuperScrollView.LoopListView2)o).mOnDragingAction;
        }
        static void set_mOnDragingAction_4(ref object o, object v)
        {
            ((SuperScrollView.LoopListView2)o).mOnDragingAction = (System.Action)v;
        }


    }
}
