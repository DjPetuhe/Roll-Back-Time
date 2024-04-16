using UnityEngine;
using System.Collections.Generic;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;
        private bool shining = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 1);
            shining = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            targetColor = new Color(1, 1, 1, 0);
            shining = false;
        }

        private void Update()
        {
            if (!shining)
                return;

            foreach (var r in runes)
                r.color = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);
        }
    }
}
