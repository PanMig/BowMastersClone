using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAI : Character
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Shoot()
    {
        _projectileObj = GameObject.Instantiate(characterStats.projectilePrefab, _projectileSocket.transform.position,
            _projectileSocket.transform.rotation);
        
        //get automatic input for shot direction
        var vel = inputHandler.GetDirectionToTarget(_projectileObj.GetComponent<Rigidbody2D>());

        OnProjectileShoot?.Invoke(_projectileObj);
        
        //apply force based on input dir
        _projectileObj.GetComponent<Rigidbody2D>().velocity = vel;
    }
}
