// SkillSelectionUI セットアップスクリプト
var canvas = GameObject.Find("Canvas");
if (canvas != null)
{
    // スプライト読み込み
    var cardSpr  = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprite/Skill/Skil_card.png");
    var edgeSpr  = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprite/Skill/skil_edge.png");
    var healSpr  = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprite/Skill/Heal_icon.png");
    var timeSpr  = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprite/Skill/Time_icon.png");
    var growSpr  = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprite/Skill/Grow_icon.png");
    var specSpr  = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprite/Skill/Special_icon.png");
    Debug.Log($"Sprites: card={cardSpr?.name} edge={edgeSpr?.name} heal={healSpr?.name} time={timeSpr?.name} grow={growSpr?.name} special={specSpr?.name}");

    // 既存パネルを削除して再作成
    var existingPanel = canvas.transform.Find("SkillSelectionPanel");
    if (existingPanel != null) UnityEngine.Object.DestroyImmediate(existingPanel.gameObject);

    // SkillSelectionPanel (全画面暗幕、初期非表示)
    var panelGO = new GameObject("SkillSelectionPanel");
    panelGO.transform.SetParent(canvas.transform, false);
    var panelImg = panelGO.AddComponent<UnityEngine.UI.Image>();
    panelImg.color = new Color(0f, 0f, 0f, 0.75f);
    var panelRT = panelGO.GetComponent<RectTransform>();
    panelRT.anchorMin = Vector2.zero;
    panelRT.anchorMax = Vector2.one;
    panelRT.offsetMin = panelRT.offsetMax = Vector2.zero;

    // CardsContainer (水平レイアウト)
    var contGO = new GameObject("CardsContainer");
    contGO.transform.SetParent(panelGO.transform, false);
    var hlg = contGO.AddComponent<UnityEngine.UI.HorizontalLayoutGroup>();
    hlg.spacing = 80f;
    hlg.childAlignment = TextAnchor.MiddleCenter;
    hlg.childForceExpandWidth = false;
    hlg.childForceExpandHeight = false;
    hlg.childControlWidth = false;
    hlg.childControlHeight = false;
    var contRT = contGO.GetComponent<RectTransform>();
    contRT.anchorMin = contRT.anchorMax = contRT.pivot = new Vector2(0.5f, 0.5f);
    contRT.anchoredPosition = Vector2.zero;
    contRT.sizeDelta = new Vector2(1020f, 560f);

    // 3枚のスキルカード生成
    var cardUIArr = new SkillCardUI[3];
    for (int i = 0; i < 3; i++)
    {
        var cardGO = new GameObject($"SkillCard_{i}");
        cardGO.transform.SetParent(contGO.transform, false);
        var cardBgImg = cardGO.AddComponent<UnityEngine.UI.Image>();
        if (cardSpr != null) { cardBgImg.sprite = cardSpr; cardBgImg.type = UnityEngine.UI.Image.Type.Sliced; }
        else cardBgImg.color = Color.white;
        cardBgImg.raycastTarget = true;
        var cardRT = cardGO.GetComponent<RectTransform>();
        cardRT.sizeDelta = new Vector2(280f, 540f);

        // Edge image (カテゴリ色で着色、実行時に Setup() でセット)
        var edgeGO = new GameObject("EdgeImage");
        edgeGO.transform.SetParent(cardGO.transform, false);
        var edgeImg = edgeGO.AddComponent<UnityEngine.UI.Image>();
        if (edgeSpr != null) { edgeImg.sprite = edgeSpr; edgeImg.type = UnityEngine.UI.Image.Type.Sliced; }
        edgeImg.color = Color.white;
        edgeImg.raycastTarget = false;
        var edgeRT = edgeGO.GetComponent<RectTransform>();
        edgeRT.anchorMin = Vector2.zero;
        edgeRT.anchorMax = Vector2.one;
        edgeRT.offsetMin = edgeRT.offsetMax = Vector2.zero;

        // Icon image (カテゴリアイコン、実行時に Setup() でセット)
        var iconGO = new GameObject("IconImage");
        iconGO.transform.SetParent(cardGO.transform, false);
        var iconImg = iconGO.AddComponent<UnityEngine.UI.Image>();
        iconImg.color = Color.white;
        iconImg.raycastTarget = false;
        var iconRT = iconGO.GetComponent<RectTransform>();
        iconRT.anchorMin = iconRT.anchorMax = iconRT.pivot = new Vector2(0.5f, 1f);
        iconRT.anchoredPosition = new Vector2(0f, -45f);
        iconRT.sizeDelta = new Vector2(115f, 115f);

        // スキル名テキスト
        var nameGO = new GameObject("NameText");
        nameGO.transform.SetParent(cardGO.transform, false);
        var nameTMP = nameGO.AddComponent<TMPro.TextMeshProUGUI>();
        nameTMP.text = "スキル名";
        nameTMP.fontSize = 52f;
        nameTMP.alignment = TMPro.TextAlignmentOptions.Center;
        nameTMP.color = new Color(0.1f, 0.1f, 0.1f);
        var nameRT = nameGO.GetComponent<RectTransform>();
        nameRT.anchorMin = nameRT.anchorMax = nameRT.pivot = new Vector2(0.5f, 0.5f);
        nameRT.anchoredPosition = new Vector2(0f, 60f);
        nameRT.sizeDelta = new Vector2(250f, 80f);

        // 説明テキスト
        var descGO = new GameObject("DescText");
        descGO.transform.SetParent(cardGO.transform, false);
        var descTMP = descGO.AddComponent<TMPro.TextMeshProUGUI>();
        descTMP.text = "説明文";
        descTMP.fontSize = 26f;
        descTMP.alignment = TMPro.TextAlignmentOptions.Center;
        descTMP.color = new Color(0.25f, 0.25f, 0.25f);
        var descRT = descGO.GetComponent<RectTransform>();
        descRT.anchorMin = descRT.anchorMax = descRT.pivot = new Vector2(0.5f, 0.5f);
        descRT.anchoredPosition = new Vector2(0f, -80f);
        descRT.sizeDelta = new Vector2(250f, 120f);

        // SkillCardUI コンポーネントを追加して SerializedObject で配線
        var scu = cardGO.AddComponent<SkillCardUI>();
        var soCard = new UnityEditor.SerializedObject(scu);
        soCard.FindProperty("edgeImage").objectReferenceValue = edgeImg;
        soCard.FindProperty("iconImage").objectReferenceValue = iconImg;
        soCard.FindProperty("nameText").objectReferenceValue = nameTMP;
        soCard.FindProperty("descText").objectReferenceValue = descTMP;
        soCard.ApplyModifiedProperties();
        cardUIArr[i] = scu;
    }

    // SkillSelectionUI コンポーネントをパネルに追加して配線
    var selUI = panelGO.AddComponent<SkillSelectionUI>();
    var soSel = new UnityEditor.SerializedObject(selUI);
    soSel.FindProperty("panel").objectReferenceValue = panelGO;
    var cardsProp = soSel.FindProperty("cards");
    cardsProp.arraySize = 3;
    for (int i = 0; i < 3; i++)
        cardsProp.GetArrayElementAtIndex(i).objectReferenceValue = cardUIArr[i];
    soSel.ApplyModifiedProperties();

    // SkillManager GO を作成（または既存を使用）して配線
    var smGO = GameObject.Find("SkillManager");
    if (smGO == null) smGO = new GameObject("SkillManager");
    var sm = smGO.GetComponent<SkillManager>() ?? smGO.AddComponent<SkillManager>();
    var soSM = new UnityEditor.SerializedObject(sm);
    soSM.FindProperty("skillSelectionUI").objectReferenceValue = selUI;
    soSM.FindProperty("healIcon").objectReferenceValue = healSpr;
    soSM.FindProperty("timeIcon").objectReferenceValue = timeSpr;
    soSM.FindProperty("growIcon").objectReferenceValue = growSpr;
    soSM.FindProperty("specialIcon").objectReferenceValue = specSpr;
    soSM.ApplyModifiedProperties();

    // パネルは初期非表示
    panelGO.SetActive(false);

    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
    Debug.Log("SkillSelectionUI setup complete! SkillManager GO: " + smGO.name);
}
else { Debug.LogError("Canvas not found!"); }
