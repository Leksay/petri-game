using System;
using UnityEngine;

namespace Petri
{
    public class CustomUpdate
    {
        private float _timer;
        private float _deltaTime;
        private Action _updateFunction;

        public CustomUpdate(float deltaTime, Action updateFunction)
        {
            _timer = 0f;
            _deltaTime = deltaTime;
            _updateFunction = updateFunction;
        }

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _deltaTime)
            {
                _timer = 0f;
                _updateFunction();
            }
        }
    }
}