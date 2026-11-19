/*
Author : schwesterium
Date   : 2026/08/01
*/

using SchwesteriumLibrary.Input.ControlPath;
using UnityEngine.InputSystem;

namespace SchwesteriumLibrary.Input
{
    public static class InputActionHelper
    {
        //groups
        public const string KEYBOARD = "Keyboard";
        public const string GAMEPAD = "Gamepad";

        public static InputAction AddMoveAction(InputActionMap map, string actionName = "Move")
        {
            var moveAction = map.AddAction(actionName, InputActionType.Value);

            moveAction.AddBinding(GamepadPaths.LEFT_STICK, groups: GAMEPAD);
            moveAction.AddCompositeBinding("Dpad")
                .With("Up", KeyboardPaths.W, groups: KEYBOARD)
                .With("Down", KeyboardPaths.S, groups: KEYBOARD)
                .With("Left", KeyboardPaths.A, groups: KEYBOARD)
                .With("Right", KeyboardPaths.D, groups: KEYBOARD);

            return moveAction;
        }
    }
}