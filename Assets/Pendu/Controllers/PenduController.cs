using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

namespace Pendu
{
    /// <summary>
    /// Controlleur pour la classe Pendu, permettant au jeu de fonctionner.
    /// </summary>
    public class PenduController : MonoBehaviour
    {
        [Tooltip("L'objet MotSecretController gérant l'affichage du mot à trouver.")]
        public MotSecretController motSecret;

        [Tooltip("L'objet ViesController gérant l'affichage des vies du joueur.")]
        public ViesController vies;

        [Tooltip("L'objet contenant l'écran de victoire.")]
        public GameObject canvasVictoire;

        [Tooltip("L'objet contenant l'écran de défaite.")]
        public GameObject canvasDefaite;

        [Tooltip("L'objet contenant l'écran d'erreur de connexion au dictionnaire.")]
        public GameObject canvasErreurDictionnaire;

        [Tooltip("L'objet contenant l'écran d'attente du mot.")]
        public GameObject canvasAttente;

        [Tooltip("L'objet TextMeshPro contenant le texte s'affichant dans l'écran de chargement.")]
        public TMP_Text texteChargement;

        [Tooltip("La liste de boutons formant le clavier.")]
        public Button[] clavier;

        private AudioSource m_audioSource;

        /// <summary>
        /// Liste de mots par défaut pouvant être séléctionnés au hasard lors de la construction de l'objet, dans le cas où l'appel de l'API ne fonctionne pas.
        /// </summary>
        private static String[] motsListe =
        {
            "TEST",
            "VOCABULAIRE",
            "EXAMEN",
            "ANTICIPATION",
            "ORDINATEUR",
            "CONJUGAISON",
            "BATEAU",
            "COORDINATEUR",
            "ASSEMBLER",
            "PERQUISITION",
            "FRACTION",
            "QUESTIONNAIRE",
            "ELECTRIQUE",
            "ACOUSTIQUE",
            "PERMANENCE",
            "ORCHESTRE",
            "CARQUOIS"
        };

        /// <summary>
        /// Mot à deviner dans le cadre du jeu du pendu
        /// </summary>
        private Mot mot;

        private List<Char> characteresUtilises;

        private void Awake()
        {
            m_audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Permet de séléctionner un nouveau mot à partir de la plateforme en ligne Dicolink.com. En cas d'erreur, affiche l'écran d'erreur de connexion demandant de réessayer, ou de choisir un mot dans le dictionnaire hors-ligne.
        /// </summary>
        public void SelectNewMot ()
        {
            characteresUtilises = new List<Char>();
            canvasAttente.SetActive(true);
            foreach (Button button in clavier)
            {
                button.interactable = true;
            }
            vies.ResetVies();

            try
            {
                StartCoroutine(GetMotOnline());
            }
            catch (Exception e)
            {
                canvasAttente.SetActive(false);
                canvasErreurDictionnaire.SetActive(true);
            }
        }

        /// <summary>
        /// Renvoie le mot sélectionné au début de la partie.
        /// </summary>
        /// <returns>Le mot attribué au controlleur lors de son acquisition.</returns>
        public String GetMot() { return mot.mot; }

        /// <summary>
        /// Renvoie le lien vers la définition du mot à trouver.
        /// </summary>
        /// <returns>Lien vers la définition du mot en ligne au format String.</returns>
        public String GetDicoLink() { return mot.dicolinkUrl; }

        /// <summary>
        /// Renvoie la liste de lettres utilisées dans la partie en cours.
        /// </summary>
        /// <returns>La liste de charactères entrés dans la partie en cours. Chaque lettre doit être à la majuscule.</returns>
        public List<Char> GetCharacteresUtilises() { return characteresUtilises; }

        /// <summary>
        /// Méthode permettant d'insérer dans la liste de charactères utilisés un nouveau charactère.
        /// <para>Met aussi à jour l'affichage du mot secret par le biais de son contrôleur, de même pour les vies.</para>
        /// <para>Si la lettre entrée cause une victoire, affiche l'écran de victoire. En cas de défaite, affiche l'écran de défaite.</para>
        /// </summary>
        /// <param name="charactere">Charactère à insérer dans la liste interne. Est contenue dans un objet TextMeshPro. Doit être une lettre en majuscule.</param>
        public void InputNewChar(TextMeshPro charactere)
        {
            characteresUtilises.Add(charactere.GetParsedText()[0]);
            motSecret.UpdateMotAffiche();
            vies.UpdateVies();

            if (GetLettresManquantes() == 0)
            {
                canvasVictoire.SetActive(true);
                m_audioSource.Stop();
            }
            if (GetFauxCharacteresUtilisesCount() == 9)
            {
                canvasDefaite.SetActive(true);
                m_audioSource.Stop();
            }
        }

        /// <summary>
        /// Pour chaque lettre dans la liste de charactères utilisées, vérifie si elle est dans le mot ou non.
        /// </summary>
        /// <returns>Le nombre total de lettres dans la liste n'étant pas dans le mot à trouver.</returns>
        public int GetFauxCharacteresUtilisesCount()
        {
            int fauxCharacteresCount = 0;
            foreach (Char charactere in characteresUtilises)
            {
                if (!mot.mot.Contains(charactere))
                {
                    fauxCharacteresCount++;
                }
            }
            return fauxCharacteresCount;
        }

        /// <summary>
        /// Retourne le nombre de lettres qui restent à découvrir dans le mot à deviner. Si cette valeur vaut 0, alors le mot entier est découvert.
        /// </summary>
        /// <returns>La valeur entière de lettres qui restent à découvrir par le joueur.</returns>
        public int GetLettresManquantes()
        {
            int charManquants = 0;
            foreach (Char charactere in mot.mot)
            {
                if (!characteresUtilises.Contains(charactere))
                {
                    charManquants++;
                }
            }
            return charManquants;
        }

        /// <summary>
        /// Réinitialise l'affichage du jeu directement depuis un écran de fin, en les désactivant, puis en utilisant la méthode pour sélectionner un nouveau mot.
        /// </summary>
        public void Redemarrer()
        {
            canvasVictoire.SetActive(false);
            canvasDefaite.SetActive(false);

            SelectNewMot();
        }

        /// <summary>
        /// Utilise l'API en ligne du site Dicolink pour obtenir un nouveau mot. En cas d'erreur de connexion, envoie une TimeoutException. Uhne fois acquis, le mot est attribué à la variable mot du controlleur.
        /// </summary>
        /// <throws>Envoie une TimeoutException si la requête n'aboutit pas ou si une connection à internet est impossible.</throws>
        /// <returns>Une coroutine accédant à internet.</returns>
        private IEnumerator GetMotOnline()
        {
            texteChargement.SetText("Connection au dictionnaire");
            UnityWebRequest requete = UnityWebRequest.Get("https://api.dicolink.com/v1/mots/motauhasard?avecdef=true&minlong=5&maxlong=-1&verbeconjugue=false&api_key=22sDO2pnFUaE7zgWfTJBYZqN4nl8wAYy");

            yield return requete.SendWebRequest();

            if (requete.result == UnityWebRequest.Result.ProtocolError || requete.result == UnityWebRequest.Result.ConnectionError)
            {
                throw new TimeoutException();
            }
            else
            {
                texteChargement.SetText("Acquisition du mot");
                string json = requete.downloadHandler.text;

                json = json.TrimStart('[');
                json = json.TrimEnd(']');

                mot = Mot.CreateFromJSON(json);
                mot.mot = RetireAccentsMot(mot.mot).ToUpper();
            }
            canvasAttente.SetActive(false);
            motSecret.UpdateMotAffiche();
            m_audioSource.Play();
        }

        /// <summary>
        /// En cas d'échec de connexion à l'API en ligne, permet la sélection d'un mot à découvrir à partir de la bibliothèque interne. Moins de mots disponibles, mais depuis n'importe où.
        /// </summary>
        public void GetMotOffline()
        {
            mot = new Mot();

            System.Random random = new System.Random();
            mot.mot = motsListe[random.Next(motsListe.Length)]; // Sélectionne un mot au hasard dans la liste hors ligne.
            mot.dicolinkUrl = "";
            motSecret.UpdateMotAffiche();
            m_audioSource.Play();
        }

        /// <summary>
        /// Retire les accents français du mot entré en paramètre, et les remplace par leur version sans accents, ex : forêt => foret. Retourne le mot finalisé.
        /// </summary>
        /// <param name="mot">Le mot dont il faut retirer les accents.</param>
        /// <returns>Le mot filtré, avec chaque accent remplacé par la version sans accent.</returns>
        public string RetireAccentsMot(string mot)
        {
            /*string motFiltre = mot.Replace('é', 'e');
            motFiltre = motFiltre.Replace('è', 'e');
            motFiltre = motFiltre.Replace('ê', 'e');
            motFiltre = motFiltre.Replace('ë', 'e');
            motFiltre = motFiltre.Replace('à', 'a');
            motFiltre = motFiltre.Replace('â', 'a');
            motFiltre = motFiltre.Replace('ï', 'i');
            motFiltre = motFiltre.Replace('î', 'i');
            motFiltre = motFiltre.Replace('ô', 'o');
            motFiltre = motFiltre.Replace('ö', 'o');
            motFiltre = motFiltre.Replace('û', 'u');
            motFiltre = motFiltre.Replace('ü', 'u');
            motFiltre = motFiltre.Replace('ù', 'u');
            motFiltre = motFiltre.Replace('ç', 'c');

            return motFiltre;*/

            return mot.Replace('é', 'e').Replace('è', 'e').Replace('ê', 'e').Replace('ë', 'e').Replace('à', 'a').Replace('â', 'a').Replace('ï', 'i').Replace('î', 'i').Replace('ô', 'o').Replace('ö', 'o').Replace('û', 'u').Replace('ü', 'u').Replace('ù', 'u').Replace('ç', 'c');
        }

        /// <summary>
        /// Permet de quitter l'application lorsque cette méthode est appelée.
        /// </summary>
        public void Quitter()
        {
            Application.Quit(0);
        }
    }
}