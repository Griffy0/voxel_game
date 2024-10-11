using UnityEngine;

public class voxel
{
    public Vector3 pos;
    public Material texture;
    
    public voxel(Vector3 position, Material mat){
        pos = position;
        texture = mat;
    }

}
