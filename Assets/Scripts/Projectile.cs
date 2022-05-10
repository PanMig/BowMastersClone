using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool _hasCollision = false;
    [SerializeField] private float _damage;
    [SerializeField] private GameObject Particle;
    [SerializeField] private AudioClip arrowHit;
    [SerializeField] private AudioClip arrowMiss;
    [SerializeField] private AudioClip TakeOff;
    
    public delegate void OnCollision(GameObject projectile);
    public static OnCollision OnEnteredCollision;
    
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        SoundManager.Instance.PlayClip(TakeOff);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_hasCollision)
        {
            RotateBasedOnDirectionAngle();
        }
    }

    void RotateBasedOnDirectionAngle()
    {
        // vectors direction angle
        var velocity = rb.velocity;
         var angle = Mathf.Atan2(velocity.y , velocity.x) * Mathf.Rad2Deg;
         //rb.MoveRotation(angle);

        //rotate projectile based on angle of the rigid body
        var quat = Quaternion.AngleAxis(angle, transform.forward);
        transform.rotation = new Quaternion(0,0,quat.z,quat.w);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_hasCollision) return;
        _hasCollision = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        OnEnteredCollision?.Invoke(this.gameObject);
        if (other.gameObject.CompareTag("upperBody"))
        {
            other.gameObject.GetComponentInParent<Character>().TakeDamage(_damage*2.0f);
            SpawnPartdile();
        }
        else if (other.gameObject.CompareTag("lowerBody"))
        {
            other.gameObject.GetComponentInParent<Character>().TakeDamage(_damage);
            SpawnPartdile();
        }
        SoundManager.Instance.PlayClip(arrowMiss);
        Destroy(gameObject, 3.0f);
    }

    public void SpawnPartdile()
    {
        if (Particle)
        {
            var part = Instantiate(Particle, transform.position + (transform.forward * 2), Quaternion.identity);
            part.GetComponent<ParticleSystem>().Play();
            Destroy(part,1);
            
            SoundManager.Instance.PlayClip(arrowHit);
        }
    }
}
