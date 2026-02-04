using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTester : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    InputAction touchPosition;
    [SerializeField] TextMeshProUGUI textDebug;
    void Awake()
    {
        touchPosition = playerInput.actions["TouchPosition"];
    }

    void Update()
    {
        textDebug.text = touchPosition.ReadValue<Vector2>().ToString();
    }
}
