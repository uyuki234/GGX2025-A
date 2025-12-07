using System.Collections.Generic;
using UnityEngine;

public class Enemy3Manager : MonoBehaviour
{


    // 現在アクティムな敵のリスト
    [SerializeField] private List<Enemy3> _activeEnemy;

    // 敵のプレファブ
    [SerializeField] private Enemy3 _enemyPrefab;

    // リスト登録待ちの敵のキュー
    private Queue<Enemy3> _registwaitingEnemy;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _activeEnemy = new List<Enemy3>();
        _registwaitingEnemy = new Queue<Enemy3>();

    }


    /// <summary>
    /// 敵の作成
    /// </summary>
    /// <param name="createPos">制作ポジション</param>
    /// <param name="type">制作されるジェムのタイプ</param>
    /// <returns>制作される敵</returns>
    public Enemy3 CreateEnemy(Vector3 createPos)
    {
        // オブジェクト生成
        var createObj = Instantiate(_enemyPrefab, createPos, Quaternion.identity);

        // 敵のスクリプトを取得
        var enemy = createObj.GetComponent<Enemy3>();

        // 初期化
       // enemy.Initialize();

        // アクティブ登録待ちのキューにエンキュー
        _registwaitingEnemy.Enqueue(enemy);

        return enemy;
    }


    ///<summary>
    ///現在のアクティブな敵のリストにキューを登録する関数
    /// </summary>
    /// <pram name="enemy">登録したい敵</pram>
    public void AddActiveEnemy()
    {
        if (_registwaitingEnemy.Count < 1) return;
        _activeEnemy.Add(_registwaitingEnemy.Dequeue());
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        RemoveNullEnemy();
        AddActiveEnemy();
    }

    /// <summary>
    /// Nullのオブジェクトを削除
    /// </summary>
    private void RemoveNullEnemy()
    {
        for (int i = _activeEnemy.Count - 1; i >= 0; i--)
        {
            if (_activeEnemy[i] == null)
                _activeEnemy.RemoveAt(i);
        }
    }
}