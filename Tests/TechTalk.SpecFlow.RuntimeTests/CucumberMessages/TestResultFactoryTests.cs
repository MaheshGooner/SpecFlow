﻿using System;
using FluentAssertions;
using Io.Cucumber.Messages;
using Moq;
using TechTalk.SpecFlow.CommonModels;
using TechTalk.SpecFlow.CucumberMessages;
using TechTalk.SpecFlow.ErrorHandling;
using Xunit;

namespace TechTalk.SpecFlow.RuntimeTests.CucumberMessages
{
    public class TestResultFactoryTests
    {
        [Fact(DisplayName = @"BuildFromScenarioContext should return a failure with an ArgumentNullException when null is passed")]
        public void BuildFromScenarioContext_Null_ShouldReturnFailureWithArgumentNullException()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());

            // ACT
            var actualTestResult = testResultFactory.BuildFromScenarioContext(null);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ExceptionFailure>().Which
                            .Exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact(DisplayName = @"BuildPassedResult should return a TestResult with status Passed")]
        public void BuildPassedResult_ValidParameters_ShouldReturnTestResultWithStatusPassed()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const Status expectedStatus = Status.Passed;

            // ACT
            var actualTestResult = testResultFactory.BuildPassedResult(10Lu);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Status.Should().Be(expectedStatus);
        }

        [Fact(DisplayName = @"BuildPassedResult should return a TestResult with the passed nanoseconds duration")]
        public void BuildPassedResult_Nanoseconds_ShouldReturnTestResultWithCorrectNanoseconds()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const ulong expectedNanoseconds = 15Lu;

            // ACT
            var actualTestResult = testResultFactory.BuildPassedResult(expectedNanoseconds);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.DurationNanoseconds.Should().Be(expectedNanoseconds);
        }

        [Fact(DisplayName = @"BuildPassedResult should return a TestResult with empty message")]
        public void BuildPassedResult_ValidParameters_ShouldReturnTestResultWithEmptyMessage()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const string expectedMessage = "";

            // ACT
            var actualTestResult = testResultFactory.BuildPassedResult(10Lu);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Message.Should().Be(expectedMessage);
        }

        [Fact(DisplayName = @"BuildFailedResult should return a TestResult with status Failed")]
        public void BuildFailedResult_ValidParameters_ShouldReturnTestResultWithStatusFailed()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const Status expectedStatus = Status.Failed;

            // ACT
            var actualTestResult = testResultFactory.BuildFailedResult(10Lu, "Test Message");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Status.Should().Be(expectedStatus);
        }

        [Fact(DisplayName = @"BuildFailedResult should return a TestResult with the passed nanoseconds duration")]
        public void BuildFailedResult_Nanoseconds_ShouldReturnTestResultWithNanoseconds()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const ulong expectedNanoseconds = 15Lu;

            // ACT
            var actualTestResult = testResultFactory.BuildFailedResult(expectedNanoseconds, "Test Message");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.DurationNanoseconds.Should().Be(expectedNanoseconds);
        }

        [Fact(DisplayName = @"BuildFailedResult should return a TestResult with the passed message")]
        public void BuildFailedResult_Message_ShouldReturnTestResultWithMessage()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const string expectedMessage = "This is a test message";

            // ACT
            var actualTestResult = testResultFactory.BuildFailedResult(10Lu, expectedMessage);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Message.Should().Be(expectedMessage);
        }

        [Fact(DisplayName = @"BuildPendingResult should return a TestResult with status Pending")]
        public void BuildPendingMessage_ValidParameters_ShouldReturnTestResultWithStatusPending()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const Status expectedStatus = Status.Pending;

            // ACT
            var actualTestResult = testResultFactory.BuildPendingResult(10Lu, "Pending test");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Status.Should().Be(expectedStatus);
        }

        [Fact(DisplayName = @"BuildPendingResult should return a TestResult with the passed nanoseconds duration")]
        public void BuildPendingResult_Nanoseconds_ShouldReturnTestResultWithNanoseconds()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const ulong expectedNanoseconds = 15Lu;

            // ACT
            var actualTestResult = testResultFactory.BuildPendingResult(expectedNanoseconds, "Pending test");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.DurationNanoseconds.Should().Be(expectedNanoseconds);
        }

        [Fact(DisplayName = @"BuildPendingResult should return a TestResult with the passed message")]
        public void BuildPendingResult_Message_ShouldReturnTestResultWithMessage()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const string expectedMessage = "This is a test message";

            // ACT
            var actualTestResult = testResultFactory.BuildPendingResult(10Lu, expectedMessage);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Message.Should().Be(expectedMessage);
        }

        [Fact(DisplayName = @"BuildAmbiguousResult should return a TestResult with status Ambiguous")]
        public void BuildAmbiguousResult_ValidParameters_ShouldReturnTestResultWithStatusAmbiguous()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const Status expectedStatus = Status.Ambiguous;

            // ACT
            var actualTestResult = testResultFactory.BuildAmbiguousResult(10Lu, "Ambiguous test");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Status.Should().Be(expectedStatus);
        }

        [Fact(DisplayName = @"BuildAmbiguousResult should return a TestResult with the passed nanoseconds duration")]
        public void BuildAmbiguousResult_Nanoseconds_ShouldReturnTestResultWithNanoseconds()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const ulong expectedNanoseconds = 15Lu;

            // ACT
            var actualTestResult = testResultFactory.BuildAmbiguousResult(expectedNanoseconds, "Ambiguous test");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.DurationNanoseconds.Should().Be(expectedNanoseconds);
        }

        [Fact(DisplayName = @"BuildAmbiguousResult should return a TestResult with the passed message")]
        public void BuildAmbiguousResult_Message_ShouldReturnTestResultWithMessage()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const string expectedMessage = "This is a test message";

            // ACT
            var actualTestResult = testResultFactory.BuildAmbiguousResult(10Lu, expectedMessage);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Message.Should().Be(expectedMessage);
        }

        [Fact(DisplayName = @"BuildUndefinedResult should return a TestResult with status Undefined")]
        public void BuildUndefinedResult_ValidParameters_ShouldReturnTestResultWithStatusUndefined()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const Status expectedStatus = Status.Undefined;

            // ACT
            var actualTestResult = testResultFactory.BuildUndefinedResult(10Lu, "Undefined test");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Status.Should().Be(expectedStatus);
        }

        [Fact(DisplayName = @"BuildUndefinedResult should return a TestResult with the passed nanoseconds duration")]
        public void BuildUndefinedResult_Nanoseconds_ShouldReturnTestResultWithNanoseconds()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const ulong expectedNanoseconds = 15Lu;

            // ACT
            var actualTestResult = testResultFactory.BuildUndefinedResult(expectedNanoseconds, "Undefined test");

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.DurationNanoseconds.Should().Be(expectedNanoseconds);
        }

        [Fact(DisplayName = @"BuildUndefinedResult should return a TestResult with the passed message")]
        public void BuildUndefinedResult_Message_ShouldReturnTestResultWithMessage()
        {
            // ARRANGE
            var errorProviderMock = GetErrorProviderMock();

            var testResultFactory = new TestResultFactory(new TestErrorMessageFactory(), new TestPendingMessageFactory(errorProviderMock.Object), new TestAmbiguousMessageFactory());
            const string expectedMessage = "This is a test message";

            // ACT
            var actualTestResult = testResultFactory.BuildUndefinedResult(10Lu, expectedMessage);

            // ASSERT
            actualTestResult.Should().BeAssignableTo<ISuccess<TestResult>>().Which
                            .Result.Message.Should().Be(expectedMessage);
        }

        public Mock<IErrorProvider> GetErrorProviderMock()
        {
            var errorProviderMock = new Mock<IErrorProvider>();
            errorProviderMock.Setup(m => m.GetPendingStepDefinitionError())
                             .Returns(new PendingStepException());
            return errorProviderMock;
        }
    }
}
