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

    [SerializeField]
    private float moveSpeed = 30f;

    [SerializeField]
    private float maxDistance = 20f;

    [SerializeField]
    private float screenBorderClamp = 0.05f; // границы, чтобы не выходил за экран

    private Joystick fireJoystick;
    private Transform player;

    private void Start()
    {
        Cursor.visible = false;
        hitInfo = new RaycastHit[1];

        if (GameManager.Instance.isMobile)
        {
            fireJoystick = GameManager.Instance.UIManager.FireJoystickUI.GetComponent<Joystick>();
        }

        player = GameManager.Instance.LevelManager.Player.transform;
    }

    private void Update()
    {
        if (virtualCamera == null || player == null)
            return;

        if (!GameManager.Instance.isMobile)
        {
            // ПК — мышь
            ray = virtualCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.RaycastNonAlloc(ray, hitInfo, 100f, groundLayer) > 0)
            {
                transform.position = hitInfo[0].point;
            }
        }
        else        
        {
            // Мобилка — джойстик
            Vector3 input = new Vector3(fireJoystick.Horizontal, 0f, fireJoystick.Vertical);
            float inputMagnitude = Mathf.Clamp01(input.magnitude);

            Vector3 targetWorldPos;

            if (inputMagnitude < 0.05f)
            {
                // Джойстик отпущен — вернуть Pointer перед игроком
                Vector3 forward = player.forward;
                targetWorldPos = player.position + forward * (maxDistance * 0.25f); // ближе к игроку
            }
            else
            {
                // Направление джойстика
                Vector3 moveDir = input.normalized;

                // Целевая позиция — дальше от игрока
                targetWorldPos = player.position + moveDir * maxDistance * inputMagnitude;
            }

            // Ограничим экранными границами
            Vector3 viewPos = virtualCamera.WorldToViewportPoint(targetWorldPos);
            viewPos.x = Mathf.Clamp(viewPos.x, screenBorderClamp, 1f - screenBorderClamp);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBorderClamp, 1f - screenBorderClamp);
            viewPos.z = Mathf.Max(1f, viewPos.z);

            Vector3 clampedWorldPos = virtualCamera.ViewportToWorldPoint(viewPos);
            clampedWorldPos.y = player.position.y;

            transform.position = Vector3.Lerp(transform.position, clampedWorldPos, moveSpeed * Time.unscaledDeltaTime);
        }
    }
}
