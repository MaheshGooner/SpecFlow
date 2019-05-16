﻿using System;
using System.IO;
using FluentAssertions;
using Io.Cucumber.Messages;
using Moq;
using TechTalk.SpecFlow.CommonModels;
using TechTalk.SpecFlow.CucumberMessages;
using TechTalk.SpecFlow.CucumberMessages.Sinks;
using TechTalk.SpecFlow.FileAccess;
using Xunit;

namespace TechTalk.SpecFlow.RuntimeTests.CucumberMessages.Sinks
{
    public class ProtobufFileSinkOutputTests
    {
        [Fact(DisplayName = @"IsInitialized should return true if the ProtobufFileSinkOutput has been initialized with a writable OutputStream")]
        public void IsInitialized_ShouldReturnTrueIfOutputStreamIsSetAndWritable()
        {
            // ARRANGE
            const bool expectedIsInitialized = true;
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var writableStream = GetWritableStream();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock(Result<Stream>.Success(writableStream));
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);
            protobufFileSinkOutput.EnsureIsInitialized();

            // ACT
            bool actualIsInitialized = protobufFileSinkOutput.IsInitialized();

            // ASSERT
            actualIsInitialized.Should().Be(expectedIsInitialized);
        }

        [Fact(DisplayName = @"IsInitialized should return false if the ProtobufFileSinkOutput has not been initialized")]
        public void IsInitialized_ShouldReturnFalseIfNotInitialized()
        {
            // ARRANGE
            const bool expectedIsInitialized = false;
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock();
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);

            // ACT
            bool actualIsInitialized = protobufFileSinkOutput.IsInitialized();

            // ASSERT
            actualIsInitialized.Should().Be(expectedIsInitialized);
        }

        [Fact(DisplayName = @"IsInitialized should return false if the initialized OutputStream is not writable")]
        public void IsInitialized_ShouldReturnFalseIfOutputStreamNotWritable()
        {
            // ARRANGE
            const bool expectedIsInitialized = false;
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var notWritableStream = GetNotWritableStream();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock(Result<Stream>.Success(notWritableStream));
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);
            protobufFileSinkOutput.EnsureIsInitialized();

            // ACT
            bool actualIsInitialized = protobufFileSinkOutput.IsInitialized();

            // ASSERT
            actualIsInitialized.Should().Be(expectedIsInitialized);
        }

        [Fact(DisplayName = @"EnsureIsInitialized should set the OutputStream property")]
        public void EnsureInitialized_ShouldSetOutputStream()
        {
            // ARRANGE
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock();
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);

            // ACT
            protobufFileSinkOutput.EnsureIsInitialized();

            // ASSERT
            protobufFileSinkOutput.OutputStream.Should().NotBeNull();
        }

        [Theory(DisplayName = @"EnsureIsInitialized should return the correct value depending on the file being able to be opened")]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void EnsureInitialized_ShouldReturnCorrectValue(bool fileSuccessfullyOpened, bool expectedSuccess)
        {
            // ARRANGE
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock(fileSuccessfullyOpened ? Result<Stream>.Success(GetWritableStream()) : Result<Stream>.Failure());
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);

            // ACT
            bool actualSuccess = protobufFileSinkOutput.EnsureIsInitialized();

            // ASSERT
            actualSuccess.Should().Be(expectedSuccess);
        }

        [Fact(DisplayName = @"EnsureInitialized should return true if the " + nameof(ProtobufFileSinkOutput) + " has been initialized previously")]
        public void EnsureInitialized_ShouldReturnTrueIfAlreadyInitialized()
        {
            // ARRANGE
            const bool expectedSuccess = true;
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock();
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);
            protobufFileSinkOutput.EnsureIsInitialized();

            // ACT
            bool actualSuccess = protobufFileSinkOutput.EnsureIsInitialized();

            // ASSERT
            actualSuccess.Should().Be(expectedSuccess);
        }

        [Fact(DisplayName = @"WriteMessage should return false if the ProtobufFileSinkOutput is not initialized")]
        public void WriteMessage_Message_ShouldReturnFalseIfNotInitialized()
        {
            // ARRANGE
            const bool expectedSuccess = false;
            var message = new TestRunStarted();
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock();
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);

            // ACT
            bool actualSuccess = protobufFileSinkOutput.WriteMessage(message);

            // ASSERT
            actualSuccess.Should().Be(expectedSuccess);
        }

        [Fact(DisplayName = @"WriteMessage should return true if the ProtobufFileSinkOutput is initialized")]
        public void WriteMessage_Message_ShouldReturnTrueIfInitialized()
        {
            // ARRANGE
            const bool expectedSuccess = true;
            var message = new TestRunStarted();
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock();
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);
            protobufFileSinkOutput.EnsureIsInitialized();

            // ACT
            bool actualSuccess = protobufFileSinkOutput.WriteMessage(message);

            // ASSERT
            actualSuccess.Should().Be(expectedSuccess);
        }

        [Fact(DisplayName = @"WriteMessage should write the specified message to OutputStream")]
        public void WriteMessage_Message_ShouldWriteTheSpecifiedMessageToOutputStream()
        {
            // ARRANGE
            var cucumberMessageFactory = new CucumberMessageFactory();
            var message = cucumberMessageFactory.BuildTestRunStartedMessage(DateTime.UtcNow);
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var writableStream = GetWritableStream();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock(Result<Stream>.Success(writableStream));
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);
            protobufFileSinkOutput.EnsureIsInitialized();

            // ACT
            protobufFileSinkOutput.WriteMessage(message);

            // ASSERT
            writableStream.Length.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = @"Dispose should dispose OutputStream")]
        public void Dispose_ShouldDisposeOutputStream()
        {
            var protobufFileSinkConfiguration = GetProtobufFileSinkConfiguration();
            var binaryFileAccessorMock = GetBinaryFileAccessorMock();
            var protobufFileSinkOutput = new ProtobufFileSinkOutput(binaryFileAccessorMock.Object, protobufFileSinkConfiguration);
            protobufFileSinkOutput.EnsureIsInitialized();

            // ACT
            protobufFileSinkOutput.Dispose();

            // ASSERT
            protobufFileSinkOutput.OutputStream.CanWrite.Should().BeFalse();
            protobufFileSinkOutput.OutputStream.CanRead.Should().BeFalse();
            protobufFileSinkOutput.OutputStream.CanSeek.Should().BeFalse();
        }

        public Mock<IBinaryFileAccessor> GetBinaryFileAccessorMock(Result<Stream> openOrAppendStream = null)
        {
            var binaryFileAccessorMock = new Mock<IBinaryFileAccessor>();
            binaryFileAccessorMock.Setup(m => m.OpenAppendOrCreateFile(It.IsAny<string>()))
                                  .Returns(openOrAppendStream ?? Result<Stream>.Success(GetWritableStream()));
            return binaryFileAccessorMock;
        }

        public Stream GetNotWritableStream()
        {
            return new MemoryStream(new byte[0], false);
        }

        public MemoryStream GetWritableStream()
        {
            return new MemoryStream();
        }

        public ProtobufFileSinkConfiguration GetProtobufFileSinkConfiguration(string targetFilePath = "CucumberMessageQueue")
        {
            return new ProtobufFileSinkConfiguration(targetFilePath);
            ////return new SinkConfiguration(
            ////    new SinkConfigurationEntry
            ////    {
            ////        TypeName = typeof(ProtobufFileSink).AssemblyQualifiedName,
            ////        ConfigurationValues =
            ////        {
            ////            [nameof(ProtobufFileSinkConfiguration.TargetFilePath)] = targetFilePath
            ////        }
            ////    });
        }
    }
}
