using UnityEngine;

public class SailRotationByWind : MonoBehaviour
{
    [Tooltip("Đối tượng thân tàu chính (PlayerShip). Kéo vào từ Hierarchy.")]
    public Transform shipBody;

    [Tooltip("Tốc độ xoay của cánh buồm để theo kịp thân tàu.")]
    public float rotationSpeed = 5f;

    [Tooltip("Góc xoay tối đa của cánh buồm so với hướng tiến của tàu (ví dụ: 45 độ).")]
    public float maxSailAngle = 45f;

    private Vector3 initialLocalEulerAngles;

    void Start()
    {
        if (shipBody == null)
        {
            Debug.LogError("Ship Body (Thân tàu) chưa được gán trong SailRotationController!", this);
            enabled = false; 
            return;
        }

        
        initialLocalEulerAngles = transform.localEulerAngles;
    }

    void LateUpdate()
    {
        if (shipBody == null) return;

        
        float shipYaw = shipBody.eulerAngles.y;
        
        Quaternion targetRotation = Quaternion.Euler(initialLocalEulerAngles.x,
                                                    initialLocalEulerAngles.y,
                                                    initialLocalEulerAngles.z);

       
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

       
        PlayerMovement shipController = PlayerManager.Instance.PlayerMovement;
        if (shipController != null)
        {
            // Hướng gió (global)
            Vector3 windDir = WindManager.Instance.currentWindForce;

            // Hướng tiến của tàu (global)
            Vector3 shipForward = shipBody.forward;
            
            float angleToWind = Vector3.SignedAngle(shipForward, windDir, Vector3.up);
            
            float clampedAngle = Mathf.Clamp(angleToWind, -maxSailAngle, maxSailAngle);
            
            Quaternion targetLocalRotation = Quaternion.Euler(initialLocalEulerAngles.x,
                                                             initialLocalEulerAngles.y + clampedAngle,
                                                             initialLocalEulerAngles.z);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {            
            Quaternion targetDefaultRotation = Quaternion.Euler(initialLocalEulerAngles.x,
                                                                initialLocalEulerAngles.y,
                                                                initialLocalEulerAngles.z);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDefaultRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
