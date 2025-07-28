using UnityEngine;

public abstract class WindAffectAble : PirateMonoBehaviour
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected float movementThreshold = 0.1f;
    [SerializeField] protected bool isActivelyBeingDriven = false;   

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rb != null) return;
        this.rb = this.GetComponentInParent<Rigidbody>();
        Debug.Log(transform.name + " LoadRigidbody: ", gameObject);
    }

    void FixedUpdate() 
    {
        if (WindManager.Instance == null) return;
        if (!this.isActivelyBeingDriven) return;
        if (rb.linearVelocity.magnitude > movementThreshold)
        {
            Vector3 windForce = WindManager.Instance.GetWindForce(rb.mass);
            rb.AddForce(windForce, ForceMode.Force);
        }
    }
}