using System;
using RPG2D.BaseClasses;
using UnityEngine;

namespace RPG2D
{
    [Serializable]
    public class Air : BaseClasses.Block
    {
        public override bool Init(Vector2Int position)
        {
            InternalName = "Air";
            
            return true;
        }

        public override bool Init()
        {
            return true;
        }

        public override Block Copy()
        {
            return new Air();
        }

        public override void Destroy()
        {
            
        }
    }
}