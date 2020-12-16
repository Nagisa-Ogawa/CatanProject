using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// ゲームの状態
public enum GAMESTATE
{
    INTI_INIT,
    INIT_WAIT,
    GAME_PREPARE,
    GAME_PREPARE_WAIT,
    GAME_ACTION_WAIT,
    GAME_ACTION_RUN,
    END_END,
    END_WAIT,
    DEFAULT,
}

public class SceneController : MonoBehaviour
{
    #region シングルトンインスタンス
    // このクラスのインスタンス
    public static SceneController Instance
    {
        get { return instance; }
    }
    private static SceneController instance;
    void Awake()
    {
        // 初回作成時
        if (instance == null)
        {
            instance = this;
        }
        // 2個目以降の作成時
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    [SerializeField]
    NetWorkAPI networkAPI = null;
    [SerializeField]
    VertexController vertexController = null;
    // ホストかどうか
    [SerializeField]
    bool isHost = false;
    // √3
    private const float SQUARE3 = 1.73205080757f;
    // 自分のプレイヤーナンバー
    public int playerNo = 0;
    // 現在のターンのプレイヤーナンバー
    public int nowTurnPlayerNo = 0;
    // 現在のゲームの状態
    GAMESTATE gameState = GAMESTATE.DEFAULT;
    // ステージの長さ
    public int[] stageLength = { 3, 4, 5, 4, 3 };
    // ステージデータの二次元リスト
    public  List<List<MassData>> stageMassDatas = new List<List<MassData>>();
    // ステージオブジェクトの二次元リスト
    public List<List<GameObject>> stageMassObjs = new List<List<GameObject>>();
    // 参加しているプレイヤーのデータ
    public PlayerData[] joinPlayerDatas = new PlayerData[4];
    // 自分のプレイヤーデータ
    public PlayerData myPlayerData = new PlayerData();
    // 資源ごとのマテリアル
    [SerializeField]
    List<Material> resourceMaterials = new List<Material>();
    // サーバーとの通信する間隔
    [SerializeField]
    float connTime = 1.0f;
    // 現在参加しているプレイヤーの人数
    public int playerCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        // 初期化処理
        StartCoroutine(InitGame());
    }

    // プレイヤーの初期化をする関数
    IEnumerator InitGame()
    {
        gameState = GAMESTATE.INTI_INIT;
        // ホストならステージを作成する
        if (isHost)
        {
            // サーバーで部屋を作る
            yield return StartCoroutine(networkAPI.CreateRoom());
            // サーバーでステージを作る
            yield return StartCoroutine(networkAPI.CreateStage());
            // サーバーで頂点データを作る
            yield return StartCoroutine(networkAPI.CreateVertexData());
            // サーバーで辺データを作る
            yield return StartCoroutine(networkAPI.CreateEdgeData());
            // プレイヤー１に設定
            playerNo = 1;
        }
        else
        {
            // サーバーに参加したことを伝える
            yield return StartCoroutine(networkAPI.SendJoinRoom());
        }
        // サーバーからステージデータを受け取る
        yield return StartCoroutine(networkAPI.ReceiveStageData());
        // サーバーから頂点データを受け取る
        yield return StartCoroutine(networkAPI.ReceiveVertexData());
        // サーバーから辺データを受け取る
        yield return StartCoroutine(networkAPI.ReceiveEdgeData());
        // ステージを作成
        CreateStage();
        // 頂点データを作成
        vertexController.CreateVertexObj();
        // 待機状態に入る
        StartCoroutine(WaitInit());
        yield break;
    }

    // プレイヤーが4人揃うまで待機する関数
    IEnumerator WaitInit()
    {
        gameState = GAMESTATE.INIT_WAIT;
        // 規定時間ごとにサーバーと通信をおこなう
        while (true)
        {
            yield return new WaitForSeconds(connTime);
            yield return StartCoroutine(networkAPI.ReceivePlayer());
            if (playerCount == 1)
            {
                yield return StartCoroutine(networkAPI.ReceiveJoinPlayerData());
                // 準備状態にして待機
                // StartCoroutine(PrepareWait());
                yield break;
            }
        }
    }

    IEnumerator PrepareWait()
    {
        gameState = GAMESTATE.GAME_PREPARE_WAIT;
        // 規定時間ごとにサーバーと通信をおこなう
        while (true)
        {
            yield return new WaitForSeconds(connTime);
            // 現在のゲームの状態と行われたアクションを返す
            // yield return StartCoroutine();
            // 何か変化があったなら現在のゲームの状況をもらう
            // 自分のターンになったなら
            if (playerNo == nowTurnPlayerNo)
            {
                // ゲーム準備状態にする
                StartCoroutine(GamePrepare());
                yield break;
            }
        }
    }

    IEnumerator GamePrepare()
    {
        gameState = GAMESTATE.GAME_PREPARE;
        // 家を置けるところを調べる
        vertexController.PrepareCheckVertex();
        // 家を置けるところを表示
        // 家を置くまで待機
        // 家を置いたことを通知
        // 道を置けるところを調べる
        // 道を置けるところを表示
        // 道を置くまで待機
        // 道を置いたことを通知
        // 他のプレイヤーが自分の行ったアクションを終えるまで待機
        // 次のターンへ
        // 待機状態へ
        yield break;
    }

    IEnumerator SetHome()
    {
        yield break;
    }

    // ステージを作成する関数
    void CreateStage()
    {
        // マスPrefab
        GameObject massPrefab = Resources.Load<GameObject>("Mass");
        // ステージを初期化
        for (int y = 0; y < stageLength.Length; y++)
        {
            List<GameObject> massObjects = new List<GameObject>();
            for (int x = 0; x < stageLength[y]; x++)
            {
                // マスを作成
                var massObj = Instantiate(massPrefab);
                massObjects.Add(massObj);
                massObj.transform.SetParent(GameObject.Find("Masses").gameObject.transform);
                massObj.GetComponent<Mass>().massData = stageMassDatas[y][x];
                // 位置を決定
                Vector3 pos = Vector3.zero;
                var offset = stageLength[0] % 2 == 0 ? -1 : 1;
                // ハニカム構造に並べる
                pos.x = ((x - (stageLength[y] / 2)) * 2.0f) + (y % 2 == 0 ? 0 : offset);
                pos.z = ((stageLength.Length / 2) - y) * SQUARE3;
                massObj.transform.position = pos;
                // 資源に合わせてマテリアルを変更
                massObj.GetComponentInChildren<MeshRenderer>().material = resourceMaterials[(int)stageMassDatas[y][x].resource];
                // 砂漠のマスなら番号を非表示にする
                if (stageMassDatas[y][x].isThief)
                {
                    massObj.transform.Find("CircleNumber").gameObject.SetActive(false);
                }
                else
                {
                    // マスに番号をセット
                    massObj.GetComponentInChildren<TextMesh>().text = stageMassDatas[y][x].number.ToString();
                }
            }
            stageMassObjs.Add(massObjects);
        }
    }


}
