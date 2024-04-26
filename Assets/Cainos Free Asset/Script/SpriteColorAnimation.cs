using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class SpriteColorAnimation : MonoBehaviour
    {
        public Gradient gradient;
        public float time;

        private SpriteRenderer sr;
        private float timer;

        private void Start()
        {
            timer = time * Random.value;
            sr = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!sr)
                return;

            timer += Time.deltaTime;
            if (timer > time)
                timer = 0.0f;

            sr.color = gradient.Evaluate(timer / time);
        }
    }
}
