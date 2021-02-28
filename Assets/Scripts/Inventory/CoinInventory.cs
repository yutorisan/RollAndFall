using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace RAF.Inventory
{
    public interface ICoinInventoryAddable
    {
        void Add();
    }
    public interface IObservableCoinInventory
    {
        IObservable<int> Coin { get; }
    }

    public class CoinInventory : ICoinInventoryAddable, IObservableCoinInventory
    {
        private readonly ReactiveProperty<int> m_coins = new ReactiveProperty<int>();

        public IObservable<int> Coin => m_coins.AsObservable();

        public void Add()
        {
            ++m_coins.Value;
        }


    }
}