using System;

namespace Domain
{
    public class ActionEventArgs : EventArgs
    {
        public string ActionName { get; }
        public bool Success { get; }
        public string Message { get; }

        public ActionEventArgs(string actionName, bool success, string message = "")
        {
            ActionName = actionName;
            Success = success;
            Message = message;
        }
    }
}