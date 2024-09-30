using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace KartGame.EditModeTests
{
    public struct PrettyScenePath
    {
        public string ScenePath;
        public override string ToString() => System.IO.Path.GetFileNameWithoutExtension(ScenePath);
    }

    /// a collection of paths to track scenes 
    public class TrackSceneSource : IEnumerable<PrettyScenePath>
    {
        public IEnumerator<PrettyScenePath> GetEnumerator()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene.path.Contains("IntroMenu") == false
                                && scene.path.Contains("WinScene") == false
                                && scene.path.Contains("LoseScene") == false)
                .Select(scene => new PrettyScenePath { ScenePath = scene.path })
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// validating every level scene
    /// </summary>
    [TestFixtureSource(typeof(TrackSceneSource))]
    public class SceneValidationTestsFixture
    {
        private readonly string m_scenePath;

        public SceneValidationTestsFixture(PrettyScenePath scene)
        {
            m_scenePath = scene.ScenePath;
        }
        
        [SetUp]
        public void SceneValidationTestsFixtureOneTimeSetUp()
        {
            EditorSceneManager.OpenScene(m_scenePath, OpenSceneMode.Single);
        }
        
        [Test]
        public void GameManager_Exactly1()
        {
            var gameManagers = Object.FindObjectsOfType<GameFlowManager>();
            Assert.That(gameManagers.Length, Is.EqualTo(1));
        }
    }
}