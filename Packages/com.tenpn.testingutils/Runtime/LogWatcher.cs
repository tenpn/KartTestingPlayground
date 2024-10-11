/**
 * Copyright (C) Andrew Fray - All Rights Reserved
 * 
 * This source code is protected under international copyright law.  All rights
 * reserved and protected by the copyright holders.
 * This file is confidential and only available to authorized individuals with the
 * permission of the copyright holders.  If you encounter this file and do not have
 * permission, please contact the copyright holders and delete this file.
 */
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace TenPN.TestingUtils
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