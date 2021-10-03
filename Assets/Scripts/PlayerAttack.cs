using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public LayerMask Enemy_Layer;
    public Transform HitZone;

    public bool canAttack = true;

    private Animator animator;
    private int previousHorizontal;
    private int previousVertical;

    private bool attack_is_available = true;

    // STATS

    private float attackSpeed = 1f;
    private float attackDamage = 5f;
    private float attackRange = 1.5f;

    ////////



    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        previousHorizontal = ((int)animator.GetFloat("previous_Horizontal"));
        previousVertical = ((int)animator.GetFloat("previous_Vertical"));

        if(previousVertical == 1)
        {
            HitZone.localPosition = new Vector2(0f, 0.5f);
        }
        else if(previousVertical == -1)
        {
            HitZone.localPosition = new Vector2(0f, -0.76f);
        }
        else if(previousHorizontal == 1)
        {
            HitZone.localPosition = new Vector2(0.67f, -0.2f);
        }
        else if(previousHorizontal == -1)
        {
            HitZone.localPosition = new Vector2(-0.67f, -0.2f);
        }


        if((Input.GetAxisRaw("Fire1") == 1 || Input.GetKey(KeyCode.Space)) && attack_is_available && canAttack)
        {
            attack_is_available = false;
            StartCoroutine(attackWait(attackSpeed));
            Attack();
        }
    }

    IEnumerator attackWait(float time)
    {
        yield return new WaitForSeconds(time);
        attack_is_available = true;
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(HitZone.position, attackRange/6, Enemy_Layer);
        if(enemiesHit.Length > 0)
        {
            foreach(Collider2D enemy in enemiesHit)
            {
                enemy.GetComponent<Enemy>().Hurt(attackDamage);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(HitZone.position, attackRange/6);
    }

}
