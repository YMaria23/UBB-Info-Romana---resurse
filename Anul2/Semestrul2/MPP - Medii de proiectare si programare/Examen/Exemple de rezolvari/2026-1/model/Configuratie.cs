namespace model;

public class Configuratie
{
    private int id;
    private int n;
    private string valPozitii;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public int N
    {
        get => n;
        set => n = value;
    }

    public string ValPozitii
    {
        get => valPozitii;
        set => valPozitii = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Configuratie(int id, int n, string valPozitii)
    {
        this.id = id;
        this.n = n;
        this.valPozitii = valPozitii;
    }
}