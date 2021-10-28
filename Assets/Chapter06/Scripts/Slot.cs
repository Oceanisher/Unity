using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chapter06.Scripts
{
    //背包格子
    public class Slot : MonoBehaviour
    {
        [Header("物体数量")]
        [SerializeField]
        private Text _text;

        public Text QuantityText => _text;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }   
}
