using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame
{
    /// <summary>
    /// An interface for anything that can accept messages
    /// </summary>
    interface IReceiver
    {
        /// <summary>
        /// Add a message to the receiver
        /// </summary>
        /// <param name="message">Message to send</param>
        void AddMessage(Message.Message message);
    }
}
