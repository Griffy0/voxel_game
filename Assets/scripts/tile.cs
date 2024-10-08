using UnityEngine;
using System.Collections.Generic;


public class tile
{
    public Vector3 pos;
    public int id;
    public List<string> tags;
    
    public tile(Vector3 position, int block_id, List<string> tag_list){
        pos = position;
        id = block_id;
        tags = tag_list;
    }

}
