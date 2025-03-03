using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Project.InteractableSystem;
using UnityEditor;
using UnityEngine;

namespace Project.ItemSystem.Editor.Tools
{
    public class ItemCreator : EditorWindow
    {
        private const string ItemPath = "Assets/_Project/Items";

        private string _itemName = "Item Name";
        private Sprite _itemSprite;
        private bool _isStackable;
        private int _maxStackSize = 1;
        private int _typeSelectionIndex = -1;
        private Vector2 _itemSelectionScrollPosition;
        private string _modifyingPath = string.Empty;

        private bool IsModifying => !string.IsNullOrEmpty(_modifyingPath);

        [MenuItem("Tools/Item Creator")]
        public static void ShowWindow()
        {
            GetWindow<ItemCreator>("Item Creator");
        }

        private void OnGUI()
        {
            if (IsModifying)
                StopModifying();

            GUILayout.BeginHorizontal();
            DrawItemList();
            GUILayout.BeginVertical();
            DrawItemProperties();

            if (IsModifying)
                ModifyItem();
            else
                CreateItem();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void Reset()
        {
            _itemName = "Item Name";
            _itemSprite = null;
            _isStackable = false;
            _maxStackSize = 1;
            _typeSelectionIndex = -1;
            _modifyingPath = string.Empty;
        }

        private void StopModifying()
        {
            if (GUILayout.Button("Stop Modifying"))
                Reset();
        }

        private void DrawItemList()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Items", EditorStyles.boldLabel);

            string[] itemFolders = GetItemFolders();
            if (itemFolders.Length == 0)
            {
                GUILayout.Label($"No items found in {ItemPath}");
            }
            else
            {
                _itemSelectionScrollPosition = EditorGUILayout.BeginScrollView(_itemSelectionScrollPosition);
                foreach (string folder in itemFolders)
                {
                    DrawItemButton(folder);
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private void DrawItemButton(string path)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(path))
            {
                StartModifying(path);
            }

            Sprite sprite = GetSprite(path);
            if (sprite)
            {
                Texture texture = sprite.texture;
                if (texture)
                    GUILayout.Label(texture, GUILayout.Width(50), GUILayout.Height(50));
            }
            GUILayout.EndHorizontal();
        }

        private void StartModifying(string path)
        {
            int lastSlashIndex = path.LastIndexOf('/');
            int lastBackSlashIndex = path.LastIndexOf('\\');
            if (lastBackSlashIndex > lastSlashIndex)
                lastSlashIndex = lastBackSlashIndex;
            
            _modifyingPath = path + path.Substring(lastSlashIndex) + ".asset";
            
            ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(_modifyingPath);

            // get second last folder name
            path = path.Substring(0, lastSlashIndex);
            lastSlashIndex = path.LastIndexOf('/');
            lastBackSlashIndex = path.LastIndexOf('\\');
            if (lastBackSlashIndex > lastSlashIndex)
                lastSlashIndex = lastBackSlashIndex;
            path = path.Substring(lastSlashIndex + 1);
            
            Debug.Log(path);
            _typeSelectionIndex = GetType(path);

            _itemName = itemData.name;
            _itemSprite = itemData.sprite;

            _isStackable = itemData is StackableItemData;

            if (_isStackable)
                _maxStackSize = ((StackableItemData)itemData).maxStackSize;
        }

        private string[] GetItemFolders()
        {
            string itemPath = ItemPath.Substring(ItemPath.IndexOf('/') + 1);
            string path = Path.Combine(Application.dataPath, itemPath);
            string[] typeDirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            string[] dirs = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);

            List<string> dirsList = new(dirs);
            foreach (string typeDir in typeDirs)
            {
                dirsList.Remove(typeDir);
            }

            for (int i = 0; i < dirsList.Count; i++)
            {
                int start = dirsList[i].LastIndexOf("Assets");
                if (start != -1)
                {
                    dirsList[i] = dirsList[i].Substring(start);
                }
                else
                {
                    dirsList[i] = string.Empty;
                }
            }

            return dirsList.ToArray();
        }

        private Sprite GetSprite(string path)
        {
            foreach (string guid in AssetDatabase.FindAssets("t:ScriptableObject", new[] { path }))
            {
                ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid));
                if (itemData)
                    return itemData.sprite;
            }
            return null;
        }

        private void DrawItemProperties()
        {
            GUILayout.Label("Item Creator", EditorStyles.boldLabel);

            _itemName = EditorGUILayout.TextField("Name", _itemName);
            _itemSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", _itemSprite, typeof(Sprite), false);
            _isStackable = EditorGUILayout.Toggle("Is Stackable", _isStackable);

            if (_isStackable)
            {
                _maxStackSize = EditorGUILayout.IntField("Max Stack Size", _maxStackSize);
                if (_maxStackSize < 1) _maxStackSize = 1;
            }

            DrawItemTypePopup();
        }

        private void DrawItemTypePopup()
        {
            string[] itemTypePaths = AssetDatabase.GetSubFolders(ItemPath);
            if (itemTypePaths.Length == 0)
            {
                Debug.LogError($"No item types found in {ItemPath}, are you sure this folder is set up?");
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

        private void ModifyItem()
        {
            if (GUILayout.Button("Modify Item"))
            {
                
            }
        }

        private void CreateItem()
        {
            if (GUILayout.Button("Create Item"))
            {
                string path = _isStackable ? CreateStackableItem() : CreateNonStackableItem();
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.EndsWith("/")) path = path.Remove(path.Length - 1);
                    var folder = AssetDatabase.LoadAssetAtPath<Object>(path);
                    if (folder != null)
                    {
                        Selection.activeObject = folder;
                        EditorGUIUtility.PingObject(folder);
                    }
                }
            }
        }

        private string CreateNonStackableItem()
        {
            NonStackableItemData itemData = ScriptableObject.CreateInstance<NonStackableItemData>();
            return CreateItemCommon(itemData);
        }

        private string CreateStackableItem()
        {
            StackableItemData itemData = ScriptableObject.CreateInstance<StackableItemData>();
            itemData.maxStackSize = _maxStackSize;
            return CreateItemCommon(itemData);
        }

        private string CreateItemCommon(ItemData itemData)
        {
            string newSubFolder = CreateItemFolder();
            if (string.IsNullOrEmpty(newSubFolder)) return string.Empty;

            SaveItemData(itemData, newSubFolder);
            SaveItemPrefab(itemData, newSubFolder);

            AssetDatabase.SaveAssets();
            return newSubFolder;
        }

        private string CreateItemFolder()
        {
            string folderPath = GetTypeSubFolder(_typeSelectionIndex);
            string newSubFolder = $"{folderPath}/{_itemName.Trim()}";
            AssetDatabase.CreateFolder(folderPath, _itemName.Trim());
            return newSubFolder;
        }

        private void SaveItemData(ItemData itemData, string folderPath)
        {
            itemData.name = _itemName;
            itemData.sprite = _itemSprite;
            AssetDatabase.CreateAsset(itemData, $"{folderPath}/{_itemName}.asset");
        }

        private void SaveItemPrefab(ItemData itemData, string folderPath)
        {
            var itemGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var itemComponent = itemGameObject.AddComponent<Item>();
            var itemDataField = typeof(Item).GetField("itemData", BindingFlags.NonPublic | BindingFlags.Instance);
            if (itemDataField == null)
            {
                Debug.LogError("Could not find itemData field in Item.cs");
                return;
            }
            itemDataField.SetValue(itemComponent, itemData);
            itemData.prefab = PrefabUtility.SaveAsPrefabAsset(itemGameObject, $"{folderPath}/{_itemName}.prefab");
            DestroyImmediate(itemGameObject);
        }

        private string GetTypeSubFolder(int index)
        {
            string[] itemTypePaths = AssetDatabase.GetSubFolders(ItemPath);
            return itemTypePaths[index];
        }
        
        private int GetType(string path)
        {
            string[] itemTypePaths = AssetDatabase.GetSubFolders(ItemPath);
            for (int i = 0; i < itemTypePaths.Length; i++)
            {
                // get last folder name
                int lastSlashIndex = itemTypePaths[i].LastIndexOf('/');
                int lastBackSlashIndex = itemTypePaths[i].LastIndexOf('\\');
                if (lastBackSlashIndex > lastSlashIndex)
                    lastSlashIndex = lastBackSlashIndex;
                itemTypePaths[i] = itemTypePaths[i].Substring(lastSlashIndex + 1);
                
                if (itemTypePaths[i] == path)
                    return i;
            }
            return -1;
        }
    }
}