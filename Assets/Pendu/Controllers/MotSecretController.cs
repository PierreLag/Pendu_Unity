using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Pendu
{
    [RequireComponent(typeof(TextMeshPro))]
    public class MotSecretController : MonoBehaviour
    {
        [Tooltip("L'objet PenduController contenant le mot � trouver et les lettres entr�es.")]
        public PenduController pendu;
        protected TextMeshProUGUI m_textmeshpro;

        void Awake()
        {
            m_textmeshpro = GetComponent<TextMeshProUGUI>();
        }

        /// <summary>
        /// Met � jour l'affichage du mot contenu dans le TextMeshPro. Si la lettre n'a pas �t� d�couverte, affiche un _ � sa place. Sinon, affiche la lettre telle qu'elle.
        /// </summary>
        public void UpdateMotAffiche()
        {
            string mot = pendu.GetMot();
            List<Char> characteresUtilises = pendu.GetCharacteresUtilises();
            string motafficher = "";

            foreach (Char charactere in mot)
            {
                if (characteresUtilises.Contains(charactere))
                {
                    motafficher = motafficher + charactere + " ";
                }
                else
                {
                    motafficher = motafficher + "_ ";
                }
            }
            m_textmeshpro.SetText(motafficher);
        }
    }
}