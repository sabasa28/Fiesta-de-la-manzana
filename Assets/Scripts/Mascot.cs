using UnityEngine;
using TMPro;
public class Mascot : MonoBehaviour
{
    [SerializeField] string conveyourBeltMinigameIntroText;
    [SerializeField] string conveyourBeltWrongApplePassedText;
    [SerializeField] string conveyourBeltGoodAppleDiscardedText;
    [SerializeField] string conveyourBeltLoseText;
    [SerializeField] TextMeshProUGUI mascotText;
    [SerializeField] GameObject mascotTextHolder;
    bool displayingText = false;
    [SerializeField] float timeDisplayingText;
    float timer = 0.0f;
    [SerializeField] MascotAnimationController mascotAnimationController;

    public enum ReactionToScore
    {
        Sad,
        Happy,
        VeryHappy
    }

    void Update()
    {
        if (displayingText)
        {
            timer += Time.deltaTime;
            if (timer > timeDisplayingText)
            {
                displayingText = false;
                mascotTextHolder.SetActive(false);
                mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.Idle);
            }
        }
    }

    public void SayConveyourBeltMinigameIntroText() //NO SE USA
    {
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.ClosedEyesHappyHand);
        displayingText = true;
        mascotTextHolder.SetActive(true);
        mascotText.text = conveyourBeltMinigameIntroText;
        timer = -4.0f;
    }

    public void SayConveyourBeltGoodAppleDiscardedText()
    {
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.Crying);
        displayingText = true;
        mascotTextHolder.SetActive(true);
        mascotText.text = conveyourBeltGoodAppleDiscardedText;
        timer = 0.0f;
    }

    public void SayConveyourBeltWrongApplePassedText()
    {
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.Crying);
        displayingText = true;
        mascotTextHolder.SetActive(true);
        mascotText.text = conveyourBeltWrongApplePassedText;
        timer = 0.0f;
    }

    public void SayConveyourBeltLoseText(int applesSaved)
    {
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.IdleHand);
        displayingText = true;
        mascotTextHolder.SetActive(true);
        conveyourBeltLoseText = conveyourBeltLoseText.Replace("X", applesSaved.ToString());
        mascotText.text = conveyourBeltLoseText;
        timer = 0.0f;
    }

    public void ShutUp()
    {
        mascotTextHolder.SetActive(false);
        displayingText = false;
    }

    public void SetAsIdle()
    { 
        mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.Idle);
    }

    public void SetEndGameFace(ReactionToScore reactionToScore)
    {
        switch (reactionToScore)
        {
            case ReactionToScore.Sad:
                mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.Crying);
                break;
            case ReactionToScore.Happy:
                mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.ClosedEyesHappy);
                break;
            case ReactionToScore.VeryHappy:
                mascotAnimationController.PlayAnim(MascotAnimationController.MascotAnimations.ClosedEyesHappyHand);
                break;
        }
    }
}
