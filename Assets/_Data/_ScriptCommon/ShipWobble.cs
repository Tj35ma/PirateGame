using UnityEngine;

public class ShipWobble : MonoBehaviour
{   
    public float wobbleSpeedY = 1.5f;    
    public float wobbleAmountY = 2f;    
    public float wobbleSpeedX = 1f;    
    public float wobbleAmountX = 1f;    
    public float floatSpeed = 0.5f;    
    public float floatAmount = 0.1f;
    
    private float _timeOffsetWobbleY;
    private float _timeOffsetWobbleX;
    private float _timeOffsetFloat;

    private Vector3 _initialLocalPosition; 
    private Quaternion _initialLocalRotation; 

    void Awake()
    {        
        _initialLocalPosition = transform.localPosition;
        _initialLocalRotation = transform.localRotation;
        
        _timeOffsetWobbleY = Random.Range(0f, 100f);
        _timeOffsetWobbleX = Random.Range(0f, 100f);
        _timeOffsetFloat = Random.Range(0f, 100f);
    }

    void Update()
    {
        ApplyWobble();
    }

    void ApplyWobble()
    {        
        float currentWobbleY = Mathf.Sin(Time.time * wobbleSpeedY + _timeOffsetWobbleY) * wobbleAmountY;        
        Quaternion wobbleRotationY = Quaternion.Euler(0, currentWobbleY, 0);        
        float currentWobbleX = Mathf.Sin(Time.time * wobbleSpeedX + _timeOffsetWobbleX) * wobbleAmountX;        
        Quaternion wobbleRotationX = Quaternion.Euler(currentWobbleX, 0, 0);       
        transform.localRotation = _initialLocalRotation * wobbleRotationY * wobbleRotationX;       
        float currentFloat = Mathf.Sin(Time.time * floatSpeed + _timeOffsetFloat) * floatAmount;        
        transform.localPosition = _initialLocalPosition + new Vector3(0, currentFloat, 0);
    }
}