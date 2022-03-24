using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public List<Effect> effects;
    public GameObject weapon;

    private StarterAssets.ThirdPersonController tpc;
    private bool attacking = false;
    private AudioSource audioSource;

    void Start()
    {
        tpc = GetComponent<StarterAssets.ThirdPersonController>();
        audioSource = GetComponent<AudioSource>();
        DisableEffects();
    }

    void Update()
    {
        if (tpc._input.attack && !attacking)
        {
            attacking = true;
            tpc._animator.SetTrigger("Attack");
            StartCoroutine(EffectAttack());
            StartCoroutine(EffectAudio());
        }
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
        tpc._animator.ResetTrigger("Attack");
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