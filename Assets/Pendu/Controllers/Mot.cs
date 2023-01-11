using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pendu
{
    [System.Serializable]
    public class Mot
    {
        public string mot;
        public string dicolinkUrl;

        /// <summary>
        /// Convertit le texte au format JSON mit en entrée en objet Mot.
        /// </summary>
        /// <param name="json">Le texte au format JSON à convertir.</param>
        /// <returns>L'objet Mot correspondant au JSON entré.</returns>
        public static Mot CreateFromJSON(string json)
        {
            return JsonUtility.FromJson<Mot>(json);
        }
    }
}