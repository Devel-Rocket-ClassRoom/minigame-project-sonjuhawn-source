using UnityEngine;
using static CharacterStateMachine;

public class CharacterMover : MonoBehaviour
{
    private static readonly int MoveHash = Animator.StringToHash("Move");

    [SerializeField] private float moveSpeed;
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float speedPerAgility = 0.2f;
    private IStatProvider stats;
    [SerializeField] private float rotationSpeed = 2000f;
    [SerializeField] private Camera cam;

    private PlayerInputHandler input;
    private CharacterStateMachine state;
    private Rigidbody rb;
    private Animator anim;

    private Vector3 moveDir;

    private void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
        state = GetComponent<CharacterStateMachine>();
        stats = GetComponent<IStatProvider>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        stats.OnStatChanged += RecalculateSpeed;
        RecalculateSpeed();
    }
    private void OnDisable() => stats.OnStatChanged -= RecalculateSpeed;

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
        if (!CanMove())
            return;

        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    bool CanMove()
    {

        return state.IsState(PlayerState.Idle) ||
               state.IsState(PlayerState.Moving);
    }

    public void EnterAttack()
    {
        moveDir = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }

    private void RecalculateSpeed()
    {
        moveSpeed = baseSpeed + Mathf.Max(0, stats.Agility - 10) * speedPerAgility * 0.2f;
    }
}