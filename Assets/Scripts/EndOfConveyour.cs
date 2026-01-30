using UnityEngine;

public class EndOfConveyour : MonoBehaviour
{
    [SerializeField] ConveyourBeltController conveyourBeltController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ConveyourApple"))
        {
            conveyourBeltController.RemoveAppleFromList(collision.GetComponent<ConveyourApple>(), true);
        }
    }
}
