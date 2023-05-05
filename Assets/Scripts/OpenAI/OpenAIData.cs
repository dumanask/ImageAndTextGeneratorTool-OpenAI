using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityOpenAI
{
    #region COMPLETIONS_DATA
    [System.Serializable]
    public class RequestCompletionsData
    {
        public string model;
        public string prompt;
        public float temperature;
        public int max_tokens;
        public int top_p;
        public int frequency_penalty;
        public int presence_penalty;
    }

    [System.Serializable]
    public class CompletionsOpenAIAPI
    {
        public string id;
        public string @object;
        public int created;
        public string model;
        public CompletionsChoice[] choices;
        public CompletionsUsage usage;
    }

    [System.Serializable]
    public class CompletionsChoice
    {
        public string text;
        public int index;
        public object logprobs;
        public string finish_reason;
    }

    [System.Serializable]
    public class CompletionsUsage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
    #endregion

    #region IMAGE_DATA
    [System.Serializable]
    public class RequestImageData
    {
        public string prompt;
        public int n;
        public string size;
        public string responseFormat { get; set; } = "url";
    }

    [System.Serializable]
    public class ResponseImageData
    {
        public ResponseImageDatum[] data;
        public long created { get; set; }
        //public List<ImageData> data { get; set; }
    }

    [System.Serializable]
    public class ResponseImageDatum
    {
        public string url;
    }

    [System.Serializable]
    public class ImageData
    {
        public string url { get; set; }
        public string b64_json { get; set; }
    }

    #endregion
}