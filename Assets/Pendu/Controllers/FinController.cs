using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pendu
{
    public class FinController : MonoBehaviour
    {
        [Tooltip("L'objet TextMesh_Pro contenant la description contenant le mot qu'il fallait découvrir.")]
        public TMP_Text description;

        [Tooltip("L'objet PenduController contenant le mot à trouver.")]
        public PenduController pendu;

        [Tooltip("L'objet contenant le bouton liant au dictionnaire en ligne.")]
        public GameObject boutonURL;

        private void OnEnable()
        {
            description.SetText("Le mot a trouver etait :" + '\n' + pendu.GetMot());    // Affiche le mot qu'il falait découvrir à la fin.
            if (pendu.GetDicoLink() == "")  // Si l'objet Mot a été obtenu à partir de l'API en ligne, la variable dicoLink contient le lien vers la définition.
            {                               // Si l'objet Mot a été obtenu à partir de la bibliothèque hors-ligne, alors cette variable est "vide", et de ce fait le lien sera désactivé.
                boutonURL.SetActive(false);
            }
            else
            {
                boutonURL.SetActive(true);
            }
        }
    }
}