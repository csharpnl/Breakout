// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the Message type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Breakout.Core.Messages
{
    using MvvmCross.Plugin.Messenger;

    /// <summary>
    ///    Defines the Message type.
    /// </summary>
    public class SpawnBonusMessage : MvxMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public SpawnBonusMessage(object sender)
            : base(sender)
        {
        }
    }
}
