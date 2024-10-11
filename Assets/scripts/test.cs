using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Linq;
using System;

public class test : MonoBehaviour
{
    private Mesh mesh; 
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] normals;
    private Vector2[] uvs;
    private Material base_texture;
    public Material mid_texture;
    public Material high_texture;
    public int x_tiles = 1;
    public int z_tiles = 1;
    public int map_x = 30;
    public int map_z = 30;
    public float amplifier = 1.0f;

    private Dictionary<string, Vector2[][]> texture_coordinates = new Dictionary<string, Vector2[][]>{
        {
            "grass", new Vector2[][]{
                new Vector2[]{
                    new Vector2(0.2890625f, 0.2578125f), //0
                    new Vector2(0.53125f, 0.2578125f),   //1
                    new Vector2(0.2890625f, 0.015625f),  //2
                    new Vector2(0.53125f, 0.015625f),    //3
                },
                new Vector2[]{
                    new Vector2(0.2890625f, 0.2578125f), //0
                    new Vector2(0.53125f, 0.2578125f),   //1
                    new Vector2(0.2890625f, 0.015625f),  //2
                    new Vector2(0.53125f, 0.015625f),    //3
                },
                new Vector2[]{
                    new Vector2(0.2890625f, 0.2578125f), //0
                    new Vector2(0.53125f, 0.2578125f),   //1
                    new Vector2(0.2890625f, 0.015625f),  //2
                    new Vector2(0.53125f, 0.015625f),    //3
                },
                new Vector2[]{
                    new Vector2(0.2890625f, 0.2578125f), //0
                    new Vector2(0.53125f, 0.2578125f),   //1
                    new Vector2(0.2890625f, 0.015625f),  //2
                    new Vector2(0.53125f, 0.015625f),    //3
                },
                new Vector2[]{
                    new Vector2(0.2578125f, 0.015625f),  //18
                    new Vector2(0.015625f, 0.015625f),   //16
                    new Vector2(0.2578125f, 0.2578125f), //19
                    new Vector2(0.015625f, 0.2578125f),  //17
                },
                new Vector2[]{
                    new Vector2(0.8046875f, 0.015625f),  //18
                    new Vector2(0.5625f, 0.015625f),   //16
                    new Vector2(0.8046875f, 0.2578125f), //19
                    new Vector2(0.5625f, 0.2578125f),  //17
                },
                new Vector2[]{
                    new Vector2(0.8046875f, 0.015625f),  //18
                    new Vector2(0.5625f, 0.015625f),   //16
                    new Vector2(0.8046875f, 0.2578125f), //19
                    new Vector2(0.5625f, 0.2578125f),  //17
                }
            }
        }
    };

    tile[][][] generate_terrain(int x_tiles, int y_tiles, int z_tiles, float x_offset, float z_offset, float perlin_offset){
        //[x, z, y]
        float flat_mult = 0.025f;
        
        tile[][][] voxel_objects = new tile[x_tiles][][];
        for (int i = 0; i < x_tiles; i++){
            voxel_objects[i] = new tile[z_tiles][];
            for (int j = 0; j < z_tiles; j++){
                float noise = Mathf.Pow(Mathf.PerlinNoise(
                    (flat_mult * (i + x_offset)) + perlin_offset, 
                    (flat_mult * (j + z_offset)) + perlin_offset) 
                    * 10.0f, 
                    amplifier);
                voxel_objects[i][j] = new tile[y_tiles];
                for (int k = 0; k < y_tiles; k++){
                    int block_id;
                    List<string> tags = new List<string>();
                    if (k <= noise){
                        block_id = 1;
                    }
                    else {
                        block_id = 0;
                        tags.Add("transparent");
                    };
                    /*if (k >= 10 && k < 25){
                        new_obj.GetComponent<Renderer>().material = mid_texture;
                    }
                    else if (k >= 25){
                        new_obj.GetComponent<Renderer>().material = high_texture;
                    }
                    else {
                        new_obj.GetComponent<Renderer>().material = base_texture;
                    }*/
                    if (noise > k){
                        Debug.Log(noise-k);
                    }
                    voxel_objects[i][j][k] = new tile(new Vector3(i, k, j), block_id, tags, Math.Min(noise-k, 1f));
                }
            }
        }
        return voxel_objects;
    }

    List<Vector3> draw_cube_vertices(Vector3 position, bool[] directional, float height){
        //bool north, bool south, bool east, bool west, bool up, bool down
        List<Vector3> vertices = new List<Vector3>();

        if (directional[0]){
            vertices.Add(new Vector3(-0.5f, height, -0.5f));  //0
            vertices.Add(new Vector3(0.5f, height, -0.5f));   //1
            vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f)); //2
            vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));  //3
        };
        if (directional[1]){
            vertices.Add(new Vector3(0.5f, height, -0.5f));   //4
            vertices.Add(new Vector3(0.5f, height, 0.5f));    //5
            vertices.Add(new Vector3(0.5f, -0.5f, -0.5f));  //6
            vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));   //7
        };
        if (directional[2]){
            vertices.Add(new Vector3(0.5f, height, 0.5f));    //8
            vertices.Add(new Vector3(-0.5f, height, 0.5f));   //9
            vertices.Add(new Vector3(0.5f, -0.5f, 0.5f));   //10
            vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));  //11
        };
        if (directional[3]){
            vertices.Add(new Vector3(-0.5f, height, 0.5f));   //12
            vertices.Add(new Vector3(-0.5f, height, -0.5f));  //13
            vertices.Add(new Vector3(-0.5f, -0.5f, 0.5f));  //14
            vertices.Add(new Vector3(-0.5f, -0.5f, -0.5f)); //15
        };
        if (directional[4]){
            vertices.Add(new Vector3(-0.5f, height, 0.5f));   //16
            vertices.Add(new Vector3(0.5f, height, 0.5f));    //17
            vertices.Add(new Vector3(-0.5f, height, -0.5f));  //18
            vertices.Add(new Vector3(0.5f, height, -0.5f));   //19
        };
        if (directional[5]){
            vertices.Add(new Vector3(0.5f, height, 0.5f));   //20
            vertices.Add(new Vector3(-0.5f, height, 0.5f));  //21
            vertices.Add(new Vector3(0.5f, height, -0.5f));  //22
            vertices.Add(new Vector3(-0.5f, height, -0.5f)); //23
        }
        return vertices.Select(i => i + position).ToList();
    }

    int[] draw_cube_triangles(int offset, bool[] directional){
        //return new int[]{2, 0, 3, 1, 3, 0, 0, 4, 1, 5, 1, 4, 4, 6, 5, 7, 5, 6, 3, 7, 2, 6, 2, 7, 3, 1, 7, 5, 7, 1, 2, 6, 4, 4, 0, 2}.Select(i => i+offset).ToArray();
        List<int> tris = new List<int>();
        int missed = 0;
        //Debug.Log(directional[2]);
        if (directional[0]){
            tris.AddRange(new List<int>{0, 1, 2, 3, 2, 1});
        }
        else {missed += 4;};

        if (directional[1]){
            tris.AddRange(new List<int>{4, 5, 6, 7, 6, 5}.Select(i => i - missed));
        }
        else {missed += 4;};

        if (directional[2]){
            tris.AddRange(new List<int>{8, 9, 10, 11, 10, 9}.Select(i => i - missed));
        }
        else {missed += 4;};

        if (directional[3]){
            tris.AddRange(new List<int>{12, 13, 14, 15, 14, 13}.Select(i => i - missed));
        }
        else {missed += 4;};

        if (directional[4]){
            tris.AddRange(new List<int>{16, 17, 18, 19, 18, 17}.Select(i => i - missed));
        }
        else {missed += 4;};
        
        if (directional[5]){
            tris.AddRange(new List<int>{20, 21, 22, 23, 22, 21}.Select(i => i - missed));
        };
        return tris.Select(i => i+offset).ToArray();
        //new int[]{0, 1, 2, 3, 2, 1, |4, 5, 6, 7, 6, 5, |8, 9, 10, 11, 10, 9, |12, 13, 14, 15, 14, 13, |16, 17, 18, 19, 18, 17, |20, 21, 22, 23, 22, 21}
    }

    List<Vector2> find_uvs(Vector2[][] uv_list, bool[] directional, float height){
        List<Vector2> uvs = new List<Vector2>();
        for (int i=0;i<6;i++){
            if (directional[i] & directional[4]){
                uvs.AddRange(uv_list[i]);
            }
            else if (directional[i] & !directional[4]){
                uvs.AddRange(uv_list[6]);
            }
        }
        return uvs;
    }

    bool check(tile[][][] tiles, int x, int y, int z, float height){
        //xzy
        if (tiles[x][z][y].tags.Contains("transparent")) return true;
        if (tiles[x][z][y].height_percentile < height)  return true;
        return false;
        /*
        if (x < 1|z < 1|y < 1)       return false;
        if (x >= tiles.Length-1)       return false;
        if (z >= tiles[x].Length-1)    return false;
        if (y >= tiles[x][z].Length-1) return false;
        
        return false;*/
    }

    void generate_chunk(Vector3 position, float seed){
        var watch = System.Diagnostics.Stopwatch.StartNew();
        
        tile[][][] voxel_objects = generate_terrain(x_tiles+2, 50, z_tiles+2, position.x-1, position.z-1, seed);
        GameObject chunk = new GameObject("chunk");
        chunk.transform.position = position;
        chunk.AddComponent<MeshRenderer>().material = GetComponent<MeshRenderer>().material;;
        mesh = chunk.AddComponent<MeshFilter>().mesh;
        //mesh = chunk.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        triangles = mesh.triangles;
        normals = mesh.normals;
        uvs = mesh.uv;
        //Debug.Log(voxel_objects.GetLength(0));
        //triangles.Length / 36 * 8
        float height_percentile;
        for (int i=1;i<voxel_objects.Length-1;i++){
            for (int j=1;j<voxel_objects[i].Length-1;j++){
                for (int k=1;k<voxel_objects[i][j].Length-1;k++){
                    if (!voxel_objects[i][j][k].tags.Contains("transparent")){
                        height_percentile = voxel_objects[i][j][k].height_percentile;
                        triangles = triangles.Concat(draw_cube_triangles(
                            vertices.Length, 
                            new bool[]{
                                check(voxel_objects, i, k, j-1, height_percentile), 
                                check(voxel_objects, i+1, k, j, height_percentile), 
                                check(voxel_objects, i, k, j+1, height_percentile), 
                                check(voxel_objects, i-1, k, j, height_percentile), 
                                check(voxel_objects, i, k+1, j, 0f), 
                                check(voxel_objects, i, k-1, j, 0f)
                            }))
                            .ToArray();

                        vertices = vertices.Concat(draw_cube_vertices(
                            voxel_objects[i][j][k].pos, 
                            new bool[]{
                                check(voxel_objects, i, k, j-1, height_percentile), 
                                check(voxel_objects, i+1, k, j, height_percentile), 
                                check(voxel_objects, i, k, j+1, height_percentile), 
                                check(voxel_objects, i-1, k, j, height_percentile), 
                                check(voxel_objects, i, k+1, j, 0f), 
                                check(voxel_objects, i, k-1, j, 0f)
                            },
                            voxel_objects[i][j][k].height_percentile))
                            .ToArray();
                        
                        uvs = uvs.Concat(find_uvs(
                            texture_coordinates["grass"], 
                            new bool[]{
                                check(voxel_objects, i, k, j+1, height_percentile), 
                                check(voxel_objects, i+1, k, j, height_percentile), 
                                check(voxel_objects, i, k, j-1, height_percentile), 
                                check(voxel_objects, i-1, k, j, height_percentile), 
                                check(voxel_objects, i, k+1, j, 0f), 
                                check(voxel_objects, i, k-1, j, 0f)
                            },
                            voxel_objects[i][j][k].height_percentile))
                            .ToArray();
                    }
                }
            }
        }
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        MeshCollider meshCollider = chunk.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        //Debug.Log(elapsedMs);
    }

    void generate_chunk_wrapper(float seed){

        float num_chunks_x = map_x / 15;
        float num_chunks_z = map_z / 15;
        for (float i=-num_chunks_x/2;i<num_chunks_x/2;i+=1f){
            for (float j=-num_chunks_z/2;j<num_chunks_z/2;j+=1f){
                generate_chunk(new Vector3(i*15, 0, j*15), seed);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float seed = UnityEngine.Random.Range(1.0f, 10000.0f);

        /*
        generate_chunk(new Vector3(0, 0, 0));
        generate_chunk(new Vector3(15, 0, 0));
        generate_chunk(new Vector3(0, 0, 15));
        generate_chunk(new Vector3(15, 0, 15));
        */
        //give each chunk a copy of its own tileset to be able to edit when blocks added/destroyed
        
        generate_chunk_wrapper(seed);
        
        //new List<Vector3>() {new Vector3(0,0,0), new Vector3(0,1,0)};//
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}