using UnityEngine;

public class PlayerManager : PirateSingleton<PlayerManager>
{
    [SerializeField] protected Rigidbody rigidPlayer;
    public Rigidbody RigidPlayer => rigidPlayer;

    [SerializeField] protected PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    [SerializeField] protected WindAffectAble windAffectAble;

    public WindAffectAble WindAffectAble => windAffectAble;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
        this.LoadPlayerMovement();
        this.LoadWindAffectAble();
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

    protected virtual void LoadWindAffectAble()
    {
        if (this.windAffectAble != null) return;
        this.windAffectAble = GetComponentInChildren<WindAffectAble>();
        Debug.Log(transform.name + " LoadWindAffectAble: ", gameObject);
    }
}
