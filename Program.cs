using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // ===== 1) Generar datos ficticios =====
        HashSet<string> ciudadanos = GenerarCiudadanos(500);

        var rnd = new Random();

        // 75 vacunados Pfizer y 75 AstraZeneca (pueden cruzarse, por eso existe "ambas dosis")
        HashSet<string> vacunadosPfizer = SeleccionarSubconjuntoAleatorio(ciudadanos, 75, rnd);
        HashSet<string> vacunadosAstra = SeleccionarSubconjuntoAleatorio(ciudadanos, 75, rnd);

        // ===== 2) Operaciones de teoría de conjuntos =====
        // Ambas dosis = intersección
        var ambasDosis = new HashSet<string>(vacunadosPfizer);
        ambasDosis.IntersectWith(vacunadosAstra);                 // P ∩ A

        // Solo Pfizer = diferencia
        var soloPfizer = new HashSet<string>(vacunadosPfizer);
        soloPfizer.ExceptWith(vacunadosAstra);                    // P \ A

        // Solo Astra = diferencia
        var soloAstra = new HashSet<string>(vacunadosAstra);
        soloAstra.ExceptWith(vacunadosPfizer);                    // A \ P

        // Vacunados = unión
        var vacunados = new HashSet<string>(vacunadosPfizer);
        vacunados.UnionWith(vacunadosAstra);                      // P ∪ A

        // No vacunados = diferencia con el universo
        var noVacunados = new HashSet<string>(ciudadanos);
        noVacunados.ExceptWith(vacunados);                        // U \ (P ∪ A)

        // ===== 3) Mostrar resumen en consola =====
        Console.WriteLine("==== Campaña de Vacunación COVID-19 (Datos Ficticios) ====");
        Console.WriteLine($"Total ciudadanos: {ciudadanos.Count}");
        Console.WriteLine($"Vacunados Pfizer: {vacunadosPfizer.Count}");
        Console.WriteLine($"Vacunados AstraZeneca: {vacunadosAstra.Count}");
        Console.WriteLine();
        Console.WriteLine(">> Operaciones de conjuntos");
        Console.WriteLine($"Ambas dosis (Pfizer ∩ AstraZeneca): {ambasDosis.Count}");
        Console.WriteLine($"Solo Pfizer (Pfizer \\ AstraZeneca): {soloPfizer.Count}");
        Console.WriteLine($"Solo AstraZeneca (AstraZeneca \\ Pfizer): {soloAstra.Count}");
        Console.WriteLine($"No vacunados (Ciudadanos \\ (Pfizer ∪ AstraZeneca)): {noVacunados.Count}");
        Console.WriteLine();

        Console.WriteLine("Ejemplo (primeros 10) - No vacunados:");
        Console.WriteLine(string.Join(", ", noVacunados.OrderBy(x => ExtraerNumero(x)).Take(10)) + (noVacunados.Count > 10 ? ", ..." : ""));

        // ===== 4) Guardar listados en archivos (para evidencia) =====
        Directory.CreateDirectory("salida");

        File.WriteAllLines("salida/1_no_vacunados.txt", noVacunados.OrderBy(x => ExtraerNumero(x)));
        File.WriteAllLines("salida/2_ambas_dosis.txt", ambasDosis.OrderBy(x => ExtraerNumero(x)));
        File.WriteAllLines("salida/3_solo_pfizer.txt", soloPfizer.OrderBy(x => ExtraerNumero(x)));
        File.WriteAllLines("salida/4_solo_astrazeneca.txt", soloAstra.OrderBy(x => ExtraerNumero(x)));

        Console.WriteLine("\nListados guardados en la carpeta: salida/");
    }

    static HashSet<string> GenerarCiudadanos(int cantidad)
    {
        var set = new HashSet<string>();
        for (int i = 1; i <= cantidad; i++)
            set.Add($"Ciudadano {i}");
        return set;
    }

    static HashSet<string> SeleccionarSubconjuntoAleatorio(HashSet<string> universo, int cantidad, Random rnd)
    {
        // Tomamos "cantidad" elementos aleatorios sin repetir
        return universo.OrderBy(_ => rnd.Next()).Take(cantidad).ToHashSet();
    }

    static int ExtraerNumero(string ciudadano)
    {
        // "Ciudadano 123" -> 123 (para ordenar bonito)
        var parts = ciudadano.Split(' ');
        return int.TryParse(parts.Last(), out int n) ? n : int.MaxValue;
    }
}