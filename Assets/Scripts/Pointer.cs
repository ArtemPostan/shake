using FlamingCore;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [HideInInspector]
    public Camera virtualCamera;

    private Ray ray;
    private RaycastHit[] hitInfo;

    [SerializeField]
    private LayerMask groundLayer;

    private Vector3 currentPosition;

    private Joystick fireJoystick;

    [SerializeField]
    private float moveSpeed = 30f;

    [SerializeField]
    private float maxDistance = 20f; // максимум, насколько Pointer может отойти от игрока

    private Transform player;

    private void Start()
    {
        Cursor.visible = false;
        hitInfo = new RaycastHit[1];

        if (GameManager.Instance.isMobile)
            fireJoystick = GameManager.Instance.UIManager.FireJoystickUI.GetComponent<Joystick>();

        currentPosition = transform.position;
        player = GameManager.Instance.LevelManager.Player.transform;
    }

    private void Update()
    {
        if (virtualCamera == null) return;

        if (!GameManager.Instance.isMobile)
        {
            // ПК — старый способ (по мыши)
            ray = virtualCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.RaycastNonAlloc(ray, hitInfo, 100f, groundLayer) > 0)
            {
                transform.position = hitInfo[0].point;
            }
        }
        else
        {
            Vector3 input = new Vector3(fireJoystick.Horizontal, 0f, fireJoystick.Vertical);
            float inputMagnitude = Mathf.Clamp01(input.magnitude);
            Vector3 moveDir = input.normalized;

            // Желаемая позиция
            Vector3 desiredWorldPos = player.position + moveDir * maxDistance * inputMagnitude;

            // Переводим в viewport (0..1)
            Vector3 viewportPos = virtualCamera.WorldToViewportPoint(desiredWorldPos);

            // Ограничим положение в пределах экрана
            viewportPos.x = Mathf.Clamp(viewportPos.x, 0.05f, 0.95f);
            viewportPos.y = Mathf.Clamp(viewportPos.y, 0.05f, 0.95f);  // можно исключить Y, если 2D

            // Возвращаем в мировые координаты
            Vector3 clampedWorldPos = virtualCamera.ViewportToWorldPoint(viewportPos);

            // Обновляем позицию
            transform.position = Vector3.Lerp(transform.position, clampedWorldPos, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}
