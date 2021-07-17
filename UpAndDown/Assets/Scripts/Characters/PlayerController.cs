using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody2D _playerRb;
    private Animator _playerAnim;
    private Transform _playerTransform;
    public Transform attackPoint;

    //Floats
    [SerializeField, Range(0f, 100f)] private float moveSpeed;
    [SerializeField, Range(0f, 30f)] private float jumpForce;
    [SerializeField, Range(0, 5f)] private float attackRange;
    private const float SpriteRotation = 180;
    
    //Layers
    public LayerMask enemyLayers;
    public LayerMask groundMask;
    
    //Vectors
    private readonly Vector2 _offset = new Vector2(0, 0.5f);
    
    //Particles
    public ParticleSystem orbParticle;
    public ParticleSystem slimeParticle;
    public ParticleSystem biscuitParticle;
    
    //Int
    public int orbsCounter;
    public int extraJumpsValue;
    private int _extraJumps;
    
    //Bool
    public bool canTeleport;

    //Audio
    private AudioSource _audioSource;
    public AudioClip[] jumpSounds;
    public AudioClip[] pickupSounds;
    public AudioClip attackSound;
    
    private void Awake()
    {
        _playerTransform = GetComponent<Transform>();
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    
    private void FixedUpdate()
    {
        //Horizontal movement
        if (Input.GetKey(KeyCode.A))
        {
            _playerTransform.rotation = Quaternion.Euler(0, SpriteRotation, 0);
            _playerRb.AddForce(Vector2.left * moveSpeed, ForceMode2D.Force);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _playerRb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Force);
            _playerTransform.rotation = Quaternion.identity;
        }
        
        //Animation to walk
        WalkAnimation();
    }

    private void Update()
    {
        if(IsGrounded())
        {
            _extraJumps = extraJumpsValue;
        } 
        
        if (canTeleport)
        {
            Invoke("Teleport", 0.1f);
            
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && _extraJumps > 0)
        {
            _playerAnim.SetTrigger("Jump");
            _playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _audioSource.PlayOneShot(jumpSounds[RandomSound()]);
            _extraJumps--;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        
        _playerAnim.SetTrigger("Attack");
        _audioSource.PlayOneShot(attackSound);

        foreach (var enemy in hitEnemies)
        {
            switch (enemy.gameObject.tag)
            {
                case "Enemy":
                    Instantiate(slimeParticle, (Vector2)enemy.transform.position + _offset, Quaternion.identity);
                    break;
                case "Enemy_Biscuit":
                    Instantiate(biscuitParticle, (Vector2)enemy.transform.position + _offset, Quaternion.identity);
                    break;
            }
            Destroy(enemy.gameObject);
            
        }
    }
    
    
    private void Teleport()
    {
        _playerTransform.position = RandomPos();
        canTeleport = false;
    }
    
    private static Vector2 RandomPos()
    {
        const float minXValue = 20f;
        const float maxXValue = 40f;
        var randomXValue = Random.Range(minXValue, maxXValue);

        const float minYValue = -1f;
        const float maxYValue = 25f;
        var randomYValue = Random.Range(minYValue, maxYValue);

        var randomPos = new Vector2(randomXValue, randomYValue);
        return randomPos;
    }
    
    
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void WalkAnimation()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _playerAnim.SetBool("IsWalking", true);
        }
        else
        {
            _playerAnim.SetBool("IsWalking", false);
        }
            
    }
    
    private bool IsGrounded()
    {
        var offsetRaycastPos = new Vector2(0, 0.5f);
        var position = (Vector2)transform.position + offsetRaycastPos;
        var direction = Vector2.down;
        const float distance = 1f;

        var hit = Physics2D.Raycast(position, direction, distance, groundMask);
        Debug.DrawRay(position, direction, Color.red);
        
        return hit.collider != null ? true : false;
    }

    private static int RandomSound()
    {
        const int minValue = 0;
        const int maxValue = 2;
        var randomValue = Random.Range(minValue, maxValue);
        return randomValue;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            Instantiate(orbParticle, other.transform.position, Quaternion.identity);
            orbsCounter++;
            _audioSource.PlayOneShot(pickupSounds[RandomSound()]);
        }
            
    }
}
