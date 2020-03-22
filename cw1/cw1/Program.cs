using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;



namespace cw1
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            if (args.Length > 1)
            {
                throw new ArgumentNullException();
            }

            foreach (var a in args)
            {
                Console.WriteLine(a);
            }
            var emails = await GetEmails(args[0]);

            foreach (var email in emails)
            {
                Console.WriteLine(email);
            }

            Uri uri;
            if (

                Uri.TryCreate(args[0], UriKind.Absolute, out uri))
            {
                throw new ArgumentException();

            }
            static async Task<IList<string>> GetEmails(string url)
            {
                var httpClient = new HttpClient();
                //var response = await httpClient.GetAsync(args[0]);
                var listOfEmails = new List<string>();
                var response = await httpClient.GetAsync(url);

                Regex emailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);


                MatchCollection emailMatches = emailRegex.Matches(response.Content.ReadAsStringAsync().Result);

                foreach (var eanilMatch in emailMatches)
                {
                    listOfEmails.Add(emailMatches.ToString());
                }

                return listOfEmails;
            }

        }

    }  

}
