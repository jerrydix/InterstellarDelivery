using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private bool floating = false;
    private bool antiGravityBool = false;
    private ConstantForce constantForce;
    private bool modifying;

    private Vector3 _originalTransform;

    private void Start()
    {
        constantForce = GetComponent<ConstantForce>();
        constantForce.enabled = false;
        _originalTransform = transform.position;
    }
    public void ChangeGravity()
    {
        //if (!modifying)
        //{
            modifying = true;
            antiGravityBool = true;
            constantForce.enabled = antiGravityBool;
        //}
        // else if (modifying)
        // {
        //     modifying = false;
        //     antiGravityBool = false;
        //     constantForce.enabled = antiGravityBool;
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutOfMapCollider"))
        {
            Debug.Log("test2");
            transform.position = _originalTransform;
            GameObject.Find("Gun").GetComponent<PlayerShooting>().Deselect();
        }
    }
}
