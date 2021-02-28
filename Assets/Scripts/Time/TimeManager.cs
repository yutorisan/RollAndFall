using System.Collections;
using System.Collections.Generic;
using RAF.Inventory;
using RAF.Time;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace RAF
{
    public interface ITimeOperator
    {
        void Pause();
        void Resume();
    }
    public class TimeManager : SerializedMonoBehaviour, ITimeOperator
    {
        [SerializeField]
        private List<ITimeInfluenced> m_timeInfluenceds;

        public void Pause()
        {
            m_timeInfluenceds.ForEach(item => item.Pause());
        }

        public void Resume()
        {
            m_timeInfluenceds.ForEach(item => item.Resume());
        }
    }
}