/**
 * Copyright (C) Andrew Fray - All Rights Reserved
 * 
 * This source code is protected under international copyright law.  All rights
 * reserved and protected by the copyright holders.
 * This file is confidential and only available to authorized individuals with the
 * permission of the copyright holders.  If you encounter this file and do not have
 * permission, please contact the copyright holders and delete this file.
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TenPN.TestingUtils
{
    /// all scenes listed in the build settings
    public class BuildSettingsSceneSource : IEnumerable<BuildSettingsSceneSource.ScenePath>
    {
        public class ScenePath
        { 
            public string Path;
            public override string ToString() => System.IO.Path.GetFileNameWithoutExtension(Path);
        }
        
        public IEnumerator<ScenePath> GetEnumerator() 
            => EditorBuildSettings.scenes.Select(scene => new ScenePath{Path=scene.path}).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    /// a collection of generic tests that should work on any scene
    [TestFixtureSource(typeof(BuildSettingsSceneSource))]
    public class SceneValidationTestFixture
    {
        private readonly string m_scenePath;
        private readonly LogWatcher m_logs = new LogWatcher();
        
        public SceneValidationTestFixture(BuildSettingsSceneSource.ScenePath scene)
        {
            m_scenePath = scene.Path;
        }

        [OneTimeSetUp]
        public void SceneValidationTestsOneTimeSetUp()
        {
            m_logs.Register();
            EditorSceneManager.OpenScene(m_scenePath);
        }

        [OneTimeTearDown]
        public void SceneValidationTestsOneTimeTearDown()
        {
            m_logs.Unregister();
        }

        // first, so we get the loading logs and nothing else 
        [Test, Order(1)]
        public void LoadedScene_NoWarningsOrErrors()
        {
            m_logs.AssertIsClean();
        }

        [Test]
        public void AllMonoBehaviours_NoMissingReferences()
        {
            var missingReferences = new List<string>();
            
            var allGameObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var go in allGameObjects)
            {
                foreach (var component in go.GetComponents<Component>())
                {
                    if (component == null)
                    {
                        // missing monobehaviour??
                        continue;
                    }
                    var serialization = new SerializedObject(component);
                    var propertyIt = serialization.GetIterator();
                    bool firstVisit = true;
                    while (propertyIt.NextVisible(firstVisit))
                    {
                        if (propertyIt.propertyType is SerializedPropertyType.ObjectReference
                            && propertyIt.objectReferenceInstanceIDValue != 0
                            && propertyIt.objectReferenceValue == null)
                        {
                            missingReferences.Add($"property {propertyIt.name} on a {component.GetType().Name} component in GameObject {go.name} has missing reference");
                        }
                        firstVisit = false;
                    }
                }
            }
            
            Assert.That(missingReferences, Is.Empty);
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