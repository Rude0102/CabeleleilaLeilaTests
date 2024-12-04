using Moq;
using CabeleleilaLeilaAPI.Service;
using CabeleleilaLeilaAPI.Models;
using CabeleleilaLeilaAPI.Repositories.Interface;

public class AgendamentoServiceTests
{
    [Fact]
    public void AgendarServico_DeveCriarNovoAgendamento()
    {
        // Arrange
        var mockRepo = new Mock<IAgendamentoRepository>();
        var clienteId = 1;
        var servicoIds = new List<int> { 1, 2 };
        var dataHora = DateTime.Now.AddDays(3);

        mockRepo.Setup(repo => repo.ObterServicosPorIds(servicoIds)).Returns(new List<Servico>
{
    new Servico { Id = 1, Nome = "Corte", Duracao = TimeSpan.FromMinutes(30), Preco = 50 }, // Alterado
    new Servico { Id = 2, Nome = "Pintura", Duracao = TimeSpan.FromMinutes(60), Preco = 100 } // Alterado
});


        var service = new AgendamentoService(mockRepo.Object);

        // Act
        var result = service.AgendarServico(clienteId, servicoIds, dataHora);

        // Assert
        Assert.True(result);
        mockRepo.Verify(repo => repo.CriarAgendamento(It.IsAny<Agendamento>()), Times.Once);
    }

    [Fact]
    public void AgendarServico_DeveSugerirConsolidacaoDeAgendamentoNaMesmaSemana()
    {
        // Arrange
        var mockRepo = new Mock<IAgendamentoRepository>();
        var clienteId = 1;
        var servicoIds = new List<int> { 1, 2 };
        var dataHora = DateTime.Now.AddDays(3);

        mockRepo.Setup(repo => repo.ObterAgendamentosPorCliente(clienteId)).Returns(new List<Agendamento>
        {
            new Agendamento { Id = 1, ClienteId = clienteId, DataHora = DateTime.Now.AddDays(2) }
        });

        var service = new AgendamentoService(mockRepo.Object);

        // Act
        var result = service.AgendarServico(clienteId, servicoIds, dataHora);

        // Assert
        Assert.True(result);
        mockRepo.Verify(repo => repo.CriarAgendamento(It.IsAny<Agendamento>()), Times.Once);
    }
}
