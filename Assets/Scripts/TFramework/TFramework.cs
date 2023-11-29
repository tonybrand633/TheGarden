using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TFramework
{



    #region EasyEvent
    public interface IEasyEvent 
    {
        IUnRegister Register(Action onEvent);
    }

    public class EasyEvent : IEasyEvent
    {
        private Action mOnEvent { get; set; }

        //开放了两个调用UnRegister的口子，一个是返回的IUnRegister的对象，一个是类本身
        public IUnRegister Register(Action onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(()=>UnRegister(onEvent));
        }

        public void UnRegister(Action onEvent) 
        {
            mOnEvent -= onEvent;
        }

        public void Trigger() 
        {
            mOnEvent?.Invoke();
        }        
    }

    public class EasyEvent<T> : IEasyEvent
    {
        private Action<T> mOnEvent = (e) => { };

        public IUnRegister Register(Action <T>onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(()=>UnRegister(onEvent));
        }

        public void UnRegister(Action<T>onEvent) 
        {
            mOnEvent -= onEvent;
        }

        //这个是实现接口，为了让泛型能够实现该接口
        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T _) => onEvent();
        }

        public void Trigger(T t) 
        {
            mOnEvent?.Invoke(t);
        }
    }

    public class EasyEvent<T, K> : IEasyEvent 
    {
        private Action<T, K> mOnEvent = (t,k) => { };


        public IUnRegister Register(Action<T,K> onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(()=>UnRegisterEvent(onEvent));
        }

        public void UnRegisterEvent(Action<T,K>OnEvent) 
        {
            mOnEvent -= OnEvent;
        }

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T t, K k) => onEvent();
        }

        public void Trigger(T t,K k) 
        {
            mOnEvent?.Invoke(t,k);
        }
    }

    public class EasyEvent<T, K, S> : IEasyEvent 
    {
        private Action<T, K, S> mOnEvent;

        public void UnRegister(Action<T,K,S>onEvent) 
        {
            mOnEvent -= onEvent;
        }

        public IUnRegister Register(Action<T,K,S>onEvent) 
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => UnRegister(onEvent));
        }

        IUnRegister IEasyEvent.Register(Action onEvent)
        {
            return Register(Action);
            void Action(T t,K k,S s)=> onEvent();
        }

        public void Trigger(T t, K k, S s) 
        {
            mOnEvent?.Invoke(t, k, s);
        }
    }

    public class EasyEvents 
    {
        private static readonly EasyEvents mGlobalEvents = new EasyEvents();

        private readonly Dictionary<Type,IEasyEvent>mTypeEvents = new Dictionary<Type,IEasyEvent>();

        public static void Register<T>() where T:IEasyEvent,new() 
        {
            //还是加到字典里进行存储
            mGlobalEvents.AddEvent<T>();
        }

        public void AddEvent<T>() where T : IEasyEvent,new()
        {
            mTypeEvents.Add(typeof(T), new T());
        }

        public T GetEvent<T>() where T :IEasyEvent,new() 
        {
            var exist = mTypeEvents.TryGetValue(typeof(T), out var value);
            if (exist)
            {
                return (T)value;
            }
            else 
            {
                return default;
            }
        }

        public T GetOrAddEvent<T>() where T:IEasyEvent,new() 
        {
            var type = typeof(T);
            if (mTypeEvents.TryGetValue(type, out var value))
            {
                return (T)value;
            }
            else 
            {
                var t = new T();
                mTypeEvents.Add(type, t);
                return t;
            }
        }
    }
    #endregion EasyEvent


    public interface IUnRegister
    {
        void UnRegister();
    }

    public struct CustomUnRegister : IUnRegister
    {
        private Action mOnUnEvent { get; set; }

        public CustomUnRegister(Action OnUnEvent) 
        {
            mOnUnEvent = OnUnEvent;
        }

        public void UnRegister()
        {
            mOnUnEvent.Invoke();
            mOnUnEvent = null;
        }
    }

    public interface ICanInteractiveDestory 
    {
        void Destory();        
    }

}
