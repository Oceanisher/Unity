using System;
using System.Collections;
using System.Collections.Generic;
using Chapter05.Scripts.SO;
using Chapter06.Scripts;
using UnityEngine;

namespace Chapter05.Scripts
{
    //玩家脚本
    public class Player : Character
    {
        //当前血量
        [Header("生命值")]
        [SerializeField]
        private HitPointSO hitPointsSO;
        
        [Header("生命条预制")]
        [SerializeField]
        private HealthBar _healthBarPrefab;
        
        [Header("背包预制")]
        [SerializeField]
        private Inventory _inventoryPrefab;

        //生命条
        private HealthBar _healthBar;
        //背包
        private Inventory _inventory;
        
        // Start is called before the first frame update
        void Start()
        {
            ResetCharacter();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //如果是可拾取物品
            if (other.gameObject.CompareTag("ItemCanPickedUp"))
            {
                ItemSO item = other.gameObject.GetComponent<Consumable>().item;
                if (null != item)
                {
                    //是否应该消失
                    bool shouldDisappear = false;
                    switch (item.itemType)
                    {
                        case ItemSO.ItemType.COIN:
                            Debug.LogError($"[test]:捡到金币");
                            // shouldDisappear = true;
                            shouldDisappear = _inventory.AddItem(item);
                            break;
                        case ItemSO.ItemType.HEALTH:
                            Debug.LogError($"[test]:捡到血瓶");
                            shouldDisappear = AdjustHp(other.gameObject.GetComponent<Consumable>().item.quantity);
                            break;
                    }

                    if (shouldDisappear)
                    {
                        other.gameObject.SetActive(false);
                    }
                }
            }
        }

        //喝血瓶操作，满血则不喝
        private bool AdjustHp(int amount)
        {
            if (hitPointsSO.value >= maxHitPoints)
            {
                return false;
            }

            hitPointsSO.value = (hitPointsSO.value + amount) > maxHitPoints
                ? maxHitPoints
                : (hitPointsSO.value + amount);
            return true;
        }

        public override void KillCharacter()
        {
            base.KillCharacter();
            
            Destroy(_healthBar.gameObject);
            Destroy(_inventory.gameObject);
        }

        public override void ResetCharacter()
        {
            hitPointsSO.value = startingHitPoints;
            _healthBar = Instantiate(_healthBarPrefab);
            _healthBar.player = this;

            _inventory = Instantiate(_inventoryPrefab);
        }

        public override IEnumerator DamageCharacter(int damage, int interval)
        {
            while (true)
            {
                //受伤闪烁
                StartCoroutine(FlickerCharater());
                hitPointsSO.value -= damage;
                //如果生命值已经为0，杀死角色
                if (hitPointsSO.value <= float.Epsilon)
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
    }
}