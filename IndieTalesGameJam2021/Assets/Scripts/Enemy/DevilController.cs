using System;
using System.Collections;
using System.Collections.Generic;
using Aarthificial.Reanimation;
using ThunderNut.Extensions;
using UnityEngine;
using UnityEngine.Timeline;

public class DevilController : MonoBehaviour {
    private static class Drivers {
        public const string IsMoving = "isMoving";
        public const string IsMovingHorizontal = "isMovingHorizontal";
        public const string IsMovingRight = "isMovingRight";
        public const string IsMovingUp = "isMovingUp";

        public const string WalkLeft = "walkLeft";
        public const string WalkRight = "walkRight";
        public const string WalkUp = "walkUp";
        public const string WalkDown = "walkDown";
        public const string Idle = "idle";
    }


    [SerializeField] private float amplitude;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask playerLayer;
    private Rigidbody2D rb;
    private Reanimator reanimator;
    private CollisionDetection collisionDetection;
    private RaycastHit2D hit2D;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Hover();
    }

    private void FixedUpdate() {
        var isHit = Helper.Raycast(transform.position, Vector2.right, detectionDistance, playerLayer, out hit2D);
        if (isHit && hit2D.transform != null) {
            Debug.Log(hit2D.transform.name.WithColour(Color.red));
            Attack();
        }

        Debug.DrawRay(transform.position, Vector2.right * detectionDistance, isHit ? Color.green : Color.red);
    }

    private void Attack() {
        transform.position = hit2D.transform.position;
        this.CallWithDelay(() =>
                hit2D.transform.gameObject.SetActive(false)
            , 1f);
    }


    private void Hover() {
        var lastPosition = transform.position;
        lastPosition.y += Mathf.Cos(Mathf.PI * Time.time) * amplitude;
        transform.position = lastPosition;
    }

    private void UpdateMovementState() {
        var previousVelocity = collisionDetection.rigidbody2D.velocity;
        var velocityChange = Vector2.zero;

        velocityChange.x = (walkSpeed - previousVelocity.x) / 4;
        velocityChange.y = (walkSpeed - previousVelocity.y) / 4;


        if (collisionDetection.wallContact.HasValue) {
            var wallDirection = (int) Mathf.Sign(collisionDetection.wallContact.Value.point.x - transform.position.x);
            var walkDirection = (int) Mathf.Sign(1);

            if (walkDirection == wallDirection)
                velocityChange.x = 0;
        }

        collisionDetection.rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
    }
}