using System;
using UnityEngine;

namespace RPG2D
{
    [Serializable]
    public class Air : BaseClasses.Block
    {
        public override bool Init(Vector3Int position)
        {
            Position = position;

            Init();
            
            return true;
        }

        public override bool Init()
        {
            API.World.IssueBlockUpdate(this);
            
            InternalName = "Air";
            
            return true;
        }

        public override void BlockUpdate(BaseClasses.Block cause)
        {
            
        }

        public override BaseClasses.Block Copy()
        {
            return new Air();
        }

        public override void Destroy()
        {
            
        }
    }
}