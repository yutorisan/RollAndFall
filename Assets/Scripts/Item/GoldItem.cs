using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAF.Item
{
    public class GoldItem : ItemBase
    {
        public override void Use()
        {
            print("ゴールドを獲得！");
        }

        // Start is called before the first frame update
        void Start()
        {

        }

    }
}