using KeyboardWrapper.Core.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SYWPipeNetworkManager;

namespace KeyboardWrapper.Core
{
    public class KeyboardManager
    {
        private IEnumerable<string> validatedSources = new List<string>()
        {
            "MidiDomotica"
        };

        public bool Setup()
        {
            PipeMessageControl.Init("Keyboard");
            PipeMessageControl.StartClient(
                (sourceName, message) =>
                {
                    if (ValidateSource(sourceName))
                    {
                        return $"{message} -> " + ProcessMessage(message);
                    }
                    else return $"{message} -> NO";
                }
            );

            return true;
        }

        private bool ValidateSource(string source)
        {
            return validatedSources.Contains(source);
        }

        public string ProcessMessage(string message)
        {
            IEnumerable<string> parts = new Regex(@"(::\[|\]::|::)|]$").Split(message).Where(x => !String.IsNullOrWhiteSpace(x) && !x.Contains("::")).Select(x => x.Trim());

            switch (parts.FirstOrDefault())
            {
                case "Get":
                    return KeyboardControl.GetAvailableKeys();

                case "Simulate":
                    return KeyboardControl.SimulateKeys(parts.Skip(1)?.FirstOrDefault()?.Split(',').Select(x => x.Trim()));
            }

            return "INVALID DATA";
        }
    }
}
