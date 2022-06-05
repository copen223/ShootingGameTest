using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Timers;
using TMPro;
using Tool;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace FloatTextModule
{
    public class FloatText: TargetInPool
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float bigSize;
        [SerializeField] private float smallSize;
        [SerializeField] private Color _color;

        [Header("动画偏移")] 
        [SerializeField] private float offsetX;
        [SerializeField] private float offsetY;
        [SerializeField] private float existTime;
        public Vector2 StartPos = Vector2.zero;
        private float timer = 0;
        
        
        public override void OnReset()
        {
            gameObject.SetActive(true);
        }

        public void StartMove()
        {
            timer = 0;
            StartCoroutine(OffsetAnimationCoroutine());
        }


        private IEnumerator OffsetAnimationCoroutine()
        {
            var finalX = Random.Range(offsetX / 2, offsetX);
            var finalY = Random.Range(offsetY / 2, offsetY);
            int dirX = Random.value > 0.5f ? 1 : -1;
            int dirY = Random.value > 0.5f ? 1 : -1;

            Vector2 finalPosition = (Vector2)StartPos + new Vector2(finalX * dirX, finalY * dirY);
            float startX = StartPos.x;
            float startY = StartPos.y;
            float startDisX = Mathf.Abs(finalPosition.x - dirX);

            while (timer < existTime)
            {
                if (Camera.main != null)
                {
                    Vector2 curPos = Camera.main.ScreenToWorldPoint(transform.position);
                    float curDisX = Mathf.Abs(finalPosition.x - curPos.x);
                
                    float Posx = Mathf.Lerp(startX, finalPosition.x, Mathf.Sin(90f * timer / existTime));
                    float Posy = Mathf.Lerp(startY, finalPosition.y, Mathf.Sin(90f * (1 - curDisX / startDisX)));

                    var position = new Vector3(Posx, Posy, -5);
                    transform.position = Camera.main.WorldToScreenPoint(position);
                }

                timer += Time.deltaTime;
                yield return null;
            }
            
            OnEndUsing();
        }

        /// <summary>
        /// 设置text
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="ifBig"></param>
        public void SetText(string _text,bool ifBig)
        {
            text.text = _text;
            if (ifBig)
                transform.localScale = new Vector3(bigSize, bigSize, 1);
            else
            {
                transform.localScale = new Vector3(smallSize, smallSize, 1);
            }
        }

        public void SetColor(Color _color)
        {
            text.color = _color;
        }
    }
}