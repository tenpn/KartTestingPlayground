using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace TenPN.UnitTestUtils
{
    /// UnityTestFramework tests fail if there's any errors or execptions, but not warnings. Since warnings are errors, we treat them the same here.
    public class LogWatcher
    {
        /// can call multiple times, even if already registered
        public void Register()
        {
            Application.logMessageReceived -= OnLogMessage;
            Application.logMessageReceived += OnLogMessage;
        }

        public void Unregister()
        {
            Application.logMessageReceived -= OnLogMessage;
        }

        public void AssertIsClean()
        {
            Assert.That(m_unexpectedLogs, Is.Empty);
        }

        public void Clear()
        {
            m_unexpectedLogs.Clear();
        }

        /*--------------------------------------------------*/

        private readonly List<string> m_unexpectedLogs = new List<string>();

        private void OnLogMessage(string condition, string stacktrace, LogType type)
        {
            if (type != LogType.Log)
            {
                m_unexpectedLogs.Add($"[{type}] {condition}");
            }
        }
    }

}