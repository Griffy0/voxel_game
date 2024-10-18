using UnityEngine;

public class drive : MonoBehaviour
{
    public Rigidbody rb;
    public float thrust = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward * thrust);
    }
}
