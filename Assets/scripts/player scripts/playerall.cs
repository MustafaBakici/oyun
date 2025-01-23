using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerall : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    public float moveSpeed = 5f;         // Hareket h�z�

    [Header("Ate� Etme Ayarlar�")]
    public GameObject bulletPrefab;     // Mermi prefab'�
    public Transform muzzle;            // Namlu pozisyonu
    public float bulletSpeed = 15f;     // Mermi h�z�

    [Header("Sa�l�k Ayarlar�")]
    public int maxHealth = 100;
    private int currentHealth;
    public HealthManager healthManager;

    private Rigidbody2D _rb;            // Rigidbody2D bile�eni
    private Vector2 movement;           // Hareket vekt�r�

    public bool isDead = false;        // �l� durum kontrol�
    private Animator _animator;         // Animator bile�eni

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>(); // Animator bile�enini al
        currentHealth = maxHealth;
        healthManager.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (isDead) return; // �l� durumdaysa hi�bir �ey yapma

        HandleMovement();
        HandleAiming();

        if (Input.GetMouseButtonDown(0) && !(pausemenu.isPause))
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
            _rb.velocity = movement;

        UpdateAnimator(); // Animator parametrelerini g�ncelle
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized * moveSpeed;
    }

    private void HandleAiming()
    {
        Vector2 mousePos = MouseUtils.GetMousePosition2d();
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        transform.up = direction;
    }

    //belki bura
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, transform.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        AudioManager.instance.Play("tabanca");

        if (bulletRb != null)
        {
            bulletRb.velocity = transform.up * bulletSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthManager.SetHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true; // �l� durumunu aktif et
        Debug.Log("Player �ld�!");

        // �l�m animasyonunu oynat
        if (_animator != null)
        {
            _animator.SetTrigger("isDead");
            AudioManager.instance.Play("death");

        }

        _rb.velocity = Vector2.zero; // Hareketi tamamen durdur
        Invoke("ReloadScene", 3f); // 2 saniye sonra sahne yeniden y�klenecek
    }

    //bura
    private void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);//
    }

    private void UpdateAnimator()
    {
        // Hareket durumunu Animator'a g�nder
        if (_animator != null)
        {
            bool isMoving = movement.magnitude > 0.1f; // Hareket varsa true
            _animator.SetBool("IsMoving", isMoving);
        }
    }
}





//player hasar ald���nda enemyden uzakla�acak
//hasar alma animasyonu gelecek
//scene kodlar� ��kart�lacak