using System;
using System.Collections.Generic;
using System.Text;

namespace Bot
{
    public interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}
