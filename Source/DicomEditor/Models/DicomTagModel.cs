namespace DicomEditor.Models;

/// <summary>
/// 
/// </summary>
/// <param name="Tag">00100020</param>
/// <param name="VR"></param>
/// <param name="Name"></param>
public record DicomTagModel(string Tag, string VR, string Name);
