using Unity.VisualScripting;
using UnityEngine;

public class InputManager : PirateSingleton<InputManager>
{
    public float MovementInput { get; private set; }
    public float TurnInput { get; private set; }

    private bool isAttacking = false;


    private string verticalAxisName = "Vertical";
    private string horizontalAxisName = "Horizontal";


    void Update()
    {
        MovementInput = Input.GetAxis(verticalAxisName);
        TurnInput = Input.GetAxis(horizontalAxisName);
        CheckAttacking();
    }
    protected virtual void CheckAttacking()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    public virtual bool IsAttacking()
    {
        return this.isAttacking;
    }
}
