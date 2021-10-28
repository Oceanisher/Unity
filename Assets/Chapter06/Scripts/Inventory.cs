using System.Collections;
using System.Collections.Generic;
using Chapter05.Scripts.SO;
using UnityEngine;
using UnityEngine.UI;

namespace Chapter06.Scripts
{
    //背包
    public class Inventory : MonoBehaviour
    {
        //背包格子数量
        private const int _slotNum = 5;
        
        [Header("背包根节点")]
        [SerializeField]
        private GameObject _inventoryGO;
        
        [Header("格子预制")]
        [SerializeField]
        private GameObject _slotPrefab;

        //背包格子数组
        private GameObject[] _slots = new GameObject[_slotNum];
        //背包格子图片数组
        private Image[] _itemImages = new Image[_slotNum];
        //背包格子配置数组
        private ItemSO[] _itemSos = new ItemSO[_slotNum];
        
        // Start is called before the first frame update
        void Start()
        {
            //创建格子
            CreateSlots();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //创建背包格子
        private void CreateSlots()
        {
            if (null == _slotPrefab)
            {
                return;
            }

            for (int i = 0; i < _slotNum; i++)
            {
                //创建新格子
                GameObject newSlot = Instantiate(_slotPrefab, _inventoryGO.transform);
                newSlot.name = "Slot_" + i;
                
                //设置父节点
                // newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);

                _slots[i] = newSlot;
                _itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }

        //添加物品到背包
        public bool AddItem(ItemSO itemSo)
        {
            for (int i = 0; i < _itemSos.Length; i++)
            {
                //已存在、且可堆叠
                if (null != _itemSos[i]
                    && _itemSos[i].itemType == itemSo.itemType
                    && _itemSos[i].stackable)
                {
                    //数量增加
                    _itemSos[i].quantity += itemSo.quantity;
                    //文字变更
                    Slot tempSlot = _slots[i].GetComponent<Slot>();
                    tempSlot.QuantityText.enabled = true;
                    tempSlot.QuantityText.text = _itemSos[i].quantity.ToString();
                    return true;
                }
                
                //其他情况，直接找空格子放入
                if (null == _itemSos[i])
                {
                    //复制一个，是为了不改变原始的so
                    _itemSos[i] = Instantiate(itemSo);
                    _itemSos[i].quantity = 1;
                    _itemImages[i].sprite = _itemSos[i].sprite;
                    _itemImages[i].enabled = true;
                    //文字变更
                    Slot tempSlot = _slots[i].GetComponent<Slot>();
                    tempSlot.QuantityText.enabled = true;
                    tempSlot.QuantityText.text = _itemSos[i].quantity.ToString();
                    return true;
                }
            }
            //无法放入
            return false;
        }
    }   
}
