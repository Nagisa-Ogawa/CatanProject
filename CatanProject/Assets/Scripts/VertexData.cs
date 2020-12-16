using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// この頂点の所有者
public enum BUILDING_OWNER
{
    NO_OWNER,
    PLAYER1,
    PLAYER2,
    PLAYER3,
    PLAYER4,
}

public enum VERTEX_BUILDING_TYPE
{
    NO_BUILDING,
    CAN_BUILDING,
    HOME,
    TOWN,
}

// 頂点データを表すクラス
public class VertexData
{
    // 頂点の配列の中でのx成分
    public int x;
    // 頂点の配列の中でのy成分
    public int y;
    // この頂点の所有者
    public BUILDING_OWNER owner = BUILDING_OWNER.NO_OWNER;
    // この頂点の建造物
    public VERTEX_BUILDING_TYPE vertexBuildingType = VERTEX_BUILDING_TYPE.NO_BUILDING;
}
