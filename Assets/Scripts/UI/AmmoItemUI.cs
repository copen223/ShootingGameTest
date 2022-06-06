using System;
using ShootModule.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AmmoItemUI : MonoBehaviour
    {
        public GameObject AmmoSprite;
        public GameObject AmmoSelectionSprite;
        public Ammo AmmoMono;
        public Text LoadedAmmoNum;
        public Text AmmoNum;

        public bool ifSelectThis = false;

        public void SetThisAmmo(bool ifSetThis)
        {
            AmmoSprite.SetActive(ifSetThis);
            ifSelectThis = ifSetThis;

            AmmoSelectionSprite.transform.localScale = ifSetThis ? Vector3.one : Vector3.one * 0.5f;

        }

        public void Update()
        {
            if (ifSelectThis)
            {
                LoadedAmmoNum.text = AmmoMono.ammoLoad + "";
                AmmoNum.text = AmmoMono.ammoReserve + "";
            }
        }
    }
}