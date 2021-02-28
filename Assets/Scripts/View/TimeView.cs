using System.Collections;
using System.Collections.Generic;
using RAF.Inventory;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RAF.View
{
    public class TimeView : MonoBehaviour
    {
        [Inject]
        private IObservableTimeInventory m_timeInventory;
        [SerializeField, Required]
        private TextMeshProUGUI m_text;
        [SerializeField, Required]
        private Slider m_slider;

        private float m_maxTime;
        // Start is called before the first frame update
        void Start()
        {
            m_maxTime = m_timeInventory.MaxTime;

            m_timeInventory.Time.Subscribe(time =>
            {
                m_text.text = $"{(int)time} s";
                m_slider.value = time / m_maxTime;
            });
        }
    }
}