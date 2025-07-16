using FlamingCore;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float fastSpeed = 10f;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float jumpSpeed = 2f;
    [SerializeField] private FpsCameraArm fpsCameraArm;
    public Transform sideCameraTransform;

    private Rigidbody rb;
    private Vector3 velocityTemp;
    private Transform pointer;
    private Combat combat;
    private float fpsPitch;
    public Transform groundChecker;
    public LayerMask groundLayer;
    private bool checkGround;

    public bool floatingMode;
    public bool dashMode;
    public float dashTime = 0.15f;
    private float dashTimer;
    public float dashCoolTime = 0.5f;
    private float dashCoolTimer;
    public float dashSpeed = 20f;
    private Vector3 dashDirection;

    public Transform firstPersonCameraTransform => fpsCameraArm.transform;
    public float MaxSpeed => maxSpeed;
    public Combat Combat => combat;

    private Joystick joystick;
    private bool isMobile;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        combat = GetComponent<Combat>();
        isMobile = GameManager.Instance.isMobile;
    }

    private void Start()
    {
        pointer = GameManager.Instance.LevelManager.Pointer;
        if (isMobile)
        {
            joystick = GameManager.Instance.UIManager.JoystickUI.GetComponent<Joystick>();
        }
    }

    private void Update()
    {
        if (!combat.IsDead && Time.timeScale != 0f)
        {
            switch (GameManager.Instance.LevelManager.game3CType)
            {
                case LevelManager.game3Ctypes.topDown:
                    UpdateTopDownMovement();
                    break;
                case LevelManager.game3Ctypes.fps:
                    UpdateFpsView();
                    UpdateFpsMovement();
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        checkGround = CheckGround();
        if (!checkGround && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
        {
            rb.AddForce(-Physics.gravity * 0.3f, ForceMode.Acceleration);
        }
    }

    private bool CheckGround()
    {
        return Physics.Raycast(groundChecker.position, Vector3.down, 0.3f, groundLayer);
    }

    private void UpdateDash(Vector3 directionInput)
    {
        if (!dashMode) return;

        if (dashCoolTimer > 0f) dashCoolTimer -= Time.deltaTime;
        if (dashTimer > 0f) dashTimer -= Time.deltaTime;

        bool dashTriggered = !isMobile
            ? Input.GetMouseButtonDown(1)
            : false; // или UI кнопка, если ты добавишь её

        if (dashTriggered && dashCoolTimer <= 0f)
        {
            dashCoolTimer = dashCoolTime;
            dashTimer = dashTime;
            dashDirection = directionInput.magnitude < 0.9f ? transform.forward : directionInput;
            AudioManager.PlaySFXAtPosition("Dash", transform.position);
        }

        if (dashTimer > 0f)
        {
            rb.velocity = dashDirection * dashSpeed;
        }
    }

    private void UpdateTopDownMovement()
    {
        Vector3 input = isMobile
            ? new Vector3(joystick.Horizontal, 0f, joystick.Vertical)
            : new Vector3(
                (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
                0f,
                (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f)
            );

        Vector3 direction = input.normalized;
        float currentSpeed = dashMode ? fastSpeed : speed;

        velocityTemp = Quaternion.Euler(0f, GameManager.Instance.CameraManager.TopDownCameraArm.transform.eulerAngles.y, 0f) * direction * currentSpeed;
        velocityTemp.y = rb.velocity.y;
        rb.velocity = velocityTemp;

        Vector3 toPointer = FCTool.Vector3YToZero(pointer.position - transform.position).normalized;
        UpdateDash(toPointer);
    }

    private void UpdateFpsMovement()
    {
        Vector3 input = isMobile
            ? new Vector3(joystick.Horizontal, 0f, joystick.Vertical)
            : new Vector3(
                (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
                0f,
                (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f)
            );

        Vector3 direction = input.normalized;
        float currentSpeed = dashMode ? fastSpeed : speed;

        if (GameManager.Instance.CameraManager.FpsCameraArm)
        {
            if (GameManager.Instance.CameraManager.FpsCameraArm.Ads)
                currentSpeed *= 0.7f;

            Vector3 forward = FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.forward).normalized;
            Vector3 right = FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.right).normalized;

            velocityTemp = (forward * direction.z + right * direction.x).normalized * currentSpeed;
            velocityTemp.y = rb.velocity.y;
        }

        if (!isMobile && Input.GetKeyDown(KeyCode.Space) && checkGround)
        {
            velocityTemp.y = jumpSpeed;
        }

        if (Input.GetKey(KeyCode.Space) && floatingMode && velocityTemp.y < 0f)
        {
            velocityTemp.y = 0f;
        }

        rb.velocity = velocityTemp;

        Vector3 dashDir = (FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.forward) * direction.z +
                           FCTool.Vector3YToZero(GameManager.Instance.CameraManager.FpsCameraArm.transform.right) * direction.x).normalized;

        UpdateDash(dashDir);
    }

    private void UpdateFpsView()
    {
        if (isMobile) return; // пока не реализовано для мобилок

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + Input.GetAxis("mouse x") * fpsCameraArm.Sensitivity, 0f);
        fpsPitch += Input.GetAxis("mouse y") * -fpsCameraArm.Sensitivity;
        fpsPitch = Mathf.Clamp(fpsPitch, -89.9f, 89.9f);
        firstPersonCameraTransform.localRotation = Quaternion.Euler(fpsPitch, 0f, 0f);
    }
   
}
