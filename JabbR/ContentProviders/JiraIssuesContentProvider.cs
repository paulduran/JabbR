using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JabbR.ContentProviders.Core;

namespace JabbR.ContentProviders
{
    public class JiraIssuesContentProvider : CollapsibleContentProvider
    {
        private static readonly Regex _jiraIssuesRegex = new Regex(@"(https?://.*?)/browse/([A-Z]+-\d+)");
        private static readonly string _jiraIssuesApiFormat = "{0}/rest/api/latest/issue/{1}?jsonp-callback=addJiraIssue";
        private static readonly string _jiraIssuesContentFormat = "<div class='jira-issue jira-issue-{0}'></div><script src='{1}'></script>";

        protected override Task<ContentProviderResult> GetCollapsibleContent(ContentProviderHttpRequest request)
        {
            var parameters = ExtractParameters(request.RequestUri);

            return TaskAsyncHelper.FromResult(new ContentProviderResult()
            {
                Content = String.Format(_jiraIssuesContentFormat,
                        parameters[1],
                    String.Format(_jiraIssuesApiFormat, parameters[0], parameters[1])
                ),
                Title = request.RequestUri.AbsoluteUri
            });
        }

        protected override Regex ParameterExtractionRegex
        {
            get
            {
                return _jiraIssuesRegex;
            }
        }

        public override bool IsValidContent(Uri uri)
        {
            return ExtractParameters(uri).Count == 2;
        }
    }
}