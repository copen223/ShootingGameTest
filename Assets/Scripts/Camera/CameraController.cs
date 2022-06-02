using System;
using System.Collections;
using System.Collections.Generic;
using ActorModule.Player;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distanceOfDirection;
    [SerializeField] private float speed;
    [SerializeField] private float minDistanceToZero;
    public float test;
    
    private Vector3 targetPosition = new Vector3(0,0,0);

    private void Awake()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 offset_Direction = Vector3.zero;
        if (target.TryGetComponent(out PlayerController player))
        {
            offset_Direction = player.ViewDirection;
        }
        targetPosition = target.transform.position + offset + offset_Direction * distanceOfDirection;

        var curPos = transform.position;
        Vector2 direction = targetPosition - curPos;
        Vector2 moveVector = direction.normalized * speed * Time.deltaTime; // 一帧移动的距离
        
        float disX = Mathf.Abs(curPos.x - targetPosition.x);
        float disY = Mathf.Abs(curPos.y - targetPosition.y);

        float finalX = disX > Mathf.Epsilon? Mathf.Lerp(curPos.x, targetPosition.x, Mathf.Abs(moveVector.x) / disX)
            : targetPosition.x;
        float finalY = disY > Mathf.Epsilon? Mathf.Lerp(curPos.y, targetPosition.y, Mathf.Abs(moveVector.y) / disY)
            : targetPosition.y;
        
        transform.position = new Vector3(finalX, finalY, targetPosition.z);
        /*if (disX < moveVector.x)
            transform.position = new Vector3(targetPosition.x, curTargetPos.y, targetPosition.z);
        if (disY < moveVector.y)
            transform.position = new Vector3(curTargetPos.x, targetPosition.y, targetPosition.z);*/
    }
}
