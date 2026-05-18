using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    private static readonly int Move = Animator.StringToHash("Move");

    private InputSystem_Actions action;
    private Rigidbody rb;
    private Animator anim;

    [SerializeField] private Camera cam;

    public float moveSpeed = 3f;

    Vector2 input;
    Vector3 moveDir;

    private void OnDestroy() => action.Dispose();

    private void Awake()
    {
        action = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        input = action.Player.Move.ReadValue<Vector2>();
        Vector3 camForward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
        moveDir = (camForward * input.y + camRight * input.x).normalized;
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion target = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 2000f * Time.deltaTime);
        }

        anim.SetFloat(Move, input.sqrMagnitude);
    }
    private void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + this.moveDir * moveSpeed * Time.fixedDeltaTime);
        
    }

    private void OnEnable() => action.Player.Enable();
    private void OnDisable() => action.Player.Disable();
}
