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
    SphereCollider leftHandCollider, rightHandCollider, leftFootCollider, rightFootCollider;

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
            leftHandCollider = animator.GetBoneTransform(HumanBodyBones.LeftHand).GetComponent<SphereCollider>();
            rightHandCollider = animator.GetBoneTransform(HumanBodyBones.RightHand).GetComponent<SphereCollider>();
            leftFootCollider = animator.GetBoneTransform(HumanBodyBones.LeftFoot).GetComponent<SphereCollider>();
            rightFootCollider = animator.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<SphereCollider>();

            DisableAllHitboxes();

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
                    EnableHitbox(attacks[comboCount]);
                    //swordCollider.enabled = true; // Enable collider
                }
            }
            else if(attackState == AttackState.Impact)
            {
                //if(normalizedTime >= impactEndTime)
                if(normalizedTime >= attacks[comboCount].ImpactEndTime)
                {
                    attackState = AttackState.Cooldown;
                    DisableAllHitboxes();
                    //swordCollider.enabled = false; // Disable collider
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

    void EnableHitbox(AttackData attack)
    {
        switch (attack.HitboxToUse)
        {
            case AttackHitbox.LeftHand:
                leftHandCollider.enabled = true;
                break;
            case AttackHitbox.RightHand:
                rightHandCollider.enabled = true;
                break;
            case AttackHitbox.LeftFoot:
                leftFootCollider.enabled = true;
                break;
            case AttackHitbox.RightFoot:
                rightFootCollider.enabled = true;
                break;
            case AttackHitbox.Sword:
                swordCollider.enabled = true;
                break;
            default:
                break;
        }

    }

    void DisableAllHitboxes()
    {

        swordCollider.enabled = false;
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        leftFootCollider.enabled = false;
        rightFootCollider.enabled = false;
    }
}
