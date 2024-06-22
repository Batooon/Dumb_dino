using UnityEngine;

namespace ParallaxBackground
{
    public class BackgroundEntity : MonoBehaviour
    {
        [SerializeField] private bool _overrideDirection;
        [SerializeField, DrawIf(nameof(_overrideDirection), true)] private Vector2 _movementDirection;
        private float _movementSpeed;
    }
}