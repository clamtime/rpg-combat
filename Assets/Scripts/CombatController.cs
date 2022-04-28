using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public List<Effect> effects;
    public GameObject weapon;
    public bool weaponDrawn = false;

    public Transform drawnWeaponPosition, sheathWeaponPosition;

    private StarterAssets.ThirdPersonController tpc;
    private bool attacking = false;
    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        tpc = GetComponent<StarterAssets.ThirdPersonController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        DisableEffects();
        SheathWeapon();
    }

    void Update()
    {
        if (tpc._input.attack && !attacking)
        {
            if (weaponDrawn)
            {
                attacking = true;
                animator.SetTrigger("Attack");
                StartCoroutine(EffectAttack());
                StartCoroutine(EffectAudio());
            }
            else
            {
                tpc._input.attack = false;

                DrawWeapon();
            }
        }
    }

    private void DrawWeapon()
    {
        animator.SetTrigger("Sheath");
        animator.SetBool("WeaponDrawn", weaponDrawn);
        weaponDrawn = true;
        transform.parent = drawnWeaponPosition;
        transform.position = new Vector3(0, 0, 0);
    }

    private void SheathWeapon()
    {
        animator.SetTrigger("Sheath");
        animator.SetBool("WeaponDrawn", weaponDrawn);
        weaponDrawn = false;
        transform.parent = sheathWeaponPosition;
        transform.position = new Vector3(0, 0, 0);
    }

    IEnumerator EffectAudio()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            yield return new WaitForSeconds(effects[i].audioDelay);
            audioSource.PlayOneShot(effects[i].audio);
        }
    }
    IEnumerator EffectAttack()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            yield return new WaitForSeconds(effects[i].effectDelay);
            effects[i].effectObject.SetActive(true);
        }

        yield return new WaitForSeconds(1);
        DisableEffects();
        tpc._input.attack = attacking = false;
        animator.ResetTrigger("Attack");
    }

    private void DisableEffects()
    {
        for (int i = 0; i < effects.Count; i++)
            effects[i].effectObject.SetActive(false);
    }

}

[System.Serializable]
public class Effect
{
    public GameObject effectObject;
    public float effectDelay, audioDelay;
    public AudioClip audio;
}