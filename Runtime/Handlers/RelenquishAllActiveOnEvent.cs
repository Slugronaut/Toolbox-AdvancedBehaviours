using Toolbox.AutoCreate;
using Toolbox.Lazarus;

namespace Toolbox.Behaviours
{
    /// <summary>
    /// Drains all Lazarus pools when triggered by a standard Untity Event.
    /// </summary>
    public class RelenquishAllActiveOnEvent : AbstractOperationOnEvent
    {
        IPoolSystem Lazarus;

        protected override void Awake()
        {
            Lazarus = AutoCreator.AsSingleton<IPoolSystem>();
        }

        /// <summary>
        /// Relenquishes all active pooled objects to their appropriate pools.
        /// </summary>
        public override void PerformOp()
        {
            Lazarus.RelenquishAll();
        }
    }
}
