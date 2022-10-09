using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    [InitializeOnLoad]
    internal static class CopyGameObjectNameMultipleMenuItem
    {
        private const string MENU_ITEM_NAME = @"GameObject/Kogane/Copy Name (Multiple)";

        private static bool m_isCopied;

        static CopyGameObjectNameMultipleMenuItem()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            m_isCopied = false;
        }

        [MenuItem( MENU_ITEM_NAME, true )]
        private static bool CanCopy()
        {
            return Selection.gameObjects is { Length: > 0 };
        }

        [MenuItem( MENU_ITEM_NAME, false, 1155841429 )]
        private static void Copy()
        {
            // ゲームオブジェクトを複数選択している状態で MenuItem を実行すると
            // ゲームオブジェクトの数分だけ関数が呼び出されてしまうため
            // 2 回目以降の呼び出しは無視するようにしています
            if ( m_isCopied ) return;
            m_isCopied = true;

            var gameObjects = Selection.gameObjects;

            if ( gameObjects == null || gameObjects.Length <= 0 ) return;

            if ( gameObjects.Length == 1 )
            {
                var name = gameObjects[ 0 ].name;
                EditorGUIUtility.systemCopyBuffer = name;
                var message = $"Copied! `{name}`";
                Debug.Log( message );
                TooltipWindow.Open( "Copied!" );
            }
            else
            {
                var names  = gameObjects.Select( x => x.name );
                var result = string.Join( "\n", names );
                EditorGUIUtility.systemCopyBuffer = result;
                var message = $"Copied!\n```\n{result}\n```";
                Debug.Log( message );
                TooltipWindow.Open( "Copied!" );
            }
        }
    }
}