using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : SingletonMonoBehavior<EnemyBulletManager>
{


    // 現在アクティムな弾のリスト
    [SerializeField] private List<EnemyBullet> _activeEnemyBullet;

    // ジェムのプレファブ
    [SerializeField] private EnemyBullet _enemyBulletPrefab;

    // リスト登録待ちの弾のキュー
    private Queue<EnemyBullet> _registwaitingEnemyBullet;

    private Camera cam;
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {
        _activeEnemyBullet = new List<EnemyBullet>();
        _registwaitingEnemyBullet = new Queue<EnemyBullet>();

    }


    /// <summary>
    /// ジェムの作成
    /// </summary>
    /// <param name="createPos">制作ポジション</param>
    /// <param name="type">制作されるジェムのタイプ</param>
    /// <returns>制作されるジェム</returns>
    public EnemyBullet CreateEnemyBullet(Vector3 createPos,Vector2 moveDir)
    {
        // オブジェクト生成
        var createObj = Instantiate(_enemyBulletPrefab, createPos, Quaternion.identity);

        // 弾のスクリプトを取得
        var enemyBullet = createObj.GetComponent<EnemyBullet>();

        // 初期化
        //enemyBullet.Initialize(moveDir);

        // アクティブ登録待ちのキューにエンキュー
        _registwaitingEnemyBullet.Enqueue(enemyBullet);

        return enemyBullet;
    }


    ///<summary>
    ///現在のアクティブな弾のリストにキューを登録する関数
    /// </summary>
    /// <pram name="enemyBullet">登録したい弾</pram>
    public void AddActiveEnemyBullet()
    {
        if (_registwaitingEnemyBullet.Count < 1) return;
        _activeEnemyBullet.Add(_registwaitingEnemyBullet.Dequeue());
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        RemoveNullEnemyBullet();
        AddActiveEnemyBullet();
    }

    /// <summary>
    /// Nullのオブジェクトを削除
    /// </summary>
    private void RemoveNullEnemyBullet()
    {
        for (int i = _activeEnemyBullet.Count - 1; i >= 0; i--)
        {
            if (_activeEnemyBullet[i] == null)
                _activeEnemyBullet.RemoveAt(i);
        }
    }
}