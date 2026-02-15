using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] float currentCharge = 0f;
    [SerializeField] float BaseDmg = 1;
    [SerializeField] float maxChargeAmt = 1f;
    [SerializeField] float chargeAddedPerFrame = 0.2f;
    [SerializeField] float chargeRemovedPerFrame = 0.5f;
    [SerializeField] float chargeDmgMultiplier = 1f;

    [SerializeField] Image chargeIndicator;

    bool attackIsHeld = false;
    float modifiedDmg = 0f;

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
        }
        else
        {
            currentCharge -= chargeRemovedPerFrame;
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
        modifiedDmg = chargeDmgMultiplier * BaseDmg;

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