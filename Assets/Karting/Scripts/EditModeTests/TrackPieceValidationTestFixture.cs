using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace KartGame.EditModeTests
{
    public struct PrefabTestData
    {
        public GameObject Prefab;
        public override string ToString() => Prefab.name;
    }

    public class TrackPrefabsSource : IEnumerable<PrefabTestData>
    {
        public IEnumerator<PrefabTestData> GetEnumerator()
            => Directory.GetFiles("Assets/Karting/Prefabs/TrackPieces", "*.prefab", SearchOption.TopDirectoryOnly)
                .Distinct()
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Select(prefab => new PrefabTestData { Prefab = prefab })
                .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// validating track prefabs in the TrackPieces folder
    /// </summary>
    [TestFixtureSource(typeof(TrackPrefabsSource))]
    public class TrackPieceValidationTestFixture
    {
        private static readonly int TRACK_LAYER = LayerMask.NameToLayer("Track");
        private GameObject m_prefabToTest;

        public TrackPieceValidationTestFixture(PrefabTestData prefabToTest)
        {
            m_prefabToTest = prefabToTest.Prefab;
        }

        [Test]
        public void Static_IsTrue()
        {
            Assert.That(m_prefabToTest.isStatic, Is.True);
        }

        [Test]
        public void Layer_IsTrackLayer()
        {
            Assert.That(m_prefabToTest.layer, Is.EqualTo(TRACK_LAYER));
        }
    }

}