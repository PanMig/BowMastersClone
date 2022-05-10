using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateProjectileTrajectory : MonoBehaviour
{
    //Projectile trajectory
    [SerializeField] private GameObject Point;
    [SerializeField] private int numberOfPoints;
    [SerializeField] private float spaceBetween;
    [SerializeField] private List<GameObject> _points = new List<GameObject>();
    [SerializeField] private GameObject _projectileSocket;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private GameObject _weaponObj;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            _points.Add(GameObject.Instantiate(Point, _projectileSocket.transform.position, Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //create projectile trajectory
        if (!inputHandler.EnableInput)
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                _points[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < numberOfPoints; i++)
            {
                _points[i].SetActive(true);
                _points[i].transform.position = PointPosition(i * spaceBetween);
            }
        }
    }
    
    private Vector2 PointPosition(float t)
    {
        var force = GetComponent<Character>().force;
        var pos = (Vector2)_projectileSocket.transform.position +
                  ((Vector2)_weaponObj.transform.right.normalized * force * t) 
                  + 0.5f * Physics2D.gravity * (t * t);
        return pos;
    }
}
