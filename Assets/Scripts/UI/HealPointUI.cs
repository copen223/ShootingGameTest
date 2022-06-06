using System;
using ActorModule.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealPointUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private PlayerMono player;
        private void SetHealPointValue(float hpValue)
        {
            slider.value = hpValue;
        }

        private void Start()
        {
            player.OnHealPointChangeEvent += SetHealPointValue;
        }
    }
}
