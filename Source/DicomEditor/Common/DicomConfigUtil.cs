using DicomEditor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DicomEditor.Common;

public class DicomConfigUtil
{
    private static List<DicomConfigItem> _itemList;

    public static List<DicomConfigItem> LoadConfig(bool force = false)
    {
        if (force == false)
        {
            if (_itemList != null && _itemList.Count > 0)
            {
                return _itemList;
            }
        }



        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_dicom.json");

        if (!File.Exists(configPath))
        {
            throw new Exception("config file not exist");
        }
        _itemList = new List<DicomConfigItem>();

        var result = JsonConvert.DeserializeObject<List<DicomConfigItem>>(File.ReadAllText(configPath));
        result.ForEach(i =>
        {
            _itemList.Add(i);
        });
        return _itemList;


    }

}
