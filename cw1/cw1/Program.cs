using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentNullException();
            }

            Uri uri;

            if (!(
                Uri.TryCreate(args[0], UriKind.Absolute, out uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                ))
            {
                throw new ArgumentException();
            }

            foreach (var a in args)
            {
                Console.WriteLine(a);
            }

            var emails = await GetEmails(args[0]);

            foreach (var email in emails)
            {
                Console.WriteLine(email.ToString());
            }
        }


        static async Task<IList<string>> GetEmails(string url)
        {
            var httpClient = new HttpClient();
            var emailList = new List<string>();

            var response = await httpClient.GetAsync(url);

            if (System.Net.HttpStatusCode.OK == response.StatusCode)
            {


                httpClient.Dispose();

                Regex emailRegex = new Regex(
                   @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                    RegexOptions.IgnoreCase
                    );

                MatchCollection emailMatches = emailRegex.Matches(response.Content.ReadAsStringAsync().Result);
                var processedEmailList = new List<string>();


                foreach (var match in emailMatches)
                {
                    if (!processedEmailList.Contains(match.ToString()))
                    {
                        processedEmailList.Add(match.ToString());
                        emailList.Add(match.ToString());
                    }
                }

                if (0 == emailMatches.Count)
                {
                    Console.WriteLine("Nie znaleziono adresów email");
                }

            }
            else
            {
                Console.WriteLine("Błąd w czasie pobierania strony");
            }

            return emailList;
        }
    }
}