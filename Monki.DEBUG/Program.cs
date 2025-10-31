using Monki.APKG;
using SQLitePCL;

Batteries.Init();
//string apkgPath = @"C:\Users\lvivh\Downloads\Goethe_Institute_A1_Wordlist.apkg";
//string apkgPath = @"C:\Users\lvivh\Downloads\Japanese_Basic_Hiragana.apkg";
//string apkgPath = @"C:\Users\lvivh\Downloads\Polish-English.apkg";
string apkgPath = @"C:\Users\lvivh\Downloads\GoF_Design_Pattern_Basics.apkg";

var importer = new MankiImport();

var testRes = await importer.ReadApkgAsync(apkgPath);

foreach(var card in testRes)
{
	Console.WriteLine($"Card ID: {card.Id}");
	Console.WriteLine($"Side A: {card.SideA}");
	Console.WriteLine($"Side B: {card.SideB}");
	Console.WriteLine($"Example A: {card.ExampleA}");
	Console.WriteLine($"Example B: {card.ExampleB}");
	Console.WriteLine("---------------------------");
}
