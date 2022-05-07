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
    public class TransferProcessExecutorTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(TransferProcessExecutor).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            TransferProcessExecutor sut)
            => sut.Should().BeAssignableTo<ITransferProcessExecutor>();

        [Theory, AutoNSubstituteData]
        public async Task Process_WhenWithDrawProcessorReturnsInValid_ShouldReturnWithInvalidResultModel(
            EventRequest request,
            EventResonse withDrawResponse,
            TransferProcessExecutor sut)
        {
            withDrawResponse.IsValid = false;
            sut.WithDrawProcessor.Process(Arg.Any<EventRequest>()).Returns(withDrawResponse);

            var actual = await sut.Process(request);

            actual.Should().Be(withDrawResponse);

            await sut.WithDrawProcessor.Received().Process(request);
            await sut.DepositProcessor.DidNotReceive().Process(Arg.Any<EventRequest>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Process_DepositProcessorReturnsInValid_ShouldReturnWithInvalidResultModel(
            EventRequest request,
            EventResonse withDrawResponse,
            EventResonse depositResponse,
            TransferProcessExecutor sut)
        {
            withDrawResponse.IsValid = true;
            depositResponse.IsValid = false;
            sut.WithDrawProcessor.Process(Arg.Any<EventRequest>()).Returns(withDrawResponse);
            sut.DepositProcessor.Process(Arg.Any<EventRequest>()).Returns(depositResponse);

            var actual = await sut.Process(request);

            actual.Should().Be(depositResponse);

            await sut.WithDrawProcessor.Received().Process(request);
            await sut.DepositProcessor.Received().Process(request);
        }

        [Theory, AutoNSubstituteData]
        public async Task Process_ShouldReturnWithResultModel(
            EventRequest request,
            EventResonse withDrawResponse,
            EventResonse depositResponse,
            TransferProcessExecutor sut)
        {
            withDrawResponse.IsValid = true;
            depositResponse.IsValid = true;
            sut.WithDrawProcessor.Process(Arg.Any<EventRequest>()).Returns(withDrawResponse);
            sut.DepositProcessor.Process(Arg.Any<EventRequest>()).Returns(depositResponse);

            var actual = await sut.Process(request);

            actual.IsValid.Should().BeTrue();
            actual.ResponseModel.Origin.Should().Be(withDrawResponse.ResponseModel.Origin);
            actual.ResponseModel.Destination.Should().Be(depositResponse.ResponseModel.Destination);

            await sut.WithDrawProcessor.Received().Process(request);
            await sut.DepositProcessor.Received().Process(request);
        }
    }
}
