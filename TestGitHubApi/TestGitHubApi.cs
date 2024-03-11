using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;

        public readonly string repo = "test-nakov-repo";

        private static int createdCommentId;

        private static int createdIssueNumber;


        [SetUp]
        public void Setup()
        {            
            client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "slabsterz", "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");
        }
        

        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            // Arrange & Act
            var issuesResponse = client.GetAllIssues(repo);

            // Assert
            Assert.That(issuesResponse, Is.Not.Null);

            foreach (var issue in issuesResponse)
            {
                Assert.That(issue.Id, Is.Not.Zero);
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.Not.Empty);
                Assert.That(issue.Body, Is.Not.Empty);
            }
            
        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            // Arrange
            int issueNumber = 21;

            // Act
            var issueResponse = client.GetIssueByNumber(repo, issueNumber);

            // Assert
            Assert.That(issueResponse, Is.Not.Null);
            Assert.That(issueResponse.Id, Is.Not.Zero);
            Assert.That(issueResponse.Number, Is.GreaterThan(0));

        }

        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            // Arrange
            int issueNumber = 21;

            // Act
            var labelsResponse = client.GetAllLabelsForIssue(repo, issueNumber);

            // Assert
            Assert.That (labelsResponse, Is.Not.Null);

            foreach (var label in labelsResponse)
            {
                Assert.That(label.Id, Is.Not.Zero);
                Assert.That(label.Name, Is.Not.Empty);
            }

        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            // Arrange
            int issueNumber = 21;  

            // Act
            var issueCommentsResponse = client.GetAllCommentsForIssue(repo, issueNumber);

            // Assert
            Assert.That(issueCommentsResponse, Is.Not.Null);

            foreach (var issue in issueCommentsResponse)
            {
                Assert.That(issue.Id, Is.Not.Zero);
                Assert.That(issue.Body, Is.Not.Empty);
            }
           
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            // Arrange
            string title = "New random title";
            string body = "New random body";

            // Act
            var createIssueResponse = client.CreateIssue(repo, title, body);

            // Assert
            Assert.Multiple(() =>
            {                
                Assert.That(createIssueResponse.Id, Is.Not.Zero);
                Assert.That(createIssueResponse.Number, Is.Not.Zero);
                Assert.That(createIssueResponse.Title, Is.EqualTo(title));
                Assert.That(createIssueResponse.Body, Is.EqualTo(body));
            });

            createdIssueNumber = createIssueResponse.Number;
        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            // Arrange
            string body = "newly added body to an issue";
            int issueNumber = createdIssueNumber;

            // Act
            var commentOnIssueResponse = client.CreateCommentOnGitHubIssue(repo, issueNumber, body);

            // Assert            
            Assert.That(commentOnIssueResponse.Body, Is.EqualTo(body));           
            createdCommentId = commentOnIssueResponse.Id;
        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            // Arrange
            int commentId = createdCommentId;

            // Act
            var commentResponse = client.GetCommentById(repo, commentId);

            // Assert
            Assert.That(commentResponse, Is.Not.Null);
            Assert.That(commentResponse.Id, Is.EqualTo(commentId));
            Assert.That(commentResponse.Body, Is.Not.Empty);
            Console.WriteLine(commentId);
        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            // Arrange
            int commentId = createdCommentId;
            string newBody = "adding some new body to update";

            // Act
            var editCommentResponse = client.EditCommentOnGitHubIssue(repo, commentId, newBody);

            // Assert
            Assert.That(editCommentResponse, Is.Not.Null);
            Assert.That(editCommentResponse.Body, Is.EqualTo(newBody));
            Assert.That(editCommentResponse.Id, Is.EqualTo(commentId));

        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            // Arrange
            int commentId = createdCommentId;

            // Act
            var deletedCommentResponse = client.DeleteCommentOnGitHubIssue(repo, commentId);

            // Assert
            Assert.That(deletedCommentResponse, Is.True);

        }
    }
}

