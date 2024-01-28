using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public enum AttackState { Idle, Windup, Impact, Cooldown }
public class MeleeFighter : MonoBehaviour
{
    [SerializeField] List<AttackData> attacks;
    [SerializeField] GameObject sword;
    BoxCollider swordCollider;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if(sword != null)
        {
            swordCollider = sword.GetComponent<BoxCollider>();
            swordCollider.enabled = false;
        }
    }

    AttackState attackState;
    bool doCombo;
    int comboCount = 0;
    public bool InAction { get; private set; } = false;
    public void TryToAttack()
    {
        if(!InAction)
        {
            StartCoroutine(Attack());
        }
        else if (attackState == AttackState.Impact || attackState == AttackState.Cooldown)
        {
            doCombo = true;
        }
    }
    IEnumerator Attack()
    {
        InAction = true;
        attackState = AttackState.Windup;

        //float impactStartTime = 0.33f;
        //float impactEndTime = 0.55f;

        //animator.CrossFade("Slash", 0.2f);
        animator.CrossFade(attacks[comboCount].AnimName, 0.2f); 
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(1);

        float timer = 0f;
        while(timer <= animState.length)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;

            if(attackState == AttackState.Windup)
            {
                //if(normalizedTime >= impactStartTime)
                if(normalizedTime >= attacks[comboCount].ImpactStartTime)
                {
                    attackState = AttackState.Impact;
                    swordCollider.enabled = true; // Enable collider
                }
            }
            else if(attackState == AttackState.Impact)
            {
                //if(normalizedTime >= impactEndTime)
                if(normalizedTime >= attacks[comboCount].ImpactEndTime)
                {
                    attackState = AttackState.Cooldown;
                    swordCollider.enabled = false; // Disable collider
                }
            }
            else if (attackState == AttackState.Cooldown)
            {
                //Handle Combos
                if (doCombo)
                {
                    doCombo = false;
                    comboCount = (comboCount + 1) % attacks.Count;
                    /*++comboCount;
                    if (comboCount >= attacks.Count)
                        comboCount = 0;
                    */
                    StartCoroutine(Attack());
                    yield break;
                }
            }
            yield return null;
        }

        //yield return new WaitForSeconds(animState.length);
        attackState = AttackState.Idle;
        comboCount = 0;
        InAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hitbox" && !InAction)
        {
            StartCoroutine(playHitReaction());
        }
    }

    IEnumerator playHitReaction()
    {
        InAction = true;

        animator.CrossFade("SwordImpact", 0.2f);
        yield return null;

        var animState = animator.GetNextAnimatorStateInfo(1);

        yield return new WaitForSeconds(animState.length * 0.8f);
        InAction = false;
    }
}
