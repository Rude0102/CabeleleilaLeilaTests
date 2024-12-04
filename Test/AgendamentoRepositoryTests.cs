using CabeleleilaLeilaAPI.Repositories;
using CabeleleilaLeilaAPI.Models;
using Microsoft.EntityFrameworkCore;
using CabeleleilaLeilaAPI;


public class AgendamentoRepositoryTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Gera um banco de dados único para cada teste
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void ObterAgendamentosPorCliente_DeveRetornarAgendamentosDoCliente()
    {

        var context = GetInMemoryDbContext();
        var repository = new AgendamentoRepository(context);

        var cliente = new Cliente { Nome = "João", Email = "joao@email.com", Telefone = "123456789" };
        var agendamento = new Agendamento
        {
            Cliente = cliente,
            DataHora = DateTime.Now.AddDays(2),
            Status = "Pendente"
        };
        context.Clientes.Add(cliente);
        context.Agendamentos.Add(agendamento);
        context.SaveChanges();


        var result = repository.ObterAgendamentosPorCliente(cliente.Id);


        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(cliente.Id, result.First().ClienteId);
    }
}
