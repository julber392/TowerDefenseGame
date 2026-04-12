using UnityEngine;
using UnityEngine.InputSystem; 

public class NewMonoBehaviourScript : MonoBehaviour
{
    void Update()
    {

        
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Нажата клавиша ПРОБЕЛ!");
        }
     
        Vector2 move = Keyboard.current.wKey.isPressed ? Vector2.up : Vector2.zero;
        
    }
}