using System.Collections;
using System.Collections.Generic;
using Chapter06.Scripts;
using UnityEngine;

namespace Chapter05.Scripts
{
    //角色抽象
    public abstract class Character : MonoBehaviour
    {
        //最大血量
        public float maxHitPoints;
        //初始血量
        public float startingHitPoints;

        //杀死角色
        public virtual void KillCharacter()
        {
            Destroy(gameObject);
        }

        //受伤时闪烁
        public virtual IEnumerator FlickerCharater()
        {
            //将精灵渲染底色变为红色
            GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        //重置角色
        public abstract void ResetCharacter();

        //伤害角色，单次+持续
        public abstract IEnumerator DamageCharacter(int damage, int interval);
    }
}
