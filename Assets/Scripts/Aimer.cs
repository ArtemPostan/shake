using FlamingCore;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    public enum aimWays { aimPointer, aimMoveDirection, aimPlayer }

    public Combat combat;
    public aimWays aimWay;
    public Rigidbody rigidbody;

    [Space]
    [Header("Joints")]
    public Transform needAim;
    public Transform clone;

    private bool gameStarted;
    private Transform pointer;
    private PlayerControl player;
    private Vector3 lookVector;

    [SerializeField] private bool playerAimer;
    public List<Aimer> subAimers;
    private bool isSubAimer;
    [SerializeField] private bool aimZonFpsMode;

    public void SetAsSubAimer() => isSubAimer = true;

    public void SetPointer(Transform newPointer)
    {
        pointer = newPointer;
    }

    private void Start()
    {
        foreach (var sub in subAimers)
            sub?.SetAsSubAimer();
    }

    private void Update()
    {
        if (!isSubAimer)
            UpdateAim();

        foreach (var sub in subAimers)
            sub?.UpdateAim();
    }

    public void UpdateAim()
    {
        if (!gameStarted)
        {
            if (GameManager.Instance.LevelManager.GameState == LevelManager.gameStates.playing)
            {
                gameStarted = true;
                player = GameManager.Instance.LevelManager.Player;
                pointer = GameManager.Instance.LevelManager.Pointer;
            }
            return;
        }

        if ((playerAimer && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps) || combat.IsDead)
            return;

        switch (aimWay)
        {
            case aimWays.aimPlayer:
                var playerPos = GameManager.Instance.LevelManager.Player.transform.position;
                base.transform.rotation = Quaternion.LookRotation(FCTool.Vector3YToZero(playerPos - base.transform.position), Vector3.up);
                if (aimZonFpsMode && GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
                {
                    base.transform.rotation = Quaternion.LookRotation((playerPos - base.transform.position).normalized, Vector3.up);
                }
                break;

            case aimWays.aimPointer:
                if (!player.Combat.IsDead)
                {
                    if (GameManager.Instance.isMobile)
                    {
                        var joystick = GameManager.Instance.UIManager.FireJoystickUI;
                        Vector3 inputDir = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

                        if (inputDir.magnitude > 0.2f)
                        {
                            Transform cam = GameManager.Instance.CameraManager.TopDownCameraArm.transform;
                            Vector3 lookDir = cam.rotation * inputDir.normalized;
                            lookDir = FCTool.Vector3YToZero(lookDir.normalized);
                            base.transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
                            break;
                        }
                    }

                    if (pointer != null)
                    {
                        base.transform.rotation = Quaternion.LookRotation(FCTool.Vector3YToZero(pointer.position - base.transform.position), Vector3.up);
                    }
                }
                break;

            case aimWays.aimMoveDirection:
                if (rigidbody != null)
                {
                    lookVector = FCTool.Vector3YToZero(rigidbody.velocity);
                    if (lookVector.magnitude > 0.15f)
                        base.transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
                }
                break;
        }
    }

    private void LateUpdate()
    {
        if (!isSubAimer)
            LateUpdateAim();

        foreach (var sub in subAimers)
            sub?.LateUpdateAim();
    }

    public void LateUpdateAim()
    {
        if (gameStarted && needAim != null && clone != null)
        {
            needAim.transform.rotation = clone.transform.rotation;
        }
    }
}
