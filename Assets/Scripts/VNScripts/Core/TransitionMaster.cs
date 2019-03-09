using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionMaster : MonoBehaviour {

    public static TransitionMaster instance;

    public RawImage overlayImage;
    public Material transitionMaterialPrefab;

    private void Awake()
    {
        instance = this;
        overlayImage.material = new Material(transitionMaterialPrefab);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowScene(!sceneVisible);
	}

    static bool sceneVisible = true;
    public static void ShowScene(bool show, float speed =1, bool smooth = false, Texture2D transitionEffect = null)
    {
        if (transitioningOverlay != null)
            instance.StopCoroutine(transitioningOverlay);

        sceneVisible = show;

        if (transitionEffect != null)
            instance.overlayImage.material.SetTexture("_AlphaTex", transitionEffect);

        transitioningOverlay = instance.StartCoroutine(TransitioningOverlay(show, speed, smooth));
    }

    static Coroutine transitioningOverlay = null;
    static IEnumerator TransitioningOverlay(bool show, float speed, bool smooth)
    {
        float targVal = show ? 1 : 0;
        float curVal = instance.overlayImage.material.GetFloat("_Cutoff");

        while(curVal != targVal) {
            curVal = smooth ? Mathf.Lerp(curVal, targVal, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, targVal, speed * Time.deltaTime);
            instance.overlayImage.material.SetFloat("_Cutoff", curVal);
            yield return new WaitForEndOfFrame();
        }
        transitioningOverlay = null;
    }


        //Transition directly to images for layers

        public static void TransitionLayer(BCFC.LAYER layer, Texture2D targetImage, Texture2D transitionEffect, float speed = 1, bool smooth = false)
    {
        if (layer.specialTransitionCoroutine != null)
            instance.StopCoroutine(layer.specialTransitionCoroutine);

        if (targetImage != null)
            layer.specialTransitionCoroutine = instance.StartCoroutine(TransitioningLayer(layer, targetImage, transitionEffect, speed, smooth));
        else
            layer.specialTransitionCoroutine = instance.StartCoroutine(TransitioningLayerToNull(layer, transitionEffect, speed, smooth));

    }

    private static IEnumerator TransitioningLayer(BCFC.LAYER layer, Texture2D targetTex, Texture2D transitionEffect, float speed, bool smooth)
    {
        GameObject ob = Instantiate(layer.newImageObjectReference, layer.newImageObjectReference.transform.parent) as GameObject;
        ob.SetActive(true);

        RawImage im = ob.GetComponent<RawImage>();
        im.texture = targetTex;

        layer.activeImage = im;
        layer.allImages.Add(im);

        im.material = new Material(instance.transitionMaterialPrefab);
        im.material.SetTexture("_AlphaTex", transitionEffect);
        im.material.SetFloat("_Cutoff", 1);
        float curVal = 1;

        while(curVal > 0) {
            curVal = smooth ? Mathf.Lerp(curVal, 0, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, 0, speed * Time.deltaTime);
            im.material.SetFloat("_Cutoff", curVal);
            yield return new WaitForEndOfFrame();
        }

        //remove material -> use alpha for transitions
        //check for null if we rapidly progress through fading and transition overlaps
        if (im != null)
        {
            im.material = null;
            //transition does not use alpha, so alpha up
            im.color = GlobalF.SetAlpha(im.color, 1);
        }

        //remove all other images on layer
        for (int i = layer.allImages.Count -1; i >= 0; i--)
        {
            if (layer.allImages[i] == layer.activeImage && layer.activeImage != null)
                continue;
            if (layer.allImages[i] != null)
                
                Destroy(layer.allImages[i].gameObject, 0.01f);

            layer.allImages.RemoveAt(i);

        }

        layer.specialTransitionCoroutine = null;
    }
    static IEnumerator TransitioningLayerToNull(BCFC.LAYER layer, Texture2D transitionEffect, float speed, bool smooth)
    {
        List<RawImage> currentImagesOnLayer = new List<RawImage>();

        foreach(RawImage r in layer.allImages)
        {
            r.material = new Material(instance.transitionMaterialPrefab);
            r.material.SetTexture("_AlphaTex", transitionEffect);
            r.material.SetFloat("_Cutoff", 0);
            currentImagesOnLayer.Add(r);

        }

        float curVal = 0;
        while(curVal < 1)
        {
            curVal = smooth ? Mathf.Lerp(curVal, 1, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, 1, speed * Time.deltaTime);
            for(int i = 0; i < layer.allImages.Count;i++)
            {
                layer.allImages[i].material.SetFloat("_Cutoff", curVal);
            }
            yield return new WaitForEndOfFrame();
        }
        foreach (RawImage r in currentImagesOnLayer)
        {
            layer.allImages.Remove(r);
            if (r != null)
                Destroy(r.gameObject, 0.01f);
        }

        layer.specialTransitionCoroutine = null;
    }
    

    

}
