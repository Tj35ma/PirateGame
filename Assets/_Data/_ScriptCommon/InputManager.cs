using Unity.VisualScripting;
using UnityEngine;

public class InputManager : PirateSingleton<InputManager>
{    
    public float MovementInput { get; private set; } 
    public float TurnInput { get; private set; }     

    
    [Header("Input Axes Names")]
    [Tooltip("Tên của trục input cho di chuyển tiến/lùi (mặc định: Vertical)")]
    [SerializeField] private string verticalAxisName = "Vertical";

    [Tooltip("Tên của trục input cho xoay xe (mặc định: Horizontal)")]
    [SerializeField] private string horizontalAxisName = "Horizontal";

    
    void Update()
    {       
        MovementInput = Input.GetAxis(verticalAxisName);
        TurnInput = Input.GetAxis(horizontalAxisName);
    }
}
