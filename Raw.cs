// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text.Json;
// using System.IO;
// using System.Threading.Tasks;

// namespace Pillar;

// /// <summary>
// /// 🌟 Pillar - Life Anchors System
// /// A universal tracker for balance, connection, and daily essentials
// /// </summary>
// class Program
// {
//     private static readonly string DataFile = "pillar_data.json";
//     private static PillarTracker tracker = new();

//     static async Task Main(string[] args)
//     {
//         Console.Clear();
//         ShowWelcome();

//         await tracker.LoadDataAsync();

//         while (true)
//         {
//             ShowMainMenu();
//             var choice = Console.ReadKey(true).KeyChar;
//             Console.Clear();

//             switch (choice)
//             {
//                 case '1':
//                     await ShowTodaysAnchors();
//                     break;
//                 case '2':
//                     await ToggleAnchor();
//                     break;
//                 case '3':
//                     ShowWeeklyReport();
//                     break;
//                 case '4':
//                     ShowSettings();
//                     break;
//                 case '5':
//                     Console.WriteLine("🌟 Stay Anchored. Stay Balanced. See you tomorrow!");
//                     await tracker.SaveDataAsync();
//                     return;
//                 default:
//                     Console.WriteLine("❌ Invalid choice. Press any key to continue...");
//                     Console.ReadKey(true);
//                     Console.Clear();
//                     break;
//             }
//         }
//     }

//     static void ShowWelcome()
//     {
//         Console.ForegroundColor = ConsoleColor.Cyan;
//         Console.WriteLine(@"
// ╔══════════════════════════════════════════════════════════════╗
// ║                      🌟 PILLAR 🌟                           ║
// ║             Life Anchors System - Stay Balanced             ║
// ║                                                              ║
// ║  🧱 Core Anchors  •  🌐 Connection  •  🎯 Growth           ║
// ╚══════════════════════════════════════════════════════════════╝
// ");
//         Console.ResetColor();
//         Console.WriteLine("Welcome! Let's build your daily anchors for a balanced life.\n");
//     }

//     static void ShowMainMenu()
//     {
//         var today = DateTime.Today;
//         var completedToday = tracker.GetTodayCompletionCount();
//         var totalAnchors = tracker.GetActiveAnchorsCount();
//         var completionRate = totalAnchors > 0 ? (int)((double)completedToday / totalAnchors * 100) : 0;

//         Console.WriteLine($"📅 {today:dddd, MMMM dd, yyyy}");
//         Console.WriteLine($"⚓ Today's Progress: {completedToday}/{totalAnchors} ({completionRate}%)\n");

//         // Show progress bar
//         ShowProgressBar(completionRate);

//         Console.WriteLine("\n🎯 MAIN MENU");
//         Console.WriteLine("═══════════");
//         Console.WriteLine("1️⃣  View Today's Anchors");
//         Console.WriteLine("2️⃣  Toggle Anchor Complete");
//         Console.WriteLine("3️⃣  Weekly Balance Report");
//         Console.WriteLine("4️⃣  Settings & Customize");
//         Console.WriteLine("5️⃣  Exit");
//         Console.Write("\n🔹 Choose option (1-5): ");
//     }

//     static void ShowProgressBar(int percentage)
//     {
//         const int barWidth = 30;
//         int filledWidth = (int)(barWidth * percentage / 100.0);

//         Console.Write("Progress: [");
//         Console.ForegroundColor = ConsoleColor.Green;
//         Console.Write(new string('█', filledWidth));
//         Console.ResetColor();
//         Console.Write(new string('░', barWidth - filledWidth));
//         Console.WriteLine($"] {percentage}%");
//     }

//     static async Task ShowTodaysAnchors()
//     {
//         Console.WriteLine("🌟 TODAY'S LIFE ANCHORS");
//         Console.WriteLine("═══════════════════════\n");

//         var anchors = tracker.GetTodaysAnchors();

//         foreach (var category in anchors.GroupBy(a => a.Category))
//         {
//             Console.WriteLine($"{GetCategoryIcon(category.Key)} {category.Key.ToUpper()} ANCHORS");
//             Console.WriteLine(new string('─', 25));

//             foreach (var anchor in category)
//             {
//                 var status = anchor.IsCompletedToday ? "✅" : "⭕";
//                 var color = anchor.IsCompletedToday ? ConsoleColor.Green : ConsoleColor.Yellow;

//                 Console.ForegroundColor = color;
//                 Console.WriteLine($"  {status} {anchor.Name}");
//                 Console.ResetColor();
//                 Console.WriteLine($"     💡 {anchor.Description}");
//                 Console.WriteLine();
//             }
//         }

//         Console.WriteLine("Press any key to return to menu...");
//         Console.ReadKey(true);
//         Console.Clear();
//     }

//     static async Task ToggleAnchor()
//     {
//         Console.WriteLine("⚓ TOGGLE ANCHOR COMPLETION");
//         Console.WriteLine("═══════════════════════════\n");

//         var anchors = tracker.GetTodaysAnchors().ToList();

//         for (int i = 0; i < anchors.Count; i++)
//         {
//             var status = anchors[i].IsCompletedToday ? "✅" : "⭕";
//             Console.WriteLine($"{i + 1}. {status} {anchors[i].Name}");
//         }

//         Console.Write("\n🔹 Enter anchor number to toggle (0 to cancel): ");

//         if (int.TryParse(Console.ReadLine(), out int choice) &&
//             choice > 0 && choice <= anchors.Count)
//         {
//             var anchor = anchors[choice - 1];
//             tracker.ToggleAnchor(anchor.Id);

//             var newStatus = anchor.IsCompletedToday ? "⭕ Marked as incomplete" : "✅ Completed";
//             Console.WriteLine($"\n🎉 {anchor.Name}: {newStatus}!");

//             await tracker.SaveDataAsync();
//         }

//         Console.WriteLine("\nPress any key to continue...");
//         Console.ReadKey(true);
//         Console.Clear();
//     }

//     static void ShowWeeklyReport()
//     {
//         Console.WriteLine("📊 WEEKLY BALANCE REPORT");
//         Console.WriteLine("═══════════════════════════\n");

//         var report = tracker.GetWeeklyReport();

//         Console.WriteLine($"📅 Week of {DateTime.Today.AddDays(-7):MMM dd} - {DateTime.Today:MMM dd}\n");

//         foreach (var category in report)
//         {
//             var percentage = (int)(category.Value * 100);
//             var icon = GetCategoryIcon(category.Key);

//             Console.WriteLine($"{icon} {category.Key} Anchors: {percentage}%");
//             ShowProgressBar(percentage);

//             // Provide insights
//             if (percentage >= 80)
//                 Console.WriteLine("   🌟 Excellent! You're crushing it!");
//             else if (percentage >= 60)
//                 Console.WriteLine("   👍 Good work! Keep building momentum!");
//             else if (percentage >= 40)
//                 Console.WriteLine("   💪 Room for growth - you've got this!");
//             else
//                 Console.WriteLine("   🎯 Focus area - small steps lead to big changes!");

//             Console.WriteLine();
//         }

//         // Overall insight
//         var overallScore = (int)(report.Values.Average() * 100);
//         Console.WriteLine($"🎯 Overall Balance Score: {overallScore}%");

//         if (overallScore >= 70)
//             Console.WriteLine("🏆 You're living a well-anchored life! Keep it up!");
//         else
//             Console.WriteLine("🌱 Your anchors are growing stronger. Consistency is key!");

//         Console.WriteLine("\nPress any key to continue...");
//         Console.ReadKey(true);
//         Console.Clear();
//     }

//     static void ShowSettings()
//     {
//         Console.WriteLine("⚙️ SETTINGS & CUSTOMIZATION");
//         Console.WriteLine("══════════════════════════\n");

//         Console.WriteLine("🎯 Current Active Anchors:");
//         var anchors = tracker.GetAllAnchors();

//         foreach (var category in anchors.GroupBy(a => a.Category))
//         {
//             Console.WriteLine($"\n{GetCategoryIcon(category.Key)} {category.Key}:");
//             foreach (var anchor in category)
//             {
//                 var status = anchor.IsActive ? "✅" : "❌";
//                 Console.WriteLine($"  {status} {anchor.Name}");
//             }
//         }

//         Console.WriteLine("\n📝 Future features:");
//         Console.WriteLine("   • Custom anchor creation");
//         Console.WriteLine("   • Notification reminders");
//         Console.WriteLine("   • Streak tracking");
//         Console.WriteLine("   • Goal setting");
//         Console.WriteLine("   • Data export");

//         Console.WriteLine("\nPress any key to continue...");
//         Console.ReadKey(true);
//         Console.Clear();
//     }

//     static string GetCategoryIcon(string category)
//     {
//         return category.ToLower() switch
//         {
//             "core" => "🧱",
//             "connection" => "🌐",
//             "growth" => "🎯",
//             _ => "⚓"
//         };
//     }
// }

// /// <summary>
// /// Represents a Life Anchor - a daily habit or activity that stabilizes and enriches life
// /// </summary>
// public class LifeAnchor
// {
//     public string Id { get; set; } = Guid.NewGuid().ToString();
//     public string Name { get; set; } = "";
//     public string Description { get; set; } = "";
//     public string Category { get; set; } = "";
//     public bool IsActive { get; set; } = true;
//     public Dictionary<string, bool> Completions { get; set; } = new();

//     public bool IsCompletedToday =>
//         Completions.ContainsKey(DateTime.Today.ToString("yyyy-MM-dd")) &&
//         Completions[DateTime.Today.ToString("yyyy-MM-dd")];
// }

// /// <summary>
// /// Core tracking system for Life Anchors
// /// </summary>
// public class PillarTracker
// {
//     private List<LifeAnchor> anchors = new();
//     private readonly string dataFile = "pillar_data.json";

//     public PillarTracker()
//     {
//         InitializeDefaultAnchors();
//     }

//     private void InitializeDefaultAnchors()
//     {
//         anchors = new List<LifeAnchor>
//         {
//             // Core Anchors (for everyone)
//             new() { Name = "Body Care", Description = "Shower, brush teeth, basic hygiene", Category = "Core" },
//             new() { Name = "Fuel Up", Description = "Eat 2-3 meals, stay hydrated", Category = "Core" },
//             new() { Name = "Rest Well", Description = "Maintain sleep schedule, get quality rest", Category = "Core" },
//             new() { Name = "Move & Breathe", Description = "Go outside, walk, light movement", Category = "Core" },
            
//             // Connection Anchors
//             new() { Name = "Social Touchpoint", Description = "Reach out to friend/family (text, call, meet)", Category = "Connection" },
//             new() { Name = "Micro-Connection", Description = "Small gestures (smile, chat, online interaction)", Category = "Connection" },
//             new() { Name = "Deep Bonding", Description = "Meaningful conversation or quality time", Category = "Connection" },
            
//             // Growth Anchors
//             new() { Name = "Learn Something", Description = "Read, watch tutorial, practice new skill", Category = "Growth" },
//             new() { Name = "Create & Build", Description = "Write, draw, code, cook - be generative", Category = "Growth" },
//             new() { Name = "Reflect & Plan", Description = "Journal, gratitude, mood check, planning", Category = "Growth" }
//         };
//     }

//     public IEnumerable<LifeAnchor> GetTodaysAnchors() =>
//         anchors.Where(a => a.IsActive);

//     public IEnumerable<LifeAnchor> GetAllAnchors() => anchors;

//     public int GetTodayCompletionCount() =>
//         anchors.Count(a => a.IsActive && a.IsCompletedToday);

//     public int GetActiveAnchorsCount() =>
//         anchors.Count(a => a.IsActive);

//     public void ToggleAnchor(string anchorId)
//     {
//         var anchor = anchors.FirstOrDefault(a => a.Id == anchorId);
//         if (anchor == null) return;

//         var today = DateTime.Today.ToString("yyyy-MM-dd");
//         if (anchor.Completions.ContainsKey(today))
//             anchor.Completions[today] = !anchor.Completions[today];
//         else
//             anchor.Completions[today] = true;
//     }

//     public Dictionary<string, double> GetWeeklyReport()
//     {
//         var report = new Dictionary<string, double>();
//         var categories = anchors.Where(a => a.IsActive).GroupBy(a => a.Category);

//         foreach (var category in categories)
//         {
//             var totalDays = 7;
//             var totalPossible = category.Count() * totalDays;
//             var totalCompleted = 0;

//             for (int i = 0; i < totalDays; i++)
//             {
//                 var date = DateTime.Today.AddDays(-i).ToString("yyyy-MM-dd");
//                 totalCompleted += category.Count(a =>
//                     a.Completions.ContainsKey(date) && a.Completions[date]);
//             }

//             report[category.Key] = totalPossible > 0 ? (double)totalCompleted / totalPossible : 0;
//         }

//         return report;
//     }

//     public async Task SaveDataAsync()
//     {
//         try
//         {
//             var json = JsonSerializer.Serialize(anchors, new JsonSerializerOptions
//             {
//                 WriteIndented = true
//             });
//             await File.WriteAllTextAsync(dataFile, json);
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"⚠️ Could not save data: {ex.Message}");
//         }
//     }

//     public async Task LoadDataAsync()
//     {
//         try
//         {
//             if (File.Exists(dataFile))
//             {
//                 var json = await File.ReadAllTextAsync(dataFile);
//                 var loadedAnchors = JsonSerializer.Deserialize<List<LifeAnchor>>(json);
//                 if (loadedAnchors != null && loadedAnchors.Any())
//                 {
//                     anchors = loadedAnchors;
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"⚠️ Could not load saved data: {ex.Message}");
//             Console.WriteLine("Starting with default anchors...");
//         }
//     }
// }