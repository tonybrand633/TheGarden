using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public abstract void Open();
    public abstract void Close();
}

public class MainMenuUI : UIBase
{
    public override void Open()
    {
        // 打开主菜单UI的逻辑
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        // 关闭主菜单UI的逻辑
        gameObject.SetActive(false);
    }
}
