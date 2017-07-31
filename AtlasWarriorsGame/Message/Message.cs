using System;
using System.Collections.Generic;
using System.Text;

namespace AtlasWarriorsGame.Message
{
    /// <summary>
    /// Message to be displayed by the interface
    //  </summary>
    public class Message
    {
        /// <summary>
        /// Construct message
        /// </summary>
        /// <param name="origin">Value of Origin field</param>
        public Message(XY origin = null)
        {
            Origin = null;
        }

        /// <summary>
        /// Breach MVC and give a basic text reprentation. Being used for now.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "";
        }

        /// <summary>
        /// Type of message that this is
        /// </summary>
        public MessageTypes MessageType;

        /// <summary>
        /// Location of message - if relevant
        /// </summary>
        public XY Origin;
    }

    /// <summary>
    /// Types of messages that might be displayed
    /// </summary>
    public enum MessageTypes
    {
        GENERIC,
        ATTACK
    }
}
