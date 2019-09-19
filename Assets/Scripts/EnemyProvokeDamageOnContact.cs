using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProvokeDamageOnContact : MonoBehaviour
{
    [SerializeField] float contactDamage = 30f;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //provoke damage
            Health health = collision.GetComponent<Health>();

            if (health != null)
            {
                health.TakeDamage(contactDamage);

            }
        }
    }
}
