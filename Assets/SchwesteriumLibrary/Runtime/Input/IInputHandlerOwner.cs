/*
Author : schwesterium
Date   : 2026/08/01
*/

namespace SchwesteriumLibrary.Input
{
    public interface IInputHandlerOwner
    {
        public MultiPlayInputHandlerBase GetInputHandler();
    }
}