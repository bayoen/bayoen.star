using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using LiteDB;

using bayoen.star.Variables;
using bayoen.library.General.Enums;

namespace bayoen.star.Workers
{
    public class Database
    {
        public Database()
        {
            
        }

        public void Insert(MatchRecord match)
        {
            using (LiteDatabase db = new LiteDatabase(DBConnectionString))
            {
                LiteCollection<MatchRecord> matchColl = db.GetCollection<MatchRecord>(MatchHeader);
                matchColl.EnsureIndex(nameof(MatchRecord.ID));
                matchColl.Insert(match);
            }
        }

        public void ClearMatch()
        {
            using (LiteDatabase db = new LiteDatabase(DBConnectionString))
            {
                db.DropCollection(MatchHeader);
                db.Shrink();
                db.GetCollection<MatchRecord>(MatchHeader);
            }
        }

        public int MatchCount()
        {
            using (LiteDatabase db = new LiteDatabase(DBConnectionString))
            {
                LiteCollection<MatchRecord> matchColl = db.GetCollection<MatchRecord>(MatchHeader);
                return matchColl.Count();
            }
        }

        public int MatchCount(Expression<Func<MatchRecord, bool>> predicate)
        {
            using (LiteDatabase db = new LiteDatabase(DBConnectionString))
            {
                LiteCollection<MatchRecord> matchColl = db.GetCollection<MatchRecord>(MatchHeader);
                return matchColl.Count(predicate);
            }
        }

        public List<MatchRecord> GetMatches()
        {
            using (LiteDatabase db = new LiteDatabase(DBConnectionString))
            {
                LiteCollection<MatchRecord> matchColl = db.GetCollection<MatchRecord>(MatchHeader);
                return matchColl.FindAll().ToList();
            }
        }

        public List<MatchRecord> GetMatches(Expression<Func<MatchRecord, bool>> predicate)
        {
            using (LiteDatabase db = new LiteDatabase(DBConnectionString))
            {
                LiteCollection<MatchRecord> matchColl = db.GetCollection<MatchRecord>(MatchHeader);
                return matchColl.Find(predicate).ToList();
            }
        }

        public List<MatchRecord> ReversedMatches()
        {
            var matches = this.GetMatches();
            matches.Reverse();
            return matches;
        }

        public List<MatchRecord> ReversedMatches(Expression<Func<MatchRecord, bool>> predicate)
        {
            var matches = this.GetMatches(predicate);
            matches.Reverse();
            return matches;
        }

        //public MatchRecord LastMatch()
        //{
        //    using (LiteDatabase db = new LiteDatabase(DBConnectionString))
        //    {
        //        LiteCollection<MatchRecord> matchColl = db.GetCollection<MatchRecord>(MatchHeader);
        //        return matchColl.FindOne(x => x.ID == (matchColl.Count()));
        //    }
        //}

        //public void UpdateLastMatchRating(int gain)
        //{
        //    using (LiteDatabase db = new LiteDatabase(DBConnectionString))
        //    {
        //        LiteCollection<MatchRecord> matchColl = db.GetCollection<MatchRecord>(MatchHeader);
        //        MatchRecord last = matchColl.FindOne(x => x.ID == (matchColl.Count()));
        //        last.RatingGain = gain;
        //        matchColl.Update(last);
        //    }
        //}

        private ObservableCollection<MatchRecord> _matches;
        public ObservableCollection<MatchRecord> Matches => this._matches ?? (this._matches = new ObservableCollection<MatchRecord>());

        private ObservableCollection<MatchRecord> _leagues;
        public ObservableCollection<MatchRecord> Leagues => this._leagues ?? (this._leagues = new ObservableCollection<MatchRecord>());

        public const string MatchHeader = "matches";
        public readonly ConnectionString DBConnectionString = new ConnectionString(Config.DataBaseFileName)
        {
            Password = "546050", // Only SemiRain Knows?
        };        
    }
}
