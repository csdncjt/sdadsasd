using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;
using UnityEngine.Events;

namespace ETModel
{
	public static class ILHelper
	{
		public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
		{
			// 注册重定向函数

			// 注册委托
			appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
			appdomain.DelegateManager.RegisterMethodDelegate<AChannel, System.Net.Sockets.SocketError>();
			appdomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
			appdomain.DelegateManager.RegisterMethodDelegate<IResponse>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session, object>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session, byte, ushort, MemoryStream>();
			appdomain.DelegateManager.RegisterMethodDelegate<Session>();
			appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
			appdomain.DelegateManager.RegisterFunctionDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();
			appdomain.DelegateManager.RegisterMethodDelegate<Google.Protobuf.Adapt_IMessage.Adaptor>();


			appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject>();
			appdomain.DelegateManager.RegisterDelegateConvertor<global::UIEventListener.VoidDelegate>((act) =>
			{
				return new global::UIEventListener.VoidDelegate((go) =>
				{
					((Action<UnityEngine.GameObject>)act)(go);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((act) =>
			{
				return new UnityAction(() =>
				{
					((Action)act)();
				});
			});
			
			appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<bool>>((act) =>
			{
				return new UnityAction<bool>((arg) => { ((Action<bool>)act)(arg); });
			});
			
			appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction<string>>((act) =>
			{
				return new UnityAction<string>((arg) => { ((Action<string>)act)(arg); });
			});

			appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.GameObject, int>();
			appdomain.DelegateManager.RegisterDelegateConvertor<global::UIEventListener.IntDelegate>((act) =>
			{
				return new global::UIEventListener.IntDelegate((go, index) =>
				{
					((Action<UnityEngine.GameObject, int>)act)(go, index);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
			{
				return new DG.Tweening.TweenCallback(() =>
				{
					((Action)act)();
				});
			});

            appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOGetter<System.Single>>((act) =>
			{
				return new DG.Tweening.Core.DOGetter<System.Single>(() =>
				{
					return ((Func<System.Single>)act)();
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.Core.DOSetter<System.Single>>((act) =>
			{
				return new DG.Tweening.Core.DOSetter<System.Single>((pNewValue) =>
				{
					((Action<System.Single>)act)(pNewValue);
				});
			});

			appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
			appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback<System.Int32>>((act) =>
			{
				return new DG.Tweening.TweenCallback<System.Int32>((value) =>
				{
					((Action<System.Int32>)act)(value);
				});
			});

			appdomain.DelegateManager.RegisterMethodDelegate<BestHTTP.HTTPRequest, BestHTTP.HTTPResponse>();
			appdomain.DelegateManager.RegisterDelegateConvertor<BestHTTP.OnRequestFinishedDelegate>((act) =>
			{
				return new BestHTTP.OnRequestFinishedDelegate((originalRequest, response) =>
				{
					((Action<BestHTTP.HTTPRequest, BestHTTP.HTTPResponse>)act)(originalRequest, response);
				});
			});

			appdomain.DelegateManager.RegisterMethodDelegate<System.String, DragonBones.EventObject>();
			appdomain.DelegateManager.RegisterDelegateConvertor<DragonBones.ListenerDelegate<DragonBones.EventObject>>((act) =>
			{
				return new DragonBones.ListenerDelegate<DragonBones.EventObject>((type, eventObject) =>
				{
					((Action<System.String, DragonBones.EventObject>)act)(type, eventObject);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<global::UIScrollEventListener.VoidDelegate>((act) =>
			{
				return new global::UIScrollEventListener.VoidDelegate((pdata) =>
				{
					((Action<UnityEngine.EventSystems.PointerEventData>)act)(pdata);
				});
			});

			appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Single>>((act) =>
			{
				return new UnityEngine.Events.UnityAction<System.Single>((arg0) =>
				{
					((Action<System.Single>)act)(arg0);
				});
			});

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.PointerEventData>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Single>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.String>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int16>();
            appdomain.DelegateManager.RegisterMethodDelegate<System.Int64>();
            appdomain.DelegateManager.RegisterFunctionDelegate<SuperScrollView.LoopListView2, System.Int32, SuperScrollView.LoopListViewItem2>();
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector2>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector2>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector2>((arg0) =>
                {
                    ((Action<UnityEngine.Vector2>)act)(arg0);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector3>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector3>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector3>((arg0) =>
                {
                    ((Action<UnityEngine.Vector3>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Vector4>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Vector4>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Vector4>((arg0) =>
                {
                    ((Action<UnityEngine.Vector4>)act)(arg0);
                });
            });
            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Rect>();
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.Rect>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<UnityEngine.Rect>((arg0) =>
                {
                    ((Action<UnityEngine.Rect>)act)(arg0);
                });
            });

            //jpush
            appdomain.DelegateManager.RegisterDelegateConvertor<global::JPushReceiver.JPushDelegate>((act) =>
            {
                return new global::JPushReceiver.JPushDelegate((str) =>
                {
                    ((Action<System.String>)act)(str);
                });
            });

            appdomain.DelegateManager.RegisterFunctionDelegate<System.Int32, System.Int32, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Int32>>((act) =>
            {
                return new System.Comparison<System.Int32>((x, y) =>
                {
                    return ((Func<System.Int32, System.Int32, System.Int32>)act)(x, y);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<ETModel.Hotfix.IMDelegate>((act) =>
            {
                return new ETModel.Hotfix.IMDelegate((mes) =>
                {
                    ((Action<System.String>)act)(mes);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Int32>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Int32>((arg0) =>
                {
                    ((Action<System.Int32>)act)(arg0);
                });
            });

            appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
            appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
                {
                    return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
                });
            });

            appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Texture2D>();

            //appdomain.DelegateManager.RegisterMethodDelegate<global::UniWebView, System.String>();
            //appdomain.DelegateManager.RegisterDelegateConvertor<global::UniWebView.PageStartedDelegate>((act) =>
            //{
            //    return new global::UniWebView.PageStartedDelegate((webView, url) =>
            //    {
            //        ((Action<global::UniWebView, System.String>)act)(webView, url);
            //    });
            //});

            //appdomain.DelegateManager.RegisterMethodDelegate<global::UniWebView, System.Int32, System.String>();
            //appdomain.DelegateManager.RegisterDelegateConvertor<global::UniWebView.PageFinishedDelegate>((act) =>
            //{
            //    return new global::UniWebView.PageFinishedDelegate((webView, statusCode, url) =>
            //    {
            //        ((Action<global::UniWebView, System.Int32, System.String>)act)(webView, statusCode, url);
            //    });
            //});

            appdomain.DelegateManager.RegisterDelegateConvertor<ETModel.Init.callback>((act) =>
            {
                return new ETModel.Init.callback((arg) =>
                {
                    ((Action<System.String>)act)(arg);
                });
            });

            CLRBindings.Initialize(appdomain);

			// 注册适配器
			Assembly assembly = typeof(Init).Assembly;
			foreach (Type type in assembly.GetTypes())
			{
				object[] attrs = type.GetCustomAttributes(typeof(ILAdapterAttribute), false);
				if (attrs.Length == 0)
				{
					continue;
				}
				object obj = Activator.CreateInstance(type);
				CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
				if (adaptor == null)
				{
					continue;
				}
				appdomain.RegisterCrossBindingAdaptor(adaptor);
			}

			LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
		}
	}
}