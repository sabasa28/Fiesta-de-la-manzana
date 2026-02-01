using UnityEngine;
using UnityEngine.SceneManagement;
public class DebugButton : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
