using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScriptTransparence : MonoBehaviour
{
    public float intervalle = 2f; // Intervalle entre chaque changement
    public float vitesse = 0.5f; // Vitesse de changement de la transparence
    public Image image; // Assurez-vous d'assigner votre image dans l'Inspector
    private Color baseColor;
    private Color transparentColor;

    void Start()
    {
        baseColor = image.color;
        transparentColor = baseColor;
        transparentColor.a = 0;
        float transitionTime = 1f / vitesse;
        
        /*
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(transparentColor, transitionTime));
        sequence.AppendInterval(intervalle);
        sequence.Append(image.DOColor(baseColor, transitionTime));
        sequence.AppendInterval(intervalle);
        sequence.SetLoops(-1);
        sequence.OnStart(delegate { Debug.Log("start sequence"); });
        */

        StartCoroutine(ChangerTransparence());
    }

    IEnumerator ChangerTransparence()
    {
        
        while (true)
        {
            
            
            // Diminuer la transparence
            while (image.color.a > 0f)
            {
                Color nouvelleCouleur = image.color;
                nouvelleCouleur.a -= vitesse * Time.deltaTime;
                image.color = nouvelleCouleur;
                yield return null;
            }

            // Attendre à l'opacité minimale
            yield return new WaitForSeconds(intervalle);

            // Augmenter la transparence
            while (image.color.a < 1f)
            {
                Color nouvelleCouleur = image.color;
                nouvelleCouleur.a += vitesse * Time.deltaTime;
                image.color = nouvelleCouleur;
                yield return null;
            }

            // Attendre à l'opacité maximale
            yield return new WaitForSeconds(intervalle);


        }
    }
}
