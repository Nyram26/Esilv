using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2Projets_Mots_Meles_SION_Martin_SUHIT_Benjamin_TDO
{
    public class Joueur
    {
        private string nom;
        private int score = 0;
        private int chrono = 0;
        private List<string> mots_trouvés = new List<string>();//stock les mots trouvés par le joueur durant le round (réinitialisé à chaque round)
        private string[] total_mots_trouvés = new string[5]; //stock les mots trouvés par le joueur à chaque round
        public Joueur(string nom)
        {
            this.nom = nom;
        }
        public string Nom
        { get { return nom; } }

        public int Score
        { get { return score; } }

        public int Chrono
        { get { return chrono; } }

        public List<string> Mots_trouvés
        { get { return mots_trouvés; } }

        public string[] Total_mots_trouvés
        { get { return total_mots_trouvés; } }

        /// <summary>
        /// permet de réinitialiser la liste de mots trouvés par le joueur
        /// </summary>
        /// <return>void donc pas de return, modifie mots_trouvés</return>
        public void Clear_mots_trouvés()//permet de réinitialiser la liste de mots trouvés
        {
            mots_trouvés.Clear();
        }
        /// <summary>
        /// incrémente le score en fonction du temps restant
        /// </summary>
        /// <param name="val"></param>
        /// <return>void donc pas de return, modifie l'attribut score</return>
        public void Add_Score(int val)
        {
            if (val > 0)
            {
                score += val;
            }
        }

        /// <summary>
        /// ajoute le temps restant au joueur si ce dernier a fini de trouver les mots dans le tableau avant la fin du temps imparti
        /// </summary>
        /// <param name="val">temps restant en secondes après que le joueur ait trouvé tous les mots</param>
        /// <return>void donc pas de return, modifie l'attribut chrono</return>
        public void Add_Chrono(int val)//on ajoute le temps restant à la fin de la session du joueur
        {
            if (val > 0)
            {
                chrono += val;
            }
        }

        /// <summary>
        /// ajoute un mot à la liste de mots du joueur
        /// </summary>
        /// <param name="mot">mot à ajouter dans la liste du joueur</param>
        /// <return>void donc pas de return, modifie l'attribut mots_trouvés</return>
        public void Add_Mots(string mot)
        {
            mots_trouvés.Add(mot);
        }

        /// <summary>
        /// ajoute chaque mot trouvé à liste totale des mots trouvés par le joueur en fonction des difficultés
        /// </summary>
        /// <param name="difficulte"></param>
        /// <return>void donc pas de return, modifie l'attribut total_mots_trouvés</return>
        public void Add_liste_mots(int difficulte)
        {
            for (int i = 0; i < mots_trouvés.Count; i++)
            {
                total_mots_trouvés[difficulte - 1] += " " + mots_trouvés[i];
            }
        }

        /// <summary>
        /// envoie sous forme de string l'ensemble des mots trouvés par le joueur durant la partie (à chaque round)
        /// </summary>
        /// <returns>retourne la chaine de caractère de l'ensemble des mots trouvés par le joueur</returns>
        public string Total_mots_trouvés_ToString()
        {
            string total = "Total des mots trouvés par " + nom + " durant la partie :\n\n";
            for (int i = 1; i < 5; i++)
            {
                for (int j = 0; j < total_mots_trouvés[i].Length; j++)
                {
                    total += "round " + i + 1 + " :" + total_mots_trouvés[i] + "\n";
                }
            }
            return total;
        }

        /// <summary>
        /// vérifie si un mot est déjà dans la liste et renvoie true si c'est le cas
        /// </summary>
        /// <param name="mot">mot dont on souhaite vérifier la présence dans la liste</param>
        /// <returns></returns>
        public bool Deja_dans_liste(string mot) //permet de vérifier si un mot a déjà été trouvé par le joueur ou pas
        {
            bool present = false;
            for (int i = 0; i < mots_trouvés.Count && !present; i++)
            {
                if (mots_trouvés[i] == mot)
                {
                    present = true;
                }
            }
            return present;
        }
        /// <summary>
        /// envoie sous forme de string les informations relatives au joueur à savoir son nom, son score ainsi que les mots trouvés
        /// </summary>
        /// <returns></returns>
        public override string ToString()//affiche nom et score du joueurs + mots trouvés
        {
            string chaine = chaine = "Joueur : " + nom + "\nScore : " + score + "\nMots trouvés : ";
            foreach (string elem in mots_trouvés)
            {
                chaine += elem + " ";
            }
            return chaine;
        }
    }
}
