# SchwesteriumLibrary
Unitプロジェクト間で再利用できる機能をまとめたライブラリです

導入(UPM) : https://github.com/schwesterium/SchwesteriumLibrary.git?path=/Assets/SchwesteriumLibrary

詳細はWikiを参照 => https://schwesterium.github.io/SchwesteriumLibraryDocs/home.html
> [!NOTE]
> 現在はスクリプトリファレンスのみ

ありがとうclaude、説明を書いてくれて
## SchwesteriumLibrary でできること

Unity プロジェクト間で使い回せる機能をまとめたライブラリ（UPM パッケージ `com.schwesterium.schwesteriumlibrary`）。
ソースを読んで機能を整理したもの。

- 対象: Unity 6000.3 以降
- 依存: `com.unity.inputsystem` 1.19.0 / `com.unity.ugui` 2.0.0（TextMeshPro 含む）
- 任意依存: UniTask 2.5.11 以上（NFC 用）/ Timeline 1.8.12 以上（Timeline 拡張用）

## 導入

```
https://github.com/schwesterium/SchwesteriumLibrary.git?path=/Assets/SchwesteriumLibrary
```

Package Manager の「Add package from git URL」に貼る。サンプル（AudioManager）は Package Manager の Samples タブからインポートする。

## 機能一覧

| 分野 | 名前空間 | 主なクラス | 一言でいうと |
|---|---|---|---|
| オーディオ | `Audio` | `AudioManager` / `AudioClipsData` / `VolumeSetter` | BGM・SE・UI音の再生と音量管理 |
| カメラ | `Camera` | `FixedCameraShaker` | 固定カメラの画面揺れ |
| コレクション | `Collections` | `SerializableDictionary` / `Pair` | Inspector で見える辞書、キーと値のペア |
| 当たり判定 | `Collision` | `TriggerChecker` | レイヤー指定のトリガー検知イベント |
| 入力 | `Input` | `LocalMultiplayManager` ほか | InputSystem のローカルマルチ対応 |
| システム | `LibSystem` | `SingletonMonoBehaviour<T>` | MonoBehaviour シングルトン基底 |
| NFC | `NFC` / `NFC.NDEF` | `NFCReader` / `NFCMonitor` / `NDEFHelper` | PC/SC 経由の NFC 読み取りと NDEF 解析 |
| パーティクル | `Particle` | `ParticleSpawner` / `ParticleColorByValue` | 名前で呼び出すパーティクル再生 |
| Timeline | `Timeline` | `TransformTweenTrack` / `TransformTweenClip` | Transform を移動させるトラック |
| UI | `UI` | `UIMonoBehaviour` / `TextDisplayer<T>` | UI 用基底クラス、TMP へのテキスト表示 |
| ユーティリティ | `Utility` | `EasingUtility` / `MathUtility` / `PlayerPrefsBool` | イージング、数学関数、bool 保存 |
| エディタ拡張 | `Editor` / `Editor.Tools` | `PrefabBrush` ほか | シーン編集を楽にするツール群 |

## 詳細

### Audio（AudioManager）

BGM・SE・UI音を一括管理するシングルトン。`AudioMixer` を使って音量を扱う。

- BGM: `PlayBGM` / `PauseBGM` / `StopBGM` / `ForcedStopBGM`（フェード中でも停止） / `ChangeBGM`
- フェード: `FadeInBGM` / `FadeOutBGM`（完了時コールバック付き）
- SE: `PlaySE` / `PlayLoopSE` / `StopLoopSE`
- UI音: `PlayUI`
- `AudioClipsData`（ScriptableObject）に名前とクリップを登録し、**名前の文字列で再生**する
- `VolumeSetter` が BGM / SE / UI の音量を 0〜1 で扱い、デシベル（-80〜0）に変換する

### Camera

`FixedCameraShaker.CameraShake(duration, magnitude)` を static で呼ぶだけで、固定カメラを揺らせる。

### Collections

- `SerializableDictionary<TKey, TValue>`: Inspector で中身が見える辞書。`AddElement` / `UpdateElementValue` / `GetElement` / `TryCreateDictionary` など。Inspector から編集すると壊れる、とコメントに注意書きあり。
- `Pair<T1, T2>`: 2つの値を保持するクラス。

### Collision

`TriggerChecker`: Collider に付けると、指定した `LayerMask` のオブジェクトが入った・出た時に `OnEnter` / `OnExit` イベントが発火する。トリガー判定にはどちらかに Rigidbody が必要。

### Input（InputSystem 拡張）

- `LocalMultiplayManager`: 接続デバイスをプレイヤーに割り当てるローカルマルチ管理。
  - 全デバイス一括登録 `RegisterAllDevices`、登録の受付開始/終了 `StartRegisterDevice` / `EndRegisterDevice`
  - 未割当プレイヤーの非表示 `SetActiveUnassignedPlayer`
  - デバイス切断の監視
- `MultiPlayInputHandlerBase`: ローカルマルチ用の入力ハンドラ基底（`Join` / `Leave` / `Enable` / `Disable`）
- `PlayerInputHandlerBase`: シングルプレイ用の入力ハンドラ基底
- `IInputHandlerOwner`: ハンドラを持つオブジェクト用のインターフェース
- `InputActionHelper.AddMoveAction`: Move アクションの追加ヘルパー
- `GamepadPaths` / `KeyboardPaths` / `MousePaths` / `PointerPaths`: コントロールパス文字列の定数集

### LibSystem

`SingletonMonoBehaviour<T>`: `Instance` で参照できるシングルトン基底。重複したら後から生成されたものを破棄する。`OnAwake` / `OnDestoryed` をオーバーライドして使う。`AudioManager` や `ParticleSpawner` がこれを継承している。

### NFC（Windows 専用・UniTask 必須）

Windows の `winscard.dll`（PC/SC）を P/Invoke して NFC カードを読む。

- `NFCReader`: コンテキスト確立、リーダー選択、カードの有無判定、`TryReadCard` による読み取り
- `NFCMonitor`: バックグラウンドで監視し、`OnCardRead` / `OnCardRemoved` / `OnError` を通知
- `NDEFHelper` ほか: NFC Forum Type2 の NDEF レコード（ヘッダ、ペイロード長、TNF）の解析。専用の例外あり

Windows 以外、または UniTask が無い環境ではこのアセンブリごと無効になる。

### Particle

- `ParticleSpawner`: 名前をキーに登録したパーティクルを、位置・回転・親を指定して `Play` する
- `ParticleColorByValue`: Gradient を元に、0〜1 の値でパーティクルの色を変える。`ParticleSpawner.Play(name, normalizedValue, ...)` から自動で適用される

### Timeline（Timeline パッケージが必要）

`TransformTweenTrack` / `TransformTweenClip`: Transform を開始位置から終了位置へ動かす Timeline トラック。

- 開始・終了は Transform 参照または座標直指定
- `AnimationCurve` でイージング指定（0〜1 に正規化されているか検証あり）
- クリップのブレンドに対応

### UI

- `UIMonoBehaviour`: `RectTransform` を自動取得する UI 基底クラス
- `TextDisplayer<T>`: `TextMeshProUGUI` に値を表示する抽象クラス。`SetText` を継承先で拡張する

### Utility

- `EasingUtility`: easings.net 準拠の 31 種類（Linear、Sine / Quad / Cubic / Quart / Quint / Expo / Circ / Back / Elastic / Bounce の In・Out・InOut）。`Evaluate(x, EaseType)` でまとめて呼べる
- `MathUtility`: 偶数判定、正弦波・矩形波・三角波・のこぎり波、方向ベクトル、二乗距離、`SteerTowards`（目標への旋回移動）、`RoundVector3`
- `PlayerPrefsBool`: PlayerPrefs で bool を扱う（`GetBool` / `SetBool` / `GetBoolHasKey` / `GetIntHasKey`）

### Editor ツール

メニュー `SchwesteriumLibrary/Editor/` から開く。

- **PrefabBrush**: シーンビュー上でブラシのように Prefab を散布。半径、密度、スケール範囲を指定でき、地面に合わせて配置する
- **Replace Objects**: 名前またはタグで検索したオブジェクトを別の Prefab / オブジェクトに一括置換。位置・回転・スケールを引き継ぎ、Undo 対応
- **ClassGenerator**: クラス名、名前空間、継承元、作者名、MIT ライセンス表記の有無を指定して C# スクリプトを生成
- **DropField / GUISize**: 独自エディタ作成用のヘルパー（ドラッグ&ドロップ欄の描画、GUI サイズ定数）

## アセンブリ構成

| アセンブリ | 役割 |
|---|---|
| `SchwesteriumLibrary.Runtime` | 本体（Shared / InputSystem / TextMeshPro を参照） |
| `SchwesteriumLibrary.Shared` | Runtime と Editor で共有する `MathUtility` |
| `SchwesteriumLibrary.Editor` | エディタ拡張（Editor 限定） |
| `SchwesteriumLibrary.NFC` | NFC（Windows かつ UniTask がある時だけ有効） |
| `SchwesteriumLibrary.Timeline` | Timeline 拡張（Timeline がある時だけ有効） |
