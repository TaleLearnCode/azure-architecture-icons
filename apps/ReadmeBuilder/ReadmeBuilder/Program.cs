
using ReadmeBuilder;
using System.Text;

string directoryPath = GetPath("Icon Source Directory: ");

Dictionary<DirectoryInfo, List<FileInfo>> files = GetDirectoryListing(directoryPath);
BuildListingMarkdowns(files);
BuildMasterListingMarkdown(files, directoryPath);
Console.WriteLine("Markdown files generated");
Console.ReadLine();



static string GetPath(string prompt)
{
	if (!prompt.EndsWith(' ')) prompt += ' ';
	string? path;
	do
	{
		Console.Write(prompt);
		path = Console.ReadLine();
		if (path is null || !Directory.Exists(path))
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Beep();
			Console.WriteLine("Invalid path specified");
			Console.ResetColor();
			Console.WriteLine();
		}
	} while (path is null);
	return path;
}

static Dictionary<DirectoryInfo, List<FileInfo>> GetDirectoryListing(string directoryPath)
{
	Dictionary<DirectoryInfo, List<FileInfo>> results = [];
	string[] directories = Directory.GetDirectories(directoryPath);
	foreach (string directory in directories)
	{
		DirectoryInfo directoryInfo = new(directory);
		string[] files = Directory.GetFiles(directory);
		List<FileInfo> fileInfoList = [];
		foreach (string file in files)
		{
			FileInfo fileInfo = new(file);
			fileInfoList.Add(fileInfo);
		}
		results.Add(directoryInfo, fileInfoList);
	}
	return results;
}

static void BuildListingMarkdowns(Dictionary<DirectoryInfo, List<FileInfo>> files)
{
	foreach (KeyValuePair<DirectoryInfo, List<FileInfo>> directory in files)
	{
		StringBuilder output = new();
		output.AppendLine($"# {GetDirectoryTitle(directory.Key.Name)}");
		output.AppendLine("This directory contains the following files:");
		output.AppendLine();
		foreach (FileInfo fileInfo in directory.Value)
			if (fileInfo.Name != "README.md")
				output.AppendLine($"- [{fileInfo.Name}]({fileInfo.Name})");
		string markdownFilePath = Path.Combine(directory.Key.FullName, "README.md");
		File.WriteAllText(markdownFilePath, output.ToString());
	}
}

static void BuildMasterListingMarkdown(Dictionary<DirectoryInfo, List<FileInfo>> files, string directoryPath)
{
	StringBuilder output = new();
	output.AppendLine("# Azure Architecture Icons");
	output.AppendLine();

	foreach (KeyValuePair<DirectoryInfo, List<FileInfo>> directory in files)
		output.AppendLine($"- [{GetDirectoryTitle(directory.Key.Name)}](#{directory.Key.Name.ToSnakeCase()})");
	output.AppendLine();

	foreach (KeyValuePair<DirectoryInfo, List<FileInfo>> directory in files)
	{
		output.AppendLine();
		output.AppendLine($"## {GetDirectoryTitle(directory.Key.Name)}");
		foreach (FileInfo fileInfo in directory.Value)
			if (fileInfo.Name != "README.md")
				output.AppendLine($"- {fileInfo.Name}");
	}
	string markdownFilePath = Path.Combine(directoryPath, "README.md");
	File.WriteAllText(markdownFilePath, output.ToString());
}

static string GetDirectoryTitle(string directoryName)
{
	string results = directoryName.FromKababToTitleCase();
	if (results == "Ai Machine Learning")
		results = "AI + Machine Learning";
	else if (results == "Hybrid Multicloud")
		results = "Hybrid + Multicloud";
	else if (results == "Management Governance")
		results = "Management + Governance";
	return results;
}