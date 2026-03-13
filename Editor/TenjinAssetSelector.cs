//
//  Copyright (c) 2022 Tenjin. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Tenjin
{

    class TenjinAssetTree : TreeView
    {
        public TenjinAssetTree(TreeViewState treeViewState)
            : base(treeViewState)
        {
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var selectedAssets = TenjinPackager.LoadManifest();
            var assets = AssetDatabase.FindAssets(null, new[] { "Assets" }).Select(x => AssetDatabase.GUIDToAssetPath(x));
            var root = new TenjinAssetViewItem { selected = false, id = 0, depth = -1, displayName = "Root" };

            var allItems = assets.Select((a, index) =>
            {
                return new TenjinAssetViewItem
                {
                    selected = selectedAssets.Contains(a),
                    id = index,
                    depth = DepthFromPath(a),
                    displayName = a
                } as TreeViewItem;
            }).ToList();

            SetupParentsAndChildrenFromDepths(root, allItems);

            return root;
        }

        public int SelectedItemCount()
        {
            var root = rootItem as TenjinAssetViewItem;
            return root.SelectedCount();
        }

        public IEnumerable<TenjinAssetViewItem> SelectedItems()
        {
            return (rootItem as TenjinAssetViewItem).GetRecusivelySelectedItems();
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Event evt = Event.current;
            extraSpaceBeforeIconAndLabel = 18f;
            TenjinAssetViewItem avi = (TenjinAssetViewItem)args.item;

            Rect toggleRect = args.rowRect;
            toggleRect.x += GetContentIndent(args.item);
            toggleRect.width = 16f;

            //// Ensure row is selected before using the toggle (usability)
            if (evt.type == EventType.MouseDown && toggleRect.Contains(evt.mousePosition))
                SelectionClick(args.item, false);

            EditorGUI.BeginChangeCheck();
            bool selected = EditorGUI.Toggle(toggleRect, avi.selected);
            if (EditorGUI.EndChangeCheck())
            {
                avi.selected = selected;
                SetSelectedRecursively(avi.children, selected);
            }

            // Text
            base.RowGUI(args);
        }

        private void SetSelectedRecursively(List<TreeViewItem> c, bool value)
        {
            if (c == null)
                return;

            var assetViewItems = c.Select(x => x as TenjinAssetViewItem);
            foreach (var child in assetViewItems)
            {
                if (child == null)
                    continue;

                child.selected = value;
                SetSelectedRecursively(child.children, value);
            }
        }

        private int DepthFromPath(string path)
        {
            return path.Split('/').Count();
        }
    }

    class TenjinAssetViewItem : TreeViewItem
    {
        public bool selected { get; set; }

        public int SelectedCount()
        {
            int childrenCount = children == null ? 0 : children.Select(x => (x as TenjinAssetViewItem).SelectedCount()).Sum();
            return (selected ? 1 : 0) + childrenCount;
        }

        public IEnumerable<TenjinAssetViewItem> GetRecusivelySelectedItems()
        {
            if (children == null)
            {
                return selected ? new List<TenjinAssetViewItem>() { this } : new List<TenjinAssetViewItem>();
            }

            var selectedItems = children.Select((arg) => (arg as TenjinAssetViewItem).GetRecusivelySelectedItems())
                .SelectMany(x => x).ToList();

            if (selected)
                selectedItems.Add(this);

            return selectedItems;
        }
    }

    class TenjinAssetWindow : EditorWindow
    {
        [SerializeField] TreeViewState m_TreeViewState;
        TenjinAssetTree m_SimpleTreeView;

        void OnEnable()
        {

            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            m_SimpleTreeView = new TenjinAssetTree(m_TreeViewState);
        }

        void OnGUI()
        {
            m_SimpleTreeView.OnGUI(new Rect(0, 0, position.width, position.height - 16));
            DoToolbar(bottomToolbarRect);
        }

        IEnumerable<string> SelectedAssets()
        {
            return m_SimpleTreeView.SelectedItems().Select(x => x.displayName);
        }
        void DoToolbar(Rect rect)
        {
            GUILayout.BeginArea(rect);

            using (new EditorGUILayout.HorizontalScope())
            {
                var style = "miniButton";
                if (GUILayout.Button("Save Manifest", style))
                {
                    TenjinPackager.SaveManifestFile(SelectedAssets());
                }

                if (GUILayout.Button("Publish Unitypackage", style))
                {
                    TenjinPackager.PublishPackage(SelectedAssets());
                }

                GUILayout.FlexibleSpace();

                var label = $"{m_SimpleTreeView.SelectedItemCount()} files in package";
                GUILayout.Label(label);
            }

            GUILayout.EndArea();
        }

        Rect bottomToolbarRect
        {
            get { return new Rect(20f, position.height - 18f, position.width - 40f, 16f); }
        }


        [MenuItem("Tenjin/Select Assets")]
        static void ShowWindow()
        {
            var window = GetWindow<TenjinAssetWindow>();
            window.titleContent = new GUIContent("Tenjin Unity SDK Files");
            window.Show();
        }
    }
}