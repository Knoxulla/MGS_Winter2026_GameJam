using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] float currentCharge;
    [SerializeField] float maxChargeAmt;
    [SerializeField] float chargeAddedPerFrame;
    [SerializeField] float chargeRemovedPerFrame;
    [SerializeField] float chargeDmgMultiplier;

    [SerializeField] Image chargeIndicator;

    bool attackIsHeld = false;

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
            currentCharge = Mathf.Clamp(currentCharge, 0, maxChargeAmt);
        }
        else
        {
            currentCharge -= chargeRemovedPerFrame;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxChargeAmt);
        }

        UpdateChargeIndicator();
        CalculateDmgMultiplier();
    }

    private void CalculateDmgMultiplier()
    {
        if (currentCharge == maxChargeAmt || !attackIsHeld) return;

        chargeDmgMultiplier += currentCharge;
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