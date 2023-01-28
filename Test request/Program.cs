#define main
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
class main
{
    static string login = "";
    static string password = "";
    static HttpClient client = null;
    static List<(string, string)> cookies = new List<(string, string)>();
    static void Main()
    {
        Console.WriteLine("Hello, World!");
        daun info = new daun(login, password);
        JObject dawd = JObject.Parse("{\"aa\":{\"dad\":123}}");
        JToken dawdd = dawd["aa"];
        Console.WriteLine(dawdd.Type);

        if (info.checkauth(info.GetClient()))
        {

            info.Auth(out client, out cookies);
        }
    }
}

class daun
{
    public string login { private get; set; } = "";
    public string password { private get; set; } = "";

    public daun(string log, string pas) { if ((log != "" && log != null) && (pas != "" && pas != null)) { password = pas; login = log; } else { throw new Exception("No auth data"); } }
    public readonly HttpClient client = new HttpClient();
    private Task<HttpResponseMessage> response;
    private List<(string, string)> cookies = new List<(string, string)>();
    private const string nuller = "{}";

    public HttpClient GetClient()
    {
        return client;
    }

    public bool checkauth(HttpClient client)
    {
        // ready tool
        /*response = GetResponse(HttpMethod.Get, new Uri("https://dispace.edu.nstu.ru/"), "", "application/json", cookies);
        response = GetResponse(HttpMethod.Post,
            new Uri("https://login.nstu.ru/ssoservice/json/authenticate?realm=/ido&goto=https://dispace.edu.nstu.ru/user/proceed?login=openam&password=auth"), nuller, "application/json", cookies);
        JObject authInstruction = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);
        authInstruction["callbacks"][0]["input"][0]["value"] = login;
        authInstruction["callbacks"][1]["input"][0]["value"] = password;
        response = GetResponse(HttpMethod.Post, new Uri("https://login.nstu.ru/ssoservice/json/authenticate"),
            authInstruction.ToString(Newtonsoft.Json.Formatting.None).ToString(), "application/json", cookies);
        cookies.Add(("NstuSsoToken", JObject.Parse(response.Result.Content.ReadAsStringAsync().Result)["tokenId"].ToString()));
        response = GetResponse(HttpMethod.Post, new Uri("https://login.nstu.ru/ssoservice/json/ido/users?_action=idFromSession"), nuller, "application/json", cookies);
        response = GetResponse(HttpMethod.Get, new Uri($"https://login.nstu.ru/ssoservice/json/ido/users/{login}"), nuller, "application/json", cookies);
        response = GetResponse(HttpMethod.Post, new Uri("https://login.nstu.ru/ssoservice/json/users?_action=validateGoto"), JObject.Parse("{'goto':'https://dispace.edu.nstu.ru/user/proceed?login=openam&password=auth'}").ToString(), "application/json", cookies);
        response = GetResponse(HttpMethod.Post, new Uri("https://dispace.edu.nstu.ru/user/proceed?login=openam&password=auth"), "{}", "application/json", cookies);

        response = GetResponse(HttpMethod.Post, new Uri("https://dispace.edu.nstu.ru/ditest/index"), "action=get_tests&discipline_id=all&start=false&filter=&orderby=&test_group_id=-1", "application/x-www-form-urlencoded", cookies); // Я трахал в рот ваше казино
        Console.WriteLine(response.Result.Content.ReadAsStringAsync().Result);

        Console.WriteLine(x_www_form_urlencoded_former(JObject.Parse("{\"action\":\"get_tests\",\"discipline_id\":\"my_results\",\"start\":\"false\",\"filter\":\"\",\"orderby\":\"\",\"test_group_id\":\"-1\"}")).Result);
        


        foreach ((string, string) ad in cookies)
        {
            Console.WriteLine(ad);
        }
        */
        JObject authInstruction = null;
        try
        {

            response = GetResponse(HttpMethod.Post,
                new Uri("https://login.nstu.ru/ssoservice/json/authenticate?realm=/ido&goto=https://dispace.edu.nstu.ru/user/proceed?login=openam&password=auth"), nuller, "application/json", cookies);
            authInstruction = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);
            authInstruction["callbacks"][0]["input"][0]["value"] = login;
            authInstruction["callbacks"][1]["input"][0]["value"] = password;
            //throw new Exception("Test ex");
            response = GetResponse(HttpMethod.Post, new Uri("https://login.nstu.ru/ssoservice/json/authenticate"), authInstruction.ToString(Newtonsoft.Json.Formatting.None).ToString(), "application/json", cookies);
            authInstruction = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);
            //Console.WriteLine(authInstruction.ToString(Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            var dfdd = new StackTrace(ex, true);
            Console.WriteLine($"Error check checkauth => args {client.BaseAddress}");
            Console.WriteLine($"Text exception: {ex.Message}");
            Console.WriteLine($"Line = {dfdd.GetFrame(0).GetFileLineNumber()}");
            Console.WriteLine($"Cookie info:");
            foreach ((string, string) ad in cookies)
            {
                Console.WriteLine("\t" + ad.ToString());
            }
            Console.WriteLine($"Returned data:\n\t{response.Result.Content.ReadAsStringAsync().Result}");

        }
        return authInstruction.ContainsKey("tokenId") && authInstruction.ContainsKey("successUrl");
    }
    public void Auth(out HttpClient httpclient, out List<(string, string)> cookiedata) // необязательно получать данные с функции, но можно убрать, ибо все хранится локально
    {
        try
        {
            response = GetResponse(HttpMethod.Get, new Uri("https://dispace.edu.nstu.ru/"), "", "application/json", cookies);
            response = GetResponse(HttpMethod.Post,
                new Uri("https://login.nstu.ru/ssoservice/json/authenticate?realm=/ido&goto=https://dispace.edu.nstu.ru/user/proceed?login=openam&password=auth"), nuller, "application/json", cookies);
            JObject authInstruction = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);
            authInstruction["callbacks"][0]["input"][0]["value"] = login;
            authInstruction["callbacks"][1]["input"][0]["value"] = password;
            response = GetResponse(HttpMethod.Post, new Uri("https://login.nstu.ru/ssoservice/json/authenticate"),
                authInstruction.ToString(Newtonsoft.Json.Formatting.None).ToString(), "application/json", cookies);
            cookies.Add(("NstuSsoToken", JObject.Parse(response.Result.Content.ReadAsStringAsync().Result)["tokenId"].ToString()));
            response = GetResponse(HttpMethod.Post, new Uri("https://login.nstu.ru/ssoservice/json/ido/users?_action=idFromSession"), nuller, "application/json", cookies);
            response = GetResponse(HttpMethod.Get, new Uri($"https://login.nstu.ru/ssoservice/json/ido/users/{login}"), nuller, "application/json", cookies);
            response = GetResponse(HttpMethod.Post, new Uri("https://login.nstu.ru/ssoservice/json/users?_action=validateGoto"), JObject.Parse("{'goto':'https://dispace.edu.nstu.ru/user/proceed?login=openam&password=auth'}").ToString(), "application/json", cookies);
            response = GetResponse(HttpMethod.Post, new Uri("https://dispace.edu.nstu.ru/user/proceed?login=openam&password=auth"), "{}", "application/json", cookies);
        }
        catch (Exception ex)
        {
            var dfdd = new StackTrace(ex, true);
            Console.WriteLine($"Error check checkauth => args {client.BaseAddress}");
            Console.WriteLine($"Text exception: {ex.Message}");
            Console.WriteLine($"Line = {dfdd.GetFrame(0).GetFileLineNumber()}");
            Console.WriteLine($"Cookie info:");
            foreach ((string, string) ad in cookies)
            {
                Console.WriteLine("\t" + ad.ToString());
            }
            Console.WriteLine($"Returned data:\n\t{response.Result.Content.ReadAsStringAsync().Result}");
        }
        httpclient = client;
        cookiedata = cookies;
    }

    public async Task<string> Tree(string json)
    {
        string text = "";
        int time = 0;
        void TreeMaker(JToken js, int count)
        {
            switch (js.Type)
            {
                case JTokenType.Object:
                    {
                        for (int i = 0; i < js.Count(); i++)
                        {
                            text += "\n";
                            if (count != 0)
                            {
                                for (int k = 0; k < count; k++)
                                {
                                    text += "│" + new string(' ', 2);
                                }
                            }
                            text += ((count < 1 || js.ElementAt(i).Type == JTokenType.Boolean || js.ElementAt(i).Type == JTokenType.String || js.ElementAt(i).Type == JTokenType.Integer) && i + 1 < js.Count() ? "├──" : "└──") + "'" + js.ElementAt(i) + "'"; //  
                        }
                    }
                    break;
                case JTokenType.Array:
                    {
                        //foreach(
                    }
                    break;
                default: { } break;
            }
        }
        TreeMaker(JObject.Parse(json).ToString(Newtonsoft.Json.Formatting.None), 0);

        return text;
    }



    private async Task<HttpResponseMessage> GetResponse(HttpMethod method, Uri uri, string? datatext, string contentType, List<(string, string)> cookie)
    {
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
        try
        {
            request.Content = new StringContent(datatext, Encoding.UTF8, contentType); //JsonContent.Create(JObject.Parse(datatext)); //new StringContent(datatext);  
        }
        catch (Exception ex) { throw new ArgumentException("Error parse, please set json data in datatext or nuller"); }
        if (cookie != null)
        {
            request.Headers.Add("Cookie", MakeCookie(cookie));
        }
        request.Method = method;
        var info = client.Send(request);
        //Console.WriteLine($"\tDATA: {uri.ToString()}");
        //Console.WriteLine(info.Headers);
        //Console.WriteLine(info.Headers.ToString());
        try
        {
            string df = info.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value.ElementAt(0);
            cookies.AddRange(TakeCookie(df));
            cookies = cookie.Distinct().ToList();
            //Console.WriteLine($"\tDATA: {uri.ToString()}");
            //Console.WriteLine(info.Headers);
        }
        catch (Exception e) { }
        return info;
    }

    private string MakeCookie(List<(string, string)> cookie)
    {
        string t = "";
        foreach ((string, string) a in cookie)
        {
            t += a.Item1 + "=" + a.Item2 + "; ";
        }
        return t;
    }
    private List<(string, string)> TakeCookie(string cookie)
    {
        string[] data = cookie.Split("; ");
        List<(string, string)> ret = new List<(string, string)>();
        foreach (string s in data)
        {
            ret.Add((s.Split('=')[0], s.Split('=')[1]));
        }
        return ret;
    }
    private async Task<string> x_www_form_urlencoded_former(JObject data) // Example: x_www_form_urlencoded_former(JObject.Parse("{\"123\":\"123\",\"3331\":\"my_res31ults\"}")).Result; хуета
    {
        return data.ToString(Newtonsoft.Json.Formatting.None).Replace(",", "&").Replace(":", "=").Replace("\"", "").Replace("{", "").Replace("}", "");
    }

}

