using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EdgeController : MonoBehaviour
{
    [SerializeField]
    VertexController vertexController = null;
    // 辺データのリスト
    public List<EdgeData> edgeDatas = new List<EdgeData>();


}
