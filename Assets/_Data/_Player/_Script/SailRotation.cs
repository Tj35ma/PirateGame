using UnityEngine;

public class SailRotation : MonoBehaviour
{
    [Tooltip("Kéo Game Object thân thuyền vào đây (ví dụ: 'BoatBody' hoặc 'ShipController').")]
    public Transform boatBodyTransform;

    [Tooltip("Tốc độ cánh buồm xoay để bắt kịp thân thuyền. Giá trị nhỏ hơn sẽ tạo độ trễ lớn hơn.")]
    [Range(0.1f, 10.0f)] // Giới hạn giá trị để dễ điều chỉnh trong Inspector
    public float rotationCatchUpSpeed = 5.0f; // Tốc độ mặc định, có thể điều chỉnh

    void Update()
    {
        // Đảm bảo rằng chúng ta đã gán thân thuyền vào script
        if (boatBodyTransform != null)
        {
            // Lấy góc quay thế giới (world rotation) hiện tại của thân thuyền.
            // Đây là góc quay mà cánh buồm muốn hướng tới.
            Quaternion targetRotation = boatBodyTransform.rotation;

            // Nội suy góc quay hiện tại của cánh buồm (transform.rotation)
            // về phía góc quay mục tiêu (targetRotation) của thân thuyền.
            // 'rotationCatchUpSpeed * Time.deltaTime' là yếu tố quyết định độ trễ.
            // Giá trị này càng nhỏ, cánh buồm càng mất nhiều thời gian để "đuổi kịp",
            // tạo ra hiệu ứng trễ mong muốn.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationCatchUpSpeed * Time.deltaTime);
        }
    }
}