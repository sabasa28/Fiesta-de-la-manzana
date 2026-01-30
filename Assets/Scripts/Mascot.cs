using UnityEngine;
using TMPro;
public class Mascot : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string conveyourBeltMinigameIntroText;
    [SerializeField] string conveyourBeltWrongApplePassedText;
    [SerializeField] string conveyourBeltGoodAppleDiscardedText;
    [SerializeField] string conveyourBeltLoseText;
    [SerializeField] TextMeshProUGUI mascotText;
    [SerializeField] GameObject mascotTextHolder;
    bool displayingText = false;
    [SerializeField] float timeDisplayingText;
    float timer = 0.0f;

    void Update()
    {
        if (displayingText)
        {
            timer += Time.deltaTime;
            if (timer > timeDisplayingText)
            {
                displayingText = false;
                mascotTextHolder.SetActive(false);
            }
        }
    }

    public void SayConveyourBeltMinigameIntroText()
    {
        displayingText = true;
        mascotTextHolder.SetActive(true);
        mascotText.text = conveyourBeltMinigameIntroText;
        timer = 0.0f;
    }

    public void SayConveyourBeltGoodAppleDiscardedText()
    {
        displayingText = true;
        mascotTextHolder.SetActive(true);
        mascotText.text = conveyourBeltGoodAppleDiscardedText;
        timer = 0.0f;
    }

    public void SayConveyourBeltWrongApplePassedText()
    {
        displayingText = true;
        mascotTextHolder.SetActive(true);
        mascotText.text = conveyourBeltWrongApplePassedText;
        timer = 0.0f;
    }

    public void SayConveyourBeltLoseText(int applesSaved)
    {
        displayingText = true;
        mascotTextHolder.SetActive(true);
        conveyourBeltLoseText = conveyourBeltLoseText.Replace("X", applesSaved.ToString());
        mascotText.text = conveyourBeltLoseText;
        timer = 0.0f;
    }
}
