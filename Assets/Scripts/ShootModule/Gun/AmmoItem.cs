using System;
using ActorModule;
using ActorModule.Player;
using Manager;
using TMPro;
using UnityEngine;

namespace ShootModule.Gun
{
    public class AmmoItem : MonoBehaviour
    {
        public SpriteRenderer renderer;
        public DamageInfo.ElementType elementType;
        public int num;

        [SerializeField] private AudioSource audio;

        [SerializeField] private TextMeshPro text;
        
        private void Start()
        {
            renderer.color = GameManager.Instance.GetElementColor(elementType);
        }

        private void Update()
        {
            text.text = num + "";
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerHit player))
            {
                var gun = player.player.gun;
                
                audio.Play();
                audio.transform.parent = GameManager.Instance.bulletParent;

                gun.AddAmmo(elementType, num);
                
                gameObject.SetActive(false);
            }
        }
    }
}
