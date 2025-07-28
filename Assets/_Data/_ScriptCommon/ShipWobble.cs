using UnityEngine;

public class ShipWobble : MonoBehaviour
{
    [Header("Wobble Settings")]
    [Tooltip("Tốc độ lắc lư ngang của tàu (trục Y cục bộ).")]
    public float wobbleSpeedY = 1.5f;
    [Tooltip("Biên độ lắc lư ngang tối đa (độ).")]
    public float wobbleAmountY = 2f;

    [Tooltip("Tốc độ lắc lư theo chiều dọc (trục X cục bộ).")]
    public float wobbleSpeedX = 1f;
    [Tooltip("Biên độ lắc lư theo chiều dọc tối đa (độ).")]
    public float wobbleAmountX = 1f;

    [Tooltip("Tốc độ nhấp nhô lên xuống của tàu (trục Y thế giới).")]
    public float floatSpeed = 0.5f;
    [Tooltip("Biên độ nhấp nhô lên xuống tối đa (đơn vị Unity).")]
    public float floatAmount = 0.1f;

    // Các offset pha để các chuyển động không đồng bộ
    private float _timeOffsetWobbleY;
    private float _timeOffsetWobbleX;
    private float _timeOffsetFloat;

    private Vector3 _initialLocalPosition; // Vị trí cục bộ ban đầu của tàu
    private Quaternion _initialLocalRotation; // Góc quay cục bộ ban đầu của tàu

    void Awake()
    {
        // Lưu lại vị trí và góc quay cục bộ ban đầu của tàu
        // Điều này quan trọng nếu bạn muốn sóng sánh không làm lệch vị trí/góc quay chính của tàu
        _initialLocalPosition = transform.localPosition;
        _initialLocalRotation = transform.localRotation;

        // Khởi tạo các offset ngẫu nhiên để hiệu ứng sóng sánh tự nhiên hơn
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
        // --- Hiệu ứng lắc lư ngang (quay quanh trục Y cục bộ) ---
        // Sử dụng Mathf.Sin để tạo chuyển động qua lại mượt mà
        float currentWobbleY = Mathf.Sin(Time.time * wobbleSpeedY + _timeOffsetWobbleY) * wobbleAmountY;
        // Áp dụng quay lắc lư quanh trục Y cục bộ
        Quaternion wobbleRotationY = Quaternion.Euler(0, currentWobbleY, 0);

        // --- Hiệu ứng lắc lư dọc (quay quanh trục X cục bộ) ---
        float currentWobbleX = Mathf.Sin(Time.time * wobbleSpeedX + _timeOffsetWobbleX) * wobbleAmountX;
        // Áp dụng quay lắc lư quanh trục X cục bộ
        Quaternion wobbleRotationX = Quaternion.Euler(currentWobbleX, 0, 0);

        // Kết hợp góc quay lắc lư với góc quay hiện tại của tàu
        // Rất quan trọng: Phải áp dụng _initialLocalRotation để giữ góc quay cơ bản của tàu do PlayerShipController điều khiển
        // và sau đó thêm các hiệu ứng wobble vào.
        transform.localRotation = _initialLocalRotation * wobbleRotationY * wobbleRotationX;

        // --- Hiệu ứng nhấp nhô lên xuống (thay đổi vị trí theo trục Y thế giới) ---
        float currentFloat = Mathf.Sin(Time.time * floatSpeed + _timeOffsetFloat) * floatAmount;
        // Áp dụng vị trí nhấp nhô theo trục Y thế giới
        // Cũng phải cộng vào vị trí ban đầu của tàu để không làm lệch tàu
        transform.localPosition = _initialLocalPosition + new Vector3(0, currentFloat, 0);
    }
}