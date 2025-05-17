using System;
using System.Collections.Generic;
using Petri.Services;
using UnityEngine;

namespace Petri.Infrostructure
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> Instances = new();

        public static T Get<T>() where T : class, IService
        {
            Instances.TryGetValue(typeof(T), out var instance);
            return (T)instance;
        }

        public static void Register<T>(T instance) where T : class, IService => Register(instance, typeof(T));

        public static void Register<T>(T instance, Type type) where T : class, IService
        {
            if (!Instances.TryAdd(type, instance)) 
                Debug.LogError($"Service of type {type} already registered");
        }

        public static void Clear()
        {
            Instances.Clear();
        }
    }
}