using System;
using System.Collections.Generic;
using System.Reflection;
using Project.InteractableSystem;
using Project.ItemSystem.Components;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

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

        private bool[] _itemComponentList;
        private int _folderSelectionIndex;

        public ItemCreator()
        {
            List<Type> types = GetValidItemComponents();
            _itemComponentList = new bool[types.Count];
        }
        
        [MenuItem("Tools/Iteam Creator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ItemCreator>("Item Creator");
        }

        private void OnGUI()
        {
            GetProperties();
            
            if (GUILayout.Button("Create Item"))
            {
                CreateItem();
            }
        }

        private void GetProperties()
        {
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

            ItemScriptType();
            FolderSelection();
        }

        private void ItemScriptType()
        {
            var types = GetValidItemComponents();
            
            if (types.Count > _itemComponentList.Length)
                _itemComponentList = new bool[types.Count];
            
            GUILayout.BeginVertical();
            GUILayout.Label("Select Components", EditorStyles.label);
            
            for (int i = 0; i < types.Count; i++)
            {
                GUILayout.BeginHorizontal();
                _itemComponentList[i] = GUILayout.Toggle(_itemComponentList[i], types[i].Name);
                GUILayout.EndHorizontal();
            }
            
            GUILayout.EndVertical();
        }

        private void FolderSelection()
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
            GUILayout.Label("Item Folder", EditorStyles.label);
        
            _folderSelectionIndex = EditorGUILayout.Popup(_folderSelectionIndex, pathsToDisplay);
            
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
            AssetDatabase.CreateFolder(GetTypeSubFolder(_folderSelectionIndex), _itemName.Trim(' ').Trim());
            string newSubFolder = GetTypeSubFolder(_folderSelectionIndex) + "/" + _itemName.Trim(' ').Trim() + "/";
            
            // Save Item Data
            AssetDatabase.CreateAsset(itemData, newSubFolder + _itemName + ".asset");
            
            var itemGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            Item itemComponent = itemGameObject.AddComponent<Item>();
            
            var itemDataField = typeof(Item).GetField("itemData", BindingFlags.NonPublic | BindingFlags.Instance);
            if (itemDataField == null)
            {
                Debug.LogError("Could not find itemData field in Item.cs");
                return string.Empty;
            }

            List<Type> validTypes = GetValidItemComponents();
            for (int i = 0; i < validTypes.Count; i++)
            {
                if (_itemComponentList[i])
                    itemGameObject.AddComponent(validTypes[i]);
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

        private List<Type> GetValidItemComponents()
        {
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<ItemComponent>();
            
            List<Type> validTypes = new List<Type>();

            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                
                validTypes.Add(type);
            }

            return validTypes;
        }
    }
}