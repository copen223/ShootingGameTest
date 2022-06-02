using UnityEngine;

namespace ActorModule.Player
{
    public class PlayerMono : ActorMono
    {
        public float SpeedMultiply_Aiming = 0.7f;
        public override void BeHit(DamageInfo info)
        {
            
        }

        public override void Death()
        {
            throw new System.NotImplementedException();
        }
    }
}
