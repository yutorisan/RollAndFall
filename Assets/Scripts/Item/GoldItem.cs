using System.Collections;
using System.Collections.Generic;
using RAF.Inventory;
using UnityEngine;
using Zenject;

namespace RAF.Item
{
    public class GoldItem : ItemBase
    {
        [Inject]
        private ICoinInventoryAddable coinInventoryAddable;

        public override void Use()
        {
            coinInventoryAddable.Add();
        }
    }
}