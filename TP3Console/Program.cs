using Microsoft.EntityFrameworkCore;
using System.Linq;
using TP3Console.Models.EntityFramework;

namespace TP3Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Add_Film_Drame();
            Console.ReadKey();
        }

        public static void Exo2Q1()
        {
            var ctx = new FilmDBContext();
            foreach (var film in ctx.Films)
            {
                Console.WriteLine(film.ToString());
            }
        }
        //Autre possibilité :
        public static void Exo2Q1Bis()
        {
            var ctx = new FilmDBContext();
            //Pour que cela marche, il faut que la requête envoie les mêmes noms de colonnes que les 
            // classes c#.
            var films = ctx.Films.FromSqlRaw("SELECT * FROM film");
            foreach (var film in films)
            {
                Console.WriteLine(film.ToString());
            }
        }

        public static void Exo2Q2()
        {
            var ctx = new FilmDBContext();
            foreach(var user in ctx.Utilisateurs)
            {
                Console.WriteLine(user.Email);
            }
        }

        public static void Exo2Q3()
        {
            var ctx = new FilmDBContext();
            foreach(var user in ctx.Utilisateurs.OrderBy(u => u.Login))
            {
                Console.WriteLine(user);
            }
        }

        public static void Exo2Q4()
        {
            var ctx = new FilmDBContext();
            Categorie categorieAction = ctx.Categories.First(c => c.Nom == "Action");
            ctx.Entry(categorieAction).Collection(c => c.Films).Load();
            foreach (var film in categorieAction.Films)
            {
                Console.WriteLine($"{film.Nom} ({film.CategorieNavigation.Nom})");
            }
        }

        public static void Exo2Q5()
        {
            var ctx = new FilmDBContext();
            Console.WriteLine($"{ctx.Categories.Count()} catégories dans la base");
        }

        public static void Exo2Q6()
        {
            var ctx = new FilmDBContext();
            Console.WriteLine($"{ctx.Avis.Min(a => a.Note)} est la note la plus basse");
        }

        public static void Exo2Q7()
        {
            var ctx = new FilmDBContext();
            foreach(var film in ctx.Films.Where(f => f.Nom.ToLower().Substring(0, 2) == "le"))
            {
                Console.WriteLine(film);
            }
        }

        public static void Exo2Q8()
        {
            var ctx = new FilmDBContext();
            Film pulp_fiction = ctx.Films.First(f => f.Nom.ToLower() == "pulp fiction");
            ctx.Entry(pulp_fiction).Collection(c => c.Avis).Load();
            Console.WriteLine(pulp_fiction.Avis.Average(a => a.Note) + " Note moyenne du film Pulp Fiction");
        }

        public static void Exo2Q9()
        {
            var ctx = new FilmDBContext();
            Avi avi_max_note = ctx.Avis.First(a => a.Note >= ctx.Avis.Max(a => a.Note));
            ctx.Entry(avi_max_note).Reference(c => c.UtilisateurNavigation).Load();
            Console.WriteLine(avi_max_note.UtilisateurNavigation.Id + " - " + avi_max_note.UtilisateurNavigation.Login);
        }

        public static void Add_User()
        {
            Utilisateur newUser = new Utilisateur();
            newUser.Login = "Dylan";
            newUser.Email = "dylan@gmouiler.com";
            newUser.Pwd = "1234";

            var ctx = new FilmDBContext();
            ctx.Utilisateurs.Add(newUser);

            ctx.SaveChanges();
        }

        public static void Edit_Film()
        {
            var ctx = new FilmDBContext();
            Categorie cate_drame = ctx.Categories.First(c => c.Nom == "Drame");
            Film the_film = ctx.Films.First(f => f.Nom == "L'armee des douze singes");

            the_film.Description = "Une armée de douze singe";
            the_film.Categorie = cate_drame.Id;

            ctx.SaveChanges();
        }

        public static void Delete_Film()
        {
            var ctx = new FilmDBContext();
            Film the_film = ctx.Films.First(f => f.Nom == "L'armee des douze singes");
            ctx.Entry(the_film).Collection(c => c.Avis).Load();

            ctx.Avis.RemoveRange(the_film.Avis);
            ctx.Remove(the_film);

            ctx.SaveChanges();
        }

        public static void Add_Avis()
        {
            var ctx = new FilmDBContext();
            Film titanic = ctx.Films.First(f => f.Nom == "Titanic");
            Utilisateur user = ctx.Utilisateurs.First(u => u.Login == "Dylan");
            List<Avi> newAvis = new List<Avi>();
            Avi avi1 = new Avi();
            avi1.UtilisateurNavigation = user;
            avi1.Avis = "Un peu long mais intriguant";
            avi1.Note = 0.69m;
            avi1.FilmNavigation = titanic;

            ctx.Avis.Add(avi1);

            ctx.SaveChanges();
        }

        public static void Add_Film_Drame()
        {
            var ctx = new FilmDBContext();
            Categorie drame = ctx.Categories.First(c => c.Nom == "Drame");

            Film f1 = new Film();
            f1.Nom = "Drame 1";
            f1.Description = "Un premier drame";
            f1.CategorieNavigation = drame;

            Film f2 = new Film();
            f2.Nom = "Drame 2";
            f2.Description = "Un deuxième drame";
            f2.CategorieNavigation = drame;

            List<Film> films = new List<Film>();
            films.Add(f1);
            films.Add(f2);

            ctx.AddRange(films);

            ctx.SaveChanges();
        }
    }
}