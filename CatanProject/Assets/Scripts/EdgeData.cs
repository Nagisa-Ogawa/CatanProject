using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDGE_BUILDING_TYPE
{
    NO_BUILDING,
    CAN_BUILDING,
    ROAD,
}

public class EdgeData
{
    // 配列の中での順番
    public int elementNum;
    // この辺と接している2つの頂点
    public VertexData startVertex;
    public VertexData endVertex;
    // この辺にある建造物
    public EDGE_BUILDING_TYPE edgeBuildingType;
    // この辺の所有者
    public BUILDING_OWNER owner;

    public EdgeData()
    {
        startVertex = new VertexData();
        endVertex = new VertexData();
    }
}
