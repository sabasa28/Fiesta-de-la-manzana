using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] string introductionText;
    [SerializeField] string mascotTickledText;
    [SerializeField] TextMeshProUGUI dialogueBubbleText;
    [SerializeField] MascotAnimationController mascotAnimationController;
    bool mascotTickled = false;
    private void Start()
    {
        dialogueBubbleText.text = introductionText;
    }
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

    public void OnMascotTickled()
    {
        if (mascotTickled)
        {
            return;
        }
        mascotTickled = true;
        dialogueBubbleText.text = mascotTickledText;
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.ClosedEyesHappy);
        StartCoroutine(WaitAndReturnToInitialAnim());
    }

    IEnumerator WaitAndReturnToInitialAnim()
    {
        yield return new WaitForSeconds(1.0f);
        mascotAnimationController.PlayAnim(mascotAnimationController.initialAnimation);
        dialogueBubbleText.text = introductionText;
        mascotTickled = false;
    }
}
