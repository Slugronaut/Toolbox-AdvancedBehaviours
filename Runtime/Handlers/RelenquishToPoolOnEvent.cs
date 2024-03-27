
namespace Peg.Behaviours
{
    /// <summary>
    /// Returns this gameobject to the pool from which it came upon having a Unity standard event trigger.
    /// </summary>
    public class RelenquishToPoolOnEvent : AbstractOperationOnEvent
    {
        public override void PerformOp()
        {
            Lazarus.Lazarus.Instance.RelenquishToPool(gameObject);
        }
    }
}
