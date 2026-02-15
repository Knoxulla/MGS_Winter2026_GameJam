using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour
{
    #region Variables
    [Header("Damage")]
    [SerializeField] float baseDmg = 1;
    [SerializeField, Tooltip("additive damage modifer to be modified by powerUps")] float damageModifier = 0;
    [SerializeField, Tooltip("Automatically changes, this is just so it can be viewed")] float modifiedDmg = 0f;

    [Header("Speed")]
    [SerializeField] float slowPerFrame = 0.01f;
    [SerializeField] float speedUpPerFrame = 0.03f;
    private float originalSPD = 1;

    [Header("Charge")]
    [SerializeField] float currentCharge = 0f;
    [SerializeField] float maxChargeAmt = 1f;
    [SerializeField] float chargeAddedPerFrame = 0.2f;
    [SerializeField] float chargeRemovedPerFrame = 0.5f;
    [SerializeField] float chargeDmgMultiplier = 1f;

    [SerializeField] Image chargeIndicator;

    [Header("Aim")]
    [SerializeField] GameObject aimObject;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPoint;

    bool attackIsHeld = false;
    

    #endregion

    private void Start()
    {
        originalSPD = PlayerMASTER.Instance.playerMovementController.adjustedSpeed;
    }

    private void FixedUpdate()
    {

        if (attackIsHeld)
        {
            currentCharge += chargeAddedPerFrame;
            if (PlayerMASTER.Instance.playerMovementController.adjustedSpeed > 0.01f)
            {
                PlayerMASTER.Instance.playerMovementController.adjustedSpeed -= slowPerFrame;
            }
            else
            {
                PlayerMASTER.Instance.playerMovementController.adjustedSpeed = 0.01f;
            }
        }
        else
        {
            currentCharge -= chargeRemovedPerFrame;

            if (PlayerMASTER.Instance.playerMovementController.adjustedSpeed < originalSPD)
            {
                PlayerMASTER.Instance.playerMovementController.adjustedSpeed += speedUpPerFrame;
            }
            else
            {
                PlayerMASTER.Instance.playerMovementController.adjustedSpeed = originalSPD;
            }
        }

        currentCharge = Mathf.Clamp(currentCharge, 0, maxChargeAmt);

        UpdateChargeIndicator();
        CalculateDmg();

    }

    #region Aim

    public void OnLook(InputAction.CallbackContext ctx)
    { 
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        //LookAt(Vector3.zero);
        LookAt(mousePos);
    }

    public void LookAt(Vector3 target)
    {
        float lookAngle = AngleBetweenTwoPoints(aimObject.transform.position, target);

        aimObject.transform.localEulerAngles = new Vector3(0, 0, lookAngle + 90f);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y -b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    #endregion

    #region Attack
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            attackIsHeld = true;
        }
        else if (ctx.canceled)
        {
            attackIsHeld = false;
            chargeDmgMultiplier = 0;

            // Do Attack
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
       GameObject obj = Instantiate(projectile, projectileSpawnPoint);

       obj.GetComponent<ProjectileController>().AssignDamage(modifiedDmg);
    }

    private float CalculateDmg()
    {
        // Returns
        if (currentCharge == maxChargeAmt || !attackIsHeld) return 0f;


        if (chargeDmgMultiplier < 1)
        {
            chargeDmgMultiplier = 1;
        }

        chargeDmgMultiplier += currentCharge;
        modifiedDmg = chargeDmgMultiplier * (baseDmg + damageModifier);
        return modifiedDmg;

    }

    private void UpdateChargeIndicator()
    { 
        chargeIndicator.fillAmount = currentCharge / maxChargeAmt;

        if (currentCharge == maxChargeAmt)
        { 
            // do special indicator/sound!
        }
    }
    #endregion

}