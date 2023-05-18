using Newtonsoft.Json;
using System;

namespace DicomEditor.Models;

public class DicomConfigItem
{
    [JsonProperty("Tag")]
    public String TagName { get; set; }
    [JsonProperty("Desc")]
    public String Description { get; set; }
}
