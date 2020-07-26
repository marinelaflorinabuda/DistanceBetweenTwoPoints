using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Componet manages scenes
/// Author: Marinela
/// </summary>
public class ScenesManager : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    
}
