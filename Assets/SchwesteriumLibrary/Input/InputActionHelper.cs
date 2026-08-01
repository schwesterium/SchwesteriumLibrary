/*
Author : schwesterium
Date   : 2026/08/01
*/

using SchwesteriumLibrary.Input.ContollePath;
using UnityEngine.InputSystem;

namespace SchwesteriumLibrary.Input
{
    public static class InputActionHelper
    {
        //groups
        public const string KEYBOARD = "Keyboard";
        public const string GAMEPAD = "Gamepad";

        public static void AddMoveAction(InputActionMap map, string actionName = "Move")
        {
            var moveAction = map.AddAction(actionName, InputActionType.Value);

            moveAction.AddBinding(GamepadPaths.LEFT_STICK, groups: GAMEPAD);
            moveAction.AddCompositeBinding("Dpad")
                .With("Up", KeyboadPaths.W, groups: KEYBOARD)
                .With("Down", KeyboadPaths.S, groups: KEYBOARD)
                .With("Left", KeyboadPaths.A, groups: KEYBOARD)
                .With("Right", KeyboadPaths.D, groups: KEYBOARD);
        }
    }
}