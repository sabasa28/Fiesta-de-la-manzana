using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    
    public void GoToQnAScene()
    {
        SceneManager.LoadScene("QnA");
    }
    public void GoToBichosAttackScene()
    {
        SceneManager.LoadScene("BichosAttack");
    }
    public void GoToConveyourRushScene()
    {
        SceneManager.LoadScene("ConveyourRush");
    }
}
