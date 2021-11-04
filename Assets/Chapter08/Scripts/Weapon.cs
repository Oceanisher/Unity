using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter08.Scripts
{
    //武器类
    [RequireComponent(typeof(Animator))]
    public class Weapon : MonoBehaviour
    {
        [Header("子弹预制")]
        [SerializeField]
        private GameObject _ammoPrefab;
        [Header("子弹速度")]
        [SerializeField]
        private float _ammoVelocity;
        [Header("对象池大小")]
        [SerializeField]
        private int _poolSize;

        //对象池
        private List<GameObject> _pool;

        //是否正在开火
        private bool _isFiring;

        [HideInInspector]
        public Animator _animator;
        
        //摄像机
        private Camera _localCamera;

        //两条屏幕对角线的斜率-左下到右上的直线斜率
        private float _positiveSlope;
        //两条屏幕对角线的斜率-左上到右下的直线斜率
        private float _negativeSlope;

        //象限
        enum Quadrant
        {
            East,
            West,
            North,
            South
        }
        
        private void Awake()
        {
            if (_pool == null)
            {
                _pool = new List<GameObject>();
            }

            //初始化对象池
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject tempObj = Instantiate(_ammoPrefab);
                tempObj.SetActive(false);
                _pool.Add(tempObj);
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _isFiring = false;
            _localCamera = Camera.main;
            
            //以屏幕左下角（0，0）为原点，计算两条对角线斜率
            //Unity屏幕坐标是从左下角为原点的
            Vector2 lowerLeft = _localCamera.ScreenToWorldPoint(Vector3.zero);
            Vector2 upperRight = _localCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
            Vector2 upperLeft = _localCamera.ScreenToWorldPoint(new Vector3(0, Screen.height));
            Vector2 lowerRight = _localCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0));
            _positiveSlope = GetSlop(lowerLeft, upperRight);
            _negativeSlope = GetSlop(upperLeft, lowerRight);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isFiring = true;
                FireAmmo();
            }
            
            //更新动画
            UpdateState();
        }

        private void OnDestroy()
        {
            _pool = null;
        }

        //生产子弹。仅从对象池返回，如果对象池对象都在使用中，那么返回空
        private GameObject SpawnAmmo(Transform trs)
        {
            foreach (var o in _pool)
            {
                if (!o.activeSelf)
                {
                    o.SetActive(true);
                    o.transform.position = trs.position;
                    return o;
                }
            }

            return null;
        }

        //发射子弹
        private void FireAmmo()
        {
            //鼠标的屏幕空间转换为世界空间坐标
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //获取子弹
            GameObject ammoObj = SpawnAmmo(transform);

            if (null != ammoObj)
            {
                Arc arc = ammoObj.GetComponent<Arc>();
                //计算子弹持续时间
                float travelDuration = 1.0f / _ammoVelocity;
                //开启子弹移动
                StartCoroutine(arc.TravelArc(mousePosition, travelDuration));
            }
        }
        
        //更新状态
        private void UpdateState()
        {
            if (_isFiring)
            {
                //根据射击方向得到的方向向量
                Vector2 quadrantVector;
                Quadrant quadrantEnum = GetQuadrant();
                switch (quadrantEnum)
                {
                    case Quadrant.East:
                        quadrantVector = Vector2.right;;
                        break;
                    case Quadrant.West:
                        quadrantVector = Vector2.left;;
                        break;
                    case Quadrant.North:
                        quadrantVector = Vector2.up;;
                        break;
                    case Quadrant.South:
                        quadrantVector = Vector2.down;;
                        break;
                    default:
                        quadrantVector = Vector2.zero;;
                        break;
                }
                
                //播放动画
                _animator.SetBool("IsFire", true);
                _animator.SetFloat("xFire", quadrantVector.x);
                _animator.SetFloat("yFire", quadrantVector.y);
                //射击动画只播放一次
                _isFiring = false;
            }
            else
            {
                _animator.SetBool("IsFire", false);
            }
        }

        //获取发射的方向
        private Quadrant GetQuadrant()
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector2 playerPosition = transform.position;
            
            //跟斜率直线进行比较
            bool higherPositive = HigherThanPositiveSlope(mousePosition);
            bool higherNegative = HigherThanNegativeSlope(mousePosition);

            if (!higherPositive && higherNegative)
            {
                return Quadrant.East;
            }
            else if (!higherPositive && !higherNegative)
            {
                return Quadrant.South;
            }
            else if (higherPositive && !higherNegative)
            {
                return Quadrant.West;
            }
            else
            {
                return Quadrant.North;
            }
        }

        //与左下到右上的直线斜率进行比较
        private bool HigherThanPositiveSlope(Vector2 inputPosition)
        {
            //玩家坐标
            Vector2 playerPosition = gameObject.transform.position;
            //鼠标点击坐标
            Vector2 mousePosition = _localCamera.ScreenToWorldPoint(inputPosition);
            
            //根据直线公式 y = mx + b，所以 b = y - mx
            //玩家到世界坐标原点直线的b
            float yIntercept = playerPosition.y - _positiveSlope * playerPosition.x;
            //鼠标到世界坐标原点直线的b
            float inputIntercept = mousePosition.y - _positiveSlope * mousePosition.x;
            
            //比较b，即可得到该玩家到鼠标的直线位于上半部分还是下半部分
            return inputIntercept > yIntercept;
        }
        
        //与左上到右下的直线斜率进行比较
        private bool HigherThanNegativeSlope(Vector2 inputPosition)
        {
            //玩家坐标
            Vector2 playerPosition = gameObject.transform.position;
            //鼠标点击坐标
            Vector2 mousePosition = _localCamera.ScreenToWorldPoint(inputPosition);
            
            //根据直线公式 y = mx + b，所以 b = y - mx
            //玩家到世界坐标原点直线的b
            float yIntercept = playerPosition.y - _negativeSlope * playerPosition.x;
            //鼠标到世界坐标原点直线的b
            float inputIntercept = mousePosition.y - _negativeSlope * mousePosition.x;
            
            //比较b，即可得到该玩家到鼠标的直线位于左半部分还是右半部分
            return inputIntercept > yIntercept;
        }

        //获取两点连线的斜率
        private float GetSlop(Vector3 second, Vector3 first)
        {
            return (second.y - first.y) / (second.x - first.x);
        }
    }   
}
