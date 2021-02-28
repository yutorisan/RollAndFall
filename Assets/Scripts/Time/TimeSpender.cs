using System;
using System.Collections;
using System.Collections.Generic;
using RAF.Inventory;
using UniRx;
using UnityEngine;
using Zenject;

namespace RAF.Time
{
    public class TimeSpender : MonoBehaviour, ITimeInfluenced
    {
        [Inject]
        private ITimeInventorySpendable m_timeSpender;

        /// <summary>
        /// 時間が流れているかどうか
        /// </summary>
        private bool m_timeFlowing = true;

        public void Pause()
        {
            m_timeFlowing = false;
        }

        public void Resume()
        {
            m_timeFlowing = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            //時間が流れている間だけ、100ms毎にSpendする
            Observable.Interval(TimeSpan.FromSeconds(0.1))
                      .Where(_ => m_timeFlowing)
                      .AsUnitObservable()
                      .Subscribe(_ => m_timeSpender.Spend(0.1f));
        }
    }
}