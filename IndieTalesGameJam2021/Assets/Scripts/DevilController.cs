using System;
using System.Collections;
using System.Collections.Generic;
using Aarthificial.Reanimation;
using ThunderNut.Extensions;
using UnityEngine;
using UnityEngine.Timeline;

public class DevilController : MonoBehaviour {
    private static class Drivers {
        public const string IsMovingHorizontal = "isMovingHorizontal";
        public const string IsMovingRight = "isMovingRight";
        public const string IsMovingUp = "isMovingUp";
    }
    
    private Reanimator reanimator;
    private CollisionDetection collisionDetection;

    [SerializeField] private Transform[] path;
    [SerializeField] private float amplitude;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform parent;
    
    private RaycastHit2D hit2D;
    private int pathIndex;
    public Vector2 facingDirection = default;
    private bool isHit;

    private void Start() {
        pathIndex = 0;
        transform.position = path[pathIndex].position;
        reanimator = GetComponent<Reanimator>();
    }

    private void Update() {
        Move();
        
        reanimator.Set(Drivers.IsMovingHorizontal, MovingHorizontal());
        reanimator.Set(Drivers.IsMovingRight, MovingRight());
        reanimator.Set(Drivers.IsMovingUp, MovingUp());
    }

    private void FixedUpdate() {
        isHit = Helper.Raycast(transform.position, facingDirection, detectionDistance, playerLayer, out hit2D);
        if (isHit && hit2D.transform != null) {
            transform.localScale *= 5f;
            Debug.Log(hit2D.transform.name.WithColour(Color.red));
            Attack();
        }

        Debug.DrawRay(transform.position, facingDirection * detectionDistance, isHit ? Color.green : Color.red);
    }

    private void Attack() {
        transform.position = hit2D.transform.position;
        SoundManager.Instance.PlayDeathSound();
        this.CallWithDelay(() =>
               GameManager.Instance.GoToCurrentScene()
            , 0.75f);
    }

    private void Move() {
        if (isHit) return;
        if (pathIndex < path.Length) {
            // sine wave
            var lastPosition = parent.position;
            lastPosition.y += Mathf.Cos(Mathf.PI * Time.time) * amplitude;
            parent.position = lastPosition;
            
            // devil movement
            var position = transform.position;
            position = Vector2.MoveTowards(position, path[pathIndex].transform.position,
                walkSpeed * Time.deltaTime);
            transform.position = position;
            
            facingDirection = (path[pathIndex].position - position).normalized;
        }
        else {
            pathIndex = 0;
        }

        if (transform.position == path[pathIndex].position) {
            pathIndex++;
        }
    }

    private bool MovingRight() {
        return Mathf.Round(facingDirection.x) > 0f;
    }

    private bool MovingUp() {
        return Mathf.Round(facingDirection.y) > 0f;
    }

    private bool MovingHorizontal() {
        return Mathf.Round(facingDirection.x) != 0;
    }
}