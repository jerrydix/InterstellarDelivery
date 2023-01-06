using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private float speed;
    
    private Rigidbody rb;
    private Vector3 velocity;

    private void Awake()
    {
        //transform.rotation = Quaternion.Euler(90,90,0);
        TryGetComponent(out rb);
    }
    
    void Start()
    {
        //var muzzleEffect = Instantiate(muzzlePrefab, transform.position, transform.rotation);
        //Destroy(muzzleEffect, 5f);
        velocity = transform.forward * speed;
    }
    void FixedUpdate()
    {
        var displacement = velocity * Time.deltaTime;
        rb.MovePosition(rb.position + displacement);
    }
}
