using AutoFixture.Idioms;
using BankApi.Controllers;
using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Domain.Models.Response;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Controllers
{
    public class EventControllerTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
            => assertion.Verify(typeof(EventController).GetConstructors());

        [Theory, AutoNSubstituteData]
        public async Task Post_WhenModelStateIsInvalid_ShouldReturnBadRequest(
            EventRequest request,
            EventController sut)
        {
            sut.ModelState.AddModelError("Test", "ModelError");

            var actual = await sut.Post(request) as BadRequestObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            sut.EventProcessorStrategy.DidNotReceive().GetProcessor(Arg.Any<EventTypeEnum>());
        }

        [Theory, AutoNSubstituteData]
        public async Task Post_WhenIsBadRequestResult_ShouldReturnBadRequest(
            EventRequest request,
            IEventProcessor processor,
            EventResonse response,
            EventController sut)
        {
            request.Type = "deposit";
            response.IsValid = false;
            response.ErroMessage = "teste";
            processor.Process(request).Returns(response);
            sut.EventProcessorStrategy.GetProcessor(Arg.Any<EventTypeEnum>()).Returns(processor);

            var actual = await sut.Post(request) as BadRequestObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            actual.Value.Should().Be(response.ErroMessage);

            sut.EventProcessorStrategy.Received().GetProcessor(EventTypeEnum.Deposit);
        }

        [Theory, AutoNSubstituteData]
        public async Task Post_WhenIsNotFoundResult_ShouldReturnFound(
            EventRequest request,
            IEventProcessor processor,
            EventResonse response,
            EventController sut)
        {
            request.Type = "deposit";
            response.IsValid = false;
            response.ErroMessage = "";
            processor.Process(request).Returns(response);
            sut.EventProcessorStrategy.GetProcessor(Arg.Any<EventTypeEnum>()).Returns(processor);

            var actual = await sut.Post(request) as NotFoundObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            actual.Value.Should().Be(0);

            sut.EventProcessorStrategy.Received().GetProcessor(EventTypeEnum.Deposit);
        }

        [Theory, AutoNSubstituteData]
        public async Task Post_ShouldReturnCreated(
            EventRequest request,
            IEventProcessor processor,
            EventResonse response,
            EventController sut)
        {
            request.Type = "deposit";
            response.IsValid = true;
            response.ErroMessage = "";
            processor.Process(request).Returns(response);
            sut.EventProcessorStrategy.GetProcessor(Arg.Any<EventTypeEnum>()).Returns(processor);

            var actual = await sut.Post(request) as CreatedResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.Created);
            actual.Value.Should().Be(response.ResponseModel);

            sut.EventProcessorStrategy.Received().GetProcessor(EventTypeEnum.Deposit);
        }
    }
}
