using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneDirector : MonoBehaviour
{
    public static sceneDirector Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public sceneTransition_object currentScene;
    private sceneTransition_object newScene;

    private float fadeStart_time;
    private float fadeDuration;
    private bool fadedIn = false;

    [Space(5)]
    public Canvas directorCanvas;
    public Image fadeImage;
    public GameObject TransitionsImages;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (newScene != null)
        {
            currentScene = newScene;
            newScene = null;
        }

        if (currentScene != null)
        {
            fadeStart_time = Time.time;
            fadeDuration = currentScene.fadeIn_duration;
            fadeImage.gameObject.SetActive(true);
            fadedIn = false;
        }
        else
            fadedIn = true;

        directorCanvas.worldCamera = Camera.main;
    }

    public void changeScene(sceneTransition_object scene)
    {
        if (scene == currentScene) return;

        fadeStart_time = Time.time;
        fadeDuration = scene.fadeOut_duration;

        fadeImage.gameObject.SetActive(true);
        newScene = scene;

        CreateFadeImages(scene);

    }

    public void FixedUpdate()
    {
        if(!fadedIn)
            FadeIn();
        else
            FadeOut();
    }

    private void FadeIn()
    {
        if (currentScene)
        {
            if (currentScene.fadeIn)
            {
                if (Time.time - fadeStart_time > fadeDuration) {

                    DestroyFadeImages(currentScene);
                    
                    fadeImage.gameObject.SetActive(false);
                    fadedIn = true;
                }
                else
                {
                    float alpha = Mathf.Lerp(1, 0, (Time.time - fadeStart_time) / fadeDuration);

                    if (fadeImage)
                    {
                        Color new_color = currentScene.fadeIn_color;
                        new_color.a = alpha;

                        fadeImage.color = new_color;

                        Image[] fade_images = fadeImage.GetComponentsInChildren<Image>();
                        for (int i = 1; i < fade_images.Length; i++)
                        {
                            new_color = fade_images[i].color;
                            new_color.a = alpha;

                            fade_images[i].color = new_color;
                        }
                    }
                }
            }
            else
            {
                fadedIn = true;
            }
        }
    }

    private void FadeOut()
    {
        if (newScene && newScene.fadeOut)
        {
            if (Time.time - fadeStart_time > fadeDuration)
            {
                SceneManager.LoadScene(newScene.NextScene_name);
            }        
            else
            {
                float alpha = Mathf.Lerp(0, 1, (Time.time - fadeStart_time) / fadeDuration);

                if (fadeImage)
                {
                    Color new_color = newScene.fadeOut_color;
                    new_color.a = alpha;

                    fadeImage.color = new_color;

                    Image[] fade_images = fadeImage.GetComponentsInChildren<Image>();
                    for (int i = 1; i < fade_images.Length; i++)
                    {
                        new_color = fade_images[i].color;
                        new_color.a = alpha;

                        fade_images[i].color = new_color;
                    }
                }
            }
        }
    }

    private void CreateFadeImages(sceneTransition_object scene)
    {
        for (int i = 0; i < scene.fade_images.Length; i++)
        {
            if(scene.fade_images[i] != null)
                Instantiate(scene.fade_images[i], TransitionsImages.transform);
        }
    }

    private void DestroyFadeImages(sceneTransition_object scene)
    {
        Image[] images = TransitionsImages.GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++)
        {
            Destroy(images[i].gameObject);
        }
    }
}
