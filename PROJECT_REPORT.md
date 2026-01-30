# 🎮 GGX2025-A プロジェクト構成レポート

**作成日**: 2026年1月10日  
**Unity バージョン**: 6000.0.62f1 (LTS)  
**開発言語**: C#

---

## 📋 プロジェクト概要

本プロジェクトは、**タイルベースのアクションゲーム**として見受けられます。
プレイヤーがタイルを掘削・移動し、敵と戦闘するゲームメカニクスが実装されています。

---

## 📂 ディレクトリ構成図

```
Assets/
├── Script/                          # メインスクリプト群
│   ├── Charactor/                   # プレイヤー関連
│   │   ├── PlayerMove.cs            # プレイヤー移動
│   │   ├── PlayerGravityController.cs
│   │   ├── AutoWalkerWithRayFlip.cs # 自動歩行・反転
│   │   └── StatusManager.cs         # ステータス管理
│   ├── Enemy/                       # 敵関連
│   │   ├── EnemyStatus.cs
│   │   ├── SurfaceCrawlerEnemy.cs   # 地表敵
│   │   ├── DigsquareController.cs
│   │   ├── Enemy3/                  # その他敵タイプ
│   │   └── (複数敵タイプ実装)
│   ├── Dig/                         # 掘削・タイル関連
│   │   ├── TilemapToObjects.cs      # タイルマップ処理
│   │   ├── SelectToDestroy.cs       # タイル選択・破壊
│   │   ├── TileReGenerate.cs        # タイル再生
│   │   ├── TRG_UL/UR/LL/LR.cs       # 方向別タイル処理
│   │   ├── WorldRectangleSelector.cs
│   │   ├── ParticleController.cs    # パーティクル
│   │   └── EnergyUI.cs              # エネルギーUI
│   ├── CameraController2D.cs        # 2Dカメラ制御
│   ├── Danmaku1.cs                  # 弾幕（発射体）
│   ├── DeleteBullet.cs              # 発射体削除
│   ├── BGMPlayer.cs                 # BGM再生
│   ├── SceneReset.cs                # シーン管理
│   └── SingletonMonoBehavior.cs     # シングルトン基盤
├── Scenes/                          # シーン
│   ├── GameScene.unity              # メインゲーム
│   └── SampleScene.unity            # サンプル/テスト用
├── Prefab/                          # プリファブ群
│   ├── Enemy/                       # 敵プリファブ
│   ├── Gem/                         # アイテム/宝石
│   ├── EnemyBullet.prefab           # 敵の発射体
│   ├── EnemyBullet_light.prefab
│   ├── digSquare.prefab             # 掘削タイル
│   ├── groundtile.prefab            # 地面タイル
│   ├── selectingSquare.prefab       # 選択タイル表示
│   ├── destroyParticle.prefab       # 破壊パーティクル
│   ├── particle_Mat.mat             # パーティクル材質
│   ├── particleShape.png
│   └── tilemask/                    # タイルマスク
├── Resources/                       # リソース
├── Sound/                           # BGM/SE
├── Sprite/                          # スプライト画像
├── Tile/                            # タイル画像・定義
├── Shadaer/                         # カスタムシェーダー
└── Settings/                        # プロジェクト設定
```

---

## 🎮 ゲームメカニクス分析

### コアシステム

| システム | 関連スクリプト | 説明 |
|---------|--------------|------|
| **プレイヤー移動** | PlayerMove.cs, AutoWalkerWithRayFlip.cs | 2D横スクロール移動、重力システム |
| **タイル掘削** | SelectToDestroy.cs, TileReGenerate.cs | タイルの選択と破壊、再生成 |
| **敵システム** | EnemyStatus.cs, SurfaceCrawlerEnemy.cs | 複数敵タイプ、ステータス管理 |
| **発射体** | Danmaku1.cs, DeleteBullet.cs | 敵/プレイヤーの弾幕発射 |
| **UI/エネルギー** | EnergyUI.cs, EnergyBarFront.cs | ゲームバー表示、エネルギー管理 |
| **カメラ** | CameraController2D.cs | プレイヤー追従カメラ |
| **オーディオ** | BGMPlayer.cs | BGM管理 |

---

## 📊 スクリプト統計

- **総スクリプト数**: 約 25+ ファイル
- **構成**:
  - プレイヤーシステム: 4 スクリプト
  - 敵システム: 4 スクリプト
  - 掘削・タイル: 8 スクリプト
  - その他（カメラ、音、UI）: 9+ スクリプト

---

## 🎯 注目すべき特徴

1. **モジュール化設計**: 敵・プレイヤー・タイルが独立したディレクトリに分離
2. **シングルトンパターン**: `SingletonMonoBehavior.cs` で共通管理
3. **方向ベース処理**: TRG_UL/UR/LL/LR で4方向処理を分割
4. **タイルマップシステム**: `TilemapToObjects.cs` でタイルマップをオブジェクト化
5. **複数敵タイプ**: Enemy3フォルダで複数敵実装

---

## 🔧 開発時の推奨ポイント

### 参加時チェックリスト
- [ ] GameScene.unity で現在のゲーム状態を確認
- [ ] PlayerMove.cs でプレイヤー操作仕様を把握
- [ ] Dig フォルダ内のタイルシステムの流れを理解
- [ ] Enemy フォルダで敵メカニクスを確認
- [ ] StatusManager.cs と EnemyStatus.cs でステータス管理方法を確認

### 重要なシングルトン/マネージャー
```
SingletonMonoBehavior.cs  # 基底クラス
StatusManager.cs          # プレイヤーステータス
EnemyStatus.cs            # 敵ステータス
```

---

## 📝 次のステップ提案

1. **ゲームプレイ体験**: SampleScene/GameScene で実際にビルド・実行
2. **コード読破**: メインループを理解（PlayerMove → Enemy → Dig の順）
3. **チームとの確認**: Git 運用ルール、アサイン内容の確認
4. **ドキュメント確認**: README.md の全文とプロジェクト設定を確認

---

**注**: 実装が進行中の段階のため、すべての機能が完成していない可能性があります。  
チームリーダーに最新の開発状況を確認することをお勧めします。
