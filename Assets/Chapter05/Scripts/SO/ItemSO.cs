using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chapter05.Scripts.SO
{
    //物品SO
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item", order = 1)]
    public class ItemSO : ScriptableObject
    {
        //物品名称
        public string objName;
        //物品图案
        public Sprite sprite;
        //物品数量/包含数量
        public int quantity;
        //是否可堆叠
        public bool stackable;
        //物品类型
        public ItemType itemType;
        
        //物品类型枚举
        public enum ItemType
        {
            //硬币
            COIN,
            //血瓶
            HEALTH
        }
    }   
}
