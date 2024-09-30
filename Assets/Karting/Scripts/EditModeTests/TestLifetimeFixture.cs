using NUnit.Framework;
using UnityEngine;

namespace KartGame.EditModeTests
{

    public class TestLifetimeFixture
    {
        [OneTimeSetUp]
        public void ExampleOneTimeSetUp()
        {
            Debug.Log("One time setup");
        }

        [SetUp]
        public void ExampleSetUp()
        {
            Debug.Log("Setup");
        }

        [Test, Order(1)]
        public void NewTestScriptSimplePasses()
        {
            Debug.Log("New Test");
        }

        [Test, Order(10)]
        public void AnotherTestScriptSimplePasses()
        {
            Debug.Log("Another Test");
        }

        [TearDown]
        public void ExampleTearDown()
        {
            Debug.Log("Teardown");
        }

        [OneTimeTearDown]
        public void ExampleOneTimeTearDown()
        {
            Debug.Log("One time teardown");
        }
    }
}