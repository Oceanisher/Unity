using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter07.Scripts
{
    //单例工具
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance => instance;

        private void Awake()
        {
            if (null == instance)
            {
                instance = (T)this;
            }
            else
            {
                Debug.LogError("Singleton error.");
            }
        }
    }   
}
