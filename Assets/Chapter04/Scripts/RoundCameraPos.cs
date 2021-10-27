using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Chapter04.Scripts
{
    //摄像机位置圆滑调整，确保其处于像素点位置
    public class RoundCameraPos : CinemachineExtension
    {
        //一个世界单元显示的元素数量
        [SerializeField]
        private float PixelPerUnit = 32;
        
        //Cinemachine Confiner处理后回调
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            //stage为虚拟摄像机后期处理管线中的阶段，Body是其中一个阶段
            if (stage == CinemachineCore.Stage.Body)
            {
                Vector3 pos = state.FinalPosition;
                Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);
                //将摄像机位置设为float与int之间的差值，确保摄像机处于像素点位置
                state.PositionCorrection += pos2 - pos;
            }
            
        }

        //四舍五入处理，返回整数
        private float Round(float x)
        {
            return Mathf.Round(x * PixelPerUnit) / PixelPerUnit;
        }
    } 
}

