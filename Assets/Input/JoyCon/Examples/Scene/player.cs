using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Momo.Example
{

    public class player : MonoBehaviour
    {
        private Vector2 move;
        public void OnMove(CallbackContext input)
        {
            move = input.ReadValue<Vector2>();
            print(move);
        }
        public void OnAction(CallbackContext input)
        {
            if (input.performed)
            {
                transform.Rotate(Vector3.forward, 20);
            }
        }

        private void Update()
        {
            //JoyCons usually have drift so it's best to add a generous deadzone usually
            if (move.magnitude > 0.2f)
            {
                transform.Translate(move * 5 * Time.deltaTime, Space.World);
            }
        }
    }
}