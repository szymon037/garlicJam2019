using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    private Rigidbody playerBody = null;
    private Animator anim = null;
    private Vector3 oldPosition = Vector3.zero;
    public bool isMoving;
    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        oldPosition = transform.position;
        isMoving = false;
    }

    void Update()
    {
        if (oldPosition != transform.position)
            isMoving = true;
        else isMoving = false;

        anim.SetBool("isMoving", isMoving);
    }
}
