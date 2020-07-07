using System;
using System.Collections.Generic;
using System.IO;

namespace GestionnaireDeStock
{
    class Article
    {
        private int reference;
        private string name;
        private double price;
        private int quantity;

        public int Reference 
        {
            get { return reference; }

            set { reference = value; } 
        }

        public string Name
        {
            get { return name; }

            set { name = value; }
        }

        public double Price
        {
            get { return price; }

            set { price = value; }
        }

        public int Quantity
        {
            get { return quantity; }

            set { quantity = value; }
        }

        public Article() { }

        public Article(int reference, string name, double price, int quantity)
        {
            this.reference = reference;
            this.name = name;
            this.price = price;
            this.quantity = quantity;
        }

        /// <summary>
        /// Lit et récupère les articles du fichier texte, les stocke dans une liste d'articles. Impose un format de fichier.
        /// </summary>
        /// <param name="path">Chemin du fichier txt passé en paramètre, à renseigner dans la fonction "Main" à l'instantiation du stock</param>
        /// <returns></returns>
        public static List<Article> GetAllCharacteristics(string path)
        {
            List<Article> articles = new List<Article>();

            using (StreamReader file = new StreamReader(path))
            {
                try
                {
                    while (!file.EndOfStream)
                    {
                        var references = file.ReadLine();
                        var names = file.ReadLine();
                        var prices = file.ReadLine();
                        var quantities = file.ReadLine();
                        _ = file.ReadLine();

                        const string prefixNumero = "Numéro: ";
                        string numero = references;

                        const string prefixNom = "Nom: ";
                        string nom = names;

                        const string prefixPrix = "Prix: ";
                        string prix = prices;

                        const string prefixquantité = "Quantité: ";
                        string quantité = quantities;    

                        if (!numero.StartsWith(prefixNumero) || !nom.StartsWith(prefixNom) || !prix.StartsWith(prefixPrix) || !quantité.StartsWith(prefixquantité))
                            throw new FormatException("Format de fichier invalide");
                        else
                        {
                            articles.Add(new Article()
                            {
                                Reference = int.Parse(references.Substring(7)),
                                Name = names.Substring(5),
                                Price = double.Parse(prices.Substring(6)),
                                Quantity = int.Parse(quantities.Substring(10))
                            });
                        } 
                    }
                    return articles;
                }
                catch (Exception except)
                {
                    throw new Exception($"L'erreur suivante est survenue: {except.Message}");
                }
            }
        }

        /// <summary>
        /// Récupère le chemin du fichier et les données d'un article pour les écrire dans le fichier.
        /// </summary>
        /// <param name="articles">Fonction de la classe "Stock" qui permet de rentrer un nouvel article</param>
        /// <param name="path">Chemin du fichier texte à écrire</param>
        public static void WriteAFile(List<Article> articles, string path)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                foreach (var item in articles)
                {
                    file.Write(item.ToString());
                }
            }
        }

        /// <summary>
        /// Patron d'impression du fichier texte: format du texte.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Numéro: {reference}\nNom: {name}\nPrix: {price}\nQuantité: {quantity}\n\n";
        }
    }
}