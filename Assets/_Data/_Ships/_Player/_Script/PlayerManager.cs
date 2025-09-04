using UnityEngine;

public class PlayerManager : PirateSingleton<PlayerManager>
{
    [SerializeField] protected Rigidbody rigidPlayer;
    public Rigidbody RigidPlayer => rigidPlayer;

    [SerializeField] protected PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;    

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
        this.LoadPlayerMovement();
    }

    protected virtual void LoadRigidbody()
    {
        if (this.rigidPlayer != null) return;
        this.rigidPlayer = GetComponent<Rigidbody>();
        Debug.Log(transform.name + " LoadRigidbody: ", gameObject);
    }

    protected virtual void LoadPlayerMovement()
    {
        if (this.playerMovement != null) return;
        this.playerMovement = GetComponentInChildren<PlayerMovement>();
        Debug.Log(transform.name + " LoadPlayerMovement: ", gameObject);
    }
    
}
