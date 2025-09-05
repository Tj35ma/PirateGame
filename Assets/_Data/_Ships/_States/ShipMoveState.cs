using UnityEngine;

public class ShipMoveState : ShipStateBase
{
    public ShipMoveState(PlayerMovement controller) : base(controller) { }

    public override void Enter()
    {

    }

    public override void Update()
    {
        controller.HandleMovement();

        if (!controller.HasMovementInput())
        {
            controller.ChangeState(new ShipIdleState(controller));
        }
    }

    public override void Exit()
    {

    }
}
