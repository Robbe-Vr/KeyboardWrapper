using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SYWCentralLogging;

namespace KeyboardWrapper.Core.Default
{
    internal partial class KeyboardControl
    {
        internal KeyboardControl()
        {
            
        }

        public static string SimulateKeys(IEnumerable<string> keys)
        {
            try
            {
                int count = keys.Count();
                if (count < 1)
                {
                    return "INVALID DATA";
                }
                else if (count == 1)
                {
                    Keys key;
                    if (Enum.TryParse(keys.ElementAt(0), out key))
                    {
                        KeySimulator.PerformKeyPress(key);
                    }
                    else
                    {
                        Logger.Log($"Unable to parse {keys.ElementAt(0)} into a valid key!");
                        return "INVALID DATA";
                    }
                }
                else
                {
                    foreach (string keyStr in keys)
                    {
                        Keys key;
                        if (Enum.TryParse(keyStr, out key))
                        {
                            KeySimulator.PressKey(key);
                        }
                        else
                        {
                            Logger.Log($"Unable to parse {keyStr} into a valid key!");
                            continue;
                        }
                    }

                    KeySimulator.ReleaseAll();
                }

                return "EXECUTED";
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to simulate keys of order {string.Join("->", keys)}!\nError: {e.Message}");
                return "FAIL";
            }
        }
    }
}
