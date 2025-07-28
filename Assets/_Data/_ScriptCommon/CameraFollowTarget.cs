using UnityEngine;

public class CameraFollowTarget : PirateMonoBehaviour
{
    [Header("Target")]
    [Tooltip("Đối tượng mà camera sẽ đi theo. Hãy kéo GameObject của Player vào đây.")]
    public Transform target; // Mục tiêu camera sẽ theo dõi

    [Header("Position Settings")]
    [Tooltip("Độ cao của camera so với mặt phẳng của mục tiêu.")]
    public float height = 2f; // Độ cao của camera
    [Tooltip("Khoảng cách lùi lại của camera so với điểm ngay trên mục tiêu (theo hướng nhìn của camera).")]
    public float distance = 6f; // Khoảng cách lùi lại
    [Tooltip("Góc nghiêng của camera theo trục X (0 = nhìn ngang, 90 = nhìn thẳng xuống).")]
    [Range(0f, 90f)] // Giới hạn giá trị trong khoảng hợp lý
    public float pitchAngle = 0f; // Góc xoay quanh trục X (pitch), điều chỉnh để có góc nhìn tốt hơn khi có distance

    [Header("Damping Settings")]
    [Tooltip("Độ mượt khi camera di chuyển theo mục tiêu (giá trị nhỏ hơn = mượt hơn).")]
    public float positionDamping = 5.0f; // Độ mượt khi di chuyển vị trí
    [Tooltip("Độ mượt khi camera xoay theo mục tiêu (giá trị nhỏ hơn = mượt hơn).")]
    public float rotationDamping = 5.0f; // Độ mượt khi xoay

    [Header("Rotation Smoothing (Alternative)")]
    [Tooltip("Bật để làm mượt hướng của mục tiêu trước khi tính toán xoay camera. Hãy thử bật Rigidbody Interpolation trên Target trước.")]
    public bool smoothTargetDirection = true;
    [Tooltip("Độ mượt khi làm mượt hướng mục tiêu (nếu bật).")]
    public float targetDirectionSmoothing = 1f;

    // Biến lưu trữ hướng mục tiêu đã được làm mượt
    private Vector3 smoothedTargetForwardHorizontal;
    // Biến lưu trữ vị trí và góc xoay mong muốn cuối cùng
    private Vector3 finalDesiredPosition;
    private Quaternion finalDesiredRotation;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTarget();
    }

    protected virtual void LoadTarget()
    {
        if (target != null) return;
        target = transform.parent.GetComponent<Transform>();
        Debug.Log(transform.name + " :LoadTarget", gameObject);
    }

    protected override void Start()
    {
        // Khởi tạo hướng làm mượt ban đầu
        if (target != null)
        {
            smoothedTargetForwardHorizontal = target.forward;
            smoothedTargetForwardHorizontal.y = 0;
            if (smoothedTargetForwardHorizontal.sqrMagnitude > 0.001f)
                smoothedTargetForwardHorizontal.Normalize();
            else
                smoothedTargetForwardHorizontal = Vector3.forward; // Hướng mặc định nếu target nhìn thẳng lên/xuống
        }
        else
        {
            smoothedTargetForwardHorizontal = Vector3.forward; // Hướng mặc định nếu không có target
        }

        // Khởi tạo vị trí và góc xoay ban đầu để tránh Lerp từ (0,0,0)
        if (target != null)
        {
            CalculateDesiredTransform(1f); // Tính toán ngay lập tức
            transform.position = finalDesiredPosition;
            transform.rotation = finalDesiredRotation;
        }
    }


    void LateUpdate() // Sử dụng LateUpdate để đảm bảo mục tiêu đã di chuyển/xoay xong
    {
        // --- Tính toán vị trí và góc xoay mong muốn ---
        CalculateDesiredTransform(rotationDamping * Time.deltaTime); // Tính toán với damping

        // --- Di chuyển và xoay camera mượt mà đến vị trí/góc xoay mong muốn ---
        // Lerp vị trí
        transform.position = Vector3.Lerp(transform.position, finalDesiredPosition, positionDamping * Time.deltaTime);
        // Slerp góc xoay (đã được thực hiện trong CalculateDesiredTransform)
        transform.rotation = finalDesiredRotation;

    }

    /// <summary>
    /// Tính toán vị trí và góc xoay mong muốn cuối cùng cho camera.
    /// </summary>
    /// <param name="currentRotationDampingFactor">Hệ số damping cho Slerp xoay.</param>
    void CalculateDesiredTransform(float currentRotationDampingFactor)
    {
        // 1. Lấy hướng phía trước của mục tiêu và chiếu lên mặt phẳng ngang (XZ)
        Vector3 currentTargetForwardHorizontal = target.forward;
        currentTargetForwardHorizontal.y = 0; // Loại bỏ thành phần Y
        // Chỉ chuẩn hóa nếu vector không quá nhỏ
        if (currentTargetForwardHorizontal.sqrMagnitude > 0.001f)
            currentTargetForwardHorizontal.Normalize();
        else
            currentTargetForwardHorizontal = smoothedTargetForwardHorizontal; // Giữ hướng cũ nếu target nhìn thẳng lên/xuống


        // (Tùy chọn) Làm mượt hướng mục tiêu
        if (smoothTargetDirection)
        {
            // Lerp hướng làm mượt hiện tại về hướng thực tế của mục tiêu
            smoothedTargetForwardHorizontal = Vector3.Lerp(smoothedTargetForwardHorizontal, currentTargetForwardHorizontal, targetDirectionSmoothing * Time.deltaTime);
            // Đảm bảo nó vẫn được chuẩn hóa sau khi lerp
            if (smoothedTargetForwardHorizontal.sqrMagnitude > 0.001f) // Tránh chuẩn hóa vector zero
                smoothedTargetForwardHorizontal.Normalize();
        }
        else
        {
            // Nếu không bật làm mượt, sử dụng hướng hiện tại trực tiếp
            smoothedTargetForwardHorizontal = currentTargetForwardHorizontal;
        }


        // 2. Tạo góc xoay mong muốn dựa trên hướng đã (có thể) được làm mượt
        Quaternion desiredRotation;
        // Chỉ xoay nếu hướng ngang có độ lớn đáng kể (tránh lỗi khi vector gần zero)
        if (smoothedTargetForwardHorizontal.sqrMagnitude > 0.001f)
        {
            // Tạo góc xoay nhìn theo hướng ngang của mục tiêu
            Quaternion horizontalRotation = Quaternion.LookRotation(smoothedTargetForwardHorizontal, Vector3.up);
            // Kết hợp với góc nghiêng (pitch) mong muốn quanh trục X cục bộ
            desiredRotation = horizontalRotation * Quaternion.Euler(pitchAngle, 0f, 0f); // Sử dụng pitchAngle
        }
        else
        {
            // Giữ nguyên góc xoay Y hiện tại nếu không có hướng rõ ràng, chỉ áp dụng pitch
            desiredRotation = Quaternion.Euler(pitchAngle, transform.eulerAngles.y, 0f);
        }

        // Sử dụng Slerp để làm mượt góc xoay và lưu vào biến cuối cùng
        finalDesiredRotation = Quaternion.Slerp(transform.rotation, desiredRotation, currentRotationDampingFactor);

        // 3. Tính toán vị trí mong muốn cuối cùng
        // Bắt đầu từ vị trí target, cộng thêm chiều cao
        Vector3 basePosition = target.position + Vector3.up * height;
        // Lùi lại theo hướng nhìn của camera (đã xoay) một khoảng distance
        // finalDesiredRotation * Vector3.back là vector hướng về phía sau của camera sau khi xoay
        Vector3 offset = finalDesiredRotation * Vector3.back * distance;
        finalDesiredPosition = basePosition + offset;
    }


    // Gizmos không thay đổi nhiều, nhưng sẽ vẽ vị trí cuối cùng chính xác hơn
    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            // Tính toán tạm thời vị trí và góc xoay mong muốn (không dùng damping) để vẽ Gizmos
            CalculateDesiredTransform(1f); // Tính toán ngay lập tức cho Gizmos

            Gizmos.color = new Color(0f, 1f, 0f, 0.5f); // Màu xanh lá cây trong suốt
            // Vẽ vị trí mong muốn cuối cùng
            Gizmos.DrawLine(target.position, finalDesiredPosition);
            Gizmos.DrawWireSphere(finalDesiredPosition, 0.5f);

            // Vẽ hướng nhìn từ vị trí mong muốn
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(finalDesiredPosition, finalDesiredPosition + finalDesiredRotation * Vector3.forward * 5f);

            // Vẽ hướng ngang của target để debug
            Gizmos.color = Color.red;
            Vector3 targetForwardHorizontalDraw = target.forward;
            targetForwardHorizontalDraw.y = 0;
            if (targetForwardHorizontalDraw.sqrMagnitude > 0.001f)
                Gizmos.DrawLine(target.position, target.position + targetForwardHorizontalDraw.normalized * 3f);

            // Vẽ hướng ngang đã làm mượt (nếu bật)
            if (smoothTargetDirection)
            {
                Gizmos.color = Color.yellow;
                // Cần tính lại smoothedTargetForwardHorizontal cho Gizmos nếu không chạy game
                Vector3 gizmoSmoothedDir = smoothedTargetForwardHorizontal;
#if UNITY_EDITOR
                if (!Application.isPlaying) // Ước lượng nếu không chạy game
                {
                    Vector3 currentTargetForwardGizmo = target.forward;
                    currentTargetForwardGizmo.y = 0;
                    if (currentTargetForwardGizmo.sqrMagnitude > 0.001f) currentTargetForwardGizmo.Normalize();
                    else currentTargetForwardGizmo = Vector3.forward;
                    gizmoSmoothedDir = currentTargetForwardGizmo; // Không thể lerp chính xác trong editor gizmo
                }
#endif
                if (gizmoSmoothedDir.sqrMagnitude > 0.001f)
                    Gizmos.DrawLine(target.position + Vector3.up * 0.1f, target.position + Vector3.up * 0.1f + gizmoSmoothedDir * 2.5f);
            }
        }
    }
}
