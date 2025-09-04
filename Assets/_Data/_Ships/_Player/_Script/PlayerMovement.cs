using UnityEngine;

public class PlayerMovement : PirateMonoBehaviour, IWindAffectAble
{
    [Header("Ship Movement Settings")]
    public float forwardThrust = 3500f;
    public float steeringTorque = 200f;
    public float maxSpeed = 100f;

    [Header("Physics Dampening")]
    public float linearDamping = 0.5f;
    public float angularDamping = 0.9f;

    private Rigidbody rigidPlayer;
    private PlayerState currentState;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rigidPlayer != null) return;
        this.rigidPlayer = GetComponentInParent<Rigidbody>();
        Debug.Log(transform.name + " LoadRigidbody: ", gameObject);
    }

    private enum PlayerState
    {
        Idle,
        Moving
    }

    protected override void Start()
    {
        base.Start();
        currentState = PlayerState.Idle;
    }

    void FixedUpdate()
    {
        float verticalInput = InputManager.Instance.MovementInput;

        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdleState(verticalInput);
                break;
            case PlayerState.Moving:
                HandleMovingState(verticalInput);
                break;
        }

        ApplyDamping();
        ClampMaxSpeed();
    }

    protected override void OnEnable()
    {
        if (WindManager.Instance != null)
        {
            WindManager.Instance.Register(this);
        }
    }

    protected override void OnDisable()
    {
        if (WindManager.Instance != null)
        {
            WindManager.Instance.Unregister(this);
        }
    }

    private void HandleIdleState(float verticalInput)
    {
        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            currentState = PlayerState.Moving;
        }
    }

    private void HandleMovingState(float verticalInput)
    {
        if (Mathf.Abs(verticalInput) < 0.01f)
        {
            currentState = PlayerState.Idle;
            return;
        }

        Vector3 thrustForce = transform.forward * verticalInput * forwardThrust;
        rigidPlayer.AddForce(thrustForce, ForceMode.Force);

        float horizontalInput = InputManager.Instance.TurnInput;
        float torqueAmount = horizontalInput * steeringTorque;

        if (verticalInput < 0)
        {
            torqueAmount *= -1f;
        }

        rigidPlayer.AddTorque(0f, torqueAmount, 0f, ForceMode.Force);
    }

    private void ClampMaxSpeed()
    {
        if (rigidPlayer.linearVelocity.magnitude > maxSpeed)
        {
            rigidPlayer.linearVelocity = rigidPlayer.linearVelocity.normalized * maxSpeed;
        }
    }

    private void ApplyDamping()
    {
        rigidPlayer.linearVelocity *= linearDamping;
        rigidPlayer.angularVelocity *= angularDamping;
    }

    public void ApplyWindForce(Vector3 windForce)
    {
        if (currentState == PlayerState.Moving)
        {
            rigidPlayer.AddForce(windForce, ForceMode.Force);
        }
    }
}