using System;
using System.Collections;
using System.Collections.Generic;
using Aarthificial.Reanimation;
using Thunder.Extensions;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum PlayerState {
    Movement = 0,
    Dash = 1,
    Hit = 2,
}


public class PlayerController : MonoBehaviour {
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


    [SerializeField] private InputReader inputReader;

    [SerializeField] private float walkSpeed = 7;
    [SerializeField] private float dashSpeed = 12;
    [SerializeField] private FixedStopwatch dashStopwatch = new FixedStopwatch();


    private Reanimator reanimator;
    private CollisionDetection collisionDetection;
    private GameObject flashLight;

    public PlayerState State { get; set; } = PlayerState.Movement;
    public Vector2 MovementInput { get; private set; }

    private bool canDash;
    private int enemyLayer;
    private Vector2 facingDirection;
    private Vector2 lightDirectionInput;
    private Vector2 lightDirection;

    private void Awake() {
        reanimator = GetComponent<Reanimator>();
        collisionDetection = GetComponent<CollisionDetection>();
        flashLight = transform.Find("PointLight").gameObject;
        enemyLayer = LayerMask.NameToLayer($"Enemy");
    }

    private void OnEnable() {
        inputReader.MoveEvent += OnMove;
        inputReader.AttackEvent += OnDash;
        inputReader.MousePosEvent += OnMouse;
        
        reanimator.AddListener(Drivers.WalkRight, () => SetFacingDirection(1, 0));
        reanimator.AddListener(Drivers.WalkLeft, () => SetFacingDirection(-1, 0));
        reanimator.AddListener(Drivers.WalkUp, () => SetFacingDirection(0, 1));
        reanimator.AddListener(Drivers.WalkDown, () => SetFacingDirection(0, -1));
        reanimator.AddListener(Drivers.Idle, () => SetFacingDirection(0, -1));
    }

    private void OnDisable() {
        inputReader.MoveEvent -= OnMove;
        inputReader.AttackEvent -= OnDash;
        inputReader.MousePosEvent -= OnMouse;

        reanimator.RemoveListener(Drivers.WalkRight, () => SetFacingDirection(1, 0));
        reanimator.RemoveListener(Drivers.WalkLeft, () => SetFacingDirection(-1, 0));
        reanimator.RemoveListener(Drivers.WalkUp, () => SetFacingDirection(0, 1));
        reanimator.RemoveListener(Drivers.WalkDown, () => SetFacingDirection(0, -1));
        reanimator.RemoveListener(Drivers.Idle, () => SetFacingDirection(0, -1));
    }
    

    private void Update() {
        UpdateLightDirection();
        
        reanimator.Set(Drivers.IsMoving, MovementInput != Vector2.zero);
        reanimator.Set(Drivers.IsMovingHorizontal, MovementInput.x != 0);
        reanimator.Set(Drivers.IsMovingRight, MovementInput.x > 0);
        reanimator.Set(Drivers.IsMovingUp, MovementInput.y > 0);
    }

    private void FixedUpdate() {
        switch (State) {
            case PlayerState.Movement:
                UpdateMovementState();
                break;
            case PlayerState.Dash:
                UpdateDashState();
                break;
            case PlayerState.Hit:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMove(Vector2 value) => MovementInput = value;
    private void OnDash() => EnterDashState();
    private void OnMouse(Vector2 value) => lightDirectionInput = Camera.main.ScreenToWorldPoint(value) - transform.position;
    public void EnterMovementState() {
        State = PlayerState.Movement;
    }

    private void EnterDashState() {
        if (State != PlayerState.Movement || !dashStopwatch.IsReady) return;
        State = PlayerState.Dash;

        dashStopwatch.Split();
    }
    private void SetFacingDirection(int x, int y) {
        facingDirection = new Vector2(x, y);
    }

    private void UpdateLightDirection() {
        if (lightDirectionInput != Vector2.zero) {
            lightDirection = lightDirectionInput;
            lightDirection.Normalize();
        }

        var angle = Vector2.SignedAngle(Vector2.right, lightDirection);
        flashLight.transform.rotation = Quaternion.Euler(0f, 0f, angle + 270);
    }

    private void UpdateDashState() {

        if (facingDirection.x != 0 && facingDirection.y == 0) {
            collisionDetection.rigidbody2D.AddForce(
                new Vector2(facingDirection.x * dashSpeed, 0) - collisionDetection.rigidbody2D.velocity,
                ForceMode2D.Impulse
            );
        } else if (facingDirection.y != 0 && facingDirection.x == 0) {
            collisionDetection.rigidbody2D.AddForce(
                new Vector2(0, facingDirection.y * dashSpeed) - collisionDetection.rigidbody2D.velocity,
                ForceMode2D.Impulse
            );
        }       
        
        
        if (dashStopwatch.IsFinished || collisionDetection.wallContact.HasValue) {
            dashStopwatch.Split();
            EnterMovementState();
        }
    }

    private void UpdateMovementState() {
        var previousVelocity = collisionDetection.rigidbody2D.velocity;
        var velocityChange = Vector2.zero;

        velocityChange.x = (MovementInput.x * walkSpeed - previousVelocity.x) / 4;
        velocityChange.y = (MovementInput.y * walkSpeed - previousVelocity.y) / 4;

        if (collisionDetection.wallContact.HasValue) {
            var wallDirection = (int) Mathf.Sign(collisionDetection.wallContact.Value.point.x - transform.position.x);
            var walkDirection = (int) Mathf.Sign(MovementInput.x);

            if (walkDirection == wallDirection)
                velocityChange.x = 0;
        }

        collisionDetection.rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
    }
}