using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [SerializeField] private TurretLogic logic;
    [SerializeField] private Mount[] mounts;
    [SerializeField] private Transform target;
    [SerializeField] private float range;
    [SerializeField] private Transform lookingPoint;
    
    private GameObject _pause;

    private void Start()
    {
        _pause = GameObject.Find("UI");
    }

    void OnDrawGizmos()
    {
        if (!target) return;

        var dashLineSize = 2f;
        foreach (var mountPoint in mounts)
        {
            var hardpoint = mountPoint.transform;
            var from = Quaternion.AngleAxis(-mountPoint.limit / 2, hardpoint.up) * hardpoint.forward;
            var projection = Vector3.ProjectOnPlane(target.position - hardpoint.position, hardpoint.up);

            // projection line
            //Handles.color = Color.white;
            //Handles.DrawDottedLine(target.position, hardpoint.position + projection, dashLineSize);

            // do not draw target indicator when out of angle
            if (Vector3.Angle(hardpoint.forward, projection) > mountPoint.limit / 2) return;

            // target line
            //Handles.color = Color.red;
            //Handles.DrawLine(hardpoint.position, hardpoint.position + projection);

            // range line
            //Handles.color = Color.green;
            //Handles.DrawWireArc(hardpoint.position, hardpoint.up, from, mountPoint.limit, projection.magnitude);
            //Handles.DrawSolidDisc(hardpoint.position + projection, hardpoint.up, .5f);

        }
    }

    void Update()
    {
        
        if (!target) return;

        bool aimed = Physics.Linecast(lookingPoint.transform.position, target.position, out var hit) &&
                     hit.collider.CompareTag("Player");
        if (aimed)
        {
            foreach (var mountPoint in mounts)
            {
                if (!mountPoint.Aim(target.position))
                {
                    aimed = false;
                }
            }
        }
        
        if (!_pause.GetComponent<PauseMenu>().active && aimed)
            logic.Shoot();
    }
    
}