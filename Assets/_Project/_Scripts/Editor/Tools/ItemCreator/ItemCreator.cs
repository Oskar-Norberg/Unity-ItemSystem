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
        private int _subfolderSelectionIndex;

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
            Debug.Log("item created :3 (not really)");
        }
    }
}