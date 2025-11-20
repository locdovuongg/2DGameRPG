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

    private bool canAct = true;
    private bool isAttacking = false;

    private bool Blocked() => !canAct || isAttacking;

    private void Awake()
    {
        playerControls = new PlayerControls();   // Input System
        myAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();   // ❗ STOP leak input
    }

    private void Start()
    {
        playerControls.Combat.Attack.started      += _ => Attack();
        playerControls.Combat.HeavyAttack.started += _ => HeavyAttack();
        playerControls.Combat.BowAttack.started   += _ => BowAttack();
    }

    private void Update()
    {
        if (playerController == null) return;
        MouseFollowWithOffsett();
    }

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

    public void DoneAttackAnimEvent()
    {
        if (!canAct) return;
        isAttacking = false;

        if (WeaponCollider)
            WeaponCollider.gameObject.SetActive(false);
    }

    public void EnableAttackCollider()
    {
        if (!canAct) return;

        if (WeaponCollider)
            WeaponCollider.gameObject.SetActive(true);
    }

    public void ShootArrowAnimEvent()
    {
        if (!canAct || arrowPrefab == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 dir = (mouseWorld - bowFirePoint.position).normalized;
        Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);

        GameObject arrow = Instantiate(arrowPrefab, bowFirePoint.position, rot);
        arrow.GetComponent<Arrow>().SetDirection(dir);
    }

    public void DisableActions()
    {
        canAct = false;
        isAttacking = false;

        if (WeaponCollider)
            WeaponCollider.gameObject.SetActive(false);

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

    private void MouseFollowWithOffsett()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        if (mousePos.x < playerScreenPoint.x)
            WeaponCollider.rotation = Quaternion.Euler(0, -180, 0);
        else
            WeaponCollider.rotation = Quaternion.Euler(0, 0, 0);
    }
}
