using UnityEngine;
using System.Collections;

public class WindManager : MonoBehaviour
{
    public float maxWindStrength = 5f; // Cường độ gió tối đa
    public float minWindStrength = 1f; // Cường độ gió tối thiểu
    public float windChangeInterval = 5f; // Thời gian thay đổi hướng gió (giây)
    public float rotationSpeed = 1f; // Tốc độ quay của hướng gió khi thay đổi

    private Vector3 currentWindDirection;
    public Vector3 CurrentWindDirection => currentWindDirection; // Thuộc tính để lấy hướng gió hiện tại
    private float currentWindStrength;
    private Coroutine windChangeCoroutine;

    public static WindManager Instance { get; private set; } // Singleton pattern

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ WindManager tồn tại qua các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentWindDirection = Random.onUnitSphere; // Hướng gió ban đầu ngẫu nhiên
        currentWindStrength = Random.Range(minWindStrength, maxWindStrength); // Cường độ gió ban đầu ngẫu nhiên
        windChangeCoroutine = StartCoroutine(ChangeWindRoutine());
    }

    IEnumerator ChangeWindRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(windChangeInterval);

            // Chọn hướng gió mới ngẫu nhiên
            Vector3 targetWindDirection = Random.onUnitSphere;
            float targetWindStrength = Random.Range(minWindStrength, maxWindStrength);

            // Làm mượt quá trình chuyển đổi hướng gió
            float elapsedTime = 0f;
            Vector3 startWindDirection = currentWindDirection;
            float startWindStrength = currentWindStrength;

            while (elapsedTime < rotationSpeed)
            {
                currentWindDirection = Vector3.Slerp(startWindDirection, targetWindDirection, elapsedTime / rotationSpeed);
                currentWindStrength = Mathf.Lerp(startWindStrength, targetWindStrength, elapsedTime / rotationSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentWindDirection = targetWindDirection;
            currentWindStrength = targetWindStrength;
        }
    }

    public Vector3 GetWindForce(float mass)
    {
        // Lực gió tác dụng lên object = hướng gió * cường độ gió * khối lượng (để đảm bảo object nhẹ hơn bị đẩy nhiều hơn)
        return currentWindDirection.normalized * currentWindStrength * mass;
    }

    // Để hiển thị hướng gió trong Scene view (chỉ dùng cho debug)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, currentWindDirection.normalized * currentWindStrength * 2);
    }
}