using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    private static readonly int MoveHash = Animator.StringToHash("Move");

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 2000f;
    [SerializeField] private Camera cam;

    private PlayerInputHandler input;
    private Rigidbody rb;
    private Animator anim;

    private Vector3 moveDir;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 moveInput = input.MoveInput;
        Vector3 camForward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
        moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion target = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }
        anim.SetFloat(MoveHash, moveInput.magnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }
}