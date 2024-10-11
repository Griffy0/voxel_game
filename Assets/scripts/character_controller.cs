using UnityEngine;

public class character_controller : MonoBehaviour
{
    public float look_sensitivity = 2f, look_smooth_damp = .5f;
    [HideInInspector]
    public float y_rotation, x_rotation;
    [HideInInspector]
    public float current_y, current_x;
    [HideInInspector]
    public float y_rotation_v, x_rotation_v;
    public Transform camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        y_rotation += Input.GetAxis("Mouse X") * look_sensitivity;
        x_rotation += Input.GetAxis("Mouse Y") * look_sensitivity;

        current_x = Mathf.SmoothDamp(current_x, x_rotation, ref x_rotation_v, look_smooth_damp);
        current_y = Mathf.SmoothDamp(current_y, y_rotation, ref y_rotation_v, look_smooth_damp);

        x_rotation = Mathf.Clamp(x_rotation, -80, 80);
        transform.rotation = Quaternion.Euler(0, current_y, 0);

        camera.rotation = Quaternion.Euler(-current_x, current_y, 0);
    }
}
