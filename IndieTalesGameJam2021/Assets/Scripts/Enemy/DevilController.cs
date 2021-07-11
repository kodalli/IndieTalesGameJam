using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilController : MonoBehaviour {
    private Rigidbody2D rb;
    public float amplitude;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       Hover(); 
    }

    private void Hover() {
        var lastpos = transform.position;
        lastpos.y += Mathf.Cos(Mathf.PI * Time.time) * amplitude;
        transform.position = lastpos;
    }
}
