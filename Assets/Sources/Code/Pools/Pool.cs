using System;
using System.Collections.Generic;
using UnityEngine;

namespace Petri.Pools
{
    public class Pool<T>
    {
        private readonly Func<T> _preload;
        private readonly Action<T> _get;
        private readonly Action<T> _returnAction;

        private readonly Queue<T> _pool = new();

        public Pool(Func<T> preload, Action<T> get, Action<T> returnAction, int preloadCount)
        {
            _preload = preload;
            _get = get;
            _returnAction = returnAction;

            if (_preload == null)
            {
                Debug.LogError($"Preload function is null");
                return;
            }

            for (var i = 0; i < preloadCount; i++) 
                Return(_preload());
        }

        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : _preload();
            _get(item);

            return item;
        }

        public void Return(T item)
        {
            _returnAction(item);
            _pool.Enqueue(item);
        }
    }
}