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
        // �����˵�UI���߼�
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        // �ر����˵�UI���߼�
        gameObject.SetActive(false);
    }
}
