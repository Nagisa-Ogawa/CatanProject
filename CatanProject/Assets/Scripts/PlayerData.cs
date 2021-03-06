﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カードの種類
public enum ITEM_CARD
{
    KNIGHT,
    VITORY_POINT,
    CREATE_HIGHWAY,
    HARVEST,
    EXCLUSIVE,
}

public class PlayerData
{
    // プレイヤーナンバー
    int playerNo;
    // プレイヤーが所持している資源
    Dictionary<RESOURCE, int> haveResources;
    // プレイヤーが所持しているカード
    Dictionary<ITEM_CARD, int> haveItemCard;
    // プレイヤーが所持している家の数
    int haveHome;
    // プレイヤーが所持している都市の数
    int haveCity;
    // 最長の道
    int longestRoad;
};
