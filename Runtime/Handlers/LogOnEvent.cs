using UnityEngine;


namespace Toolbox.Behaviours
{
    /// <summary>
    /// Logs a message upong a Unity standard GameObject event.
    /// 
    /// </summary>
    [UnityEngine.AddComponentMenu("Toolbox/Action Triggers/Log on Event")]
    public class LogOnEvent : AbstractOperationOnEvent
    {
        public LogType Log;

        [TextArea(3,5)]
        public string Message;

        public override void PerformOp()
        {
            switch(Log)
            {
                case LogType.Log:
                    {
                        Debug.Log(Message);
                        break;
                    }
                case LogType.Warning:
                    {
                        Debug.LogWarning(Message);
                        break;
                    }
                case LogType.Error:
                    {
                        Debug.LogError(Message);
                        break;
                    }
                case LogType.Assert:
                    {
                        Debug.LogAssertion(Message);
                        break;
                    }
                case LogType.Exception:
                    {
                        //obviously can't handle exceptions - just log as error
                        Debug.LogError(Message);
                        break;
                    }
            }

        }
    }
}
