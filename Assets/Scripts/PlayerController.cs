using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Look")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rotateSpeed = 15f;

    [Header("Animation")]
    [SerializeField] private Animator animator;   // Elf Animator 연결

    private CharacterController controller;
    private Camera mainCam;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;

        // Animator 자동 찾기 (안 넣어도 Elf에 있으면 자동 연결)
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

        Vector3 dir = new Vector3(h, 0f, v).normalized;

        controller.Move(dir * moveSpeed * Time.deltaTime);

        // ===== 애니메이션 =====
        bool moving = dir.magnitude > 0.1f;

        animator.SetBool("isMoving", moving);
    }

    private void LookAtMouse()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;

            Vector3 dir = (target - transform.position).normalized;

            if (dir.sqrMagnitude > 0.0001f)
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
