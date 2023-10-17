using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{

    internal Player playerScript;

    //main player properties
    [Header("Motion Options: ")]
    [SerializeField] internal float _maxSpeed = 8.0f;
    [SerializeField] internal float _moveAccel = 50.0f;
    [SerializeField] internal float _maxMoveAccel = 200.0f;
    [SerializeField] internal AnimationCurve _accelFactorFromDot;                       // Acceleration force curve when changing directions rapidly
    [SerializeField] internal Vector3 _forceScale = new Vector3 (1.0f, 1.0f, 1.0f);      // Scale of the forces applied to the player on each axes

    [Space(5)]

    [Header("Jump Options: ")]
    [SerializeField] internal float _JumpForce = 400.0f;                                // Amount of force added when the player jumps
    [SerializeField] internal float _jumpBuffer = 0.25f;                                // Amount of forgiveness the input of Jump should be when near the ground
    private float t_jumpBufferTimer;
    private bool _canJump;
    [SerializeField] internal float _coyoteBuffer = 0.25f;                              // Amount of forgiveness the player should have when leaving the ground too early for a Jump
    private float t_coyoteBufferTimer;

    Vector2 _inputScalar;

    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("\"Player_Movement\" script starting...");
        playerScript = GetComponent<Player>();

        // error checking
        if (!playerScript || _accelFactorFromDot == null)
        {
            Debug.LogError("Something isn't right here... Please check if all values have been assigned (Player_Movement.cs)");
        }
    }

    // Handles the timers for jumping
    void Update()
    {
        t_coyoteBufferTimer -= Time.deltaTime;
        t_jumpBufferTimer -= Time.deltaTime;
    }

    // Handles the movement based on 
    void FixedUpdate()
    {
        if (playerScript.playerCollision.isGrounded)
        {
            _canJump = true;
            t_coyoteBufferTimer = _coyoteBuffer;
        }

        // Creates Vector3 based on the where the player wants the character to go and translates target vector according to where the player is facing
        Vector3 _desiredMovement = new Vector3(_inputScalar.x, playerScript.playerRB.velocity.y, _inputScalar.y);
        Vector3 _translatedMovementVector = playerScript.transform.TransformDirection(_desiredMovement);

        // Creates a target Velocity that the player is striving to achieve while maintaining its jumping/falling velocity
        Vector3 _targetVelocity = _translatedMovementVector * _maxSpeed;
        _targetVelocity.y = playerScript.playerRB.velocity.y;

        // Gets the normalized player velocity vector and adjusts the acceleration force when changing directions rapidly using a custom Animation Curve
        Vector3 _playerVelocity = playerScript.playerRB.velocity.normalized;
        float _velocityDot = Vector3.Dot(_translatedMovementVector, _playerVelocity);
        float _adjustedAcceleration = _moveAccel * _accelFactorFromDot.Evaluate(_velocityDot);

        // calculates the acceleration needed to move towards the goal speed per physics update
        Vector3 _goalVelocity = Vector3.MoveTowards(playerScript.playerRB.velocity, _targetVelocity, _adjustedAcceleration * Time.fixedDeltaTime);
        Vector3 _neededAcceleration = ((_goalVelocity - playerScript.playerRB.velocity) / Time.fixedDeltaTime);
        _neededAcceleration = Vector3.ClampMagnitude(_neededAcceleration, _moveAccel);

        // applys the force to the player
        playerScript.playerRB.AddForce(Vector3.Scale(_neededAcceleration * playerScript.playerRB.mass, _forceScale));

        // Determines if the Player can or cannot jump
        if ((_canJump || t_coyoteBufferTimer > 0.0f) && t_jumpBufferTimer > 0.0f)
        {
            Debug.Log("Jumping!");
            playerScript.playerRB.velocity = new Vector3(playerScript.playerRB.velocity.x, 0.0f, playerScript.playerRB.velocity.z);
            playerScript.playerRB.AddForce(Vector3.up * _JumpForce);

            _canJump = false;
        }

    }

    public void OnMove(InputValue value)
    {
        // sets the inputs into a vector to determine left/right and back/forth
        _inputScalar = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // sets the action of pressing jump in a buffer
        t_jumpBufferTimer = _jumpBuffer;
    }
}
