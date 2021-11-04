using System;
using System.Collections;
using System.Collections.Generic;
using Chapter05.Scripts;
using UnityEngine;

namespace Chapter07.Scripts
{
    //敌方角色
    public class Enemy : Character
    {
        [Header("伤害值")]
        [SerializeField]
        private int _damageStrength = 2;
        
        //生命值
        private float _hitPoint;
        //当前伤害协程
        private Coroutine _damageCoroutine;

        private void OnEnable()
        {
            ResetCharacter();
        }

        public override void ResetCharacter()
        {
            _hitPoint = startingHitPoints;
        }

        public override IEnumerator DamageCharacter(int damage, int interval)
        {
            while (true)
            {
                //受伤闪烁
                StartCoroutine(FlickerCharater());
                _hitPoint -= damage;
                //如果生命值已经为0，杀死角色
                if (_hitPoint <= float.Epsilon)
                {
                    KillCharacter();
                    break;
                }

                if (interval > float.Epsilon)
                {
                    yield return new WaitForSeconds(interval);
                }
                else
                {
                    break;
                }
            }
        }
        
        //检测碰到玩家
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.LogError("角色伤害");
            if (other.gameObject.CompareTag("Player"))
            {
                if (null == _damageCoroutine)
                {
                    Player player = other.gameObject.GetComponent<Player>();
                    _damageCoroutine = StartCoroutine(player.DamageCharacter(_damageStrength, 1));
                }
            }
        }

        //检测离开玩家
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (null != _damageCoroutine)
                {
                    StopCoroutine(_damageCoroutine);
                    _damageCoroutine = null;
                }
            }
        }
    }
}