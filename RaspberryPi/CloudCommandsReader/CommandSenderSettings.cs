using System.Collections.Generic;

namespace CloudCommandsReader
{
    public class CommandSenderSettings
    {
        public string SwitcherUrlFormat { get; set; }

        public Command[] CommandMapping { get; set; }
    }
}