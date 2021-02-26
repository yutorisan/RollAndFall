using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAF.Player
{
    public interface IPlayerPauser
    {
        void Pause();
        void Resume();
    }
    public class PlayerPauser : MonoBehaviour, IPlayerPauser
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