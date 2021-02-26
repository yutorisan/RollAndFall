using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAF.Item
{
    public interface IItem
    {
        void Use();
    }
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        public abstract void Use();
    }
}