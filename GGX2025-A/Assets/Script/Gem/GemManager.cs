using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ジェムのタイプ
/// </summary>
public enum GemType
{
    None,
    MoveSpeed,
    Attak,
    ChargeEnergy,
    ViewRange
}

public class GemManager : MonoBehaviour
{


    // 現在アクティムな宝石のリスト
    [SerializeField] private List<GemBase> _activeGems;

    // ジェムのプレファブ
    [SerializeField] private GemBase[] _gemPrefab;

    // リスト登録待ちの宝石キュー
    private Queue<GemBase> _registwaitingGems;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _activeGems = new List<GemBase>();
        _registwaitingGems = new Queue<GemBase>();
        CreateGem(new Vector2(0,5),GemType.None);
    }


    /// <summary>
    /// ジェムの作成
    /// </summary>
    /// <param name="createPos">制作ポジション</param>
    /// <param name="type">制作されるジェムのタイプ</param>
    /// <returns>制作されるジェム</returns>
    public GemBase CreateGem(Vector3 createPos, GemType type)
    {
        // オブジェクト生成
        var createObj = Instantiate(_gemPrefab[(int)type], createPos, Quaternion.identity);

        // 宝石のスクリプトを取得
        var gem = createObj.GetComponent<GemBase>();
        if (gem == null)
        {
            // 付いていなければ追加
            gem = createObj.AddComponent<GemBase>();
        }

        // 初期化
        gem.Initialize();

        // アクティブ登録待ちのキューにエンキュー
        _registwaitingGems.Enqueue(gem);

        return gem;
    }


    ///<summary>
    ///現在のアクティブな宝石のリストに球を登録する関数
    /// </summary>
    /// <pram name="gem">登録したい宝石</pram>
    public void AddActiveGem()
    {
        if (_registwaitingGems.Count < 1) return;
        _activeGems.Add(_registwaitingGems.Dequeue());
    }

    private void Awake()
    {
        Initialize();
    }
    
    private void Update()
    {
        RemoveNullGem();
        AddActiveGem();
    }

    /// <summary>
    /// Nullのオブジェクトを削除
    /// </summary>
    private void RemoveNullGem()
    {
        for (int i = _activeGems.Count - 1; i >= 0; i--)
        {
            if (_activeGems[i] == null)
                _activeGems.RemoveAt(i);
        }
    }
}
