﻿using Breakout.Core.Entities;
using MvvmCross.Plugin.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout.Core.Messages
{
    public class BrickDestroyedMessage: MvxMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public BrickDestroyedMessage(object sender)
            : base(sender)
        {
        }
    }
}
