using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Look")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rotateSpeed = 15f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private CharacterController controller;
    private Camera mainCam;

    // ===== Magic Number Á¦°Å =====
    private const float FLAT_Y_VALUE = 0f;
    private const float MOVE_ANIMATION_THRESHOLD = 0.1f;
    private const float RAYCAST_DISTANCE = 1000f;
    private const float ROTATION_THRESHOLD = 0.0001f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Move();
        LookAtMouse();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, FLAT_Y_VALUE, v).normalized;

        controller.Move(dir * moveSpeed * Time.deltaTime);

        bool moving = dir.magnitude > MOVE_ANIMATION_THRESHOLD;

        animator.SetBool("isMoving", moving);
    }

    private void LookAtMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, RAYCAST_DISTANCE, groundLayer))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;

            Vector3 dir = (target - transform.position).normalized;

            if (dir.sqrMagnitude > ROTATION_THRESHOLD)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            }
        }
    }

    public void AddMoveSpeed(float amount)
    {
        moveSpeed += amount;
    }
}