using System.IO;
using System.Threading.Tasks;

namespace MyApp.Application.Common.Services.Storage;

public abstract class Storage
{
    protected delegate bool HasFileDelegate(string pathOrContainerName, string fileName);

    protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFileDelegate hasFileMethod, bool first = true)
    {
        string newFileName = await Task.Run(async () =>
        {
            string extension = Path.GetExtension(fileName);
            string name = string.Empty;

            if (first)
            {
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                name = $"{NameService.CharacterRegulatory(oldName)}{extension}";
            }
            else
            {
                name = fileName;
                int indexNo1 = name.LastIndexOf("-");
                if (indexNo1 == -1)
                {
                    name = $"{Path.GetFileNameWithoutExtension(name)}-2{extension}";
                }
                else
                {
                    int indexNo2 = name.LastIndexOf(".");
                    string fileNo = name.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);

                    if (int.TryParse(fileNo, out int _fileNo))
                    {
                        _fileNo++;
                        name = name.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                   .Insert(indexNo1 + 1, _fileNo.ToString());
                    }
                    else
                    {
                        name = $"{Path.GetFileNameWithoutExtension(name)}-2{extension}";
                    }
                }
            }

            if (hasFileMethod(pathOrContainerName, name))
                return await FileRenameAsync(pathOrContainerName, name, hasFileMethod, false);

            return name;
        });

        return newFileName;
    }
}
