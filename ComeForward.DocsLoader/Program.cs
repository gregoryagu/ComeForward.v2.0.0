using System;
using System.Collections.Generic;
using System.IO;
using ComeForward.Domain;
using ComeForwardLoader;

// path to repository (backwards works up from bin output directory)
const string DocsPath = @"C:\Dev\Docs\EntityFramework.Docs-main\EntityFramework.Docs-main\entity-framework";

// Azure Cosmos DB endpoint, defaults to emulator
const string EndPoint = "https://cosmos-db-hello-world.documents.azure.com:443/";

// Secret key for Azure Cosmos DB, defaults to emulator
const string AccessKey = "RSNEuzV4H2nX2pWIuoyPRHZmzYm5UxMtf9gSKqQnqeAcOqF3mi1ELIzKWVm4BxPpYOlU9wZcHSVEACDb7cHsJA==";

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
