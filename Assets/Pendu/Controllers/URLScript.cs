using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pendu
{
    public class URLScript : MonoBehaviour
    {
        [Tooltip("L'objet PenduController contenant le mot � trouver et les lettres entr�es.")]
        public PenduController pendu;

        /// <summary>
        /// Lorsque cette m�thode est appel�e dans l'application, elle ouvre sur le navigateur web par d�faut de l'ordinateur le lien vers la d�finition du mot � trouver.
        /// </summary>
        public void AccesURLOnClick()
        {
            Application.OpenURL(pendu.GetDicoLink());
        }
    }
}