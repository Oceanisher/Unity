using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Chapter08.Scripts
{
    //巡逻代码
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Animator))]
    public class Wander : MonoBehaviour
    {
        [Header("追击速度")]
        [SerializeField]
        private float _pursuitSpeed;
        [Header("巡逻速度")]
        [SerializeField]
        private float _wanderSpeed;
        [Header("改变巡逻方向的频率")]
        [SerializeField]
        private float _directionChangeInterval;
        [Header("是否追击玩家")]
        [SerializeField]
        private bool _followPlayer;
       
        //当前速度
        private float _currentSpeed;
        //巡逻协程
        private Coroutine _coroutine;
        //目标Transform
        private Transform _targetTrs;
        //巡逻目的地
        private Vector3 _endPos;
        //改变巡逻方向时增加的角度
        private float _currentAngle;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private CircleCollider2D _circleCollider2D;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _currentSpeed = _wanderSpeed;
            StartCoroutine(WanderRoutine());
        }

        private void Update()
        {
            Debug.DrawLine(_rb.position, _endPos, Color.red);
        }

        //总协程
        private IEnumerator WanderRoutine()
        {
            while (true)
            {
                //选择一个新的目的地
                ChooseNewEndpoint();
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }

                _coroutine = StartCoroutine(Move(_rb, _currentSpeed));
                yield return new WaitForSeconds(_directionChangeInterval);
            }
        }

        //执行移动协程
        private IEnumerator Move(Rigidbody2D rbToMove, float speed)
        {
            //计算出剩余移动距离
            float remainDistance = (transform.position - _endPos).sqrMagnitude;
            //如果需要继续移动
            while (remainDistance > float.Epsilon)
            {
                //判断是否处于追击状态，如果是，那么目的地改成玩家
                if (_targetTrs != null)
                {
                    _endPos = _targetTrs.position;
                }
                
                //移动
                if (rbToMove != null)
                {
                    //开启移动动画
                    _animator.SetBool("IsWalking", true);
                    //计算出本帧应该的位置
                    Vector3 currentFramePosition = Vector3.MoveTowards(
                        rbToMove.position, 
                        _endPos, 
                        speed * Time.deltaTime);
                    _rb.MovePosition(currentFramePosition);
                    //更新剩余距离
                    remainDistance = (transform.position - _endPos).sqrMagnitude;
                }
                //让步到下一个固定帧更新
                yield return new WaitForFixedUpdate();
            }
            _animator.SetBool("IsWalking", false);
        }
        
        //看到玩家
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && _followPlayer)
            {
                _currentSpeed = _pursuitSpeed;
                _targetTrs = other.gameObject.transform;
                
                //结束原先的线程
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }

                //开启追击线程
                _coroutine = StartCoroutine(Move(_rb, _currentSpeed));
            }
        }

        //玩家消失
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && _followPlayer)
            {
                //为了让敌人困惑一会
                _animator.SetBool("IsWalking", false);
                
                _currentSpeed = _wanderSpeed;
                _targetTrs = null;
                
                //结束原先的线程
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }
            }
        }

        //绘制Gizmo下的敌人视野圆圈
        private void OnDrawGizmos()
        {
            if (null != _circleCollider2D)
            {
                Gizmos.DrawWireSphere(transform.position, _circleCollider2D.radius);
            }
        }

        //选择新的巡逻点
        private void ChooseNewEndpoint()
        {
            _currentAngle += Random.Range(0, 360);
            //相当于取模
            _currentAngle = Mathf.Repeat(_currentAngle, 360);
            _endPos += Vector3FromAngle(_currentAngle);
        }

        //角度转弧度、弧度转方向
        private Vector3 Vector3FromAngle(float inputAngleDegree)
        {
            //角度*弧度转换常量=弧度，1弧度约等于57.3度，360度等于2π
            float inputAngleRadian = inputAngleDegree * Mathf.Deg2Rad;
            //弧度转方向，且是单位向量；由于函数入参是弧度，所以必须用弧度
            return new Vector3(Mathf.Cos(inputAngleRadian), Mathf.Sin(inputAngleRadian), 0);
        }
    }
}