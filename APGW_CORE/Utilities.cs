using System;


namespace APGW
{
    public class Utilities
    {
        /// <summary>
        /// Updates the URL.
        /// </summary>
        /// <returns>The URL.</returns>
        /// <param name="baseUrl">Base URL.</param>
        /// <param name="newBaseOrUriToAppend">New base or URI to append.</param>
        public static string UpdateUrl (string baseUrl, string newBaseOrUriToAppend)
        {

            if (string.IsNullOrEmpty (newBaseOrUriToAppend)) {
                return baseUrl;
            }

            Uri uriFormatted = null;

            try {
                 uriFormatted = new Uri (newBaseOrUriToAppend, UriKind.Absolute);          
            } catch(Exception e) {
                 uriFormatted = new Uri (newBaseOrUriToAppend, UriKind.Relative); 
            }

            if (uriFormatted.IsAbsoluteUri && (uriFormatted.Scheme.ToLower ().Equals ("http") || uriFormatted.Scheme.ToLower ().Equals ("https"))) {
                return newBaseOrUriToAppend;
            } else {
                Uri baseUrlFormatted = new Uri (baseUrl);

                // Append url
                if (!baseUrlFormatted.IsAbsoluteUri) {
                    throw new Exception ("base url should be absolute");
                } else {
                    if (!newBaseOrUriToAppend.StartsWith ("/")) {
                        newBaseOrUriToAppend = "/" + newBaseOrUriToAppend;
                    }
                    Uri combinedUrl = new Uri (baseUrl + newBaseOrUriToAppend);
         
                    return combinedUrl.AbsoluteUri.ToString ();
                }

            }
        }
    }
}

