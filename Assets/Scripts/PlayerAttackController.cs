using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] float baseDmg = 1;
    [SerializeField, Tooltip("additive damage modifer to be modified by powerUps")] float damageModifier = 0;

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

    bool attackIsHeld = false;
    float modifiedDmg = 0f;

    private void Start()
    {
        originalSPD = PlayerMASTER.Instance.playerMovementController.adjustedSpeed;
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            attackIsHeld = true;
        }
        else if(ctx.canceled)
        {
            attackIsHeld = false;
            chargeDmgMultiplier = 0;
        }
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
        CalculateDmgMultiplier();
    }

    private void CalculateDmgMultiplier()
    {
        // Returns
        if (currentCharge == maxChargeAmt || !attackIsHeld) return;


        if (chargeDmgMultiplier < 1)
        {
            chargeDmgMultiplier = 1;
        }

        chargeDmgMultiplier += currentCharge;
        modifiedDmg = chargeDmgMultiplier * (baseDmg + damageModifier);

    }

    private void UpdateChargeIndicator()
    { 
        chargeIndicator.fillAmount = currentCharge / maxChargeAmt;

        if (currentCharge == maxChargeAmt)
        { 
            // do special indicator/sound!
        }
    }
}