using Client.Domain;

namespace Client.Tests.Domain;

public class PersonaTests
{
    [Fact]
    public void Constructor_ValidData_CreatesPersona()
    {
        var persona = new Persona("Juan Perez", Genero.Masculino, 30, "1234567890", "Calle 1", "0999999999");

        Assert.Equal("Juan Perez", persona.Nombre);
        Assert.Equal(Genero.Masculino, persona.Genero);
        Assert.Equal(30, persona.Edad);
        Assert.Equal("1234567890", persona.Identificacion);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_InvalidNombre_ThrowsArgumentException(string? nombre)
    {
        Assert.Throws<ArgumentException>(() =>
            new Persona(nombre!, Genero.Masculino, 30, "1234567890", "Calle 1", "0999999999"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_InvalidIdentificacion_ThrowsArgumentException(string? identificacion)
    {
        Assert.Throws<ArgumentException>(() =>
            new Persona("Juan Perez", Genero.Masculino, 30, identificacion!, "Calle 1", "0999999999"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(121)]
    public void Constructor_InvalidEdad_ThrowsArgumentOutOfRangeException(int edad)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Persona("Juan Perez", Genero.Masculino, edad, "1234567890", "Calle 1", "0999999999"));
    }

    [Fact]
    public void UpdateDetails_ValidData_UpdatesFieldsAndKeepsIdentificacion()
    {
        var persona = new Persona("Juan Perez", Genero.Masculino, 30, "1234567890", "Calle 1", "0999999999");

        persona.UpdateDetails("Juan Actualizado", Genero.Otro, 40, "Calle 2", "0988888888");

        Assert.Equal("Juan Actualizado", persona.Nombre);
        Assert.Equal(Genero.Otro, persona.Genero);
        Assert.Equal(40, persona.Edad);
        Assert.Equal("Calle 2", persona.Direccion);
        Assert.Equal("0988888888", persona.Telefono);
        Assert.Equal("1234567890", persona.Identificacion);
    }

    [Fact]
    public void UpdateDetails_InvalidNombre_ThrowsArgumentException()
    {
        var persona = new Persona("Juan Perez", Genero.Masculino, 30, "1234567890", "Calle 1", "0999999999");

        Assert.Throws<ArgumentException>(() =>
            persona.UpdateDetails(string.Empty, Genero.Masculino, 30, "Calle 1", "0999999999"));
    }

    [Fact]
    public void UpdateDetails_InvalidEdad_ThrowsArgumentOutOfRangeException()
    {
        var persona = new Persona("Juan Perez", Genero.Masculino, 30, "1234567890", "Calle 1", "0999999999");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            persona.UpdateDetails("Juan Perez", Genero.Masculino, 200, "Calle 1", "0999999999"));
    }
}
