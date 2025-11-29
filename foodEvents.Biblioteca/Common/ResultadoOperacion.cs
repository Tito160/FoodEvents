namespace FoodEvents.Biblioteca;

public class ResultadoOperacion<T>
{
    public bool Exito => Errores.Count == 0;
    public T? Valor { get; set; }
    public List<string> Errores { get; } = new();
}
