using UnityEngine;

public abstract class ShipStateBase
{
    protected ShipController controller;

    public ShipStateBase(ShipController controller)
    {
        this.controller = controller;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
