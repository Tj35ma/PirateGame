using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : PirateMonoBehaviour
{
    [Header("Ship Movement Settings")]
    public float forwardThrust = 3500f;
    public float steeringTorque = 60f;
    public float maxSpeed = 100f;    

    [Header("Physics Dampening")]
    public float linearDamping = 0.85f; 
    public float angularDamping = 0.9f;

    private Rigidbody rigidPlayer;    
    
    [SerializeField] protected bool isActivelyBeingDriven = false;    

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rigidPlayer != null) return;        
        this.rigidPlayer = GetComponentInParent<Rigidbody>();
        Debug.Log(transform.name + "LoadRigidbody: ", gameObject);
    }   

    void FixedUpdate() 
    {
        this.PlayerMoving();
    }   

    protected virtual void PlayerMoving()
    {
        float verticalInput = InputManager.Instance.MovementInput;
        float horizontalInput = InputManager.Instance.TurnInput;

        Vector3 thrustForce = transform.forward * verticalInput * forwardThrust;

        if (thrustForce.magnitude > 0.01f)
        {
            rigidPlayer.AddForce(thrustForce, ForceMode.Force);
            this.isActivelyBeingDriven = true;            
        }

        

        if (verticalInput != 0f)
        {
            float torqueAmount = horizontalInput * steeringTorque;
            rigidPlayer.AddTorque(0f, torqueAmount, 0f, ForceMode.Force);
        }

        if (rigidPlayer.linearVelocity.magnitude > maxSpeed)
        {
            rigidPlayer.linearVelocity = rigidPlayer.linearVelocity.normalized * maxSpeed;
        }

        rigidPlayer.linearVelocity *= linearDamping;
        rigidPlayer.angularVelocity *= angularDamping;
    }
    
}