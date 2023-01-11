using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pendu
{
    public class URLScript : MonoBehaviour
    {
        [Tooltip("L'objet PenduController contenant le mot à trouver et les lettres entrées.")]
        public PenduController pendu;

        /// <summary>
        /// Lorsque cette méthode est appelée dans l'application, elle ouvre sur le navigateur web par défaut de l'ordinateur le lien vers la définition du mot à trouver.
        /// </summary>
        public void AccesURLOnClick()
        {
            Application.OpenURL(pendu.GetDicoLink());
        }
    }
}