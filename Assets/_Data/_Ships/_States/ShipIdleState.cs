using UnityEngine;

public class ShipIdleState : ShipStateBase
{
    public ShipIdleState(ShipController controller) : base(controller) { }

    public override void Enter()
    {
        // Có thể phát animation Idle nếu cần
    }

    public override void Update()
    {
        // Nếu người chơi bấm phím di chuyển => chuyển sang Move
        if (controller.HasMovementInput())
        {
            controller.ChangeState(new ShipMoveState(controller));
        }
    }

    public override void Exit()
    {
        // Code khi thoát Idle (nếu cần)
    }
}
