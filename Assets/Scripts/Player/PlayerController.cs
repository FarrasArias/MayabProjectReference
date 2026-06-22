using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float sprintSpeed = 12f;
    [SerializeField] float acceleration = 60f;
    [SerializeField] float deceleration = 50f;

    [Header("Aiming")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform aimPivot;
    [SerializeField] float aimSmoothing = 15f;
    [SerializeField] float groundPlaneHeight;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] string speedParam = "Speed";
    [SerializeField] string moveXParam = "MoveX";
    [SerializeField] string moveZParam = "MoveZ";

    [Header("References")]
    [SerializeField] InputReader input;
    [SerializeField] PlayerDash dash;
    [SerializeField] WeaponController weaponController;

    Rigidbody rb;
    Camera mainCam;
    Vector3 aimPoint;
    float currentSpeed;

    public Vector3 AimPoint => aimPoint;
    public Vector3 AimDirection => (aimPoint - transform.position).normalized;
    public Vector3 MoveDirection { get; private set; }
    public float CurrentSpeed => currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        mainCam = Camera.main;
        if (aimPivot == null) aimPivot = transform;
    }

    void Update()
    {
        UpdateAiming();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        if (dash != null && dash.IsDashing) return;

        Vector2 rawInput = input.Move;
        Vector3 inputDir = new Vector3(rawInput.x, 0f, rawInput.y);

        if (mainCam != null)
        {
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight = mainCam.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            inputDir = camForward * rawInput.y + camRight * rawInput.x;
        }

        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();

        MoveDirection = inputDir;

        float targetSpeed = moveSpeed;
        if (input.Sprint) targetSpeed = sprintSpeed;

        if (weaponController != null && weaponController.IsFiring)
            targetSpeed *= weaponController.CurrentWeapon != null ? weaponController.CurrentWeapon.movementPenalty : 0.6f;

        Vector3 targetVelocity = inputDir * targetSpeed;
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.y = 0f;

        float accel = inputDir.sqrMagnitude > 0.01f ? acceleration : deceleration;
        Vector3 newVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, accel * Time.fixedDeltaTime);
        newVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = newVelocity;
        currentSpeed = new Vector3(newVelocity.x, 0f, newVelocity.z).magnitude;
    }

    void UpdateAiming()
    {
        Vector2 mousePos = input.MousePosition;
        Ray ray = mainCam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0f));

        Plane ground = new Plane(Vector3.up, new Vector3(0f, groundPlaneHeight, 0f));
        if (ground.Raycast(ray, out float distance))
            aimPoint = ray.GetPoint(distance);

        Vector3 lookDir = aimPoint - aimPivot.position;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            aimPivot.rotation = Quaternion.Slerp(aimPivot.rotation, targetRotation, aimSmoothing * Time.deltaTime);
        }
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetFloat(speedParam, currentSpeed / moveSpeed);

        Vector3 localMove = transform.InverseTransformDirection(MoveDirection);
        animator.SetFloat(moveXParam, localMove.x, 0.1f, Time.deltaTime);
        animator.SetFloat(moveZParam, localMove.z, 0.1f, Time.deltaTime);
    }
}
