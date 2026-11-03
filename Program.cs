using System.Globalization;

namespace linqgyakorlas
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LINQ gyakorlás");
            /*
            List<int> szamok = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 14, 16, 20 };
            //List<int> eredmeny = new List<int>() ;

            var eredmeny = szamok.Where(szam => szam > 10);

            foreach (int szam in eredmeny)
            {
                Console.WriteLine(szam);
            }

            int[] szamTomb = { 20, 2, 54, 100, 1, 5, 33 };

            Console.WriteLine("---------------");

            var eredmeny2 = szamTomb
                .Where(szam => szam > 10)
                .OrderBy(szam => szam);

            foreach (int szam in eredmeny2)
            {
                Console.WriteLine(szam);
            }

            Console.WriteLine("----------------------");

            var eredmeny3 = from szam in szamTomb
                            where szam > 10
                            orderby szam
                            select szam;

            foreach (int szam in eredmeny3)
            {
                Console.WriteLine(szam);
            }
            */

            List<Diak> diakok = File.ReadAllLines("diakok.csv")
                .Skip(1)
                .Select(sor =>
                {
                    string[] adatok = sor.Split(';');
                    double atlag = double.Parse(adatok[3].Replace(',', '.'), CultureInfo.InvariantCulture);
                    return new Diak(adatok[0], int.Parse(adatok[1]), adatok[2], atlag);
                })
                .ToList();

            var eredmeny = diakok.Where(d => d.Atlag > 4.5 && d.Eletkor > 18).OrderBy(d => d.Nev);

            foreach (Diak diak in eredmeny)
            {
                Console.WriteLine($"{diak.Nev} - {diak.Eletkor} - {diak.Osztaly} - {diak.Atlag}");
            }

            Console.WriteLine("----------------------");
            var eredmeny2 = diakok.Where(d => d.Osztaly == "11A" || d.Osztaly == "13A");

            foreach (Diak diak in eredmeny2)
            {
                Console.WriteLine($"{diak.Nev} - {diak.Osztaly} ");
            }

            Console.WriteLine("----------------------");
            var eredmeny3 = diakok.Where(d => d.Osztaly != "12B");

            foreach (Diak diak in eredmeny3)
            {
                Console.WriteLine($"{diak.Nev} - {diak.Osztaly} ");
            }

            Console.WriteLine("----------------------");
            var e3 = diakok.Select(d => d.Nev);

            foreach (string nev in e3)
            {
                Console.WriteLine($"{nev}");
            }

            Console.WriteLine("----------------------");
            var e4 = diakok.Select(d => d.Eletkor);

            foreach (int eletkor in e4)
            {
                Console.WriteLine($"{eletkor}");
            }

            Console.WriteLine("----------------------");
            var e5 = diakok.Select(d => new
            {
                d.Nev,
                d.Eletkor
            });

            foreach (var diak in e5)
            {
                Console.WriteLine($"{diak.Nev} - {diak.Eletkor}");
            }

            Console.WriteLine("----------------------");
            bool vanejelestanulo = diakok.Any(d => d.Atlag < 4.5);
            Console.WriteLine(vanejelestanulo?"Van jeles tanuló.":"Nincs jeles tanuló.");
            


            Console.WriteLine("----------------------");
            Console.WriteLine("Osztályátlag az egyes osztályokban");

            var osztalyAtlagok = diakok
                .GroupBy(d => d.Osztaly)
                .Select(csoport => new
                {
                    Osztaly = csoport.Key,
                    Atlag = csoport.Average(d => d.Atlag)
                });

            foreach (var osztaly in osztalyAtlagok)
            {
                Console.WriteLine($"{osztaly.Osztaly} - {osztaly.Atlag:F2}");
            }


            Console.WriteLine("----------------------");
            Console.WriteLine("Rangsorold az osztályokat a tanulmányi átlaguk alapján csökkenő sorrendben.");

            var osztalyRangsor = diakok
                .GroupBy(d => d.Osztaly)
                .Select(csoport => new
                {
                    Osztaly = csoport.Key,
                    Atlag = csoport.Average(d => d.Atlag)
                })
                .OrderByDescending(x => x.Atlag);

            foreach (var osztaly in osztalyRangsor)
            {
                Console.WriteLine($"{osztaly.Osztaly} - {osztaly.Atlag:F2}");
            }


            Console.WriteLine("----------------------");
            Console.WriteLine("Ki a legjobb tanuló és mennyi az átlaga az egyes osztályokban?");

            var legjobbTanulok = diakok
                .GroupBy(d => d.Osztaly)
                .SelectMany(csoport =>
                {
                    double legmagasabbAtlag = csoport.Max(d => d.Atlag);
                    return csoport.Where(d => d.Atlag == legmagasabbAtlag);
                });

            foreach (Diak diak in legjobbTanulok)
            {
                Console.WriteLine($"{diak.Osztaly} - {diak.Nev} - {diak.Atlag}");
            }


            Console.WriteLine("----------------------");
            Console.WriteLine("Készíts statisztikát, hogy hány adott életkorú tanuló van.");

            var eletkorStatisztika = diakok
                .GroupBy(d => d.Eletkor)
                .Select(csoport => new
                {
                    Eletkor = csoport.Key,
                    Darab = csoport.Count()
                })
                .OrderBy(x => x.Eletkor);

            foreach (var adat in eletkorStatisztika)
            {
                Console.WriteLine($"{adat.Eletkor} éves - {adat.Darab} tanuló");
            }


            Console.WriteLine("----------------------");
            Console.WriteLine("Minden osztályból keresd meg a legmagasabb átlagú diákot.");

            var legmagasabbAtlagok = diakok
                .GroupBy(d => d.Osztaly)
                .SelectMany(csoport =>
                {
                    double legmagasabbAtlag = csoport.Max(d => d.Atlag);
                    return csoport.Where(d => d.Atlag == legmagasabbAtlag);
                });

            foreach (Diak diak in legmagasabbAtlagok)
            {
                Console.WriteLine($"{diak.Osztaly} - {diak.Nev} - {diak.Atlag}");
            }


            Console.WriteLine("----------------------");
            Console.WriteLine("Van-e olyan diák, akinek az átlaga 2,0 alatti?");

            var gyengeTanulok = diakok.Where(d => d.Atlag < 2.0);

            if (gyengeTanulok.Any())
            {
                foreach (Diak diak in gyengeTanulok)
                {
                    Console.WriteLine(diak.Nev);
                }
            }
            else
            {
                Console.WriteLine("Nincs 2,0 alatti átlagú tanuló.");
            }

            Console.WriteLine("----------------------");
        

        }
    }
}
