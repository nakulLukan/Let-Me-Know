using LiteDB;
using System;
using System.Diagnostics;
using System.IO;

namespace LetMeKnow.Entities
{
    public class AppDbContext
    {
        readonly LiteDatabase database = null;

        public ILiteCollection<State> States { get; set; }
        public ILiteCollection<District> Districts { get; set; }
        public ILiteCollection<Setting> Settings { get; set; }
        public ILiteCollection<VaccineSessionHistory> SessionHistories { get; set; }

        /// <summary>
        /// Initialise db context
        /// </summary>
        public AppDbContext()
        {
            var db = new LiteDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LetMeKnow.db"));
            database = db;

            States = db.GetCollection<State>(nameof(State));
            Districts = db.GetCollection<District>(nameof(District));
            Settings = db.GetCollection<Setting>(nameof(Setting));
            SessionHistories = db.GetCollection<VaccineSessionHistory>(nameof(VaccineSessionHistory));
        }

        /// <summary>
        /// Spawn db context
        /// </summary>
        /// <returns></returns>
        public static AppDbContext Spawn()
        {
            return new AppDbContext();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~AppDbContext()
        {
            Debug.WriteLine("Database disposed successfully");
            database.Dispose();
        }
    }
}
