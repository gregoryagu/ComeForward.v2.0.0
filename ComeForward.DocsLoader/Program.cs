using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ComeForward.Domain;
using ComeForwardLoader;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();


// path to repository (backwards works up from bin output directory)
string DocsPath = config["ComeForward:DocsPath"] ?? throw new ArgumentNullException("ComeForward:DocsPath");

// Azure Cosmos DB endpoint, defaults to emulator
string EndPoint = config["ComeForward:EndPoint"] ?? throw new ArgumentNullException("ComeForward:EndPoint");

// Secret key for Azure Cosmos DB, defaults to emulator
string AccessKey = config["ComeForward:AccessKey"] ?? throw new ArgumentNullException("ComeForward:AccessKey");

// set to true to re-run tests without rebuilding db
var testsOnly = false;

Console.Clear();

Console.WriteLine($"Cosmos Loader for Planetary Docs executing in directory: {Environment.CurrentDirectory}");

if (!testsOnly && !Directory.Exists(DocsPath))
{
    Console.WriteLine($"Invalid path to docs: {DocsPath}");
    return;
}

List<Document> docsList = null;

if (!testsOnly)
{
    var filesToParse = FileSystemParser.FindCandidateFiles(DocsPath);
    docsList = MarkdownParser.ParseFiles(filesToParse);
}

await CosmosLoader.LoadDocumentsAsync(docsList, EndPoint, AccessKey);
