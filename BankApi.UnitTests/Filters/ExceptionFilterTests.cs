using AutoFixture.Idioms;
using BankApi.Creators;
using BankApi.Domain.Entities;
using BankApi.Domain.Interfaces;
using BankApi.Domain.Models.Requests;
using BankApi.Filters;
using BankApi.UnitTests.AutoData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Filters;
using NSubstitute;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace BankApi.UnitTests.Filters
{
    public class ExceptionFilterTests
    {
        [Theory, AutoNSubstituteData]
        public void Sut_ShouldGuardItsClause(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(ExceptionFilter).GetConstructors());
        }

        [Theory, AutoNSubstituteData]
        public async Task OnExceptionAsync_(
            ExceptionContext context,
            ExceptionFilter sut)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

            await sut.OnExceptionAsync(context);

            context.HttpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
