using System.Collections;
using System.Collections.Generic;
using RAF.Time;
using UnityEngine;

namespace RAF.Player
{
    public class PlayerPauser : MonoBehaviour, ITimeInfluenced
    {
        private Rigidbody2D m_rd;

        private void Start()
        {
            m_rd = GetComponent<Rigidbody2D>();
        }

        public void Pause()
        {
            m_rd.Pause(gameObject);
        }

        public void Resume()
        {
            m_rd.Resume(gameObject);
        }
    }
}