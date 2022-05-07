using AutoFixture.Idioms;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;
using BankApi.Processors;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Processors
{
    public class TransferProcessorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(TransferProcessor).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            TransferProcessor sut)
            => sut.Should().BeAssignableTo<IEventProcessor>();

        [Theory, AutoNSubstituteData]
        public async Task Process_ShouldReturnResultModel(
            EventRequest request,
            EventResonse response,
            TransferProcessor sut)
        {
            sut.TransferProcssor.Process(Arg.Any<EventRequest>()).Returns(response);

            var actual = await sut.Process(request);

            actual.Should().Be(response);

            await sut.TransferProcssor.Received().Process(request);
        }
    }
}
