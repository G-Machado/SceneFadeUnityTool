using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptables/Scene Transition Object")]
public class sceneTransition_object : ScriptableObject
{
    [Space(10)]
    [Header("        SCENE TRANSITION OBJECT")]

    public string NextScene_name;

    [Header("Current Scene FadeOut Effect")]
    public bool fadeOut;
    public float fadeOut_duration;
    public Color fadeOut_color;

    [Header("Next Scene FadeIn Effect")]
    public bool fadeIn;
    public float fadeIn_duration;
    public Color fadeIn_color;

    [Header("Transition images")]
    [Tooltip("Transition's specific images")]
    public Image[] fade_images;

    public void Trigger_transition()
    {
        sceneDirector.Instance.changeScene(this);
    }
}
