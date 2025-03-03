using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Project.InteractableSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace Project.ItemSystem.Editor.Tools
{
    public class ItemCreator : EditorWindow
    {
        // NO TRAILING SLASHES
        private const string ItemPath = "Assets/_Project/Items";
        
        private string _itemName = "Item Name";
        private Sprite _itemSprite;

        private bool _isStackable;
        private int _maxStackSize = 1;

        private int _typeSelectionIndex;
        
        
        private Vector2 _itemSelectionScrollPosition;

        [MenuItem("Tools/Iteam Creator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ItemCreator>("Item Creator");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            
            GetListOfItems();
            GetProperties();
            
            GUILayout.EndHorizontal();
        }

        # region Item List
        
        private void GetListOfItems()
        {
            GUILayout.BeginVertical();
            
            GUILayout.Label("Items", EditorStyles.boldLabel);

            string[] itemFolders = GetItemFolders();

            if (itemFolders.Length == 0)
            {
                GUILayout.Label("No items found in " + ItemPath);
            }
            else
            {
                _itemSelectionScrollPosition = EditorGUILayout.BeginScrollView(_itemSelectionScrollPosition);
                GUILayout.BeginVertical();
                
                foreach (string folder in itemFolders)
                {
                    GUILayout.Label(folder, EditorStyles.label);
                    GUILayout.Label(folder, EditorStyles.label);
                    GUILayout.Label(folder, EditorStyles.label);
                    GUILayout.Label(folder, EditorStyles.label);
                    GUILayout.Label(folder, EditorStyles.label);
                    GUILayout.Label(folder, EditorStyles.label);
                }
                
                GUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }
            
            GUILayout.EndVertical();
        }

        private string[] GetItemFolders()
        {
            List<string> paths = new();

            // Remove Assets/ from the path
            string itemPath = ItemPath.Substring(ItemPath.IndexOf('/') + 1);
            
            string path = Path.Combine(Application.dataPath, itemPath);
        
            string[] typeDirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            
            // Remove the type directories from the list
            List<string> dirsList = new(dirs);
            foreach (string typeDir in typeDirs)
            {
                dirsList.Remove(typeDir);
            }
            dirs = dirsList.ToArray();

            // Convert from full-system paths to unity database Asset relative paths
            for (int i = 0; i < dirs.Length; i++)
            {
                int start = dirs[i].LastIndexOf("Assets");
            
                if (start != -1)
                {
                    dirs[i] = dirs[i].Substring(start);
                }
                else
                {
                    dirs[i] = string.Empty;
                }
            }

            return dirs;
        }
        
        # endregion

        # region Item Creation
        private void GetProperties()
        {
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Item Creator", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _itemName = EditorGUILayout.TextField("Name", _itemName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            _itemSprite = (Sprite) EditorGUILayout.ObjectField("Sprite", _itemSprite, typeof(Sprite), false);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            _isStackable = EditorGUILayout.Toggle("Is Stackable", _isStackable);
            GUILayout.EndHorizontal();

            if (_isStackable)
            {
                GUILayout.BeginHorizontal();
                _maxStackSize = EditorGUILayout.IntField("Max Stack Size", _maxStackSize);
                
                if (_maxStackSize < 1)
                    _maxStackSize = 1;
                GUILayout.EndHorizontal();
            }
            
            ItemType();
            
            if (GUILayout.Button("Create Item"))
            {
                CreateItem();
            }
            
            GUILayout.EndVertical();
        }

        private void ItemType()
        {
            string[] itemTypePaths = AssetDatabase.GetSubFolders(ItemPath);
            
            if (itemTypePaths.Length == 0)
            {
                Debug.LogError("No item types found in " + ItemPath + ", are you sure this folder is set up?");
                return;
            }
            
            string[] pathsToDisplay = new string[itemTypePaths.Length];

            for (int i = 0; i < itemTypePaths.Length; i++)
            {
                int lastSlashIndex = itemTypePaths[i].LastIndexOf('/');
                pathsToDisplay[i] = itemTypePaths[i].Substring(lastSlashIndex + 1);
            }
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("Item Type", EditorStyles.label);
        
            _typeSelectionIndex = EditorGUILayout.Popup(_typeSelectionIndex, pathsToDisplay);
            
            GUILayout.EndHorizontal();
        }

        private void CreateItem()
        {
            string path = string.Empty;
            
            path = _isStackable ? CreateStackableItem() : CreateNonStackableItem();
            
            if (path != string.Empty)
            {
                if (path.EndsWith("/"))
                    path = path.Remove(path.Length - 1);
                
                var folder = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (folder != null)
                {
                    Selection.activeObject = folder;
                    EditorGUIUtility.PingObject(folder);
                }
            }
        }

        private string CreateNonStackableItem()
        {
            NonStackableItemData itemData = ScriptableObject.CreateInstance<NonStackableItemData>();
            return CreateItemCommon(itemData as ItemData);
        }
        
        private string CreateStackableItem()
        {
            StackableItemData itemData = ScriptableObject.CreateInstance<StackableItemData>();
            itemData.maxStackSize = _maxStackSize;
            
            return CreateItemCommon(itemData as ItemData);
        }
        
        // TODO: Clean up on failure
        private string CreateItemCommon(ItemData itemData)
        {
            AssetDatabase.CreateFolder(GetTypeSubFolder(_typeSelectionIndex), _itemName.Trim(' ').Trim());
            string newSubFolder = GetTypeSubFolder(_typeSelectionIndex) + "/" + _itemName.Trim(' ').Trim() + "/";
            
            // Save Item Data
            AssetDatabase.CreateAsset(itemData, newSubFolder + _itemName + ".asset");
            
            var itemGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var itemComponent = itemGameObject.AddComponent<Item>();
            
            var itemDataField = typeof(Item).GetField("itemData", BindingFlags.NonPublic | BindingFlags.Instance);
            if (itemDataField == null)
            {
                Debug.LogError("Could not find itemData field in Item.cs");
                return string.Empty;
            }

            itemDataField.SetValue(itemComponent, itemData);
            
            // Save GameObject as prefab
            var itemPrefab = PrefabUtility.SaveAsPrefabAsset(itemGameObject, newSubFolder + _itemName + ".prefab");
            
            itemData.name = _itemName;
            itemData.sprite = _itemSprite;
            itemData.prefab = itemPrefab;
            
            // Save Changes
            AssetDatabase.SaveAssets();
            
            DestroyImmediate(itemGameObject);

            return newSubFolder;
        }

        private string GetTypeSubFolder(int index)
        {
            string[] itemTypePaths = AssetDatabase.GetSubFolders(ItemPath);
            return itemTypePaths[index];
        }
        
        # endregion
    }
}