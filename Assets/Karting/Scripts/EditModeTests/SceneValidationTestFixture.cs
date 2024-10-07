using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace KartGame.EditModeTests
{
    public class BuildSettingsSceneSource : IEnumerable<BuildSettingsSceneSource.ScenePath>
    {
        public class ScenePath
        { 
            public string Path;
            public override string ToString() => System.IO.Path.GetFileNameWithoutExtension(Path);
        }
        
        public IEnumerator<BuildSettingsSceneSource.ScenePath> GetEnumerator() 
            => EditorBuildSettings.scenes.Select(scene => new ScenePath{Path=scene.path}).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    [TestFixtureSource(typeof(BuildSettingsSceneSource))]
    public class SceneValidationTestFixture
    {
        private string m_scenePath;
        
        public SceneValidationTestFixture(BuildSettingsSceneSource.ScenePath scene)
        {
            m_scenePath = scene.Path;
        }

        [OneTimeSetUp]
        public void SceneValidationTestsOneTimeSetup()
        {
            EditorSceneManager.OpenScene(m_scenePath);
        }

        [Test]
        public void AllMonoBehaviours_Present()
        {
            var objectsWithMissingMonoBehaviours = new List<string>();
            
            var allGameObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var go in allGameObjects)
            {
                int missingCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
                if (missingCount > 0)
                {
                    objectsWithMissingMonoBehaviours.Add(go.name);
                }
            }
            
            Assert.That(objectsWithMissingMonoBehaviours, Is.Empty);
        }
    }

}