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

        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            observableGetItem.GettedItem
                             .Subscribe(item => audioSource.Play());
        }
    }
}