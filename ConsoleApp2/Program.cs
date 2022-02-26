using System;
using System.Net;
using System.Net.Http;

namespace Teams
{
    internal static class Teams
    {
        static async Task Main(string[] args, string Email, string Pass)
        
        {
            string AuthUrl = "https://m.vk.com/login";
            string logoutId = "act=logout";
            string MainUrl = "https://vk.com";
            
            var httpHandler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };
            
            var httpClient = new HttpClient(httpHandler) {Timeout = TimeSpan.FromSeconds(150)};
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
            
            Uri authUri = new Uri(AuthUrl);
            string data = await httpClient.GetStringAsync(authUri);
            var IsAuth = data.Contains(logoutId);
            if (!IsAuth)
            {
                string result = data.Substring(Convert.ToInt32("<form method=\"post\" action=\""), Convert.ToInt32("\""));//достаем ссылку для авторизации
                authUri = new Uri(result);
                var body = new Dictionary<string, string> {{"act", "login"}, {"email", Email}, {"pass", Pass}};//тут заполняем логин и пароль
                var response = await httpClient.PostAsync(authUri, new FormUrlEncodedContent(body));
                var page = await response.Content.ReadAsStringAsync();
                IsAuth = page.Contains(logoutId);//если вошли на странице появится ссыль для выхода :)
                Uri mainUri = new Uri(MainUrl);//переходим на главную страницу
                data = await httpClient.GetStringAsync(mainUri);
            }
            Console.Write(IsAuth);
        }
    }
}