using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Chapter07.Scripts
{
    //摄像机管理
    public class CameraManager : Singleton<CameraManager>
    {
        [Header("Cinemachine虚拟相机")]
        [SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        //跟随目标
        public void Follow(GameObject obj)
        {
            _virtualCamera.Follow = obj.transform;
        }
    }
}