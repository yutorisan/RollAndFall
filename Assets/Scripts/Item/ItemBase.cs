using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAF.Item
{
    public interface IItem
    {

    }
    public abstract class ItemBase : MonoBehaviour, IItem
    {
        // Start is called before the first frame update
        void Start()
        {

        }
    }
}