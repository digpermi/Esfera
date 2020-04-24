using Microsoft.AspNetCore.Razor.TagHelpers;
using Utilities.Messages;

namespace Portal.TagHelpers
{
    [HtmlTargetElement("message")]
    public class MessageTagHelper : TagHelper
    {
        private const string parentCssClass = "alert";

        public ApplicationMessage UserMessage { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            if (this.UserMessage != null)
            {
                string messageStyleClass;
                switch (this.UserMessage.Type)
                {
                    case MessageType.Ok:
                        messageStyleClass = "alert alert-success";
                        break;
                    case MessageType.Information:
                        messageStyleClass = "alert alert-warning";
                        break;
                    case MessageType.Error:
                        messageStyleClass = "alert alert-danger";
                        break;
                    default:
                        messageStyleClass = "alert alert-error";
                        break;
                }

                output.Attributes.SetAttribute("class", $"{parentCssClass} { messageStyleClass }");

                output.Content.SetHtmlContent(this.UserMessage.Text);
                output.TagMode = TagMode.StartTagAndEndTag;
            }
        }
    }
}
