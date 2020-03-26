using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessingEffects : MonoBehaviour
{
    private static PostProcessingEffects _instance;

    public static PostProcessingEffects Instance { get { return _instance; } }

    private VolumeProfile postProcessing;
    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    [SerializeField] private float vignette_intensity_min = 0.5f;
    [SerializeField] private float vignette_intensity_max = 0.8f;

    [SerializeField] private float vignette_smoothness_min = 0.25f;
    [SerializeField] private float vignette_smoothness_max = 0.5f;

    [SerializeField] private float vignette_duration = 2f;

    [SerializeField] private float ca_intensity_min = 0.25f;
    [SerializeField] private float ca_intensity_max = 1.0f;
    [SerializeField] private float ca_duration = 1.0f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(VolumeProfile));

        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        if (!volumeProfile.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));

        vignette.intensity.min = vignette_intensity_min;
        vignette.intensity.max = vignette_intensity_max;

        vignette.smoothness.min = vignette_smoothness_min;
        vignette.smoothness.max = vignette_smoothness_max;

        chromaticAberration.intensity.min = ca_intensity_min;
        chromaticAberration.intensity.max = ca_intensity_max;
    }

    public void ChromaticAbberation()
    {
        StopCoroutine(AnimateChromaticAbberation());
        StartCoroutine(AnimateChromaticAbberation());
    }

    public void Vignette(bool on)
    {
        StopCoroutine(AnimateVignette(!on));
        StartCoroutine(AnimateVignette(on));
    }

    IEnumerator AnimateChromaticAbberation()
    {
        Debug.Log("Chromatic Abberation Animation");
        float time = 0;

        while (time < ca_duration)
        {
            time += Time.smoothDeltaTime;

            chromaticAberration.intensity.Interp(chromaticAberration.intensity.min, chromaticAberration.intensity.max, time / ca_duration);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(ca_duration);
        time = 0;

        while (time < ca_duration)
        {
            time += Time.smoothDeltaTime;

            chromaticAberration.intensity.Interp(chromaticAberration.intensity.max, chromaticAberration.intensity.min, time / ca_duration);

            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator AnimateVignette(bool on)
    {
        Debug.Log("Vignette Animation");
        float time = 0;

        while(time < vignette_duration)
        {
            time += Time.smoothDeltaTime;

            if(on)
            {
                vignette.intensity.Interp(vignette.intensity.min, vignette.intensity.max, time / vignette_duration);
                vignette.smoothness.Interp(vignette.smoothness.min, vignette.smoothness.max, time / vignette_duration);
            }
            else
            {
                vignette.intensity.Interp(vignette.intensity.max, vignette.intensity.min, time / vignette_duration);
                vignette.smoothness.Interp(vignette.smoothness.max, vignette.smoothness.min, time / vignette_duration);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
