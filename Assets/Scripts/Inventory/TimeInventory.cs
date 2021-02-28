using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;

namespace RAF.Inventory
{
    public interface ITimeInventoryAddable
    {
        void Add(float sec);
    }
    public interface ITimeInventorySpendable
    {
        void Spend(float sec);
    }
    public interface IObservableTimeInventory
    {
        IObservable<float> Time { get; }
        float MaxTime { get; }
    }
    public class TimeInventory : ITimeInventoryAddable, ITimeInventorySpendable, IObservableTimeInventory
    {
        private readonly ReactiveProperty<float> m_times = new ReactiveProperty<float>(30);

        public IObservable<float> Time => m_times.AsObservable();

        public float MaxTime => 30;

        public void Add(float sec)
        {
            m_times.Value += sec;
        }

        public void Spend(float sec)
        {
            m_times.Value -= sec;
        }
    }
}