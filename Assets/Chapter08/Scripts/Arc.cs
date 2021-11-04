using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter08.Scripts
{
    //子弹轨迹控制
    public class Arc : MonoBehaviour
    {
        //子弹移动协程
        public IEnumerator TravelArc(Vector3 destination, float duration)
        {
            //子弹初始位置
            var startPosition = transform.position;
            //移动完成度
            var percentComplete = 0.0f;

            //如果没有移动完成，那么持续移动
            while (percentComplete < 1.0f)
            {
                //完成度每帧增加
                percentComplete += Time.deltaTime / duration;
                /**
                 //线性插值移动，这是直线子弹的处理方式
                transform.position = Vector3.Lerp(startPosition, destination, percentComplete);
                yield return null;
                 */
                //抛物线运动的高度函数，用三角函数计算
                var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);
                transform.position = Vector3.Lerp(startPosition, destination, percentComplete)
                    + Vector3.up * currentHeight;
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }   
}
