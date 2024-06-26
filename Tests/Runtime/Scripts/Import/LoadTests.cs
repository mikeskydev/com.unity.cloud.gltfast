// SPDX-FileCopyrightText: 2024 Unity Technologies and the glTFast authors
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;
using GLTFast.Logging;

namespace GLTFast.Tests.Import
{
    /// <summary>
    /// Tests all of <see cref="GltfImport"/>'s load methods.
    /// </summary>
    [Category("Import")]
    class LoadTests
    {
        const string k_RelativeUriFilter = @"\/RelativeUri\.gl(b|tf)$";

        [GltfTestCase("glTF-test-models", 2, k_RelativeUriFilter)]
        public IEnumerator LoadString(GltfTestCaseSet testCaseSet, GltfTestCase testCase)
        {
            var go = new GameObject();
            var deferAgent = new UninterruptedDeferAgent();
            var logger = new ConsoleLogger();
            using var gltf = new GltfImport(deferAgent: deferAgent, logger: logger);
            var path = Path.Combine(testCaseSet.RootPath, testCase.relativeUri);
            var task = gltf.Load(path);
            yield return Utils.WaitForTask(task);
            var success = task.Result;
            Assert.IsTrue(success);
            var instantiator = new GameObjectInstantiator(gltf, go.transform, logger);
            task = gltf.InstantiateMainSceneAsync(instantiator);
            yield return Utils.WaitForTask(task);
            success = task.Result;
            Assert.IsTrue(success);
            Object.Destroy(go);
        }

        [GltfTestCase("glTF-test-models", 2, k_RelativeUriFilter)]
        public IEnumerator LoadUri(GltfTestCaseSet testCaseSet, GltfTestCase testCase)
        {
            var go = new GameObject();
            var deferAgent = new UninterruptedDeferAgent();
            var logger = new ConsoleLogger();
            using var gltf = new GltfImport(deferAgent: deferAgent, logger: logger);
            var path = Path.Combine(testCaseSet.RootPath, testCase.relativeUri);
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            var task = gltf.Load(uri);
            yield return Utils.WaitForTask(task);
            var success = task.Result;
            Assert.IsTrue(success);

            task = gltf.InstantiateMainSceneAsync(go.transform);
            yield return Utils.WaitForTask(task);
            success = task.Result;
            Assert.IsTrue(success);

            var firstSceneGameObject = new GameObject("firstScene");
            task = gltf.InstantiateSceneAsync(firstSceneGameObject.transform);
            yield return Utils.WaitForTask(task);
            success = task.Result;
            Assert.IsTrue(success);

            Object.Destroy(go);
        }

        [GltfTestCase("glTF-test-models", 2, k_RelativeUriFilter)]
        public IEnumerator Load(GltfTestCaseSet testCaseSet, GltfTestCase testCase)
        {
            var path = Path.Combine(testCaseSet.RootPath, testCase.relativeUri);
            Debug.Log($"Testing {path}");
            var data = File.ReadAllBytes(path);
            var go = new GameObject();
            var deferAgent = new UninterruptedDeferAgent();
            var logger = new ConsoleLogger();
            using var gltf = new GltfImport(deferAgent: deferAgent, logger: logger);
            var task = gltf.Load(data, new Uri(path));
            yield return Utils.WaitForTask(task);
            var success = task.Result;
            Assert.IsTrue(success);
            var instantiator = new GameObjectInstantiator(gltf, go.transform, logger);
            task = gltf.InstantiateMainSceneAsync(instantiator);
            yield return Utils.WaitForTask(task);
            success = task.Result;
            Assert.IsTrue(success);
            Object.Destroy(go);
        }

        [GltfTestCase("glTF-test-models", 2, k_RelativeUriFilter)]
        public IEnumerator LoadSyncInstantiation(GltfTestCaseSet testCaseSet, GltfTestCase testCase)
        {
            var path = Path.Combine(testCaseSet.RootPath, testCase.relativeUri);
            Debug.Log($"Testing {path}");
            var data = File.ReadAllBytes(path);
            var go = new GameObject();
            var deferAgent = new UninterruptedDeferAgent();
            var logger = new ConsoleLogger();
            using var gltf = new GltfImport(deferAgent: deferAgent, logger: logger);
            var task = gltf.Load(data, new Uri(path));
            yield return Utils.WaitForTask(task);
            var success = task.Result;
            Assert.IsTrue(success);
            var instantiator = new GameObjectInstantiator(gltf, go.transform, logger);
#pragma warning disable CS0618
            success = gltf.InstantiateMainScene(instantiator);
#pragma warning restore CS0618
            Assert.IsTrue(success);
            Object.Destroy(go);
        }

        [GltfTestCase("glTF-test-models", 2, k_RelativeUriFilter)]
        public IEnumerator LoadFile(GltfTestCaseSet testCaseSet, GltfTestCase testCase)
        {
            var path = Path.Combine(testCaseSet.RootPath, testCase.relativeUri);
            Debug.Log($"Testing {path}");
            var go = new GameObject();
            var deferAgent = new UninterruptedDeferAgent();
            var logger = new ConsoleLogger();
            using var gltf = new GltfImport(deferAgent: deferAgent, logger: logger);
            var task = gltf.LoadFile(path, new Uri(path));
            yield return Utils.WaitForTask(task);
            var success = task.Result;
            Assert.IsTrue(success);
            var instantiator = new GameObjectInstantiator(gltf, go.transform, logger);
            task = gltf.InstantiateMainSceneAsync(instantiator);
            yield return Utils.WaitForTask(task);
            success = task.Result;
            Assert.IsTrue(success);
            Object.Destroy(go);
        }

        [GltfTestCase("glTF-test-models", 2, k_RelativeUriFilter)]
        public IEnumerator LoadBinary(GltfTestCaseSet testCaseSet, GltfTestCase testCase)
        {
            var path = Path.Combine(testCaseSet.RootPath, testCase.relativeUri);
            if (!path.EndsWith(".glb"))
            {
                Assert.Ignore("Wrong glTF type. Skipping");
            }
            Debug.Log($"Testing {path}");
            var data = File.ReadAllBytes(path);
            var go = new GameObject();
            var deferAgent = new UninterruptedDeferAgent();
            var logger = new ConsoleLogger();
            using var gltf = new GltfImport(deferAgent: deferAgent, logger: logger);
            var task = gltf.LoadGltfBinary(data, new Uri(path));
            yield return Utils.WaitForTask(task);
            var success = task.Result;
            Assert.IsTrue(success);
            var instantiator = new GameObjectInstantiator(gltf, go.transform, logger);
            task = gltf.InstantiateMainSceneAsync(instantiator);
            yield return Utils.WaitForTask(task);
            success = task.Result;
            Assert.IsTrue(success);
            Object.Destroy(go);
        }

        [GltfTestCase("glTF-test-models", 2, k_RelativeUriFilter)]
        public IEnumerator LoadJson(GltfTestCaseSet testCaseSet, GltfTestCase testCase)
        {
            var path = Path.Combine(testCaseSet.RootPath, testCase.relativeUri);
            if (!path.EndsWith(".gltf"))
            {
                Assert.Ignore("Wrong glTF type. Skipping");
            }
            Debug.Log($"Testing {path}");
            var json = File.ReadAllText(path);
            var go = new GameObject();
            var deferAgent = new UninterruptedDeferAgent();
            var logger = new ConsoleLogger();
            using var gltf = new GltfImport(deferAgent: deferAgent, logger: logger);
            var task = gltf.LoadGltfJson(json, new Uri(path));
            yield return Utils.WaitForTask(task);
            var success = task.Result;
            Assert.IsTrue(success);
            var instantiator = new GameObjectInstantiator(gltf, go.transform, logger);
            task = gltf.InstantiateMainSceneAsync(instantiator);
            yield return Utils.WaitForTask(task);
            success = task.Result;
            Assert.IsTrue(success);
            Object.Destroy(go);
        }
    }
}
