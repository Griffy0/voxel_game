using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class generate_terrain : MonoBehaviour
{
    public Material base_texture;
    public Material mid_texture;
    public Material high_texture;
    public GameObject tile;
    public int x_tiles = 50;
    public int y_tiles = 50;
    public float amplifier = 2.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public List<List<voxel>> voxels = new List<List<voxel>>();
    public List<GameObject> voxel_objects = new List<GameObject>();
    void Start(){        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        float perlin_offset = Random.Range(1.0f, 10000.0f);
        for (int i = 0; i < x_tiles; i++){
            //voxel_objects.Add(new List<List<GameObject>>());
            for (int j = 0; j < y_tiles; j++){
                //voxel_objects[i].Add(new List<GameObject>());
                int noise = Mathf.RoundToInt(Mathf.Pow(Mathf.PerlinNoise(
                    ((1.0f / x_tiles) * i) + perlin_offset, 
                    ((1.0f / y_tiles) * j) + perlin_offset) 
                    * 10.0f, 
                    amplifier));

                Debug.Log(((0.9f / y_tiles) * j) + 0.05f);
                for (int k = 0; k < noise + 1; k++){
                    GameObject new_obj = Instantiate(tile, new Vector3(i, k, j), Quaternion.identity);
                    if (k >= 10 && k < 25){
                        new_obj.GetComponent<Renderer>().material = mid_texture;
                    }
                    else if (k >= 25){
                        new_obj.GetComponent<Renderer>().material = high_texture;
                    }
                    else {
                        new_obj.GetComponent<Renderer>().material = base_texture;
                    }
                    voxel_objects.Add(new_obj);
                    
                }
            }
        }
        for (int i=0;i<voxel_objects.Count;i++){
            Debug.Log(voxel_objects[i].transform.position);
        }
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
