using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 資源の種類を表す
public enum RESOURCE
{
    WOOD,
    LIVESTOCK,
    CROPS,
    BRICK,
    MINERAL,
    DESERT,
}

// 1つのマスのデータを表すクラス
public class MassData
{
    // そのマスの数字
    public int number;
    // そのマスにある資源
    public RESOURCE resource;
    // そのマスに泥棒がいるかどうか
    public bool isThief;
};
