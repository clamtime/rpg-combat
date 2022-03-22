using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    private StarterAssets.ThirdPersonController tpc;

    void Start()
    {
        tpc = GetComponent<StarterAssets.ThirdPersonController>();
    }

    void Update()
    {
        if (tpc._input.attack)
        {
            tpc._animator.SetBool("StartAttack", true);
            if (tpc._animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95)
            {
                tpc._input.attack = false;
                tpc._animator.SetBool("StartAttack", false);
            }
        }
    }


}
