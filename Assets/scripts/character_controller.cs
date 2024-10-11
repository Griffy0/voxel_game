using UnityEngine;

public class character_controller : MonoBehaviour
{
    public float look_sensitivity = 2f, look_smooth_damp = .05f;
    [HideInInspector]
    public float y_rotation, x_rotation;
    [HideInInspector]
    public float current_y, current_x;
    [HideInInspector]
    public float y_rotation_v, x_rotation_v;
    public Transform camera_pos;
    public float move_speed = 40;

    public int jump_height = 2000;

    public Rigidbody rb;
    public bool isgrounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //Debug.Log(moveHorizontal);
        Vector3 movement = transform.forward * moveVertical + transform.right * moveHorizontal;
        movement.Normalize();
        //if (!isgrounded){
        //    movement *= 0.8f;
        //}
        rb.AddForce(movement * move_speed, ForceMode.Acceleration);// * Time.deltaTime);
        if (Input.GetKey(KeyCode.Space)) {
            isgrounded = is_grounded();
            if (isgrounded)
            {
                isgrounded = false;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(Vector3.up * jump_height, ForceMode.Impulse);
            }
        }
    }

    bool is_grounded()
    {
        // Implement a ground check (using Raycast or Collider checks)
        return Physics.Raycast(transform.position, Vector3.down, 1.02f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        y_rotation += Input.GetAxis("Mouse X") * look_sensitivity;
        x_rotation += Input.GetAxis("Mouse Y") * look_sensitivity;

        current_x = Mathf.SmoothDamp(current_x, x_rotation, ref x_rotation_v, look_smooth_damp);
        current_y = Mathf.SmoothDamp(current_y, y_rotation, ref y_rotation_v, look_smooth_damp);

        x_rotation = Mathf.Clamp(x_rotation, -80, 80);
        transform.rotation = Quaternion.Euler(0, current_y, 0);

        camera_pos.rotation = Quaternion.Euler(-current_x, current_y, 0);
        MovePlayer();
    }
}
