using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter07.Scripts
{
    //游戏管理器
    public class GameManager : Singleton<GameManager>
    {
        [Header("玩家刷新")]
        [SerializeField]
        private SpawnPoint _playerSpawnPoint;
        
        [Header("怪物刷新")]
        [SerializeField]
        private SpawnPoint _enemySpawnPoint;
        
        // Start is called before the first frame update
        void Start()
        {
            SetScene();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //初始化场景
        private void SetScene()
        {
            SpawnPlayer();
            SpawnEnemy();
        }

        //生产玩家
        private void SpawnPlayer()
        {
            if (null != _playerSpawnPoint)
            {
                GameObject player = _playerSpawnPoint.SpawnObj();
                CameraManager.Instance.Follow(player);
            }
        }
        
        //生产敌人
        private void SpawnEnemy()
        {
            if (null != _enemySpawnPoint)
            {
                GameObject enemy = _enemySpawnPoint.SpawnObj();
            }
        }
    }
}
