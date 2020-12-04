using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームの状態
public enum GAMESTATE
{
    INTI_INIT,
    INIT_WAIT,
    GAME_PREPARE,
    GAME_PREPARE_WAIT,
    GAME_ROLLDICE,
    GAME_ROLLDICE_WAIT,
    GAME_ACTION_CHOICE,
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
    // 自分のID
    int id = 0;
    // ホストかどうか
    bool isHost = false;
    // 現在のゲームの状態
    GAMESTATE gameState = GAMESTATE.DEFAULT;
    // ステージデータの二次元リスト
    List<List<MassData>> stages = new List<List<MassData>>();
    // ステージオブジェクトの二次元リスト
    List<List<GameObject>> stageObjs = new List<List<GameObject>>();
    // ステージの大きさ
    [SerializeField]
    int[] stageLength = { 3, 4, 5, 4, 3 };
    // 資源ごとの最大数
    Dictionary<RESOURCE, int> resourcesMax = new Dictionary<RESOURCE, int>()
    {
        {RESOURCE.WOOD,      4},
        {RESOURCE.LIVESTOCK, 4},
        {RESOURCE.CROPS,     4},
        {RESOURCE.BRICK,     3},
        {RESOURCE.MINERAL,   3},
        {RESOURCE.DESERT,    1},
    };
    // 現在の使用した資源数
    Dictionary<RESOURCE,int> useResourecs=new Dictionary<RESOURCE, int>()
    {         
        {RESOURCE.WOOD,      0},
        {RESOURCE.LIVESTOCK, 0},
        {RESOURCE.CROPS,     0},
        {RESOURCE.BRICK,     0},
        {RESOURCE.MINERAL,   0},
        {RESOURCE.DESERT,    0},
    };
    // 資源ごとのマテリアル
    [SerializeField]
    List<Material> resourceMaterials = new List<Material>();
    // マスに置く番号の配列
    int[] massNumber = { 5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11 };
    // マスに置く番号を決めるときの進む方向
    List<Vector2Int> moveDirectionWhenDecideNum = new List<Vector2Int>()
    {
        new Vector2Int( 0, 1),  // 左下、右下
        new Vector2Int( 1, 0),  // 右
        new Vector2Int( 1,-1),  // 右上
        new Vector2Int(-1,-1),  // 左上
        new Vector2Int(-1, 0),  // 左
    };

    // Start is called before the first frame update
    void Start()
    {
        isHost = true;
        // 初期化処理
        StartCoroutine(InitGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InitGame()
    {
        gameState = GAMESTATE.INTI_INIT;
        // サーバーと通信を行いホストかどうか確認
        // ホストならステージを作成する
        if (isHost)
        {
            // ステージを作成
            CreateStage();
            // 作成したステージをサーバーへ送る
        }
        // クライアントならサーバーから受け取ったステージを作成
        else
        {
            // ステージを作成
            // 作成したことをサーバーへ通知
        }
        // 待機状態に入る
        yield break;
    }

    // ステージを作成する関数
    void CreateStage()
    {
        // マスPrefab
        GameObject massPrefab = Resources.Load<GameObject>("Mass");
        // ステージを初期化
        for(int y = 0; y < stageLength.Length; y++)
        {
            List<GameObject> massObjects = new List<GameObject>();
            List<MassData> massDatas = new List<MassData>();
            for(int x = 0; x < stageLength[y]; x++)
            {
                // マスを作成
                var massObj = Instantiate(massPrefab);
                massObjects.Add(massObj);
                // 位置を決定
                Vector3 pos = Vector3.zero;
                var offset = stageLength[0] % 2 == 0 ? -1 : 1;
                // ハニカム構造に並べる
                pos.x = ((x - (stageLength[y] / 2)) * 2.0f)+(y%2==0?0:offset);
                pos.z = ((stageLength.Length / 2) - y) * 1.75f;
                massObj.transform.position = pos;
                // このマスのデータを決める
                MassData mass = new MassData();
                // マスの資源を決める
                while (true)
                {
                    // 乱数で資源を決める
                    RESOURCE resource = (RESOURCE)Random.Range(0, System.Enum.GetValues(typeof(RESOURCE)).Length);
                    // これ以上その資源を置けるかチェック
                    if ((useResourecs[resource] + 1) > resourcesMax[resource])
                    {
                        continue;
                    }
                    useResourecs[resource]++;
                    mass.resource = resource;
                    break;
                }
                // 資源に合わせてマテリアルを変更
                massObj.GetComponentInChildren<MeshRenderer>().material = resourceMaterials[(int)mass.resource];
                massDatas.Add(mass);
            }
            stages.Add(massDatas);
            stageObjs.Add(massObjects);
        }
        // マスに番号をセット
        SetMassNumber();
        // ゲームオブジェクトに反映
        for (int y = 0; y < stageLength.Length; y++)
        {
            for (int x = 0; x < stageLength[y]; x++)
            {
                stageObjs[y][x].GetComponentInChildren<TextMesh>().text = stages[y][x].number.ToString();
            }
        }
    }

    // マスに番号をセットする関数
    void SetMassNumber()
    {
        // 開始地点を決める
        Vector2Int nowPos = Vector2Int.zero;
        // 進んだ数
        int moveCout = 0;
        // 現在の進行方向
        Vector2Int direction = moveDirectionWhenDecideNum[0];
        // 現在の進行方向の番号
        int directionNum = 0;
        while (true)
        {
            // 現在のマスが砂漠かチェック
            if (stages[nowPos.y][nowPos.x].resource != RESOURCE.DESERT)
            {
                // 現在の位置の番号をセット
                stages[nowPos.y][nowPos.x].number = massNumber[moveCout];
            }
            if (moveCout >= massNumber.Length - 1)
            {
                break;
            }
            var nextPos = nowPos + direction;
            // 進行方向に進めるかチェック
            if (nextPos.y < 0 || nextPos.y > stages.Count-1 || nextPos.x < 0 || nextPos.x > stages[nextPos.y].Count-1)
            {
                // 進行方向を次の進行方向にして進みなおし
                directionNum++;
                if (directionNum > moveDirectionWhenDecideNum.Count-1)
                {
                    directionNum = 0;
                }
                direction = moveDirectionWhenDecideNum[directionNum];
                continue;
            }
            // 進行方向にすでに番号があるかチェック
            if (stages[nextPos.y][nextPos.x].number != 0)
            {
                // 進行方向を次の進行方向にして進みなおし
                directionNum++;
                if (directionNum > moveDirectionWhenDecideNum.Count - 1)
                {
                    directionNum = 0;
                }
                direction = moveDirectionWhenDecideNum[directionNum];
                continue;
            }
            // 現在のマスが砂漠かチェック
            if (stages[nowPos.y][nowPos.x].resource != RESOURCE.DESERT)
            {
                moveCout++;
            }
            else
            {
                // 現在の位置の番号をセット
                stages[nowPos.y][nowPos.x].number = 100;
            }
            nowPos += direction;
        }
    }
}
