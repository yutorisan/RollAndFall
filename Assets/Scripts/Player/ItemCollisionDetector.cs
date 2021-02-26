using System;
using System.Collections;
using System.Collections.Generic;
using RAF.Item;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RAF.Player
{
    public interface IObservableGetItem
    {
        IObservable<IItem> GettedItem { get; }
    }
    public class ItemCollisionDetector : MonoBehaviour, IObservableGetItem
    {
        public IObservable<IItem> GettedItem { get; private set; }

        // Start is called before the first frame update
        void Awake()
        {
            GettedItem =
            this.OnTriggerEnter2DAsObservable()
                .Where(col => col.tag == "Item")
                .Do(col => col.gameObject.SetActive(false))
                .Select(col => col.GetComponent<IItem>())
                .Share();
        }

    }
}