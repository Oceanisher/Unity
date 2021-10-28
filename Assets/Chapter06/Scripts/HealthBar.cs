using System.Collections;
using System.Collections.Generic;
using Chapter05.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Chapter06.Scripts
{
    //生命条
    public class HealthBar : MonoBehaviour
    {
        [Header("生命值配置")]
        [SerializeField]
        private HitPointSO _hitPointSo;

        [Header("生命条")]
        [SerializeField]
        private Image _meterImage;

        [Header("生命值-文本")]
        [SerializeField]
        private Text _hpText;
        
        //玩家
        [HideInInspector]
        public Player player;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (null != player)
            {
                _meterImage.fillAmount = _hitPointSo.value / player.maxHitPoints;
                _hpText.text = "HP:" + (int)(_meterImage.fillAmount * 100);
            }
        }
    }
}
