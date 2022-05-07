using AutoFixture.Idioms;
using BankApi.Domain.Enums;
using BankApi.Domain.Interfaces;
using BankApi.Processors;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace BankApi.UnitTests.Processors
{
    public class EventProcessorStrategyTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardClauses(
            GuardClauseAssertion assertion)
            => assertion.Verify(typeof(EventProcessorStrategy).GetConstructors());

        [Theory, AutoNSubstituteData]
        public void Sut_ShouldImplementProperlyInterface(
            EventProcessorStrategy sut)
            => sut.Should().BeAssignableTo<IEventProcessorStrategy>();

        [Theory, AutoNSubstituteData]
        public void Get_WhenEventProcessorIsNotFound_ShouldThrowNotImplementedException(
            IEventProcessor eventProcessor)
        {
            var type = EventTypeEnum.Deposit;

            eventProcessor.Type.Returns(EventTypeEnum.Transfer);
            var processors = new List<IEventProcessor>();
            processors.Add(eventProcessor);

            var sut = new EventProcessorStrategy(processors);

            var eventProcessorStrategyException = Assert.Throws<NotImplementedException>(() => sut.GetProcessor(type));
        }

        [Theory]
        [AutoInlineData(EventTypeEnum.Deposit)]
        [AutoInlineData(EventTypeEnum.Withdraw)]
        [AutoInlineData(EventTypeEnum.Transfer)]
        public void Get_WhenProviderProcessorIsImplemented_ShouldReturnCorrectly(
            EventTypeEnum type,
            IEventProcessor eventProcessor)
        {
            eventProcessor.Type.Returns(type);
            var processors = new List<IEventProcessor>();
            processors.Add(eventProcessor);

            var sut = new EventProcessorStrategy(processors);

            var instance = sut.GetProcessor(type);

            instance.Type.Should().Be(type);
        }
    }
}
