using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public enum ACTION_TYPE
{
    NO_ACTION,
    CREATE_HOME,
    CREATE_TOWN,
    CREATE_ROAD,
} 

public class NetWorkAPI : MonoBehaviour
{

    // 部屋のID
    [SerializeField]
    int roomID = 0;
    [SerializeField]
    VertexController vertexController = null;
    [SerializeField]
    EdgeController edgeController = null;

    // サーバーに部屋を作成するよう通知する関数
    public IEnumerator CreateRoom()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/CreateRoom.php?roomName="+"部屋1");
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のランキングを取得
        string roomIDJson = request.downloadHandler.text;
        // クラスに変換
        var roomInfo = JsonUtility.FromJson<RoomInfo>(roomIDJson);
        Debug.Log("ルームを作成しました。");
        Debug.Log("部屋番号は" + roomInfo.roomID+"  部屋名は"+roomInfo.roomName);
        roomID = roomInfo.roomID;
    }

    // サーバーにステージデータを作成するよう通知する関数
    public IEnumerator CreateStage()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/CreateStage.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のデータを取得
        string roomIDJson = request.downloadHandler.text;
        // クラスに変換
        var result = JsonUtility.FromJson<Result>(roomIDJson);
        if (result.result)
        {
            Debug.Log("ステージを作成しました。");
        }
        else
        {
            Debug.Log("ステージを作成できませんでした。");
        }
    }

    // サーバーに頂点データを作成するように通知する関数
    public IEnumerator CreateVertexData()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/CreateVertexData.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のデータを取得
        string resultJson = request.downloadHandler.text;
        // クラスに変換
        var result = JsonUtility.FromJson<Result>(resultJson);
        if (result.result)
        {
            Debug.Log("頂点データを作成しました。");
        }
        else
        {
            Debug.Log("頂点データを作成できませんでした。");
        }
    }

    // サーバーに辺データを作成するように通知する関数
    public IEnumerator CreateEdgeData()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/CreateEdgeData.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のデータを取得
        string resultJson = request.downloadHandler.text;
        // クラスに変換
        var result = JsonUtility.FromJson<Result>(resultJson);
        if (result.result)
        {
            Debug.Log("辺データを作成しました。");
        }
        else
        {
            Debug.Log("辺データを作成できませんでした。");
        }

    }

    // サーバーで作成したステージデータを受け取る関数
    public IEnumerator ReceiveStageData()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/ReceiveStageData.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のランキングを取得
        string roomIDJson = request.downloadHandler.text;
        // クラスに変換
        var receiveMassDatas = JsonUtility.FromJson<ReceiveMassData>(roomIDJson);

        // ReceiveMassDataからMassDataへ変換
        var stageLength = SceneController.Instance.stageLength;
        List<List<MassData>> stageMassDatas = new List<List<MassData>>();
        int counter = 0;
        for (int y = 0; y < stageLength.Length; y++)
        {
            List<MassData> massDatas = new List<MassData>();
            for (int x = 0; x < stageLength[y]; x++)
            {
                var massData = new MassData();
                massData.number = receiveMassDatas.numbers[counter];
                massData.resource = (RESOURCE)receiveMassDatas.resources[counter];
                massData.isThief = receiveMassDatas.isThiefs[counter] == 0 ? false : true;
                massDatas.Add(massData);
                counter++;
            }
            stageMassDatas.Add(massDatas);
        }
        SceneController.Instance.stageMassDatas = stageMassDatas;
        Debug.Log("ステージデータを受け取りました");

    }

    // サーバーで作成した頂点データを受け取る関数
    public IEnumerator ReceiveVertexData()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/SendVertexData.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のランキングを取得
        string resultJson = request.downloadHandler.text;
        // クラスに変換
        var nullableReceiveVertexDatas = JsonUtility.FromJson<NullableReceiveVertexData>(resultJson);

        // ReceiveMassDataからMassDataへ変換
        var vertexLength = vertexController.vertexLength;
        List<List<VertexData>> vertexDatas = new List<List<VertexData>>();
        int counter = 0;
        for (int y = 0; y < vertexLength.Length; y++)
        {
            List<VertexData> vertices = new List<VertexData>();
            for (int x = 0; x < 6; x++)
            {
                if (nullableReceiveVertexDatas.receiveVertexData[counter].x == x &&
                    nullableReceiveVertexDatas.receiveVertexData[counter].y == y)
                {
                    var vertexData = new VertexData();
                    vertexData.x = nullableReceiveVertexDatas.receiveVertexData[counter].x;
                    vertexData.y = nullableReceiveVertexDatas.receiveVertexData[counter].y;
                    vertexData.owner = (BUILDING_OWNER)nullableReceiveVertexDatas.receiveVertexData[counter].vertexOwner;
                    vertexData.vertexBuildingType = (VERTEX_BUILDING_TYPE)nullableReceiveVertexDatas.receiveVertexData[counter].buildingType;
                    vertices.Add(vertexData);
                }
                else
                {
                    vertices.Add(null);
                }
                counter++;
            }
            vertexDatas.Add(vertices);
        }
        vertexController.vertexDatas = vertexDatas;
        Debug.Log("頂点データを受け取りました");
    }

    // サーバーで作成した辺データを受け取る関数
    public IEnumerator ReceiveEdgeData()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/SendEdgeData.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のランキングを取得
        string resultJson = request.downloadHandler.text;
        // クラスに変換
        var resultEdgeDatas = JsonUtility.FromJson<ResultEdgeData>(resultJson);
        // EdgeData型に変換
        foreach(var resultEdgeData in resultEdgeDatas.edgeDatas)
        {
            EdgeData edgeData = new EdgeData();
            edgeData.elementNum = resultEdgeData.elementNum;
            edgeData.startVertex.x = resultEdgeData.startVertex.x;
            edgeData.startVertex.y = resultEdgeData.startVertex.y;
            edgeData.startVertex.vertexBuildingType = (VERTEX_BUILDING_TYPE)resultEdgeData.startVertex.vertexOwner;
            edgeData.startVertex.owner = (BUILDING_OWNER)resultEdgeData.startVertex.vertexOwner;
            edgeData.endVertex.x = resultEdgeData.endVertex.x;
            edgeData.endVertex.y = resultEdgeData.endVertex.y;
            edgeData.endVertex.vertexBuildingType = (VERTEX_BUILDING_TYPE)resultEdgeData.endVertex.vertexOwner;
            edgeData.endVertex.owner = (BUILDING_OWNER)resultEdgeData.endVertex.vertexOwner;
            edgeData.edgeBuildingType = (EDGE_BUILDING_TYPE)resultEdgeData.edgeBuildingType;
            edgeData.owner = (BUILDING_OWNER)resultEdgeData.owner;
            edgeController.edgeDatas.Add(edgeData);
        }
    }

    // サーバーから参加しているプレイヤーのデータを受け取るクラス
    public IEnumerator ReceiveJoinPlayerData()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/SendJoinPlayerData.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のランキングを取得
        string resultJson = request.downloadHandler.text;
        // クラスに変換
        var resultPlayerData = JsonUtility.FromJson<ResultPlayerData>(resultJson);
        Debug.Log("プレイヤー全員のデータを受け取りました。");

    }

    // サーバーに参加したことを通知する関数
    public IEnumerator SendJoinRoom()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/ReceiveJoinRoom.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のランキングを取得
        string resultJson = request.downloadHandler.text;
        // クラスに変換
        var result = JsonUtility.FromJson<ResultJoinRoom>(resultJson);
        if (result.result)
        {
            Debug.Log("部屋ID" + roomID + "の部屋にプレイヤー" + result.playerNo + "として参加しました。");
        }
        else
        {
            Debug.Log("部屋ID" + roomID + "の部屋が見つからないか、満員でした。");
        }
        SceneController.Instance.playerNo = result.playerNo;
    }

    // サーバーから現在の部屋の参加人数を受け取る関数
    public IEnumerator ReceivePlayer()
    {
        var request = UnityWebRequest.Get("http://18.183.75.169/SendPlayer.php?roomID=" + roomID.ToString());
        // タイムアウトするまでの時間
        request.timeout = 3;
        // リクエストする回数
        int requestCount = 5;
        // 現在のリクエスト回数
        int nowRequestCount = 0;
        // エラーの有無
        var isError = true;
        while (isError && requestCount > nowRequestCount)
        {
            isError = false;
            nowRequestCount++;
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            if (requestCount <= nowRequestCount)
            {
                Debug.Log("5回リクエストを送りましたが、レスポンスが返ってきませんでした。");
            }
        }
        // JSON形式のランキングを取得
        string resultJson = request.downloadHandler.text;
        // クラスに変換
        var result = JsonUtility.FromJson<ResultPlayerCount>(resultJson);
        Debug.Log("現在の参加人数は" + result.playerCount + "人です。");
        SceneController.Instance.playerCount = result.playerCount;
    }

}



// 部屋の情報を表すクラス
[Serializable]
public class RoomInfo
{
    public int roomID;
    public string roomName;
}

// ステージデータを受け取るクラス
[Serializable]
public class ReceiveMassData
{
    public List<int> numbers;
    public List<int> resources;
    public List<int> isThiefs;
}

[Serializable]
public class NullableReceiveVertexData
{
    public List<ReceiveVertexData> receiveVertexData;
}

// 頂点データを受け取るクラス
[Serializable]
public class ReceiveVertexData
{
    public int x;
    public int y;
    public int vertexOwner;
    public int buildingType;
}

// 辺データを受け取るクラス
[Serializable]
public class ResultEdgeData
{
    public List<ReceiveEdgeData> edgeDatas;
}

[Serializable]
public class ReceiveEdgeData
{
    // 配列の中での順番
    public int elementNum;
    // この辺と接している2つの頂点
    public ReceiveVertexData startVertex;
    public ReceiveVertexData endVertex;
    // この辺にある建造物
    public int edgeBuildingType;
    // この辺の所有者
    public int owner;
}

// サーバーでの処理の結果を受け取るクラス
[Serializable]
public class Result
{
    public bool result;
}

// サーバーからプレイヤーデータを受け取るクラス
[Serializable]
public class ReceivePlayerData
{
    public int playerNo;
    public List<int> haveResources;
    public List<int> haveCards;
    public int haveHome;
    public int haveTown;
    public int longestRoad;
}

[Serializable]
public class ResultPlayerData
{
    public List<ReceivePlayerData> playerDatas;
}

// サーバでの参加処理の結果を受け取るクラス
[Serializable]
public class ResultJoinRoom
{
    public bool result;
    public int playerNo;
}

// サーバーでの参加人数を受け取るクラス
[Serializable]
public class ResultPlayerCount
{
    public int playerCount;
}

// サーバーからのアクションを受け取るクラス
[Serializable]
public class ResultAction
{
    public int resultAction;
}


