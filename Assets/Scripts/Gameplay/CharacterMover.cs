using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class CharacterMover : NetworkBehaviour
{
    public float speed;
    float horizontal;
    float vertical;
    private void Start()
    {
        if (!IsOwner)
        {
            this.enabled = false;
            Destroy(GetComponent<Rigidbody>());
        }
    }
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(horizontal * Time.deltaTime * speed, 0, vertical * Time.deltaTime * speed);
        transform.position += dir;
    }
}
