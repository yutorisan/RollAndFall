using System.Collections;
using System.Collections.Generic;
using RAF.Inventory;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

namespace RAF.View
{
    public class CoinView : MonoBehaviour
    {
        [Inject]
        private IObservableCoinInventory m_coinInventory;
        [SerializeField, Required]
        private TextMeshProUGUI m_coinText;

        // Start is called before the first frame update
        void Start()
        {
            m_coinInventory.Coin
                           .Select(n => n.ToString("N0")) //3桁ごとに区切りをつける
                           .Subscribe(txt => m_coinText.text = txt);
        }
    }
}