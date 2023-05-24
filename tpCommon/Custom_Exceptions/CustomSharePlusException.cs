using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using ViewModels.SharePlus;

namespace Custom_Exceptions
{
    public class CustomSharePlusException : Exception
    {
        private SharePlusArticleViewModel sharePlusArticleViewModel { get; set; }
        public int LoggedInEmployeeId { get; set; }
        public Dictionary<string, object> articleProps { get; set; }

        public CustomSharePlusException()
        {
        }

        public CustomSharePlusException(string message) : base(message)
        {
        }

        public CustomSharePlusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomSharePlusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public CustomSharePlusException(string message, SharePlusArticleViewModel sharePlusArticleViewModel, int LoggedInEmployeeId)
            : base(message)
        {
            this.sharePlusArticleViewModel = sharePlusArticleViewModel;
            this.LoggedInEmployeeId = LoggedInEmployeeId;

            articleProps = new Dictionary<string, object>();
            articleProps.Add("Article properties : ", null);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(sharePlusArticleViewModel))
            {
                var name = descriptor.Name;
                if (name != nameof(sharePlusArticleViewModel.Htmlbody) && name != nameof(sharePlusArticleViewModel.Contents))
                {
                    var value = descriptor.GetValue(sharePlusArticleViewModel);
                    articleProps.Add(name, value);
                }
            }
        }
    }
}
