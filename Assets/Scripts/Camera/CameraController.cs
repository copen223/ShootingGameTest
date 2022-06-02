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

        var position = transform.position;
        Vector2 direction = targetPosition - position;
        Vector2 moveVector = direction.normalized * speed * Time.deltaTime;
        
        var curTargetPos = (Vector2)position + moveVector;
        float disX = Mathf.Abs(transform.position.x - targetPosition.x);
        float disY = Mathf.Abs(transform.position.y - targetPosition.y);
        
        transform.position = new Vector3(curTargetPos.x, curTargetPos.y, targetPosition.z);
        if (disX < minDistanceToZero)
            transform.position = new Vector3(targetPosition.x, curTargetPos.y, targetPosition.z);
        if (disY < minDistanceToZero)
            transform.position = new Vector3(curTargetPos.x, targetPosition.y, targetPosition.z);

        test = disX;


    }
}
