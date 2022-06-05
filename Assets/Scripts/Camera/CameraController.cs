using System;
using System.Collections;
using System.Collections.Generic;
using ActorModule.Player;
using UnityEngine;
using Random = System.Random;

public class CameraController : MonoBehaviour
{
    //----------相机追踪----------------
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distanceOfDirection;
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToZero;
    public float test;
    
    //-----------屏幕抖动--------------
    [SerializeField] private float ShakeAmplitude_weak;
    [SerializeField] private float ShakeAmplitude_Strong;
    [SerializeField] private float ShakeAttenuationSpeed;
    public Vector2 shakeOffset = Vector2.zero;
    
    
    private Vector3 targetPosition = new Vector3(0,0,0);
    private Vector3 basedPosition = Vector3.zero;

    public static CameraController MainInstance;
    
    private void Awake()
    {
        if (MainInstance == null)
            MainInstance = this;
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        basedPosition = transform.position;
    }

    private void Update()
    {
        Vector3 offset_Direction = Vector3.zero;
        if (target.TryGetComponent(out PlayerController player))
        {
            offset_Direction = player.ViewDirection;
        }
        targetPosition = target.transform.position + offset + offset_Direction * distanceOfDirection;

        var curPos = basedPosition;
        Vector2 direction = targetPosition - curPos;
        Vector2 moveVector = direction.normalized * speed * Time.deltaTime; // 一帧移动的距离
        
        float disX = Mathf.Abs(curPos.x - targetPosition.x);
        float disY = Mathf.Abs(curPos.y - targetPosition.y);

        float finalX = disX > Mathf.Epsilon? Mathf.Lerp(curPos.x, targetPosition.x, Mathf.Abs(moveVector.x) / disX)
            : targetPosition.x;
        float finalY = disY > Mathf.Epsilon? Mathf.Lerp(curPos.y, targetPosition.y, Mathf.Abs(moveVector.y) / disY)
            : targetPosition.y;
        
        basedPosition = new Vector3(finalX, finalY, targetPosition.z);
        transform.position = basedPosition + (Vector3) shakeOffset;
    }

    private float basedShakeAmplitude = 0;
    
    /// <summary>
    /// 让屏幕进行抖动
    /// </summary>
    /// <param name="ifStrong"></param>
    public void Shake(bool ifStrong)
    {
        float nextShakeAmplitude = ifStrong ? ShakeAmplitude_Strong : ShakeAmplitude_weak;
        if (basedShakeAmplitude < nextShakeAmplitude)
            basedShakeAmplitude = nextShakeAmplitude;
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        while (basedShakeAmplitude >= Mathf.Epsilon)
        {
            // 抖动
            float offsetY = UnityEngine.Random.Range(-basedShakeAmplitude, basedShakeAmplitude);
            float offsetX = UnityEngine.Random.Range(-basedShakeAmplitude, basedShakeAmplitude);

            shakeOffset = new Vector2(offsetX, offsetY);

            basedShakeAmplitude -= ShakeAttenuationSpeed * Time.deltaTime;
            
            yield return null;
        }

        shakeOffset = Vector2.zero;
    }
    
    
}
