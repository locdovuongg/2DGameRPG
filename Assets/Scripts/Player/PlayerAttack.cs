using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform WeaponCollider;
    [SerializeField] private Transform bowFirePoint;
    [SerializeField] private GameObject arrowPrefab;

    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;

    private bool canAct = true;       // có thể hành động?
    private bool isAttacking = false; // đang tấn công?

    private bool Blocked() => !canAct || isAttacking; // hàm check khóa

    private void Awake()
    {
        playerControls = new PlayerControls();
        myAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started       += _ => Attack();
        playerControls.Combat.HeavyAttack.started  += _ => HeavyAttack();
        playerControls.Combat.BowAttack.started    += _ => BowAttack();
    }

    private void Update()
    {
        MouseFollowWithOffsett();
    }

    // ------------------ Attack Commands ------------------ //
    private void Attack()
    {
        if (Blocked()) return;

        isAttacking = true;
        myAnimator.SetTrigger("Attack");
    }

    private void HeavyAttack()
    {
        if (Blocked()) return;

        isAttacking = true;
        myAnimator.SetTrigger("HeavyAttack");
    }

    private void BowAttack()
    {
        if (Blocked()) return;

        isAttacking = true;
        myAnimator.SetTrigger("BowAttack");
    }

    // ------------------ Animation Events ------------------ //

    // Kết thúc animation attack
    public void DoneAttackAnimEvent()
    {
        if (!canAct) return;               // ⬅ CHẶN event khi bị đánh
        isAttacking = false;
        if (WeaponCollider) 
            WeaponCollider.gameObject.SetActive(false);
    }

    public void EnableAttackCollider()
    {
        if (!canAct) return;               // Không bật khi đang bị hit
        if (WeaponCollider)
            WeaponCollider.gameObject.SetActive(true);
    }

    // Bắn tên
    public void ShootArrowAnimEvent()
    {
        if (!canAct) return;

        if (arrowPrefab == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 dir = (mouseWorld - bowFirePoint.position).normalized;

        Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);
        GameObject arrow = Instantiate(arrowPrefab, bowFirePoint.position, rot);

        arrow.GetComponent<Arrow>().SetDirection(dir);
    }

    // ------------------ Action Lock / Unlock ------------------ //
    public void DisableActions()
    {
        canAct = false;
        isAttacking = false;

        if (WeaponCollider)
            WeaponCollider.gameObject.SetActive(false);

        // reset trigger để tránh lỗi stuck animation
        myAnimator.ResetTrigger("Attack");
        myAnimator.ResetTrigger("HeavyAttack");
        myAnimator.ResetTrigger("BowAttack");
    }

    public void EnableActions()
    {
        canAct = true;
        isAttacking = false;

        if (WeaponCollider)
            WeaponCollider.gameObject.SetActive(false);
    }

    // ------------------ Weapon Rotation ------------------ //
    private void MouseFollowWithOffsett()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            WeaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            WeaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
