using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    public class RectTransformSnapAnchorsToCorners
    {
        private static Rect _anchorRect = new Rect();
        private static RectTransform _currentRectTransform;
        private static RectTransform _parentRectTransform;

        [MenuItem("Tools/UI/Snap Anchors To Corners %&t")] //ctrl + alt + t
        public static void SnapAnchors()
        {
            TryToGetRectTransform();
            if (_currentRectTransform != null && _parentRectTransform != null)
            {
                Undo.RegisterCompleteObjectUndo(_currentRectTransform, string.Empty);
                Snap();
            }
            else
            {
                Debug.LogWarning("The object you're attempting to anchor snap must have a rect transform component on both itself and it's parent.");
            }
        }

        private static void Snap()
        {
            CalculateCurrentWandH();
            CalculateCurrentXandY();
            AnchorsToCorners();

            EditorUtility.SetDirty(_currentRectTransform.gameObject);
        }

        private static void TryToGetRectTransform()
        {
            if (Selection.activeGameObject != null)
            {
                _currentRectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
                if (_currentRectTransform != null && _currentRectTransform.parent != null)
                {
                    _parentRectTransform = _currentRectTransform.parent.gameObject.GetComponent<RectTransform>();
                }
            }
            else
            {
                Debug.LogWarning("You must select a gameobject in order to snap the anchors to the rect transform.");
            }
        }

        private static void CalculateCurrentXandY()
        {
            float pivotX = _anchorRect.width * _currentRectTransform.pivot.x;
            float pivotY = _anchorRect.height * (1 - _currentRectTransform.pivot.y);

            _anchorRect.x = _currentRectTransform.anchorMin.x * _parentRectTransform.rect.width + _currentRectTransform.offsetMin.x + pivotX;
            _anchorRect.y = -(1 - _currentRectTransform.anchorMax.y) * _parentRectTransform.rect.height + _currentRectTransform.offsetMax.y - pivotY + _parentRectTransform.rect.height;
        }

        private static void CalculateCurrentWandH()
        {
            _anchorRect.width = _currentRectTransform.rect.width;
            _anchorRect.height = _currentRectTransform.rect.height;
        }

        private static void AnchorsToCorners()
        {
            float pivotX = _anchorRect.width * _currentRectTransform.pivot.x;
            float pivotY = _anchorRect.height * (1 - _currentRectTransform.pivot.y);

            _currentRectTransform.offsetMin = new Vector2(_anchorRect.x / _currentRectTransform.localScale.x, _anchorRect.y / _currentRectTransform.localScale.y - _anchorRect.height);
            _currentRectTransform.offsetMax = new Vector2(_anchorRect.x / _currentRectTransform.localScale.x + _anchorRect.width, _anchorRect.y / _currentRectTransform.localScale.y);
            _currentRectTransform.anchorMin = new Vector2((_currentRectTransform.offsetMin.x - pivotX) / _parentRectTransform.rect.width * _currentRectTransform.localScale.x,
                (_currentRectTransform.offsetMin.y + pivotY) / _parentRectTransform.rect.height * _currentRectTransform.localScale.y);
            _currentRectTransform.anchorMax = new Vector2((_currentRectTransform.offsetMax.x - pivotX) / _parentRectTransform.rect.width * _currentRectTransform.localScale.x,
                (_currentRectTransform.offsetMax.y + pivotY) / _parentRectTransform.rect.height * _currentRectTransform.localScale.y);
            _currentRectTransform.offsetMin = new Vector2(-_currentRectTransform.pivot.x * _anchorRect.width * (1 - _currentRectTransform.localScale.x),
                -_currentRectTransform.pivot.y * _anchorRect.height * (1 - _currentRectTransform.localScale.y));
            _currentRectTransform.offsetMax = new Vector2((1 - _currentRectTransform.pivot.x) * _anchorRect.width * (1 - _currentRectTransform.localScale.x),
                (1 - _currentRectTransform.pivot.y) * _anchorRect.height * (1 - _currentRectTransform.localScale.y));
        }
    }
}