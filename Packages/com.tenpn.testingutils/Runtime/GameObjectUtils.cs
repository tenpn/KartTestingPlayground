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
using UnityEngine;
using UnityEngine.Assertions;

namespace TenPN.TestingUtils
{
    public static class GameObjectUtils
    {
        /// <returns>path/to/go based on the scene hierarchy</returns>
        public static string GetHierarchyPath(this GameObject go)
        {
            Assert.IsNotNull(go, "expected valid gamoebject");

            s_pathStack.Clear();
            s_pathStack.Push(go.name);
            while (go.transform.parent != null)
            {
                go = go.transform.parent.gameObject;
                s_pathStack.Push(go.name);
            }

            var path = new System.Text.StringBuilder();
            while (s_pathStack.Count > 0)
            {
                if (path.Length > 0)
                {
                    path.Append("/");
                }

                path.Append(s_pathStack.Pop());
            }

            return path.ToString();
        }

        private static readonly Stack<string> s_pathStack = new Stack<string>();

        /// <returns>as gameobject.GetHierarchyPath, but includes component type on end</returns>
        public static string GetHierarchyPath(this Component comp)
        {
            Assert.IsNotNull(comp, "expected valid component");

            return $"{comp.gameObject.GetHierarchyPath()}/{comp.GetType().Name}";
        }
    }
}