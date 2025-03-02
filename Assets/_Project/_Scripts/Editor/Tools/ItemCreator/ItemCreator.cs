using System.Reflection;
using Project.InteractableSystem;
using UnityEditor;
using UnityEngine;

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
            
            ItemType();
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
            bool created = false;
            
            created = _isStackable ? CreateStackableItem() : CreateNonStackableItem();
            
            if (created)
            {
                GUILayout.Label("Item created successfully!");
            }
        }

        private bool CreateNonStackableItem()
        {
            NonStackableItemData itemData = ScriptableObject.CreateInstance<NonStackableItemData>();
            return CreateItemCommon(itemData as ItemData);
        }
        
        private bool CreateStackableItem()
        {
            StackableItemData itemData = ScriptableObject.CreateInstance<StackableItemData>();
            itemData.maxStackSize = _maxStackSize;
            
            return CreateItemCommon(itemData as ItemData);
        }
        
        private bool CreateItemCommon(ItemData itemData)
        {
            var itemGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var itemComponent = itemGameObject.AddComponent<Item>();
            
            itemData.name = _itemName;
            itemData.sprite = _itemSprite;
            itemData.prefab = itemGameObject;

            AssetDatabase.CreateFolder(GetTypeSubFolder(_typeSelectionIndex), _itemName.Trim(' ').Trim());
            string newSubFolder = GetTypeSubFolder(_typeSelectionIndex) + "/" + _itemName.Trim(' ').Trim() + "/";

            // Save Item Data
            AssetDatabase.CreateAsset(itemData, newSubFolder + _itemName + ".asset");
            
            var itemDataField = typeof(Item).GetField("itemData", BindingFlags.NonPublic | BindingFlags.Instance);
            if (itemDataField == null)
            {
                Debug.LogError("Could not find itemData field in Item.cs");
                return false;
            }

            itemDataField.SetValue(itemComponent, itemData);
            
            PrefabUtility.SaveAsPrefabAsset(itemGameObject, newSubFolder + _itemName + ".prefab");
            AssetDatabase.SaveAssets();
            
            DestroyImmediate(itemGameObject);
            
            return true;
        }

        private string GetTypeSubFolder(int index)
        {
            string[] itemTypePaths = AssetDatabase.GetSubFolders(ItemPath);
            return itemTypePaths[index];
        }
    }
}