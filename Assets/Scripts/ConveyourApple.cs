using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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
    [SerializeField] Sprite shoeSprite;
    public bool isBadApple;
    public ConveyourBeltController conveyourBeltController;
    bool usingTouch = false;
    int dragId;
    private void Start()
    {
        isBadApple = Random.Range(0, oneOutOfIsBadApple) == 0; //not acab??
        if (isBadApple)
        {
            if (Random.Range(0, 10) == 0)
            {
                graphicTarget.GetComponent<SpriteRenderer>().sprite = shoeSprite;
            }
            else
            {
                graphicTarget.GetComponent<SpriteRenderer>().sprite = badApplePosibleSprites[Random.Range(0, badApplePosibleSprites.Length)];            
            }
        }
        maxDistanceToThrow = Vector3.Distance(Camera.main.WorldToScreenPoint(graphicTarget.GetComponent<SpriteRenderer>().bounds.min), Camera.main.WorldToScreenPoint(graphicTarget.GetComponent<SpriteRenderer>().bounds.center));
         //HACER BIEN ESTO
        
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
            if (usingTouch)
            {
                graphicTarget.transform.position = Camera.main.ScreenToWorldPoint(conveyourBeltController.ReturnTouchPos(dragId));
            }
            else
            {
                graphicTarget.transform.position = Camera.main.ScreenToWorldPoint(conveyourBeltController.ReturnTouchPos());
            }
            graphicTarget.transform.position = new Vector3(graphicTarget.transform.position.x, graphicTarget.transform.position.y, 0.0f);
            Vector2 GFXScreenPos = Camera.main.WorldToScreenPoint(graphicTarget.transform.position);
            if (Vector2.Distance(screenPosWhenDragged, GFXScreenPos) > maxDistanceToThrow)
            {
                directionThrown = (GFXScreenPos - screenPosWhenDragged).normalized;
                thrown = true;
                GetComponent<Collider2D>().enabled = false;
                StartCoroutine(DestroyAfterThrow());
                gfxTargetFollowsTouch = false;
            }
        }
    }

    public void DragApple(BaseEventData pointerEventData)
    {
        ExtendedPointerEventData castedEventData = (ExtendedPointerEventData)pointerEventData;
        dragId = castedEventData.touchId;
        usingTouch = castedEventData.pointerType != UIPointerType.MouseOrPen;
        if (usingTouch)
        {
            screenPosWhenDragged = conveyourBeltController.ReturnTouchPos(dragId);
        }
        else
        { 
            screenPosWhenDragged = conveyourBeltController.ReturnTouchPos();
        }
        gfxTargetFollowsTouch = true;
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

    public void Test(BaseEventData pointerEventData)
    {
        ExtendedPointerEventData castedEventData = (ExtendedPointerEventData)pointerEventData;
        dragId = castedEventData.touchId;
    }
}
