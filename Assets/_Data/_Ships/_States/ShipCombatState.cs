using UnityEngine;

public class ShipCombatState : ShipStateBase
{   

    public ShipCombatState(PlayerMovement controller) : base(controller) { }

    public override void Enter()
    {
        Debug.Log("Ship entered COMBAT mode!");
    }

    public override void Update()
    {
        controller.HandleMovement();


        if (InputManager.Instance.IsAttacking())
        {
            controller.FireCannon();
        }
        
        if (!controller.HasMovementInput() && !InputManager.Instance.IsAttacking())
        {
            controller.ChangeState(new ShipIdleState(controller));
        }       
        else if (controller.HasMovementInput() && !InputManager.Instance.IsAttacking())
        {
            controller.ChangeState(new ShipMoveState(controller));
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting COMBAT mode");
    }
}
