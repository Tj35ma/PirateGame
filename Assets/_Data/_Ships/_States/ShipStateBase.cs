using UnityEngine;

public abstract class ShipStateBase
{
    protected PlayerMovement controller;

    public ShipStateBase(PlayerMovement controller)
    {
        this.controller = controller;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
