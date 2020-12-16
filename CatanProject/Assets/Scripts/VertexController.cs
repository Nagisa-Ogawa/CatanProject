using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VertexController : MonoBehaviour
{

    // √3
    private const float SQUARE3 = 1.73205080757f;
    public int[] vertexLength = { 3, 4, 4, 5, 5, 6, 6, 5, 5, 4, 4, 3 };
    // 頂点データの二次元リスト
    public List<List<VertexData>> vertexDatas = new List<List<VertexData>>();
    // 頂点オブジェクトの二次元リスト
    public List<List<GameObject>> vertexObjs = new List<List<GameObject>>();

    // 深さ
    int checkVertexCount = 0;
    // その頂点に建造物を置けるかどうかのフラグ
    bool isCanBuild = true;


    // 頂点オブジェクトを作成する関数
    public void CreateVertexObj()
    {
        // 頂点オブジェクトのPrefab
        GameObject vertexObjPrefab = Resources.Load<GameObject>("VertexObject");
        for (int y = 0; y < vertexLength.Length; y++)
        {
            int counter = 0;
            List<GameObject> vertices = new List<GameObject>();
            for (int x = 0; x < 6; x++)
            {
                if (vertexDatas[y][x] == null)
                {
                    vertices.Add(null);
                    continue;
                }
                // オブジェクトを作成
                var vertexObj = Instantiate(vertexObjPrefab);
                vertexObj.transform.SetParent(GameObject.Find("Vertices").gameObject.transform);
                vertexObj.GetComponent<Vertex>().vertexData = vertexDatas[y][x];
                // 位置をセット
                Vector3 pos = Vector3.zero;
                pos.x = ((counter - (vertexLength[y] / 2)) * 2.0f) + (vertexLength[y] % 2 == 0 ? 1 : 0);
                pos.y = 0.3f;
                pos.z = (vertexLength.Length - y) * (1 / SQUARE3) + ((vertexLength.Length - (y + 1)) / 2) * (1 / SQUARE3);
                pos.z -= (vertexLength.Length / 2) * (1 / SQUARE3) + ((vertexLength.Length / 2) / 2) * (1 / SQUARE3);
                vertexObj.transform.position = pos;
                // リストにAdd
                vertices.Add(vertexObj);
                counter++;
            }
            vertexObjs.Add(vertices);
        }
    }

    // 建造物を置ける頂点オブジェクトを調べる関数
    public void PrepareCheckVertex()
    {
        // すべての頂点を調べる
        for (int y = 0; y < vertexDatas.Count; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                // nullなら調べない
                if (vertexDatas[y][x] == null)
                {
                    continue;
                }
                // 調べる頂点データにすでに建造物があるなら調べない
                if (vertexDatas[y][x].vertexBuildingType != VERTEX_BUILDING_TYPE.NO_BUILDING &&
                    vertexDatas[y][x].vertexBuildingType != VERTEX_BUILDING_TYPE.CAN_BUILDING)
                {
                    continue;
                }
                // 家を置けるかどうか調べる
                CheckCanBuild(new Vector2Int(x, y), new Vector2Int(int.MaxValue, int.MaxValue));
                // 家が置けるならその頂点オブジェクトの色を変える
                if (isCanBuild)
                {
                    vertexDatas[y][x].vertexBuildingType = VERTEX_BUILDING_TYPE.CAN_BUILDING;
                    vertexObjs[y][x].GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    vertexDatas[y][x].vertexBuildingType = VERTEX_BUILDING_TYPE.NO_BUILDING;
                }
                isCanBuild = true;
                checkVertexCount = 0;
            }
        }
    }

    void CheckCanBuild(Vector2Int vertexPos, Vector2Int beforeVertexPos)
    {
        // 深さ更新
        checkVertexCount++;
        bool[] isCheck = { false, false, false };
        // yによって進む方向を決める
        List<Vector2Int> direction = new List<Vector2Int>();
        if (vertexPos.y % 2 == 0)
        {
            direction.Add(new Vector2Int(0, -1));
            direction.Add(new Vector2Int(1, 1));
            direction.Add(new Vector2Int(0, 1));
        }
        else
        {
            direction.Add(new Vector2Int(0, -1));
            direction.Add(new Vector2Int(0, 1));
            direction.Add(new Vector2Int(-1, -1));
        }
        // そこに家があるかチェック
        if (vertexDatas[vertexPos.y][vertexPos.x].vertexBuildingType != VERTEX_BUILDING_TYPE.NO_BUILDING &&
                vertexDatas[vertexPos.y][vertexPos.x].vertexBuildingType != VERTEX_BUILDING_TYPE.CAN_BUILDING)
        {
            isCanBuild = false;
            return;
        }
        // 深さ3まで行ったなら戻る
        if (checkVertexCount == 2)
        {
            checkVertexCount--;
            return;
        }
        // すべての方向をチェックするまでループ
        while (true)
        {
            // 既にどこかで家が見つかっていたならreturn
            if (!isCanBuild)
            {
                return;
            }
            // すべての方向をチェックしたならreturn
            if (isCheck.Contains(false) == false)
            {
                checkVertexCount--;
                return;
            }
            // 乱数でチェックする方向を決める
            var moveDirection = Random.Range(0, direction.Count);
            // 既にその方向をチェックしているなら他の方向へ
            if (isCheck[moveDirection])
            {
                continue;
            }
            // 範囲外チェック
            var nextPos = vertexPos + direction[moveDirection];
            if (nextPos.y < 0 || nextPos.y >= vertexLength.Length ||
                nextPos.x < 0 || nextPos.x >= vertexLength[nextPos.y] || vertexDatas[nextPos.y][nextPos.x] == null)
            {
                isCheck[moveDirection] = true;
                continue;
            }
            // 前回のポジションの方向なら他の方向へ
            if (nextPos == beforeVertexPos)
            {
                isCheck[moveDirection] = true;
                continue;
            }
            // 進む方向をチェック済みにする
            isCheck[moveDirection] = true;
            // その方向へ進む
            CheckCanBuild(nextPos, vertexPos);
        }
    }

}
