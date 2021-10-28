using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter06.Scripts
{
    //生命点数SO
    [CreateAssetMenu(fileName = "HitPointSO", menuName = "SO/HitPoint", order = 1)]
    public class HitPointSO : ScriptableObject
    {
        //点数值
        public float value;
    }
}
