using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    private bool _moveTowardPlayer;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedTowardPlayer;
    [SerializeField] private int _powerupID;

    private AudioSource _audioSource;
    private SpriteRenderer _renderer;

    private Transform _player;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        Player.callPowerups += MoveTowardPlayer;
    }

    private void OnDisable()
    {
        Player.callPowerups -= MoveTowardPlayer;
    }
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (_moveTowardPlayer == false)
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, 20 * Time.deltaTime);

        if (transform.position.y <= -5.4f)
            Destroy(this.gameObject);
    }

    public void MoveTowardPlayer()
    {
        _moveTowardPlayer = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AddHealth();
                        break;
                    case 4:
                        player.RefillAmmo();
                        break;
                    case 5:
                        player.ChainLaserActive();
                        break;
                    default:
                        break;
                }
            }
            _renderer.enabled = false;
            _audioSource.Play();
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(this.gameObject, _audioSource.clip.length + 0.3f);
        }
    }
}
