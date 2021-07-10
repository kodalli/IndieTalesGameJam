using System;
using System.Collections;
using System.Collections.Generic;
using Aarthificial.Reanimation;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private static class Drivers {
        public const string State = "state";
        public const string IsMoving = "isMoving";
        public const string IsMovingHorizontal = "isMovingHorizontal";
        public const string IsMovingRight = "isMovingRight";
        public const string IsMovingUp = "isMovingUp";
    }


    [SerializeField] private PlayerInputData playerInputData;

    [SerializeField] float walkSpeed = 7;

    private Reanimator reanimator;
    private CollisionDetection collisionDetection;

    [HideInInspector] public int enemyLayer;

    private void Awake() {
        reanimator = GetComponent<Reanimator>();
        collisionDetection = GetComponent<CollisionDetection>();
        enemyLayer = LayerMask.NameToLayer($"Enemy");
    }
    

    private void Update() {
        reanimator.Set(Drivers.IsMoving, playerInputData.MovementInput != Vector2.zero);
        reanimator.Set(Drivers.IsMovingHorizontal, playerInputData.MovementInput.x != 0);
        reanimator.Set(Drivers.IsMovingRight, playerInputData.MovementInput.x > 0);
        reanimator.Set(Drivers.IsMovingUp, playerInputData.MovementInput.y > 0);
    }

    private void FixedUpdate() {
        UpdateMovementState();
    }

    private void UpdateMovementState() {
        var previousVelocity = collisionDetection.rigidbody2D.velocity;
        var velocityChange = Vector2.zero;

        velocityChange.x = (playerInputData.MovementInput.x * walkSpeed - previousVelocity.x) / 4;
        velocityChange.y = (playerInputData.MovementInput.y * walkSpeed - previousVelocity.y) / 4;

        if (collisionDetection.wallContact.HasValue) {
            var wallDirection = (int) Mathf.Sign(collisionDetection.wallContact.Value.point.x - transform.position.x);
            var walkDirection = (int) Mathf.Sign(playerInputData.MovementInput.x);

            if (walkDirection == wallDirection)
                velocityChange.x = 0;
        }
        collisionDetection.rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
    }
}