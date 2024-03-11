using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpServices.Models;




namespace RestSharpServices
{
    public class GitHubApiClient
    {
        private RestClient client;        

        public GitHubApiClient(string baseUrl, string username, string token)
        {
            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(username, token)
            };        

            this.client = new RestClient(options);
        }

        public List<Issue>?  GetAllIssues(string repo)
        {
            var request = new RestRequest($"{repo}/issues");
            var response = client.Execute(request, Method.Get);

            if (response.Content != null)
            {
                return JsonSerializer.Deserialize<List<Issue>>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public Issue?  GetIssueByNumber(string repo, int issueNumber)
        {
            var request = new RestRequest($"{repo}/issues/{issueNumber}");
            var response = client.Execute(request, Method.Get);

            if (response.Content != null)
            {
                return JsonSerializer.Deserialize<Issue>(response.Content);
            }
            else
            {
                return null;
            }

        }

        public Issue?  CreateIssue(string repo, string title, string body)
        {
            var request = new RestRequest($"{repo}/issues");
            request.AddJsonBody( new { title, body });

            var response = client.Execute(request, Method.Post);

            if (response.Content != null)
            {
                return JsonSerializer.Deserialize<Issue>(response.Content);
            }
            else
            {
                return null;
            }


        }

        public List<Label>?  GetAllLabelsForIssue(string repo, int issueNumber)
        {
            var request = new RestRequest($"{repo}/issues/{issueNumber}/labels");
            var response = client.Execute(request, Method.Get);

            if (response.Content != null)
            {
                return JsonSerializer.Deserialize<List<Label>>(response.Content);
            }
            else
            {
                return null;
            }

        }

        public List<Comment>?  GetAllCommentsForIssue(string repo, int issueNumber)
        {
            var request = new RestRequest($"{repo}/issues/{issueNumber}/comments");
            var response = client.Execute(request, Method.Get);

            if (response.Content != null)
            {
                return JsonSerializer.Deserialize<List<Comment>>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public Comment? CreateCommentOnGitHubIssue(string repo, int issueNumber, string body)
        {
            var request = new RestRequest($"{repo}/issues/{issueNumber}/comments");
            request.AddJsonBody(new { body });
            var response = client.Execute(request, Method.Post);

            if (response.Content != null)
            {
                return JsonSerializer.Deserialize<Comment>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public Comment?  GetCommentById (string repo, long commentId)
        {
            var request = new RestRequest($"{repo}/issues/comments/{commentId}");
            var response = client.Execute(request, Method.Get);

            if (response.Content != null)
            {
                return JsonSerializer.Deserialize<Comment>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public Comment?  EditCommentOnGitHubIssue( string repo, int commentId, string newBody)
        {
            var request = new RestRequest($"{repo}/issues/comments/{commentId}", Method.Patch);
            request.AddJsonBody(new { body = newBody });

            var response = client.Execute(request);

            if (response.Content != null && response.IsSuccessful)
            {
                return JsonSerializer.Deserialize<Comment>(response.Content);
            }
            else
            {
                return null;
            }
        }

        public bool DeleteCommentOnGitHubIssue(string repo, int commentId)
        {
            var request = new RestRequest($"{repo}/issues/comments/{commentId}");
            var response = client.Execute(request, Method.Delete);
            return response.IsSuccessful;

        }

    }
}
