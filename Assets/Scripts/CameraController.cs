using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    public float followSpeed = 5.0f;
    private Camera _cam;
    public Transform TargetTransform { get; set; }

    private void AddCameraShake()
    {
        StartCoroutine(Shake(0.08f, 0.1f));   
    }

    void Start()
    {
        _cam = GetComponent<Camera>();
        TargetTransform = this.transform;
    }

    private void FixedUpdate()
    {
        // we are in 2D, therefore we need to hardcode camera location
        var targetPos = new Vector3(TargetTransform.position.x , TargetTransform.position.y, -10.0f);
        //smoothly interpolate to the target position based on frame rate independent speed
        var newPos  = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.fixedDeltaTime);
        transform.position = new Vector3(newPos.x, newPos.y, -10);
    }
    
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }
}
