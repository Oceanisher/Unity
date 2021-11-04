using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Chapter03.Scripts
{
    //移动
    public class MovementController : MonoBehaviour
    {
        //速度
        [SerializeField]
        private float speed;

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private string _aniStateStr = "AnimationState";
        private Vector2 _movement = new Vector2();
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            ListenBoard();
            MoveCharacter();
            ChangeAni();
        }

        //键盘监听
        private void ListenBoard()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
            _movement.Normalize();
        }

        //移动角色
        private void MoveCharacter()
        {
            _rigidbody2D.velocity = _movement * speed;
        }

        //改变动画
        private void ChangeAni()
        {
            //如果按键趋近于0，那么停止移动
            if (Mathf.Approximately(_movement.x, 0) 
                && Mathf.Approximately(_movement.y, 0))
            {
                _animator.SetBool("IsWalking", false);
            }
            else
            {
                _animator.SetBool("IsWalking", true);
                _animator.SetFloat("xDir", _movement.x);
                _animator.SetFloat("yDir", _movement.y);
            }

            /*
             //使用Blend Tree之后，不再需要此种方式
             if (_movement.x > 0)
            {
                _animator.SetInteger(_aniStateStr, (int)AnimationState.MoveEast);
            }
            else if (_movement.x < 0)
            {
                _animator.SetInteger(_aniStateStr, (int)AnimationState.MoveWest);
            }
            else if (_movement.y > 0)
            {
                _animator.SetInteger(_aniStateStr, (int)AnimationState.MoveNorth);
            }
            else if (_movement.y < 0)
            {
                _animator.SetInteger(_aniStateStr, (int)AnimationState.MoveSouth);
            }
            else
            {
                _animator.SetInteger(_aniStateStr, (int)AnimationState.Idle);
            }
             */
        }
    }

    /*
    //使用Blend Tree之后，不再需要枚举
    enum AnimationState
    {
        MoveEast = 1,
        MoveWest = 2,
        MoveNorth = 3,
        MoveSouth = 4,
        Idle = 5,
    }
     */
}
