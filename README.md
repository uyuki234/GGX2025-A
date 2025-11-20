# GGX2025-A

Unity を使ったゲームプロジェクト

## プロジェクト概要
このプロジェクトは Unity で作成されたゲームです。

## Unity バージョン
Unity 2022.3.10f1

## プロジェクト構成
- **Assets/Scenes**: ゲームシーン
  - SampleScene.unity: メインのサンプルシーン
- **Assets/Scripts**: ゲームスクリプト
  - GameManager.cs: ゲーム全体を管理するスクリプト
  - PlayerController.cs: プレイヤーの操作を管理するスクリプト
- **Assets/Prefabs**: ゲームオブジェクトのプレハブ
- **Assets/Materials**: マテリアル
- **Assets/Textures**: テクスチャ

## 使い方
1. Unity Hub で Unity 2022.3.10f1 以降をインストール
2. Unity Hub でこのプロジェクトを開く
3. Assets/Scenes/SampleScene.unity を開く
4. 再生ボタンを押してゲームを実行

## スクリプトの説明
### GameManager.cs
ゲーム全体の管理を行うスクリプトです。ゲーム開始時のログ出力などを処理します。

### PlayerController.cs
プレイヤーキャラクターの移動とジャンプを制御します。
- **移動**: WASD キーまたは矢印キー
- **ジャンプ**: スペースキー
- **移動速度**: 5.0 (調整可能)
- **ジャンプ力**: 7.0 (調整可能)