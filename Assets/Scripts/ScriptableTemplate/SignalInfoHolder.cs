using UnityEngine;


[CreateAssetMenu(menuName = ("SceneSignal/SignalInfoHolder"),fileName = ("SceneSignalInfo"))]

public class SignalInfoHolder : ScriptableObject
{
    public string sceneName;
    public int sceneIndex;
    public int spawnID;
    public bool isFaceRight;
}