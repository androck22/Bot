using System;
using System.Collections.Generic;
using System.Text;

namespace Bot.Commands
{
    interface IChatTextCommandWithAction : IChatTextCommand
    {
        bool DoAction(Conversation chat);
    }
}
