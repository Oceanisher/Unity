using System;
using System.Collections;
using System.Collections.Generic;
using Chapter07.Scripts;
using UnityEngine;

namespace Chapter08.Scripts
{
    //子弹类
    public class Ammo : MonoBehaviour
    {
        [Header("子弹伤害程度")]
        [SerializeField]
        private int _damageInflicted;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other is BoxCollider2D)
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                StartCoroutine(enemy.DamageCharacter(_damageInflicted, 0));
                gameObject.SetActive(false);
            }
        }
    }
}
