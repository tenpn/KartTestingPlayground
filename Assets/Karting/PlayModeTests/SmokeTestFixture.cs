using System;
using System.Collections;
using KartGame.KartSystems;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace KartGame.PlayModeTests
{
    [TestFixture]
    public class SmokeTestFixture
    {
        [OneTimeSetUp]
        public void LevelSmokeTestsOneTimeSetup()
        {
            SceneManager.LoadScene(0);
            Application.logMessageReceived += OnLog;
        }

        [SetUp]
        public void SetUp()
        {
            m_isTestFailed = false;
        }

        [TearDown]
        public void TearDown()
        {
            var player = Object.FindObjectOfType<ArcadeKart>();
            if (player)
            {
                player.RemoveInputSource(m_input);
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Application.logMessageReceived -= OnLog;
        }

        private static readonly LogType[] FAILING_LOG_TYPES = new[]
        {
            LogType.Assert,
            LogType.Error,
            LogType.Exception,
            LogType.Warning,
        };

        private bool m_isTestFailed;

        private void OnLog(string condition, string stacktrace, LogType type)
        {
            m_isTestFailed |= Array.IndexOf(FAILING_LOG_TYPES, type) >= 0;
        }

        [UnityTest, Order(1)]
        public IEnumerator MainMenu_LoadLevel_NoError()
        {
            yield return SceneManager.LoadSceneAsync("MainScene");
            yield return null;
            Assert.That(m_isTestFailed, Is.False);
        }

        [UnityTest, Order(3)]
        public IEnumerator PlayerKart_OnStartLine_DoesNotFallThroughFloor()
        {
            var player = Object.FindObjectOfType<ArcadeKart>();
            var startY = player.transform.position.y;

            yield return new WaitForSeconds(1);

            var endY = player.transform.position.y;

            Assert.That(endY, Is.EqualTo(startY).Within(0.05f));
        }

        [UnityTest, Order(8)]
        public IEnumerator Race_Started_NoErrors()
        {
            var timeManager = Object.FindObjectOfType<TimeManager>();
            yield return new WaitUntil(() => timeManager.IsRaceStarted);
            yield return new WaitForSeconds(0.5f);
            Assert.That(m_isTestFailed, Is.False);
        }

        class StubInput : IInput
        {
            public InputData GenerateInput()
            {
                return new InputData
                {
                    Accelerate = true,
                };
            }
        }

        private readonly StubInput m_input = new StubInput();

        [UnityTest, Order(10)]
        public IEnumerator PlayerKart_DriveForwards_NoErrors()
        {
            var player = Object.FindObjectOfType<ArcadeKart>();
            player.AddInputSource(m_input);

            yield return new WaitForSeconds(3);

            Assert.That(m_isTestFailed, Is.False);
        }

    }

}