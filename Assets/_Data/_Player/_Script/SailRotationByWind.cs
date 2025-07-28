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
            enabled = false; // Tắt script nếu không có thân tàu
            return;
        }

        // Lưu lại góc quay ban đầu của cánh buồm trong không gian cục bộ của tàu
        initialLocalEulerAngles = transform.localEulerAngles;
    }

    void LateUpdate()
    {
        if (shipBody == null) return;

        // Lấy góc quay hiện tại của thân tàu trên trục Y (quay ngang)
        float shipYaw = shipBody.eulerAngles.y;

        // Tính toán hướng gió tương đối so với tàu
        // (Để thực tế hơn, bạn sẽ lấy hướng gió từ script AdvancedShipController)
        // Hiện tại, chúng ta giả định cánh buồm sẽ cố gắng vuông góc với hướng gió chính
        // hoặc đơn giản là xoay theo một giới hạn nhất định so với hướng tiến của tàu.

        // Cách đơn giản nhất: Làm cho cánh buồm hướng thẳng về phía trước của tàu
        // Khi tàu xoay, cánh buồm sẽ xoay theo
        Quaternion targetRotation = Quaternion.Euler(initialLocalEulerAngles.x,
                                                    initialLocalEulerAngles.y,
                                                    initialLocalEulerAngles.z);

        // Áp dụng góc xoay này trong không gian cục bộ
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);

        // --- Cải thiện: Xoay cánh buồm dựa trên hướng gió và hướng tàu ---
        // Giả sử bạn có thể lấy hướng gió từ PlayerShipController hoặc một Manager
        // Ví dụ: Lấy gió từ AdvancedShipController (nếu script đó gắn trên shipBody)
        PlayerMovement shipController = PlayerManager.Instance.PlayerMovement;
        if (shipController != null)
        {
            // Hướng gió (global)
            Vector3 windDir = WindManager.Instance.CurrentWindDirection;

            // Hướng tiến của tàu (global)
            Vector3 shipForward = shipBody.forward;

            // Tính toán góc giữa hướng gió và hướng tiến của tàu
            // Chúng ta muốn cánh buồm xoay để hứng gió tốt nhất, tức là vuông góc với gió
            // nhưng vẫn bị giới hạn bởi cấu trúc của tàu.

            // Cách đơn giản hóa: Hướng của cánh buồm sẽ là sự kết hợp giữa hướng của tàu 
            // và một hướng tối ưu so với gió.
            // Để đơn giản hơn cho ví dụ này, chúng ta chỉ làm cánh buồm quay theo tàu,
            // và có thể hơi lệch một chút để mô phỏng sự điều chỉnh.

            // Một cách tiếp cận thực tế hơn: Cánh buồm sẽ cố gắng xoay vuông góc với hướng gió
            // trong giới hạn xoay cho phép của nó so với thân tàu.
            // Tính toán góc giữa hướng gió và hướng "ngang" của tàu (tức là trục Right)
            float angleToWind = Vector3.SignedAngle(shipForward, windDir, Vector3.up);

            // Giới hạn góc xoay của cánh buồm để nó không xoay quá 90 độ so với thân tàu
            // (vì nó được gắn trên tàu)
            float clampedAngle = Mathf.Clamp(angleToWind, -maxSailAngle, maxSailAngle);

            // Tạo quaternion xoay cho cánh buồm dựa trên góc đã tính
            // Lưu ý: Đây là góc xoay TƯƠNG ĐỐI với hướng của tàu
            Quaternion targetLocalRotation = Quaternion.Euler(initialLocalEulerAngles.x,
                                                             initialLocalEulerAngles.y + clampedAngle,
                                                             initialLocalEulerAngles.z);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // Nếu không có AdvancedShipController, chỉ đơn giản là giữ cánh buồm quay theo tàu
            Quaternion targetDefaultRotation = Quaternion.Euler(initialLocalEulerAngles.x,
                                                                initialLocalEulerAngles.y,
                                                                initialLocalEulerAngles.z);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDefaultRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
