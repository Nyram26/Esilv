using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2Projets_Mots_Meles_SION_Martin_SUHIT_Benjamin_TDO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool stop_principal = false;
            int rep = 0;
            while (rep != 3 && !stop_principal)
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Menu de démarrage\n\nQue souhaitez vous faire (un entier est attendu) ?\n\n1) Accéder à l'interface de test de méthodes\n2) Lancer le jeu\n3) Quitter le programme\n\n");
                    Console.Write(">");
                    rep = Convert.ToInt32(Console.ReadLine());
                }
                while (rep != 1 && rep != 2 && rep != 3);
                if (rep == 2)//Execution et gestion du jeu
                {
                    Console.Clear();
                    string titre = " _____ ______   ________  _________  ________           _____ ______   _______   ___       _______   ________      \r\n|\\   _ \\  _   \\|\\   __  \\|\\___   ___\\\\   ____\\         |\\   _ \\  _   \\|\\  ___ \\ |\\  \\     |\\  ___ \\ |\\   ____\\     \r\n\\ \\  \\\\\\__\\ \\  \\ \\  \\|\\  \\|___ \\  \\_\\ \\  \\___|_        \\ \\  \\\\\\__\\ \\  \\ \\   __/|\\ \\  \\    \\ \\   __/|\\ \\  \\___|_    \r\n \\ \\  \\\\|__| \\  \\ \\  \\\\\\  \\   \\ \\  \\ \\ \\_____  \\        \\ \\  \\\\|__| \\  \\ \\  \\_|/_\\ \\  \\    \\ \\  \\_|/_\\ \\_____  \\   \r\n  \\ \\  \\    \\ \\  \\ \\  \\\\\\  \\   \\ \\  \\ \\|____|\\  \\        \\ \\  \\    \\ \\  \\ \\  \\_|\\ \\ \\  \\____\\ \\  \\_|\\ \\|____|\\  \\  \r\n   \\ \\__\\    \\ \\__\\ \\_______\\   \\ \\__\\  ____\\_\\  \\        \\ \\__\\    \\ \\__\\ \\_______\\ \\_______\\ \\_______\\____\\_\\  \\ \r\n    \\|__|     \\|__|\\|_______|    \\|__| |\\_________\\        \\|__|     \\|__|\\|_______|\\|_______|\\|_______|\\_________\\\r\n                                       \\|_________|                                                    \\|_________|\r\n                                                                                                                   \r\n                                                                                                                   \r\n\r\n";
                    Console.WriteLine(titre);
                    Console.WriteLine("Par Martin SION & Benjamin SUHIT");
                    Console.WriteLine("\n\n\n\n\n                                   *Taper n'importe quelle touche pour continuer*");
                    Console.ReadKey();
                    int type_plateau;
                    string langue;
                    int temps;
                    int joueurs;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Pour utiliser des plateaux issus de fichiers .csv saisir 1\nPour des plateaux générés aléatoirement saisir 2");
                        type_plateau = Convert.ToInt32(Console.ReadLine());
                    }
                    while (type_plateau != 1 && type_plateau != 2);
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Saisir la langue des mots FR/EN");
                        langue = Console.ReadLine().ToUpper().Trim();
                    }
                    while (langue != "FR" && langue != "EN");
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Saisir la durée du temps imparti (en secondes)");
                        temps = Convert.ToInt32(Console.ReadLine());
                    }
                    while (temps <= 0);
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Saisir le nombre de participants");
                        joueurs = Convert.ToInt32(Console.ReadLine());
                    }
                    while (joueurs <= 0);
                    Jeu jeu = new Jeu(type_plateau, langue, temps, joueurs);
                    Console.Clear();
                    stop_principal = jeu.Cycle_jeu();//
                    Console.WriteLine("Fin du jeu - taper n'importe quelle touche pour sortir de la console -");//sortie de la boucle while et fin du programme
                    Console.ReadKey();
                }
                else
                {
                    if (rep == 1)//Interface de test de méthodes
                    {
                        Console.WriteLine("Bienvenue dans l'interface de test de méthodes");
                        Dictionnaire d = new Dictionnaire("EN");

                        /*Plateau plateau2 = new Plateau(2, 2, d); //1 fichier CSV 2 random                      
                        Plateau plateau3 = new Plateau(2, 3, d);
                        Plateau plateau4 = new Plateau(2, 4, d);
                        Plateau plateau5 = new Plateau(2, 5, d);
                        plateau1.ToFile("EN_Difficulte_1.csv");
                        plateau2.ToFile("EN_Difficulte_2.csv");
                        plateau3.ToFile("EN_Difficulte_3.csv");
                        plateau4.ToFile("EN_Difficulte_4.csv");
                        plateau5.ToFile("EN_Difficulte_5.csv");*/
                        /*Console.WriteLine(d.RechDichoRecursif("you "));
                        char[,] mat = new char[6,8];
                         for(int i = 0; i < 6; i++)
                         {
                            for (int j = 0; j < 8; j++)
                            {
                                mat[i, j] = 'A';
                            }
                         }*/
                        //Console.WriteLine(String(mat));

                        //Console.WriteLine(d.ToString());
                        Plateau plateau1 = new Plateau(1, 1, d);
                        Console.WriteLine("Taper n'importe quelle touche pour retourner au menu principal");
                        Console.ReadKey();
                    }
                }

            }
            Console.Clear();
            Console.WriteLine("fin");
            Console.ReadKey();
        }
    }
}
