namespace FoodEvents.Biblioteca;

public class ResultadoValidacion
{
    public bool EsValido => Errores.Count == 0;
    public List<string> Errores { get; } = new();
}
