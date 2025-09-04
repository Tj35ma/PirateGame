using UnityEngine;
using UnityEngine.Playables;

public class ShipController : PirateMonoBehaviour, IWindAffectAble
{
    [Header("Movement Settings")]
    public float moveForce = 0f;
    public float turnForce = 50f;
    public float maxSpeed = 10f;

    [Header("Combat Settings")]
    public GameObject cannonballPrefab;
    public Transform firePoint;
    public float cannonballSpeed = 20f;

    public float fireCooldown = 1.5f;
    private float lastFireTime = -999f;
    public float LastFireTime => lastFireTime;

    private Rigidbody rb;
    private ShipStateBase currentState;
    private Vector2 moveInput;    

    protected override void Start()
    {
        this.moveForce = this.rb.mass * 100;
        ChangeState(new ShipIdleState(this));
    }

    private void Update()
    {
        moveInput = new Vector2(InputManager.Instance.TurnInput, InputManager.Instance.MovementInput);

        if (InputManager.Instance.IsAttacking())
        {
            FireCannon();
            
            if (!(currentState is ShipCombatState))
            {
                ChangeState(new ShipCombatState(this));
            }
        }

        currentState?.Update();
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

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = GetComponentInParent<Rigidbody>();
        Debug.Log(transform.name + " LoadRigidbody: ", gameObject);
    }

    public void ChangeState(ShipStateBase newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public bool HasMovementInput()
    {
        return moveInput.y != 0f;
    }

    public void HandleMovement()
    {


        Vector3 thrustForce = transform.forward * moveInput.y * moveForce;
        rb.AddForce(thrustForce, ForceMode.Force);

        float turnAmount = moveInput.x * turnForce;
        if (moveInput.y < 0f) turnAmount = -turnAmount;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        if (moveInput.y != 0f) rb.MoveRotation(rb.rotation * turnRotation);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
    

    public void ApplyWindForce(Vector3 windForce)
    {
        if (currentState is ShipMoveState && WindManager.Instance != null)
        {
            rb.AddForce(windForce, ForceMode.Force);
        }
    }


    public void FireCannon()
    {
        if (Time.time < lastFireTime + fireCooldown)
            return;

        if (cannonballPrefab != null && firePoint != null)
        {
            GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
            Rigidbody cbRb = cannonball.GetComponent<Rigidbody>();
            if (cbRb != null)
            {
                cbRb.linearVelocity = firePoint.forward * cannonballSpeed;
            }
        }

        lastFireTime = Time.time;
    }
}
