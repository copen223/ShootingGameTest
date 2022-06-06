using System;
using ActorModule.Player;
using UnityEngine;

namespace ActorModule.Monster
{
    public class DamageBody : MonoBehaviour
    {
        public float damage;
        public DamageInfo.ElementType damageElementType;
        public ActorMono sourceActor;
        public bool ifOnlyDamagePlayer;
        
        private void OnTriggerStay2D(Collider2D other)
        {
//            Debug.Log("2");
            if (other.TryGetComponent(out BeHitPoint behit))
            {
                if(behit.actor == sourceActor)
                    return;
                if(ifOnlyDamagePlayer && !(behit.actor is PlayerMono))
                    return;
                
                behit.BeHit(this);
            }
        }
    }
}
