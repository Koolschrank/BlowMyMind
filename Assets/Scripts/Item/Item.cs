using System;
using UnityEngine;

namespace Item
{
    public abstract class Item : MonoBehaviour
    {
        public Action Depleted;
        public Action OnUseCompleted;
        
        public bool InUse { get; private set; }

        protected PlayerCaracter Player;
        
        public abstract void Initialize(PlayerCaracter player);
        public abstract void Use();
    }
}
