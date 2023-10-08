using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2Projets_Mots_Meles_SION_Martin_SUHIT_Benjamin_TDO
{
    public class Jeu
    {
        private Joueur[] joueurs;
        private Dictionnaire dictionnaire;
        //private DateTime temps_imparti; //faire gaffe lors de l'affichage etc tkt
        private int temps_imparti;
        private int difficulte = 1;
        private int type_plateau;
        public Jeu(int type_plateau, string langue, int temps_imparti, int nb_joueurs)
        {
            this.type_plateau = type_plateau;
            dictionnaire = new Dictionnaire(langue);
            //this.temps_imparti = new DateTime(0, 0, 0, 0, temps_imparti / 60, temps_imparti % 60);//seules les minutes et les secondes nous intéressent dans le contexte du jeu
            this.temps_imparti = temps_imparti;
            SaisirJoueurs(nb_joueurs);
        }
        public Joueur[] Joueurs
        {
            get { return joueurs; }
        }
        public int Difficulte
        {
            get { return difficulte; }
        }

        /// <summary>
        /// permet d'initialiser chaque joueur de la partie. les joueurs sont stockés dans le tableau de Joueur "joueurs"
        /// </summary>
        /// <param name="nb_joueurs">le nombre de joueurs durant cette partie</param>
        public void SaisirJoueurs(int nb_joueurs) //permet d'intialiser les joueurs
        {
            joueurs = new Joueur[nb_joueurs];
            for (int i = 0; i < nb_joueurs; i++)
            {
                Console.Write("Saisir le nom du joueur " + (i + 1) + " : ");
                joueurs[i] = new Joueur(Console.ReadLine());
                Console.WriteLine();

            }
        }
        /// <summary>
        /// renvoie un string de présentation du jeu et des conditions de ce dernier à savoir le nom de chaque joueur, les règles et le temps imparti
        /// </summary>
        /// <returns>blabla</returns>
        public string Demarrage()
        {
            string demarrage = "Le jeu peut commencer !\nLes joueurs sont :";
            for (int i = 0; i < joueurs.Length; i++)
            {
                demarrage += " " + joueurs[i].Nom;
            }
            return demarrage += "\n\nRappel des règles :\n\nVous avez " + temps_imparti / 60 + " minute(s) et " + temps_imparti % 60 + " seconde(s) pour trouver les mots de chaque grille !\nAttention la difficulté augmente à chaque round (et le score par mot aussi) !\nSi vous trouvez l'ensemble des mots avant la fin du temps imparti un bonus de points vous est accordé :)\nLe joueur avec le meilleur score remporte la partie\n";
        }

        /// <summary>
        /// réinitialise la liste de mots trouvés de chaque joueur
        /// </summary>
        public void Clear_mot_trouvés_all_joueurs()//permet de réinitialiser la liste de mots trouvés de chaque joueur pour le prochain round
        {
            for (int i = 0; i < joueurs.Length; i++)
            {
                joueurs[i].Clear_mots_trouvés();
            }
        }
        /// <summary>
        /// détermine le gagnant de la session en se basant sur le score de chaque joueur ainsi que le temps restant en cas d'égalité
        /// </summary>
        /// <returns></returns>
        public Joueur Gagnant() //renvoie le nom du gagnant et son score sous la forme d'un tableau
        {
            Joueur joueur_gagnant = joueurs[0];
            for (int i = 1; i < joueurs.Length; i++)
            {
                if (joueurs[i].Score > joueur_gagnant.Score)//on compare le score de chaque joueur pour déterminer le gagnant
                {
                    joueur_gagnant = joueurs[i];
                }
                else
                {
                    if (joueurs[i].Score == joueur_gagnant.Score && joueurs[i].Chrono > joueur_gagnant.Chrono) //si les deux joueurs ont le même score, on regarde lequel a le mieux performé du point de vue du temps
                    {
                        joueur_gagnant = joueurs[i];
                    }
                }
            }
            return joueur_gagnant;
        }

        /// <summary>
        /// renvoie true si la direction saisie par le joueur est valide càd si elle est écrite dans le bon format : N, S, E, O, NE...
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool direction_valide(string direction)
        {
            bool valide = true;
            if(direction != "N" && direction != "S" && direction != "E" && direction != "O" && direction != "NE" && direction != "NO" && direction != "SE" && direction != "SO")
            {
                valide = false;
            }
            return valide;
        }
        /// <summary>
        /// Il s'agit de la fonction principale de la classe Jeu. Elle gère la succession de chaque manche (1 à 5) de la partie, le passage de chaque joueur, la création de plateaux et annonce le gagnant à la fin de la partie.
        /// </summary>
        /// <returns></returns>
        public bool Cycle_jeu()
        {
            bool stop_principal = false;
            TimeSpan time;
            DateTime start_time;
            bool stop;
            string mot;
            int x;
            int y;
            string direction;
            Console.WriteLine(Demarrage());
            Console.WriteLine("*Taper n'importe quelle touche pour continuer*");
            Console.ReadKey();
            while (difficulte < 6 && !stop_principal)
            {
                for (int i = 0; i < joueurs.Length && !stop_principal; i++)
                {
                    stop = false;
                    Console.Clear();
                    Console.WriteLine("*Manche " + difficulte + "*\n\nC'est au tour de " + joueurs[i].Nom + " !\n\n*Appuyer sur n'importe quelle touche pour lancer la partie*");
                    Console.ReadKey();
                    Plateau plateau = new Plateau(type_plateau, difficulte, dictionnaire);
                    start_time = DateTime.Now;//on sauvegarde la date au moment où le joueur lance la partie
                    do
                    {
                        Console.Clear();
                        time = DateTime.Now - start_time;//début du chronomètre (mis à jour à chaque itération de boucle)
                        if(((double)(temps_imparti) - time.TotalSeconds) > 0.0)
                        {
                            Console.WriteLine("C'est parti ! Il vous reste " + ((double)(temps_imparti) - time.TotalSeconds) + " seconde(s) pour trouver les mots cachés !");
                        }
                        else
                        {
                            Console.WriteLine("Dernière chance !");
                        }
                        Console.WriteLine(plateau.ToString() + "\n" + joueurs[i].ToString());
                        do
                        {
                            Console.Write("\nSaisir mot >");
                            mot = Console.ReadLine().ToUpper().Trim();
                        }
                        while (mot == "");
                        do
                        {
                            Console.Write("Saisir la ligne >");//voir avec les \r
                            x = Convert.ToInt32(Console.ReadLine());
                        } while (x < 0);
                        do
                        {
                            Console.Write("Saisir la colonne >");
                            y = Convert.ToInt32(Console.ReadLine());
                        } while (y < 0);
                        do
                        {
                            Console.Write("Saisir la direction du mot (N, S, E, O, NE, NO, SE, SO) >");
                            direction = Console.ReadLine().ToUpper().Trim();
                        }
                        while(!direction_valide(direction));

                        if(plateau.Test_plateau(mot, x, y, direction))
                        {
                            if (!joueurs[i].Deja_dans_liste(mot))
                            {
                                joueurs[i].Add_Mots(mot);//on ajoute le mot à la liste des mots trouvés par le joueur
                                joueurs[i].Add_Score(mot.Length * difficulte * 100);//ajoute la longueur du mot au score du joueur
                                plateau.Remove_mot_from_mots_à_trouver(mot);//on "enlève" le mot à trouver de la liste des mots à trouver;
                            }
                            /*else
                            {
                                Console.WriteLine("Vous avez déjà trouvé ce mot !");
                            }*/
                        }
                        /*else
                        {
                            Console.WriteLine("Ce mot ne se trouve pas dans cette position dans la grille ou n'existe pas !");
                        }*/
                        if (joueurs[i].Mots_trouvés.Count == plateau.Mots_à_trouver.Length)
                        {
                            stop = true;
                        }
                        //time = DateTime.Now - start_time;
                    }
                    while (((double)(temps_imparti) - time.TotalSeconds) > 0.0 && !stop);

                    if(stop)//si stop est true cela veut dire que le joueur a trouvé tous les mots avant la fin du temps imparti et donc on ajoute un bonus au score du joueur
                    {
                        joueurs[i].Add_Chrono(temps_imparti - (int)(time.TotalSeconds));
                        joueurs[i].Add_Score(100 * difficulte);//ajout du bonus
                        Console.WriteLine("Tous les mots ont été trouvés avant la fin du temps imparti -> bonus de " + difficulte * 100 + "points accordés !");
                    }
                    joueurs[i].Add_liste_mots(difficulte);//on ajoute la liste de mots de trouvés de cette manche dans le tableau de mots trouvés du joueur
                }
                Clear_mot_trouvés_all_joueurs();//on réinitialise la liste de mots trouvés de chaque joueur
                difficulte++;//on incrémente la difficulté
            }
            Console.Clear();
            Joueur gagnant = Gagnant();
            Console.WriteLine("Le joueur gagnant est " + gagnant.Nom + " avec un score total de " + gagnant.Score + " !\n" + gagnant.Total_mots_trouvés_ToString() + "\n\n*Tapper n'importe quelle touche pour retourner au menu principal*");
            Console.ReadKey();
            return stop_principal;
        }
    }
}
