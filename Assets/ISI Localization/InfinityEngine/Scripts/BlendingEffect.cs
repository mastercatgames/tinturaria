using UnityEngine;
namespace InfinityEngine
{
    [ExecuteInEditMode]
    public class BlendingEffect : MonoBehaviour
    {
        [Range(0, 1)]
        public float Amount = 0.5f;

        /// <summary>
        /// >=1
        /// </summary>
        public float edgeSharpness = 1;

        [Range(0, 1)]
        public float minBlending = .15f;

        [Range(0, 1)]
        public float maxBlending = 1;
        /// <summary>
        /// blends between 2 ways of applying the frost effect: 0=normal blend mode, 1="overlay" blend mode
        /// </summary>
        public float seethroughness = 1;

        /// <summary>
        /// how much the original image is distorted through the frost (value depends on normal map)
        /// </summary>
        public float distortion = 0.2f;

        public Texture2D mainTexture;
        public Texture2D normalTexture;
        public Shader shader;

        private Material material;



        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (mainTexture == null || normalTexture == null || shader == null)
            {
                material = null;
                return;
            }
            if (material == null)
            {
                material = new Material(shader);
                material.SetTexture("_BlendTex", mainTexture);
                material.SetTexture("_BumpMap", normalTexture);
            }

            if (!Application.isPlaying)
            {
                material.SetTexture("_BlendTex", mainTexture);
                material.SetTexture("_BumpMap", normalTexture);
                edgeSharpness = Mathf.Max(1, edgeSharpness);
            }
            material.SetFloat("_BlendAmount", Mathf.Clamp01(Mathf.Clamp01(Amount) * (maxBlending - minBlending) + minBlending));
            material.SetFloat("_EdgeSharpness", edgeSharpness);
            material.SetFloat("_SeeThroughness", seethroughness);
            material.SetFloat("_Distortion", distortion);

            Graphics.Blit(source, destination, material);
        }
    }
}