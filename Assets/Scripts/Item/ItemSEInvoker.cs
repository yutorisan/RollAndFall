using System.Collections;
using System.Collections.Generic;
using RAF.Player;
using UnityEngine;
using Zenject;
using UniRx;

namespace RAF.Item
{
    public class ItemSEInvoker : MonoBehaviour
    {
        [Inject]
        private IObservableGetItem observableGetItem;

        // Start is called before the first frame update
        void Start()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            observableGetItem.GettedItem
                             .Subscribe(item => item.Sound(audioSource));
        }
    }
}