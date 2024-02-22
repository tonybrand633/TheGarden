using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignalInfo : MonoBehaviour
{    
    public SignalInfoHolder SignalInfoHolder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.AnalyzeTheSignal(SignalInfoHolder);
    }
}
