using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building_Minigun : DPSTurret
{

    [Header("Model setup")]
    [SerializeField] Transform hinge;
    [SerializeField] Transform gunPoint;
    [SerializeField] GameObject shootParticle;

    void Update()
    {
        if (target == null) //Ha nincsen c�lpont..
        {
            GetNewTarget();
        }
        else //Ha van c�lpont...
        {
            if (target != null)
            {
                Vector3 lookAtPos = target.transform.position;
                lookAtPos.y = transform.position.y;
                hinge.LookAt(lookAtPos);

                if (attackSpeedTimer <= 0)
                {
                    GetNewTarget();
                    if (target != null) Shoot();
                    //Ha 1 frame alatt siker�l meg�lni az ellens�get, �s nincsen m�s ellens�g a k�zelben, akkor error-t dob.
                }
            }
        }

        if (attackSpeedTimer > 0)
        {
            attackSpeedTimer -= Time.deltaTime;
        }
    }

    protected override void Shoot()
    {
        if (!upgradeType) //Gyors l�v� �g
        {
            base.Shoot();
            
        }
        else //�gy� �g
        {
            RaycastHit[] hit;

            Vector3 direction = (target.transform.position - transform.position).normalized;

            //Ez az �g egy cs�kba l�, itt a c�lzott ellens�gre c�lozva l� egy hossz� l�ved�ket, akiket eltal�l benne azt sebzi.
            hit = Physics.SphereCastAll(transform.position, 0.2f, direction, range + 1);
            Debug.DrawRay(transform.position, direction, Color.yellow, attackSpeed);
            if (hit.Length > 0)
            {
                List<Enemy> hitEnemies = new List<Enemy>();
                foreach (var item in hit)
                {
                    Enemy hitEnemy = item.transform.gameObject.GetComponentInChildren<Enemy>();
                    if (hitEnemy != null) hitEnemies.Add(hitEnemy);
                }

                foreach (var item in hitEnemies)
                {
                    item.Damage(damage, damageType);
                }
            }
        }
        attackSpeedTimer = attackSpeed;
        Instantiate(shootParticle, gunPoint.position, hinge.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
        if (target != null)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        }
        if (upgradeType) //�gy� �g
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, range+1);
        }
    }

    public override void Upgrade(bool uptype)
    {
        upgradeType = uptype;
        if (maxLevel())
        {
            Debug.Log("Max level!");
            return;
        }

        if (hasEnoughMoney())
        {
            if (!upgradeType) //A minigun �g
            {
                switch (upgradeLvl)
                {
                    case 1:
                    case 2:
                        attackSpeed -= 0.2f;
                        break;
                }
            }
            else //Az �gy� �g
            {
                switch (upgradeLvl)
                {
                    case 1:
                        attackSpeed += 0.1f;
                        damage += 3;
                        range += 1;
                        break;
                    case 2:
                        damage += 2;
                        range += 0.5f;
                        break;
                }
            }
        }
    }

}
