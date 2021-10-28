using System.Collections;
using System.Collections.Generic;
using Chapter06.Scripts;
using UnityEngine;

namespace Chapter05.Scripts
{
    //角色抽象
    public abstract class Character : MonoBehaviour
    {
        //当前血量
        public HitPointSO hitPointsSO;
        //最大血量
        public float maxHitPoints;
        //初始血量
        public float startingHitPoints;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
