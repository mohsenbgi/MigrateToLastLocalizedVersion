Console.WriteLine("Enter project full path:");
var projectPath = Console.ReadLine().ToString();

var directories1 = GetDirectories(projectPath);

var Files = new List<string>();

foreach (var d in directories1)
{
   Files.AddRange(Directory.GetFiles(d).Where(x => x.EndsWith(".cs") || x.EndsWith(".cshtml")));
}

List<string> GetDirectories(string path)
{

   var directories1 = new List<string>();

   directories1.Add(path);

   var newDiectory = new List<string>();
   newDiectory.AddRange(directories1);

   var tempDirectory = new List<string>();

   while (true)
   {
       foreach (var item in newDiectory)
       {
           tempDirectory.AddRange(Directory.GetDirectories(item));
       }

       if (tempDirectory.Count == 0) break;

       newDiectory.Clear();
       newDiectory.AddRange(tempDirectory);
       directories1.AddRange(tempDirectory);
       tempDirectory.Clear();
   }

   return directories1;
}

foreach (var file in Files)
{
    var lines = File.ReadAllLines(file).ToList();
    var newLines = new List<string>();
    foreach (var line in lines)
    {
        if(line.Contains("FilterConditions<")){
            var entityName = line.Substring(line.IndexOf("<") + 1, line.IndexOf(">") - line.IndexOf("<") - 1);

            if(entityName != "TEntity" && entityName != "TModel"){
                
                newLines.Add(line.Replace($"FilterConditions<{entityName}>", $"Filter.GenerateConditions<{entityName}>")
                        .Replace(".GenerateNew", ""));

                System.Console.WriteLine(newLines.Last());
            }
            else{
                newLines.Add(line);
            }
        }
        else{
            newLines.Add(line);
        }
    }

    if(newLines.Any()){
        File.WriteAllLines(file, newLines);
    }
}