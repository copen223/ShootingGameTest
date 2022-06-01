﻿using System;
using UnityEngine;

namespace ShootModule.Gun.Bullets
{
    public class NormalBullet:Bullet
    {
        [SerializeField] Rigidbody2D rigidbody;
        
        [SerializeField] private float shootImpulse;
        [SerializeField] private float maxTime_Existence;
        private float time_Existence = 0;

        private void Update()
        {
            time_Existence += Time.deltaTime;
            if(time_Existence >= maxTime_Existence)
               OnEndUsing();
        }

        public override void OnReset()
        {
            time_Existence = 0;
            gameObject.SetActive(true);
        }

        public override void ShootTo(Vector2 direction)
        {
            rigidbody.AddForce(direction.normalized * shootImpulse,ForceMode2D.Impulse);
        }
    }
}