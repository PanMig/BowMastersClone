using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [field: SerializeField]
    public float MaxHealth { get; set; }
    public float force;
    
    public SO_Character characterStats;
    public Vector2 angleBounds; //used to restrict aim rotation
    public IInputHandling inputHandler;
    public String animationShot;
    
    //shooter character parts -- child obj
    protected GameObject[] _bodyParts = new GameObject[2];
    protected GameObject _weaponSocket;
    protected GameObject _projectileSocket;
    protected GameObject _projectileObj;
    protected Animator anim;

    
    //Delegates & Events
    public delegate void ShootProjectile(GameObject projectile);
    public ShootProjectile OnProjectileShoot;
    
    public delegate void OnTakenDamage(float damage);
    public OnTakenDamage ONTakenDamage;
    
    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    private void OnDisable()
    {
        if (inputHandler.ONShootFired != null) inputHandler.ONShootFired -= Shoot;
    }

    public virtual void Init()
    {
        _bodyParts[0] = Helpers.FindComponentInChildWithTag<SpriteRenderer>(this.gameObject, "upperBody").gameObject;
        _bodyParts[1] = Helpers.FindComponentInChildWithTag<SpriteRenderer>(this.gameObject, "lowerBody").gameObject;
        _weaponSocket = Helpers.FindComponentInChildWithTag<Transform>(_bodyParts[0], "weaponSocket").gameObject;
        _projectileSocket = Helpers.FindComponentInChildWithTag<Transform>(_bodyParts[0], "arrowSocket").gameObject;
        anim = GetComponent<Animator>();
        // register callback
        inputHandler.ONShootFired += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    public virtual void Aim()
    {
        // get input based on screen drag
        var shootVelocity = inputHandler.GetDirectionToTarget(new Rigidbody2D());
        var angle = Mathf.Atan2(shootVelocity.y, shootVelocity.x) * Mathf.Rad2Deg;
        //clamp angle to avoid strange rotations
        angle = Mathf.Clamp(angle, angleBounds.x, angleBounds.y);
        //calculate rotation amount
        var rot = Quaternion.AngleAxis(angle, Vector3.forward);
        //rotate upper body and weapon
        _bodyParts[0].transform.rotation = rot;
        _weaponSocket.transform.rotation = rot;
        _projectileSocket.transform.rotation = rot;
        //set force based on drag amount
        force = shootVelocity.magnitude * 2.5f;
        force = Mathf.Clamp(force, 5.0f, 30.0f);
    }

    public virtual void Shoot()
    {
        _projectileObj = GameObject.Instantiate(characterStats.projectilePrefab, _projectileSocket.transform.position,
            _projectileSocket.transform.rotation);
        
        OnProjectileShoot?.Invoke(_projectileObj);
        
        //apply force based on input dir
        _projectileObj.GetComponent<Rigidbody2D>().AddForce(_bodyParts[0].transform.right * force, ForceMode2D.Impulse);
        anim.Play(animationShot);
    }

    public void TakeDamage(float damageAmount)
    {
        MaxHealth -= damageAmount;
        ONTakenDamage?.Invoke(damageAmount);
    }

    public void SetCollidersActive(bool status)
    {
        _bodyParts[0].GetComponent<PolygonCollider2D>().enabled = status;
        _bodyParts[1].GetComponent<PolygonCollider2D>().enabled = status;
    }
}
