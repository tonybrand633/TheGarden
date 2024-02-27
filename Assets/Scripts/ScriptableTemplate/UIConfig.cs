using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUIConfig", menuName = "UI System/UI Config")]
public class UIConfig : ScriptableObject
{
    public List<UIItem> uiItems;
}

[System.Serializable]
public class UIItem
{
    public string key;
    public UIBase uiPrefab;
}
