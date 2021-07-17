using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    //Enums
    public EnemyType enemyType;

    //Scripts
    private PlayerController _playerController;
    
    //Components
    private Rigidbody2D _enemyRb;
    
    //Movement related
    [SerializeField, Range(0, 50)] private float moveForce;
    [SerializeField, Range(0, 50)] private float jumpForce;
    private float _enemyXPos;
    private const float MinX = 21f;
    private const float MaxX = 35f;
    private int _direction = -1;
    private const float FirstTime = 2;
    private float _timer;
    
    //Audio
    private AudioSource _audioSource;
    public AudioClip hitAudio;
    
    private void Awake()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _enemyRb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();

        _timer = FirstTime;
    }

    private void FixedUpdate()
    {
        _enemyXPos = transform.position.x;
        
        //Moving left and right
        switch (_direction)
        {
            //Moving left
            case -1:
                if (_enemyXPos > MinX)
                {
                    _enemyRb.AddForce(Vector2.left * moveForce, ForceMode2D.Force); 
                }
                else
                {
                    _direction = 1;
                }
                break;
            
            //Moving right
            case 1:
                if (_enemyXPos < MaxX)
                {
                    _enemyRb.AddForce(Vector2.right * moveForce, ForceMode2D.Force);
                }
                else
                {
                    _direction = -1;
                }
                break;
        }

        
    }

    private void Update()
    {
        
        //If timer value will be lower than 0, enemy will jump
        _timer -= Time.deltaTime;
        
        if (_timer <= 0 && RandomIntValue(0, 5) <= 2)
        {
            _enemyRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _timer = FirstTime;
        }
    }

    private static int RandomIntValue(int minValue, int maxValue)
    {
        var randomValue = Random.Range(minValue, maxValue);
        return randomValue;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !_playerController.canTeleport)
            switch (enemyType)
            {
                case EnemyType.Slime:
                    _playerController.orbsCounter -= RandomIntValue(1, 10);
                    _audioSource.PlayOneShot(hitAudio);
                    break;
                
                case EnemyType.Biscuit:
                    const int amplifier = 2;
                    _playerController.orbsCounter -= RandomIntValue(1, 10) * amplifier;
                    _audioSource.PlayOneShot(hitAudio);
                    break;
                
                default:
                    _playerController.orbsCounter--;
                    break;
            }
    }
    
    public enum EnemyType
    {
        Slime,
        Biscuit
    }
}
