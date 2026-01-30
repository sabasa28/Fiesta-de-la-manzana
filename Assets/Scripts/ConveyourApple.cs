using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConveyourApple : MonoBehaviour
{
    public float movementSpeed;
    [SerializeField] float baseSpeed;
    [SerializeField] float throwSpeed;
    [SerializeField] GameObject graphicTarget;
    bool gfxTargetFollowsTouch = false;
    [SerializeField] Vector2 screenPosWhenDragged;
    bool thrown = false;
    Vector3 directionThrown;
    float maxDistanceToThrow;
    [SerializeField] float rotationSpeedWhenThrown;
    [SerializeField] int oneOutOfIsBadApple;
    [SerializeField] Sprite[] badApplePosibleSprites;
    public bool isBadApple;
    public ConveyourBeltController conveyourBeltController;
    private void Start()
    {
        isBadApple = Random.Range(0, oneOutOfIsBadApple) == 0; //not acab??
        if (isBadApple)
        {
            graphicTarget.GetComponent<SpriteRenderer>().sprite = badApplePosibleSprites[Random.Range(0, badApplePosibleSprites.Length)]; 
        }
        maxDistanceToThrow = Vector3.Distance(Camera.main.WorldToScreenPoint(graphicTarget.GetComponent<SpriteRenderer>().bounds.min), Camera.main.WorldToScreenPoint(graphicTarget.GetComponent<SpriteRenderer>().bounds.center));
        Debug.Log(graphicTarget.GetComponent<SpriteRenderer>().bounds.min.x); //HACER BIEN ESTO
        Debug.Log(graphicTarget.GetComponent<SpriteRenderer>().bounds.max.x);
    }
    void Update()
    {
        if (thrown)
        {
            transform.position += directionThrown * throwSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + Time.deltaTime * rotationSpeedWhenThrown);
        }
        else
        { 
            transform.position += Vector3.right * movementSpeed * baseSpeed * Time.deltaTime;
        }
        if (gfxTargetFollowsTouch)
        {
            graphicTarget.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            graphicTarget.transform.position = new Vector3(graphicTarget.transform.position.x, graphicTarget.transform.position.y, 0.0f);
            Vector2 GFXScreenPos = Camera.main.WorldToScreenPoint(graphicTarget.transform.position);
            if (Vector2.Distance(screenPosWhenDragged, GFXScreenPos) > maxDistanceToThrow)
            {
                directionThrown = (GFXScreenPos - screenPosWhenDragged).normalized;
                thrown = true;
                GetComponent<Collider2D>().enabled = false;
                StartCoroutine(DestroyAfterThrow());
                gfxTargetFollowsTouch = false;
                Debug.Log("distance reached");
            }
        }
    }

    public void DragApple()
    {
        gfxTargetFollowsTouch = true;
        screenPosWhenDragged = Input.mousePosition;
    }
    public void StopDraggingApple()
    {
        if (thrown)
        {
            return;
        }
        graphicTarget.transform.localPosition = Vector3.zero;
        gfxTargetFollowsTouch = false;
    }

    IEnumerator DestroyAfterThrow()
    {
        yield return new WaitForSeconds(2);
        conveyourBeltController.RemoveAppleFromList(this, false);
    }
}
