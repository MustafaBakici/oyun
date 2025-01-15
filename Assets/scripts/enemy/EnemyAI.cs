using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAI : MonoBehaviour
{
    [Header("Player Ayarları")]
    public Transform player; // Oyuncunun pozisyonu
    public LayerMask playerLayer; // Oyuncu Layer'ı
    public LayerMask obstacleLayer; // Duvar Layer'ı

    [Header("Görüş Ayarları")]
    public float sightRange = 10f; // Görüş mesafesi
    public float fieldOfView = 90f; // Görüş açısı (derece)

    [Header("Hareket Ayarları")]
    public float speed = 3f; // Hareket hızı

    [Header("Saldırı Ayarları")]
    public float attackRange = 1.5f; // Saldırı mesafesi
    public float attackCooldown = 1f; // Saldırı soğuma süresi

    [Header("Dönüş Ayarları")]
    public float rotationSpeed = 200f; // Dönüş hızı

    [Header("Sağlık ve Hasar")]
    public int maxHealth = 100; // Maksimum sağlık
    private int currentHealth; // Mevcut sağlık

    [Header("Düşman Saldırı Hasarı")]
    public int enemyDamage = 10; // Düşmanın vereceği hasar

    private float attackTimer = 0f; // Saldırı zamanlayıcısı
    private bool canSeePlayer = false; // Oyuncuyu görme durumu
    private bool isPlayerInAttackRange = false; // Oyuncunun saldırı menzilinde olup olmadığı durumu
    private Vector3 targetPosition; // Geri dönülmesi gereken pozisyon
    private Quaternion initialRotation; // Başlangıç yönü
    private bool EnemyisDead = false; // Ölüm durumu kontrolü

    private NavMeshAgent _agent;
    private Rigidbody2D _rb; // Rigidbody bileşeni
    private Animator _enemyAnimator; // Animator bileşeni
    private Vector3 _lastseen;
    private bool hizsifir = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAnimator = GetComponentInChildren<Animator>(); // Animator'ı alt öğeden al
        player = GameObject.FindGameObjectWithTag("player").transform; // Oyuncuyu "player" tag'ine göre bul
        targetPosition = transform.position; // Başlangıç pozisyonunu kaydet
        initialRotation = transform.rotation; // Başlangıç yönünü kaydet
        currentHealth = maxHealth; // Başlangıçta sağlık maksimum değerine ayarlandı
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void Update()
    {
        Debug.Log(_lastseen);
        if (EnemyisDead) return; // Düşman öldüyse hiçbir şey yapma
        // Saldırı zamanlayıcısını azalt
        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;

        // Oyuncuyu görme durumunu kontrol et
        CheckIfPlayerInSight();

        // Oyuncu saldırı menzilinde mi kontrol et
        CheckIfPlayerInAttackRange();

        if (canSeePlayer && !isPlayerInAttackRange) // Eğer oyuncu görülüyorsa ve saldırı menzilinde değilse
        {
            MoveTowardsPlayer();
            FaceTarget();

        }
        else if (canSeePlayer && isPlayerInAttackRange) // Eğer oyuncu saldırı menzilindeyse, saldır
        {


            // Sadece saldırı soğuma süresi dolduysa saldır
            if (attackTimer <= 0f)
            {
                FaceTarget();
                Attack();
                attackTimer = attackCooldown; // Zamanlayıcıyı sıfırla
            }
        }
        else 
        {
            MoveTowardsTargetPosition();
            FaceTarget();

        }

        // Animator parametrelerini güncelle
        UpdateAnimator();
    }



    void CheckIfPlayerInSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector2.Angle(transform.up, directionToPlayer); //burayı düzelt

        var hit = Physics2D.RaycastAll(transform.position, directionToPlayer, sightRange);


        var hittag = hit.Select(c => c.transform.gameObject.tag);



        if (!(hittag.Any(c => c == "Wall")) && hittag.Any(c => c == "player"))
        {
            canSeePlayer = true;
            _lastseen = player.position;
            return;
        }



        canSeePlayer = false;



    }

    void CheckIfPlayerInAttackRange()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            isPlayerInAttackRange = true;
        }
        else
        {
            isPlayerInAttackRange = false;
        }
    }

    void FaceTarget()
    {
        var vel = _agent.velocity;
        vel.z = 0;

        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(
                                    Vector3.forward,
                                    vel
            );
        }
    }

    void MoveTowardsPlayer()
    {
        Debug.Log("ilerle");
        _agent.SetDestination(player.position);
    }

    void MoveTowardsTargetPosition()
    {
        if (_lastseen != Vector3.zero)
        { 
            _agent.SetDestination(_lastseen); 
        }

        var islem = Vector3.Distance(_lastseen,targetPosition);
        

        if (_lastseen != Vector3.zero && hizsifir && islem > 0.1 )
        {

            _lastseen = Vector3.zero;
            StartCoroutine(EnemyWait(5));
            _agent.SetDestination(targetPosition);
        }
        
        

    }

    //float delaytime = 5;
    IEnumerator EnemyWait(float delaytime) { yield return new WaitForSeconds(delaytime); }
    


    void Attack()
    {
        playerall playerScript = player.GetComponent<playerall>();

        if (playerScript != null && !playerScript.isDead) // playerScript üzerinden kontrol
        {


            if (_enemyAnimator != null)
            {
                _enemyAnimator.SetTrigger("isAttack");
                AudioManager.instance.Play("enemyattack");
            }

            playerScript.TakeDamage(enemyDamage); // Oyuncuya hasar ver
        }
    }


    public void EnemyTakeDamage(int damage)
    {
        currentHealth -= damage;

        if (_enemyAnimator != null)
        {
            _enemyAnimator.SetTrigger("takeHit"); // Saldırı animasyonu tetikleniyor
            AudioManager.instance.Play("enemyattack");
        }

        if (currentHealth <= 0)
        {

            EnemyDie();
        }
    }

    void EnemyDie()
    {
        EnemyisDead = true; // Ölüm durumunu işaretle
        GetComponent<Collider2D>().enabled = false; // Çarpışmayı devre dışı bırak
        GetComponent<NavMeshAgent>().enabled = false;
        if (_agent != null)
        {
            _agent.velocity = Vector2.zero; // Hızı sıfırla
            //_agent.angularVelocity = 0f;   // Dönmeyi sıfırla
            //_agent.isKinematic = true;     // Fizik etkileşimlerini devre dışı bırak
        }

        if (_enemyAnimator != null)
        {
            _enemyAnimator.SetTrigger("isDead"); // Ölüm animasyonu tetikleniyor
            AudioManager.instance.Play("enemydie");
        }

        //Destroy(gameObject,1.5f);
    }

    void UpdateAnimator()
    {
        // Hareket durumunu Animator'a gönder
        Vector2 velocity = _agent.velocity;
        bool isMoving = velocity.magnitude > 0.1f; // Hareket varsa true

        // Animator parametrelerini güncelle
        _enemyAnimator.SetBool("isMoving", isMoving);

        hizsifir = !isMoving;

        if (!isMoving && !canSeePlayer)
        {
                
                transform.rotation = initialRotation;
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Vector3 forward = transform.up * sightRange;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -fieldOfView / 2) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, fieldOfView / 2) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        if (canSeePlayer && player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}




//player hasar aldığında enemyden uzaklaşacak
//vurulunca sendeleyecek
//kapı mekaniği ekle
//kc ku
//player ölünce yerine dönmeli veya idle başlatmalı
//delaytime çalışmıyor
//görüş açısı bozuk
//enemy playerin konumunu değil alanını almalı

