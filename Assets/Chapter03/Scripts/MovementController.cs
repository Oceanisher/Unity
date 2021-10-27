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

        // Update is called once per frame
        void Update()
        {
        
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
        }
    }

    enum AnimationState
    {
        MoveEast = 1,
        MoveWest = 2,
        MoveNorth = 3,
        MoveSouth = 4,
        Idle = 5,
    }
}
