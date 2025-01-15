using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mermii : MonoBehaviour
{
    public float lifetime = 3f; // Mermi yaþam süresi
    public int damage = 5; // Mermi hasarý

    void Start()
    {
        // Belirli bir süre sonra mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Eðer bir düþmana çarparsa, düþmaný yok et
        if (collision.CompareTag("enemy"))
        {
            // Düþmana hasar ver
            collision.gameObject.GetComponent<EnemyAI>().EnemyTakeDamage(damage);
            Destroy(gameObject); // Mermi yok ediliyor
        }
        // Eðer bir oyuncuya çarparsa, oyuncuya hasar ver
        if (collision.CompareTag("player"))
        {
            // Oyuncuya hasar ver
            collision.gameObject.GetComponent<playerall>().TakeDamage(damage);
            Destroy(gameObject); // Mermi yok ediliyor
        }

        if (collision.CompareTag("Wall"))
        {Destroy(gameObject);   }
    }
}
