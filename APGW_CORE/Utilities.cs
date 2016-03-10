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
            if (!newBaseOrUriToAppend.StartsWith("/")) {
                newBaseOrUriToAppend = "/" + newBaseOrUriToAppend;
            }
            Uri uriFormatted = new Uri (newBaseOrUriToAppend);
            Uri baseUrlFormatted = new Uri (baseUrl);
           
            if (!string.IsNullOrEmpty (uriFormatted.Scheme) && (uriFormatted.Scheme.ToLower ().Equals ("http") || uriFormatted.Scheme.ToLower ().Equals ("https"))) {
                return newBaseOrUriToAppend;
            } else {
                // Append url
                if (!baseUrlFormatted.IsAbsoluteUri) {
                    throw new Exception ("base url should be absolute");
                } else {
                    Uri combinedUrl = new Uri (baseUrl + newBaseOrUriToAppend);
                    return combinedUrl.AbsoluteUri.ToString ();
                }

            }
        }
    }
}

