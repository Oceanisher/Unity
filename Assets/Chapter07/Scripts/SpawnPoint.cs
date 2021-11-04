using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Chapter07.Scripts
{
    //刷新点
    public class SpawnPoint : MonoBehaviour
    {
        [Header("待刷新物体")]
        [SerializeField]
        private GameObject _spawnPrefab;
        [Header("刷新间隔")]
        [SerializeField]
        private float _spawnInterval;

        //刷新的物体
        private List<GameObject> _spawnObjList = new List<GameObject>();
        
        // Start is called before the first frame update
        void Start()
        {
            if (_spawnInterval > 0)
            {
                InvokeRepeating("SpawnObj", 0.0f, _spawnInterval);
            }
        }

        //刷新物体
        public GameObject SpawnObj()
        {
            if (null != _spawnPrefab)
            {
                GameObject tempGO = Instantiate(_spawnPrefab, transform.position, Quaternion.identity);
                _spawnObjList.Add(tempGO);
                return tempGO;
            }

            return null;
        }
    }   
}
