using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAF.Item
{
    public interface IItem
    {
        void Use();
        void Sound(AudioSource audioSource);
    }
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        [SerializeField]
        private AudioClip clip;

        public abstract void Use();
        public void Sound(AudioSource audioSource)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}