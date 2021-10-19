using System;
using RestSharp;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HtmlToPdf
{
    class HtmlToPdf
    {
        private string size, orientation, width, conversion_delay;
        public HtmlToPdf()
        {
            this.size = "A4";
            this.orientation = "Portrait";
            this.width = "1300";
            this.conversion_delay = "1";
        }

        public byte[] GetPdf(string url)
        {
            RestClient client = new RestClient("https://www.html-to-pdf.net/free-online-pdf-converter.aspx");
            RestRequest request = new RestRequest(Method.GET);
            RestResponse response = (RestResponse)client.Execute(request);

            string pattern = "id=\"__VIEWSTATE\" value=\"(.*?)\"";
            string view_state = Regex.Match(response.Content, pattern, RegexOptions.IgnoreCase).Groups[1].Value;

            pattern = "id=\"__EVENTVALIDATION\" value=\"(.*?)\"";
            string event_validation = Regex.Match(response.Content, pattern, RegexOptions.IgnoreCase).Groups[1].Value;

            request = new RestRequest(Method.POST);

            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>()
            {
            new KeyValuePair<string, string>("authority", "www.html-to-pdf.net"),
            new KeyValuePair<string, string>("cache-control", "max-age=0"),
            new KeyValuePair<string, string>("sec-ch-ua", "\"Google Chrome\";v=\"89\", \"Chromium\";v=\"89\", \";Not A Brand\";v=\"99\""),
            new KeyValuePair<string, string>("sec-ch-ua-mobile", "?0"),
            new KeyValuePair<string, string>("upgrade-insecure-requests", "1"),
            new KeyValuePair<string, string>("origin", "https://www.html-to-pdf.net"),
            new KeyValuePair<string, string>("content-type", "application/x-www-form-urlencoded"),
            new KeyValuePair<string, string>("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_2_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36"),
            new KeyValuePair<string, string>("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"),
            new KeyValuePair<string, string>("sec-fetch-site", "same-origin"),
            new KeyValuePair<string, string>("sec-fetch-mode", "navigate"),
            new KeyValuePair<string, string>("sec-fetch-user", "?1"),
            new KeyValuePair<string, string>("sec-fetch-dest", "document"),
            new KeyValuePair<string, string>("referer", "https://www.html-to-pdf.net/free-online-pdf-converter.aspx"),
            new KeyValuePair<string, string>("accept-language", "en-US,en;q=0.9,vi;q=0.8")
            };

            request.AddHeaders(headers);

            request.AddParameter("__EVENTTARGET", "ctl00$maincontent$BtnExport", ParameterType.GetOrPost);
            request.AddParameter("__VIEWSTATE", view_state, ParameterType.GetOrPost);
            request.AddParameter("__EVENTVALIDATION", event_validation, ParameterType.GetOrPost);
            request.AddParameter("ctl00$maincontent$TxtURL", url, ParameterType.GetOrPost);
            request.AddParameter("ctl00$maincontent$DdlPageSize", this.size, ParameterType.GetOrPost);
            request.AddParameter("ctl00$maincontent$DdlPageOrientation", this.orientation, ParameterType.GetOrPost);
            request.AddParameter("ctl00$maincontent$TxtPageWidth", this.width, ParameterType.GetOrPost);
            request.AddParameter("ctl00$maincontent$TxtConversionDelay", this.conversion_delay, ParameterType.GetOrPost);

            response = (RestResponse)client.Execute(request);

            return response.RawBytes;

            //System.IO.File.WriteAllBytes(filename, response.RawBytes);
        }

        public bool ValidateURL(string url)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
