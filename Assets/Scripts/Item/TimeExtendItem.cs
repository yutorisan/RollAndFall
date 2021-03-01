using System.Collections;
using System.Collections.Generic;
using RAF.Inventory;
using UnityEngine;
using Zenject;

namespace RAF.Item
{
    /// <summary>
    /// 制限時間延長アイテム
    /// </summary>
    public class TimeExtendItem : ItemBase
    {
        [Inject]
        private ITimeInventoryAddable timeInventoryAddable;

        public override void Use()
        {
            timeInventoryAddable.Add(5);
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}