
namespace Peg.Behaviours
{
    /// <summary>
    /// Destroys the GameObject this component is attached to upon the set Unity event.
    /// </summary>
    public class DestroyOnEvent : AbstractOperationOnEvent
    {
        public override void PerformOp()
        {
            Destroy(gameObject);
        }
    }
}
