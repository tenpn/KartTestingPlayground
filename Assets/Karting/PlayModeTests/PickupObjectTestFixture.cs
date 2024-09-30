using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace KartGame.PlayModeTests
{
    [TestFixture]
    public class PickupObjectTestFixture
    {
        [OneTimeSetUp]
        public void LoadPickupScene()
        {
            SceneManager.LoadScene("PlaymodeTestPlayground");
        }

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return null;
            yield return null;
        }

        [Test]
        public void OnTriggerEnter_Player_TimeIncreases()
        {
            var time = Object.FindObjectOfType<TimeManager>();
            var startTime = time.TimeRemaining;

            var playerStub = GameObject.Find("StubPlayer").GetComponent<Collider>();
            var checkpoint = Object.FindObjectOfType<PickupObject>();

            checkpoint.SendMessage("OnTriggerEnter", playerStub);

            Assert.That(time.TimeRemaining, Is.GreaterThan(startTime));
        }
    }

}