using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.Application
{
    public sealed class HFMirrorService
    {
        public static readonly string Client = "hf-mirror";
        private IHttpClientFactory _httpClientFactory;
        public HFMirrorService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HfMirrorPageResult?> GetListAsync(string query, int index)
        {
            using var client = _httpClientFactory.CreateClient(Client);
            string param = string.Empty;
            if (index > 0)
            {
                param = $"p={index}&";
            }
            param += "sort=trending&withCount=true";
            if (!string.IsNullOrWhiteSpace(query))
            {
                param += $"&search={query}";
            }
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/models-json?{param}");
            httpRequest.Headers.Add("Referer", $"{client.BaseAddress}{httpRequest.RequestUri}");
            var response = await client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            return await JsonSerializer.DeserializeAsync<HfMirrorPageResult>(await response.Content.ReadAsStreamAsync(),new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
    public class HfMirrorPageResult
    {

        public HfMirrorModel[] Models { get; set; } = [];
        public int NumItemsPerPage { get; set; }
        public int NumTotalItems { get; set; }
        public int PageIndex { get; set; }
    }
    public class HfMirrorModel
    {
        public string Author { get; set; }
        public Authordata AuthorData { get; set; }
        public int Downloads { get; set; }
        public object Gated { get; set; }
        public string Id { get; set; }
        public DateTime LastModified { get; set; }
        public int Likes { get; set; }
        public string Pipeline_tag { get; set; }
        public bool _Private { get; set; }
        public string RepoType { get; set; }
        public bool IsLikedByUser { get; set; }
    }

    public class Authordata
    {
        public string AvatarUrl { get; set; }
        public string Fullname { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsHf { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsPro { get; set; }
    }
}
