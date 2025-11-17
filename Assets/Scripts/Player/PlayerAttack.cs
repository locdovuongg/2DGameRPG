using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform WeaponCollider;
    [SerializeField] private Transform bowFirePoint;
    [SerializeField] private GameObject arrowPrefab;
    private PlayerControls playerControls;
    private bool canAct = true;     
    private bool Blocked() => !canAct || isAttacking;   
    private Animator myAnimator;
    private PlayerController playerController;
    private bool isAttacking = false;
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
        playerControls.Combat.Attack.started += _ => Attack();
        playerControls.Combat.HeavyAttack.started += _ => HeavyAttack();
        playerControls.Combat.BowAttack.started   += _ => BowAttack();
    }

    private void Update()
    {
        MouseFollowWithOffsett(); 
    }

    private void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        myAnimator.SetTrigger("Attack");
    }
    private void HeavyAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        myAnimator.SetTrigger("HeavyAttack");
    }
    private void BowAttack()
    {
       if (isAttacking) return;

        isAttacking = true;
        myAnimator.SetTrigger("BowAttack");
    }
    public void DoneAttackAnimEvent()
    {
        WeaponCollider.gameObject.SetActive(false);
        isAttacking = false;

    }
    public void EnableAttackCollider() {
    WeaponCollider.gameObject.SetActive(true);
}
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
   public void ShootArrowAnimEvent()
{
    if (arrowPrefab == null) return;

    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mouseWorld.z = 0f;

    Vector3 dir = (mouseWorld - bowFirePoint.position).normalized;

    // quay mũi tên theo hướng bay
    Quaternion rot = Quaternion.FromToRotation(Vector3.right, dir);
    GameObject arrow = Instantiate(arrowPrefab, bowFirePoint.position, rot);

    arrow.GetComponent<Arrow>().SetDirection(dir);
}

    public void DisableActions() {
        canAct = false;
        isAttacking = false;                     // ⬅️ quan trọng: hủy kẹt attack
        if (WeaponCollider) WeaponCollider.gameObject.SetActive(false);
        // Xoá các trigger tấn công đang treo (tránh kẹt transition)
        myAnimator.ResetTrigger("Attack");
        myAnimator.ResetTrigger("HeavyAttack");
        myAnimator.ResetTrigger("BowAttack");
    }
    // Gọi ở frame cuối clip TakeDamage
    public void EnableActions() {
        canAct = true;
        isAttacking = false;
        if (WeaponCollider) WeaponCollider.gameObject.SetActive(false);
    }


}
