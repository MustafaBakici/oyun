using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mermii : MonoBehaviour
{
    public float lifetime = 3f; // Mermi ya�am s�resi
    public int damage = 5; // Mermi hasar�

    void Start()
    {
        // Belirli bir s�re sonra mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // E�er bir d��mana �arparsa, d��man� yok et
        if (collision.CompareTag("enemy"))
        {
            // D��mana hasar ver
            collision.gameObject.GetComponent<EnemyAI>().EnemyTakeDamage(damage);
            Destroy(gameObject); // Mermi yok ediliyor
        }
        // E�er bir oyuncuya �arparsa, oyuncuya hasar ver
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
