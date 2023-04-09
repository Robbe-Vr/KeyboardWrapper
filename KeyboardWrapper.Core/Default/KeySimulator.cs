using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SYWCentralLogging;

namespace KeyboardWrapper.Core.Default
{
    internal class KeySimulator
    {
        private static List<Keys> PressedKeys = new List<Keys>();

        private static Timer releaseTimer;
        private static int rerelease = 0;

        [DllImport("PerformKeystroke.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void PressKey(string keyName);
        [DllImport("PerformKeystroke.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ReleaseKey(string keyName);
        [DllImport("PerformKeystroke.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void PerformKey(string keyName);

        internal static void PerformKeyPress(Keys key)
        {
            if (PressedKeys.Contains(key))
            {
                ReleaseKey(key);
            }

            PerformKey(key.ToString());
        }

        internal static void PressKey(Keys key)
        {
            if (PressedKeys.Contains(key))
            {
                ReleaseKey(key);
            }

            string kbKey = (int)key < 9 ? key.ToString().Replace("NUM_", "") : key.ToString();

            PressKey(kbKey);

            PressedKeys.Add(key);

            releaseTimer = new Timer(5000);
            releaseTimer.Elapsed += releaseTimerElapsed;
            releaseTimer.Start();
        }
        internal static void ReleaseKey(Keys key)
        {
            ReleaseKey(key.ToString().Replace("NUM_", ""));

            PressedKeys.Remove(key);
        }

        private static void releaseTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (PressedKeys.Count > 0)
            {
                ReleaseAll();
            }
        }

        internal static void ReleaseAll()
        {
            try
            {
                foreach (Keys key in PressedKeys)
                {
                    string kbKey = (int)key < 9 ? key.ToString().Replace("NUM_", "") : key.ToString();

                    ReleaseKey(kbKey);
                }
            }
            catch (Exception e)
            {
                Logger.Log("Failed to release pressed keys: " + e.Message + "; Retrying...");
                if (rerelease < 10)
                {
                    System.Threading.Thread.Sleep(5);
                    rerelease++;
                    ReleaseAll();
                }
                else rerelease = 0;
            }
        }
    }
}
