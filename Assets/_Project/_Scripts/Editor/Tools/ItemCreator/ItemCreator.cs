using UnityEditor;
using UnityEngine;

namespace Project.ItemSystem.Editor.Tools
{
    public class ItemCreator : EditorWindow
    {
        private string _itemName = "Item Name";
        private Sprite _itemSprite;

        private bool _isStackable;
        private int _maxStackSize = 1;

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
        }

        private void CreateItem()
        {
            Debug.Log("item created :3 (not really)");
        }
    }
}